using UnityEngine;

public class PredatorPreyManager : MonoBehaviour
{
    public float predatorPopulation;
    public float preyPopulation;

    public void SimulateInteractions()
    {
        // Simple Lotka-Volterra model for predator-prey interactions
        float birthRate = 0.1f;
        float deathRate = 0.01f;
        float predationRate = 0.001f;
        float predatorEfficiency = 0.01f;

        preyPopulation += (birthRate * preyPopulation) - (predationRate * predatorPopulation * preyPopulation);
        predatorPopulation += (predatorEfficiency * predationRate * predatorPopulation * preyPopulation) - (deathRate * predatorPopulation);

        // Clamp populations to non-negative values
        preyPopulation = Mathf.Max(0, preyPopulation);
        predatorPopulation = Mathf.Max(0, predatorPopulation);
    }
}
