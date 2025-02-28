using System.Collections;
using UnityEngine;

public class FlagilePlatform : MonoBehaviour
{
    [SerializeField] private float TimeToDestroy = 1f,TimeToRespawn = 2f;
    [SerializeField] private GameObject DestroyPaticle;
    private Rigidbody2D Physick;
    private Collider2D Collider;
    private SpriteRenderer Sprite;
    private Vector2 StartPosition;
    private int Layer = 10;
    private bool Destroyed = false;

    private void Start()
    {
        Sprite = GetComponent<SpriteRenderer>();
        Collider = GetComponent<Collider2D>();
        Physick = GetComponent<Rigidbody2D>();
        StartPosition = transform.position;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            if (!Destroyed)
            {
                Destroyed = true;
                StartCoroutine(Destroy());
            }
        }
    }
    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(TimeToDestroy);
        Instantiate(DestroyPaticle, transform.position, Quaternion.identity);
        Collider.isTrigger = true;
        Physick.bodyType = RigidbodyType2D.Dynamic;
        gameObject.layer = 1;
        while (Sprite.color.a > 0)
        {
            var color = Sprite.color;
            color.a -= 0.03f;
            color.a = Mathf.Clamp(color.a, 0, 1);
            Sprite.color = color;
            yield return null;
        }
        StartCoroutine(ReSpawn());
        yield break;
    }
    private IEnumerator ReSpawn()
    {
        yield return new WaitForSeconds(TimeToRespawn);
        Physick.bodyType = RigidbodyType2D.Static;
        transform.position = StartPosition;
        while (Sprite.color.a < 1)
        {
            var color = Sprite.color;
            color.a += 0.03f;
            color.a = Mathf.Clamp(color.a, 0, 1);
            Sprite.color = color;
            yield return null;
        }
        gameObject.layer = Layer;
        Collider.isTrigger = false;
        Destroyed = false;
        yield break;
    }
}
