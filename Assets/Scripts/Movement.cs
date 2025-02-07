using System.Collections;
using UnityEngine;

public class Movement : Health
{
    private enum PlayerStates 
    {
        Idle,
        Crouch,
        Dash,
        Jump
    }

    private PlayerStates States = PlayerStates.Idle;
    [HideInInspector]public Animator animator;
    [HideInInspector]public Rigidbody2D Physick;
    [SerializeField] private float Speed = 5, Force = 6, DashForce = 10, DashTime = .2f, DashCoolDown = 2f, AttackRange = 3f, ParryTiming = 0.1f, StepBackForce = 10, StepBackTime = .2f, StepBackCoolDown = 1f, SneakCoolDown = 0.5f;
    [SerializeField] private LayerMask CeilingLayer,AttackLayer, GroundLayer = 6;
    [SerializeField] private Transform TopPosition, AttackPoint;
    private IUsable UsableEntity;
    [SerializeField] private bool CanDoubleJump = true,CanAttack = true,Runing = false;
    private bool CanDash = true, DoubleJump, CanStepBack = true, CanSneak = true;
    [HideInInspector] public bool CanJump = true,Hidden = false, IsTouchingLadder = false,Jumped = false,Forced = false;
    private Transform Transforming;
    [HideInInspector] public float AttackCooldown = 0.5f;
    public int Damage = 15;
    public static Movement Instance { get; private set; }

