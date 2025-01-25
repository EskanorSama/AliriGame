using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<OldHealth>() != null)
        {
            collision.GetComponent<OldHealth>().ApplyDamage(10000);
        }

    }
}
