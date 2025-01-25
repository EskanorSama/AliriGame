using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : Enemy
{
    private Animator animator;
    [SerializeField] private float ReachTime = 2, JumpForce = 2, AttackColldown = 2f, TimeToReturn = 1f;
    [SerializeField] private int Damage = 10;
    private bool CanAttack = true;
    private float _savedSpeed;



    private void Start()
    {
        _savedSpeed = Speed;
        animator = GetComponent<Animator>();
    }
    public override void Attack()
    {
        if (CanAttack)
        {
            Physick.AddForce(new Vector2(transform.localScale.x * JumpForce, JumpForce), ForceMode2D.Impulse);
            animator.SetTrigger("Attack");
            CanAttack = false;
            Player.Instance.ApplyDamage(Damage);
            var RigidBody = Player.Instance.Physick;
            Player.Instance.Forced = true;
            RigidBody.velocity = Vector2.zero;
            RigidBody.AddForce(new Vector2((RigidBody.transform.position.x - transform.position.x) * JumpForce /2,JumpForce),ForceMode2D.Impulse);

            //Movement.Instance.Forced = false;
            //    RigidBody.velocity = new Vector2(transform.localScale.x * 1000, Physick.velocity.y);
            StartCoroutine(AttackTimer());
        }
    }
    private IEnumerator AttackTimer()
    {
        Speed = 0;
        yield return new WaitForSeconds(0.5f);
        Freeze();
        Player.Instance.Forced = false;
        yield return new WaitForSeconds(AttackColldown);
        UnFreeze();
        CanAttack = true;
        Flip();
        Speed = _savedSpeed;
        Speed *= 4;
        yield break;
    }

    public override void Damaged()
    {
      //  throw new System.NotImplementedException();
    }

    private void Update()
    {
        animator.SetFloat("Speed", Speed);
    }
    
    public override void ReachBorder()
    {
        Freeze();
        StopPatroling = true;
        CanFlip = false;
        //_savedSpeed = Speed;
        Speed = 0;
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(ReachTime);
        UnFreeze();
        if (!PlayerChasing)
        {
            Speed = _savedSpeed;
            Flip();
            StopPatroling = false;
            CanFlip = true;
        }
        yield break;
    }

    public override void OnPlayerChasing()
    {
        if (Speed <= 0) Speed = _savedSpeed;
        Speed *= 2;
    }

    public override void OnStopChasing()
    {
      if(Speed > 2) Speed /= 2;
    }

    public override void OnDeath()
    {
        animator.SetTrigger("Death");
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        Destroy(Physick);
       GetComponent<Collider2D>().enabled = false;
        GetComponent<Enemy>().enabled = false;
        enabled = false;
    }
}
