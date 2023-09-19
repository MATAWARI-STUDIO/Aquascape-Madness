using UnityEngine;

public class PredatorBehavior : MonoBehaviour
{
    public float maxHealth = 150f;
    public float currentHealth;
    public float attackDamage = 20f;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void OnCollisionEnter(Collision collision)
    {
        PreyBehavior prey = collision.gameObject.GetComponent<PreyBehavior>();
        if (prey != null)
        {
            // Perform predation based on randomness
            float randomValue = Random.value;
            if (randomValue < 0.5f || currentHealth == maxHealth)
            {
                PredatorPreyInteraction(prey);
            }
        }
    }

    private void PredatorPreyInteraction(PreyBehavior prey)
    {
        FishBehavior fishBehavior = GetComponent<FishBehavior>();
        if (fishBehavior != null)
        {
            // Handle predator-prey interaction through the FishBehavior
            fishBehavior.PredatorPreyInteraction(this);
        }
    }

    public void IncreaseHealth(PreyBehavior prey)
    {
        float healthGained = prey.attackDamage * 0.5f;
        currentHealth += healthGained;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}
