
using System.Collections;
using UnityEngine;

public class RotatingPlatformDelay : MonoBehaviour
{
    [SerializeField] private float Delay = 0f;
    private void Start()
    {
        StartCoroutine(Delaying());
    }
    private IEnumerator Delaying()
    {
        yield return new WaitForSeconds(Delay);
        GetComponent<Animator>().enabled = true;
        yield break;
    }
}