    private void Awake() => Instance = this;
    private void Start()
    {
        Transforming = transform;
        Physick = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && CanJump)
        {
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Sneak();
        }
        else if (!Physics2D.OverlapCircle(TopPosition.position, 0.3f, CeilingLayer) && Input.GetKeyUp(KeyCode.LeftControl))
        {
            StandUp();
        }
        /*    if (Input.GetKeyDown(KeyCode.LeftShift))
             {
                 StartCoroutine(Dash());
             }
        */
        if (Input.GetKeyDown(KeyCode.LeftShift) && Input.GetAxis("Horizontal") != 0)
        {
            Runing = !Runing;
            Run();
        }
        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            StartCoroutine(StepBack());
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Using();
        }
        if (Input.GetKeyDown(KeyCode.X ))
        {
            Attack();
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            Blockign();
        }
        animator.SetFloat("Speed", Mathf.Abs(Input.GetAxis("Horizontal") * Speed));
    }
    private void FixedUpdate()
    {
        if (States == PlayerStates.Dash)
        {
            return;
        }
        if(!Forced)Physick.velocity = new Vector2(Input.GetAxis("Horizontal") * Speed, Physick.velocity.y);
        if (IsTouchingLadder) Physick.velocity = new Vector2(Physick.velocity.x, Input.GetAxis("Vertical") * Speed);
        Flip();
    }
    private void Blockign()
    {
        Block = !Block;
        StartCoroutine(Parry());
        if (Block)
        {
            Speed /= 2;
        }
        else
        {
            Speed *= 2;
        }
    }
    private void Run()
    {
        if (Runing)
        {
            Speed *= 2;
            StartCoroutine(WaitForStop());
            animator.SetBool("RunStop", false);
        }
        else
        {
            if (Speed > 4.9f)
            {
                Speed /= 2;
                animator.SetBool("RunStop",true);
            }
        }
    }
    private IEnumerator WaitForStop()
    {
        yield return new WaitUntil(() => Input.GetAxis("Horizontal") == 0);
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
        {
            StartCoroutine(WaitForStop());
            animator.SetBool("Flip", true);
            yield break;
        }
        if(!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
              animator.SetBool("Flip", false);
              Runing = false;
             Run();
            yield break;
        }
    }
    private IEnumerator Parry()
    {
        IdealParryTiming = true;
        yield return new WaitForSeconds(ParryTiming);
        IdealParryTiming = false;
        yield break;
    }
    private void Flip()
    {
        if (Input.GetAxis("Horizontal") < 0)
        {
            Transforming.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
            Transforming.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void Attack()
    {
        if (CanAttack)
        {
            Collider2D[] collider = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, AttackLayer);
            foreach (Collider2D coll in collider)
            {
                if (coll.GetComponent<Health>() != null)
                {
                    coll.GetComponent<Health>().ApplyDamage(Damage);
                }
            }
            StartCoroutine(AttackingCooldown(AttackCooldown));
        }
    }
    private IEnumerator AttackingCooldown(float second)
    {
        CanAttack = false;
        yield return new WaitForSeconds(second);
        CanAttack = true;
        yield break;
    }
    private void Jump()
    {
        if (GroundCheck.Instance.GetOnGround() && States != PlayerStates.Crouch)
        {
            animator.SetTrigger("Jumping");
            Physick.velocity = new Vector2(Physick.velocity.x, 0);
            Physick.AddForce(transform.up * Force, ForceMode2D.Impulse);
            DoubleJump = true;
            GroundCheck.Instance.Flyed = true;
            Jumped = true;
            States = PlayerStates.Jump;
        }else if (DoubleJump && CanDoubleJump)
        {
          //  Physick.velocity = new Vector2(Physick.velocity.x, 0);
            Physick.AddForce(transform.up * Force, ForceMode2D.Impulse);
            Jumped = false;
            States = PlayerStates.Jump;
            DoubleJump = false;
        }
    }
    private void Sneak()
    {
        if (GroundCheck.Instance.GetOnGround()) States = PlayerStates.Idle;
        if(States == PlayerStates.Idle && CanSneak)
        {
            Speed /= 2;
            transform.localScale = new Vector2(transform.localScale.x, Transforming.localScale.y / 2);
            States = PlayerStates.Crouch;
            CanSneak = false;
            StartCoroutine(WaitForStandUp());
            StartCoroutine(SneakTimeOut());
        }
    }
    private IEnumerator SneakTimeOut() 
    {
        yield return new WaitForSeconds(SneakCoolDown);
        CanSneak = true;
        yield break;
    }
    private IEnumerator WaitForStandUp()
    {
        yield return new WaitUntil(() => Physics2D.OverlapCircle(TopPosition.position, 0.3f, CeilingLayer));
        yield return new WaitUntil(() => !Physics2D.OverlapCircle(TopPosition.position, 0.3f, CeilingLayer) && Input.GetKeyDown(KeyCode.LeftControl) == false);
        StandUp();
        yield break;
    }
    private void StandUp()
    {
        if(States == PlayerStates.Crouch && Input.GetKeyDown(KeyCode.LeftControl) == false)
        {
            Speed *= 2;
            States = PlayerStates.Idle;
            transform.localScale = new Vector3(transform.localScale.x, Transforming.localScale.y * 2);
            StopCoroutine(WaitForStandUp());
        }
    }
    private IEnumerator StepBack()
    {
        if (States != PlayerStates.Crouch && CanStepBack && GroundCheck.Instance.GetOnGround())
        {
            Physick.velocity = new Vector2(0, Physick.velocity.y); ;
            CanStepBack = false;
            States = PlayerStates.Dash;
            if(Transforming.localEulerAngles.y == 0)
            {
                if(StepBackForce > 0)
                {
                    StepBackForce *= -1;
                }
            }
            else
            {
                if (StepBackForce < 0)
                {
                    StepBackForce *= -1;
                }
            }
            Physick.velocity = new Vector2(Transforming.localScale.x * StepBackForce, Physick.velocity.y);
            yield return new WaitForSeconds(StepBackTime);
            States = PlayerStates.Idle;
            yield return new WaitForSeconds(StepBackCoolDown);
            CanStepBack = true;
            yield break;
        }
        yield break;
    }
 /*   private IEnumerator Dash()
    {
        if(States != PlayerStates.Crouch && CanDash)
        {
            CanDash = false;
            States = PlayerStates.Dash;
           Physick.velocity = new Vector2(Input.GetAxis("Horizontal") * DashForce, Physick.velocity.y);
            yield return new WaitForSeconds(DashTime);
            States = PlayerStates.Idle;
            yield return new WaitForSeconds(DashCoolDown);
            CanDash = true;
            yield break;
        }
        yield break;
    }*/
    private void Using()
    {
        UsableEntity?.Use();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<IUsable>() != null)
        {
           UsableEntity = collision.GetComponent<IUsable>();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<IUsable>() != null)
        {
            UsableEntity = null;
        }
    }
    public void Freeze()
    {
        Physick.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
        Physick.velocity = Vector2.zero;
        CanJump = false;
    }
    public void UnFreeze()
    {
        Physick.constraints = RigidbodyConstraints2D.None;
        Physick.constraints = RigidbodyConstraints2D.FreezeRotation;
        CanJump = true;
    }

    private void OnDrawGizmos()
    {
        if(AttackPoint != null)Gizmos.DrawWireSphere(AttackPoint.position, AttackRange);
    }


    public override void OnDamaged(int damage)
    {
       //
    }

    public override void OnDeath()
    {
        //
    }
}
