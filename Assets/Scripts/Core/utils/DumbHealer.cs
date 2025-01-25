using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbHealer : MonoBehaviour
{
    public int HealingValue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var target = collision.gameObject.GetComponent<Health>();
        target.Heal(HealingValue);
    }
}
