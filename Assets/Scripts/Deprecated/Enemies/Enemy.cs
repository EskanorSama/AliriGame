using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public  abstract class Enemy : OldHealth
{
    [HideInInspector]public Rigidbody2D Physick;
    private Transform Transforming;
    private Vector2 PointOfPatrol;
    public float Speed = 1, PatrolDistance = 2, AttackDistance = 1;
    [HideInInspector] public bool StopPatroling,CanFlip = true, PlayerChasing = false;
    public abstract void Attack();
    public abstract void Damaged();

    public abstract void ReachBorder();
    public abstract void OnPlayerChasing();
    public abstract void OnStopChasing();

    private void Awake()
    {
        Physick = GetComponent<Rigidbody2D>();
        Transforming = GetComponent<Transform>();
        PointOfPatrol = transform.position;

    }

    private void FixedUpdate()
    {
        Patroling();
    }
    private void Patroling()
    {
        if (!StopPatroling)
        {
            Move(Speed);
            if (Transforming.position.x > PointOfPatrol.x + PatrolDistance )
            {
                if (Transforming.localScale.x > 0)
                {
                    ReachBorder();
                    if(CanFlip)
                    {
                        Flip();
                    }
                }
            }
            else if(Transforming.position.x < PointOfPatrol.x - PatrolDistance)
            {
                if (Transforming.localScale.x < 0)
                {
                    ReachBorder();
                    if(CanFlip)
                    {
                        Flip();
                    }
                }
            }
        }
    }
    public IEnumerator PlayerChase()
    {
        OnPlayerChasing();
        PlayerChasing = true;
        StopPatroling = true;
        while (true)
        {
            MoveTo(Player.Instance.transform, Speed);
            yield return null;
        }
    }

    public void Move(float Speed)
    {
        Physick.velocity = new Vector2(transform.localScale.x * Speed, Physick.velocity.y);
    }
    public void MoveTo(Transform Target, float _speed)
    {
        if (Target.position.x > transform.position.x)
        {
            if (Transforming.localScale.x < 0)
            {
                Flip();
            }
        }
        else if (Target.position.x < transform.position.x)
        {
            if (Transforming.localScale.x > 0)
            {
                Flip();
            }
        }
        Move(_speed);
    }

    public void Flip()
    {
        Vector3 scale = Transforming.localScale;
        scale.x *= -1;
        Transforming.localScale = scale;
    }

    public override void OnDamaged(int damage)
    {
        Damaged();
    }
    public void Freeze()
    {
        if (Physick != null)
        {
            Physick.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
        }
    }
    public void UnFreeze()
    {
        if(Physick != null)
        {
            Physick.constraints = RigidbodyConstraints2D.None;
            Physick.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
}
