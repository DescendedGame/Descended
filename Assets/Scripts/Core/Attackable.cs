using UnityEngine;

public class Attackable : MonoBehaviour
{
    [SerializeField] protected float maxHealth = 100;
    protected float currentHealth;  // "protected" så att ärvande klasser kan komma åt det

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        //Debug.Log(gameObject.name + " took " + damage + " damage. Remaining health: " + currentHealth);

        if (currentHealth <= 0)
        {
            OnDeath();
        }
    }

    public virtual void Hit(Hazard damage)
    {
        TakeDamage(damage.impact);
        TakeDamage(damage.temperature);
        TakeDamage(damage.cut);
        TakeDamage(damage.suffocation);
        // Lägg till effekter här (ex. blinkande sprite, ljud)
    }

    public virtual void OnDeath()
    {
        Destroy(gameObject);
    }

    public float GetHealth()
    {
        return currentHealth;
    }
    public float GetMaxHealth()
    {
        return maxHealth;
    }
}
