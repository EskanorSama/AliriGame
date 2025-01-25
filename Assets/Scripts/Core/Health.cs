using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [field: SerializeField] public int CurrentHP { get; protected set; }
    [field: SerializeField] public int MaximumHp { get; protected set; }

    private void Update()
    {
        if (CurrentHP <= 0) die();
    }

    //����� ��������� �����
    public void TakeDamage(int damage)
    {
        //�������� �� �� �������� �� ���� �������������
        if (damage >= 0) CurrentHP -= damage;
        else throw new HealthSystemException("Trying to deal negative damage!");
    }

    //����� ���������
    public void Heal(int damage)
    {
        if (damage < 0) throw new HealthSystemException("Trying to heal with negative value!"); // �������� �� ������������� �������
        if (CurrentHP + damage > MaximumHp) CurrentHP = MaximumHp; // �������� ��������
        else CurrentHP += damage;
    }

    //�����, ��������� ��������
    private void die() { Destroy(gameObject); }
}


//���������� ��� ��������� �� ������� ������� Heatlh � �����������
class HealthSystemException : Exception
{
    public HealthSystemException(string message) : base(message){}
}
