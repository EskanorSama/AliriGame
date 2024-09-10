using System.Collections;
using UnityEngine;

public class Echo : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private Collider2D _collider;
    private bool CanTake = true;
    private void Start()
    {
        CanTake = false;
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _rigidbody.AddForce(new Vector2(Random.Range(-5, 5), Random.Range(3, 5)),ForceMode2D.Impulse);
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(0.15f);
        _collider.isTrigger = false;
        CanTake = true;
        yield break;
     }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Movement>() != null && CanTake)
        {
            EchoSystem.Instance.AddEcho(1);
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            _rigidbody.bodyType = RigidbodyType2D.Static;
            _collider.isTrigger = true;
        }
    }
}
