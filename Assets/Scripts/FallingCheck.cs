using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingCheck : MonoBehaviour
{
    private Movement Player;
    [SerializeField] private float DamageStartedOn = -12.4f;
    [SerializeField] private int OneBlockDamage = 10;
    private int FinaleDamage = 10;

    private void Start() => Player = Movement.Instance;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(Player.Physick.velocity.y < DamageStartedOn)
        {
            Vector2 vel = Player.Physick.velocity;
            float cons = (vel.y - DamageStartedOn) / 2f;
            int Blocks = (int)(cons) * -1;
            if(Blocks > 1)
            {
                FinaleDamage = OneBlockDamage * Blocks;
            }
            Player.ApplyDamage(FinaleDamage);
        }
    }
}
