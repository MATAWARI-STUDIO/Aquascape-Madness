using UnityEngine;

public class PredatorBehavior : MonoBehaviour
{
    public float maxHealth = 150f;
    public float currentHealth;
    public float attackDamage = 20f;
    public float nutritionValue = 100.0f;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void OnCollisionEnter(Collision collision)
    {
        PreyBehavior prey = collision.gameObject.GetComponent<PreyBehavior>();
        if (prey != null)
        {
            AttackPrey(prey);
        }
    }

    private void AttackPrey(PreyBehavior prey)
    {
        float attackChance = (currentHealth / maxHealth) * 0.5f + 0.5f;  // Healthier predator has a better chance
        if (Random.value < attackChance)
        {
            prey.TakeDamage(attackDamage);
            if (prey.currentHealth <= 0)
            {
                EatPrey(prey);  // Changed the method name to better reflect the action
            }
        }
    }

    public void EatPrey(PreyBehavior prey)  // Changed the method name and parameter type
    {
        nutritionValue += prey.GetNutritionValue();  // Using the GetNutritionValue method from PreyBehavior
        Debug.Log($"{name} ate {prey.name}!");
        prey.GetConsumed();
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
