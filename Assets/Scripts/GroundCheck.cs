
using System.Collections;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public float JumpDelay = 0.1f;
    private int GroundLayer = 6,FlagileLayer = 10;
    private bool OnGround = false, StopingCoroutine = false;
    public static GroundCheck Instance;
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
            if(!StopingCoroutine && !Player.Jumped) StartCoroutine(Delay());
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
        yield break;

    }
    private void OnApplicationQuit()
    {
        StopingCoroutine = true;
    }
}
