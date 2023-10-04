using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreyBehavior : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public float attackDamage = 10f; // Adjust as needed
    public float nutritionValue = 50.0f;  // Added this line

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void OnCollisionEnter(Collision collision)
    {
        PredatorBehavior predator = collision.gameObject.GetComponent<PredatorBehavior>();
        if (predator != null)
        {
            // Perform predation
            PredatorPreyInteraction(predator);
        }
    }

    public float GetNutritionValue()
    {
        return nutritionValue;
    }

    public void GetConsumed()
    {
        // Handle the prey being eaten.
        Debug.Log($"{name} has been eaten!");
        Die();
    }

    private void PredatorPreyInteraction(PredatorBehavior predator)
    {
        FishBehavior fishBehavior = GetComponent<FishBehavior>();
        if (fishBehavior != null)
        {
            // Handle prey-predator interaction through the FishBehavior
            fishBehavior.Predation(this);
        }
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
        // Destroy the game object when health reaches 0
        Destroy(gameObject);
    }
}
