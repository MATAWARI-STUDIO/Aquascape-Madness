using UnityEngine;

public class PreyBehavior : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public float attackDamage = 10f;
    public float nutritionValue = 50.0f;
    private FishAI fishAI; // Reference to the FishAI script

    private void Start()
    {
        currentHealth = maxHealth;
        fishAI = GetComponent<FishAI>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        PredatorBehavior predator = collision.gameObject.GetComponent<PredatorBehavior>();
        if (predator != null)
        {
            if (fishAI)
            {
                fishAI.SetFleeState(true); // Trigger flee behavior
            }
            FleeOrCounterAttack(predator);
        }
    }

    private void FleeOrCounterAttack(PredatorBehavior predator)
    {
        if (currentHealth < maxHealth * 0.5f)
        {
            // Fleeing behavior is handled in FishAI
        }
        else
        {
            predator.TakeDamage(attackDamage * 0.5f);
        }
    }

    public float GetNutritionValue()
    {
        return nutritionValue;
    }

    public void GetConsumed()
    {
        Debug.Log($"{name} has been eaten!");
        Die();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
