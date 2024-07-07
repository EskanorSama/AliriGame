using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingCheck : MonoBehaviour
{
    private Movement Player;
    [SerializeField] private float HeightToDamage = 7;
    [SerializeField] private int OneBlockDamage = 10;
    private int FinaleDamage = 10;
    private Vector2 ExitPoint, EnterPoint;

    private void Start()
    {
        Player = Movement.Instance;
        ExitPoint = transform.position;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnterPoint = transform.position;
        if (GroundCheck.Instance.Flyed)
        {
            Player.animator.SetTrigger("Landing");
            GroundCheck.Instance.Flyed = false;
        }
        if (Vector2.Distance(ExitPoint, EnterPoint) > HeightToDamage && ExitPoint.y > EnterPoint.y)
        {
            Debug.Log(Vector2.Distance(ExitPoint, EnterPoint));
            float Whole = Mathf.Floor(Vector2.Distance(ExitPoint, EnterPoint));
            Debug.Log(Whole);
            if (Whole != HeightToDamage)
            {
                FinaleDamage = (int)(Whole - HeightToDamage) * OneBlockDamage;
            }
            else
            {
                FinaleDamage = OneBlockDamage;
            }
            Player.ApplyDamage(FinaleDamage);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        ExitPoint = transform.position;
    }
}
