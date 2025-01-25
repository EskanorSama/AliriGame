using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbAttacker : MonoBehaviour
{
    public int Damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Health target = collision.gameObject.GetComponent<Health>();
        target.TakeDamage(Damage); 
    }
}
