using UnityEngine;
using Unity.Netcode;

/// <summary>
/// Inherit from, or add this as a component, to create an object that can take damage.
/// </summary>
public class Attackable : NetworkBehaviour
{
    [SerializeField] protected float maxHealth = 100;

    /// <summary>
    /// Don't modify this directly, use Hit(), TakeDamage(), or RestoreHealth()
    /// </summary>
    protected float currentHealth;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
    }

    /// <summary>
    /// The health of this attackable will be reduced by the amount passed here,
    /// and DIE if it reaches 0.
    /// </summary>
    /// <param name="damage"></param>
    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            OnDeath();
        }
    }

    /// <summary>
    /// The health of this attackable will be increased by the amount passed here,
    /// and cannot be increased to more than maxHealth.
    /// </summary>
    /// <param name="damage"></param>
    public virtual void RestoreHealth(float health)
    {
        currentHealth += health;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    /// <summary>
    /// Override this to create specific behaviour for how to handle all the damage types when hit by a Hazard.
    /// </summary>
    /// <param name="damage"></param>
    public virtual void Hit(Hazard damage)
    {
        // Just reacts the same to all types of damage to begin with...
        TakeDamage(damage.impact);
        TakeDamage(damage.temperature);
        TakeDamage(damage.cut);
        TakeDamage(damage.suffocation);
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
