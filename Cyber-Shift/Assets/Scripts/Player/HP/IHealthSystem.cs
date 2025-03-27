using UnityEngine;

public interface IHealthSystem
{
    void TakeDamage(int damage);
    void Heal(int amount);
    int GetHealth();
    int GetMaxHealth();
}