using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Health : MonoBehaviour
{
    public SpriteRenderer _renderer;
    private Material _default;
    [Header("HealthSYS")]
    [SerializeField] private Material _flash;
    [SerializeField] private float _flashDelay = 0.125f;
    [SerializeField]private int HealthCount,MaxHealth = 100;
    [SerializeField] private bool IsPlayer = false;
    [HideInInspector] public bool Block = false, IdealParryTiming = false, Amulet = false;
    [HideInInspector] public int BlocPercentage = 20, Difference;
    public abstract void OnDamaged(int damage);
    public abstract void OnDeath();

    public int GetHealth()
    {
        return HealthCount;
    }
    public int GetMaxHealth()
    {
        return MaxHealth;
    }
    public void ApplyDamage(int damage)
    {
        if (HealthCount > 0)
        {
            if (Block)
            {
                damage /= 2;
                if (IdealParryTiming)
                {
                    damage = 0;
                }
            }
            if (Amulet)
            {
                damage -= (damage * BlocPercentage) / 100;
            }
            StartCoroutine(Flash());
            HealthCount -= damage;
            if (HealthCount <= 0)
            {
                HealthCount = 0;
                Death();
            }
            if (IsPlayer) HealtDisplay.Instance.Display(HealthCount, MaxHealth);
        }
        OnDamaged(damage);
    }

    private IEnumerator Flash()
    {
        if(_renderer == null)
        {
            _renderer = GetComponent<SpriteRenderer>();
            _default = _renderer.material;
        }
        _renderer.material = _flash;
        yield return new WaitForSeconds(_flashDelay);
        _renderer.material = _default;
        yield break;
    }

    public void SetMaxHealth(int maxhealth)
    {
        MaxHealth = maxhealth;
        if (IsPlayer)
        {
            Difference = maxhealth - MaxHealth;
            Heal(Difference);
            HealtDisplay.Instance.Display(HealthCount, MaxHealth);
        }
    }
    private void Death()
    {
        if (IsPlayer)
        {
            EchoSystem.Instance.DropEchoes();
            Saver.Instance.Load();
            
        }
        else
        {
            OnDeath();
        }
    }
    public void Heal(int healcount)
    {
        if (HealthCount < MaxHealth)
        {
            HealthCount += healcount;
            if(HealthCount > MaxHealth)
            {
                HealthCount = MaxHealth;
            }
            if (IsPlayer) HealtDisplay.Instance.Display(HealthCount, MaxHealth);
        }
    }
}
