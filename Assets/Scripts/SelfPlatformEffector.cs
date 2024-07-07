using System.Collections;
using UnityEngine;

public class SelfPlatformEffector : MonoBehaviour
{
    [SerializeField] private Collider2D Platform;
    [SerializeField] private float Delay = 0.1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Movement>() != null)
        {
            Platform.isTrigger = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Movement>() != null)
        {
            StartCoroutine(CollDown());
        }
    }
    private IEnumerator CollDown()
    {
        yield return new WaitForSeconds(Delay);
        Platform.isTrigger = false;
        yield break;
    }
}
