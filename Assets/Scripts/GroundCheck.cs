
using System.Collections;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public float JumpDelay = 0.1f;
    private int GroundLayer = 6;
    private bool OnGround = false;
    public static GroundCheck Instance;
    private void Awake() => Instance = this;

    public bool GetOnGround()
    {
        return OnGround;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GroundLayer)
        {
            OnGround = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GroundLayer && enabled)
        {
            StartCoroutine(Delay());
        }
    }
    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(JumpDelay);
        OnGround = false;
        yield break;

    }
}
