using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] int maxHealth = 100;
    public float health;
    public bool hit;

    public Image healthBar;

    private void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        hit = true;
        health -= damage;
        healthBar.fillAmount = health/maxHealth;
    }
}
