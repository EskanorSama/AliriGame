
using System.Collections;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public float JumpDelay = 0.1f;
    private int GroundLayer = 6,FlagileLayer = 10;
    private bool OnGround = false,StopCoroutine = false;
    public static GroundCheck Instance;
    [HideInInspector] public bool Flyed = false;
    private Movement Player;
    private void Awake() => Instance = this;
    private void Start() => Player = Movement.Instance;
    public bool GetOnGround()
    {
        return OnGround;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GroundLayer || collision.gameObject.layer == FlagileLayer)
        {
            OnGround = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GroundLayer || collision.gameObject.layer == FlagileLayer)
        {
            OnGround = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GroundLayer || collision.gameObject.layer == FlagileLayer)
        {
            if (!Player.Jumped && !StopCoroutine)
            {
                StartCoroutine(Delay());
            }
            if (Player.Jumped)
            {
                OnGround = false;
                Player.Jumped = false;
            }
        }
    }
    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(JumpDelay);
        OnGround = false;
        RaycastHit2D raycastHit2 = Physics2D.Raycast(new Vector2(transform.position.x,transform.position.y -0.1f), Vector2.down);
        if (!Player.Jumped && !GetOnGround() && raycastHit2.distance > 1f)
        {
            Flyed = true;
            Movement.Instance.animator.SetTrigger("Fly");
        }
        yield break;

    }
    private void OnApplicationQuit()
    {
        StopCoroutine = true;
        StopAllCoroutines();
    }
}
