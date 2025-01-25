using UnityEngine;

public class FallingStoneTrigger : MonoBehaviour
{
    [SerializeField] private Rigidbody2D Stone;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null)
        {
            if(Stone != null)Stone.gravityScale = 1f;
        }
    }
}
