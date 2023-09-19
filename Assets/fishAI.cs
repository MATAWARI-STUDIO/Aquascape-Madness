using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAI : MonoBehaviour
{
    public Transform tankCenterGoal;
    public float obstacleSensingDistance = 0.3f;
    public float swimSpeedMin = 0.2f;
    public float swimSpeedMax = 0.6f;
    public float maxTurnRateY = 5f;
    public float maxWanderAngle = 45f;
    public float wanderPeriodDuration = 0.8f;
    public float wanderProbability = 0.15f;

    [HideInInspector]
    public float swimSpeed;

    private Vector3 swimDirection
    {
        get { return transform.TransformDirection(Vector3.forward); }
    }

    private bool obstacleDetected = false;
    private float wanderPeriodStartTime;
    private Quaternion goalLookRotation;
    private Transform bodyTransform;
    private float randomOffset;

    void Start()
    {
        if (tankCenterGoal == null)
        {
            Debug.LogError("The tankCenterGoal parameter is required but is null.");
            UnityEditor.EditorApplication.isPlaying = false;
        }

        bodyTransform = transform.Find("Body");
        randomOffset = Random.value;
    }

    private void Update()
    {
        Wiggle();
        Wander();
        AvoidObstacles();

        // Slow down the fish if it's near the tank boundary
        float distanceToCenter = Vector3.Distance(transform.position, tankCenterGoal.position);
        float tankRadius = 5.0f;  // Replace with the actual radius of your tank
        if (distanceToCenter > tankRadius * 0.8f)
        {
            swimSpeed *= 0.8f;
        }

        UpdatePosition();
    }

    void Wiggle()
    {
        float speedPercent = swimSpeed / swimSpeedMax;
        float minWiggleSpeed = 12f;
        float maxWiggleSpeed = minWiggleSpeed + 1f;
        float wiggleSpeed = Mathf.Lerp(minWiggleSpeed, maxWiggleSpeed, speedPercent);
        float angle = Mathf.Sin(Time.time * wiggleSpeed) * 5f;
        var wiggleRotation = Quaternion.AngleAxis(angle, Vector3.up);
        bodyTransform.localRotation = wiggleRotation;
    }

    void Wander()
    {
        float noiseScale = .5f;
        float speedPercent = Mathf.PerlinNoise(Time.time * noiseScale + randomOffset, randomOffset);
        speedPercent = Mathf.Pow(speedPercent, 2);
        swimSpeed = Mathf.Lerp(swimSpeedMin, swimSpeedMax, speedPercent);

        if (obstacleDetected) return;

        if (Time.time > wanderPeriodStartTime + wanderPeriodDuration)
        {
            wanderPeriodStartTime = Time.time;

            if (Random.value < wanderProbability)
            {
                var randomAngle = Random.Range(-maxWanderAngle, maxWanderAngle);
                var relativeWanderRotation = Quaternion.AngleAxis(randomAngle, Vector3.up);
                goalLookRotation = transform.rotation * relativeWanderRotation;
            }
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, goalLookRotation, Time.deltaTime / 2f);
    }

    void AvoidObstacles()
    {
        RaycastHit hit;
        obstacleDetected = Physics.Raycast(transform.position, swimDirection, out hit, obstacleSensingDistance);

        if (obstacleDetected)
        {
            Vector3 reflectionVector = Vector3.Reflect(swimDirection, hit.normal);
            float goalPointMinDistanceFromHit = 1f;
            Vector3 reflectedPoint = hit.point + reflectionVector * Mathf.Max(hit.distance, goalPointMinDistanceFromHit);
            Vector3 goalPoint = (reflectedPoint + tankCenterGoal.position) / 2f;
            Vector3 goalDirection = goalPoint - transform.position;
            goalLookRotation = Quaternion.LookRotation(goalDirection);
            float dangerLevel = Mathf.Pow(1 - (hit.distance / obstacleSensingDistance), 4f);
            dangerLevel = Mathf.Max(0.01f, dangerLevel);
            float turnRate = maxTurnRateY * dangerLevel;
            Quaternion rotation = Quaternion.Slerp(transform.rotation, goalLookRotation, Time.deltaTime * turnRate);
            transform.rotation = rotation;
        }
    }

    private void UpdatePosition()
    {
        Vector3 position = transform.position + swimDirection * swimSpeed * Time.fixedDeltaTime;
        transform.position = position;
    }
}
