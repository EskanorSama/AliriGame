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

    //Метод получения урона
    public void TakeDamage(int damage)
    {
        //Проверка на то является ли урон положительным
        if (damage >= 0) CurrentHP -= damage;
        else throw new HealthSystemException("Trying to deal negative damage!");
    }

    //Метод исцеления
    public void Heal(int damage)
    {
        if (damage < 0) throw new HealthSystemException("Trying to heal with negative value!"); // Проверка на отрицательное лечение
        if (CurrentHP + damage > MaximumHp) CurrentHP = MaximumHp; // Проверка оверхила
        else CurrentHP += damage;
    }

    //Метод, убивающий сущность
    private void die() { Destroy(gameObject); }
}


//Исключение для извещения об ошибках системы Heatlh и наследников
class HealthSystemException : Exception
{
    public HealthSystemException(string message) : base(message){}
}
