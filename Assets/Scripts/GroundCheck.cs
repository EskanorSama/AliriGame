
using System.Collections;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public float JumpDelay = 0.1f;
    private int GroundLayer = 6,FlagileLayer = 10;
    private bool OnGround = false, StopCoroutine = false;
    public static GroundCheck Instance;
    private void Awake() => Instance = this;

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
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GroundLayer || collision.gameObject.layer == FlagileLayer)
        {
            if(!StopCoroutine) StartCoroutine(Delay());
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
        StopCoroutine = true;
    }
}
