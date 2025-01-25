using System.Collections;
using UnityEngine;

public class VisibilityArea : MonoBehaviour
{
    private Enemy _Owner;
    private Coroutine _Coroutine;

    private void Start()
    { 
        _Owner  = transform.parent.GetComponent<Enemy>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null)
        {
            _Owner.StopPatroling = true;
            _Coroutine = StartCoroutine(_Owner.PlayerChase());
            StartCoroutine(CheckDistance());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            StopCoroutine(_Coroutine);
            _Owner.PlayerChasing = false;
           _Owner.StopPatroling = false;
            _Owner.OnStopChasing();
        }
    }
    private IEnumerator CheckDistance()
    {
        yield return new WaitUntil(() => Vector2.Distance(_Owner.transform.position, Player.Instance.transform.position) <= _Owner.AttackDistance);
        StopCoroutine(_Coroutine);
        _Owner.PlayerChasing = false;
        _Owner.StopPatroling = true;
        _Owner.Attack();
      //  if(Vector2.Distance(_Owner.transform.position, Movement.Instance.transform.position) > _Owner.AttackDistance)
      ////  {
      //      _Coroutine = StartCoroutine(_Owner.PlayerChase());
       // }
        yield break;
    }
}
