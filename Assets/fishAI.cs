using UnityEngine;

public class FishAI : MonoBehaviour
{
    public Transform tankCenterGoal;
    public GameObject tankBoundsObject;
    public float obstacleSensingDistance = 0.3f;
    public float swimSpeedMin = 0.2f;
    public float swimSpeedMax = 0.6f;
    public float maxTurnRateY = 5f;
    public float maxWanderAngle = 45f;
    public float wanderPeriodDuration = 0.8f;
    public float wanderProbability = 0.15f;
    public LayerMask fishLayer;
    public bool isPredator;
    public float chaseSpeed = 1.0f;
    public float maxChaseDistance = 10.0f;
    [HideInInspector]
    public float swimSpeed;

    
    private bool nearBounds = false;
    private bool obstacleDetected = false;
    private bool isFleeing = false;
    private float wanderPeriodStartTime;
    private Quaternion goalLookRotation;
    private Transform bodyTransform;
    private float randomOffset;
    private Transform targetFish;
    private BoxCollider tankBounds;
    private float boundaryThreshold = 1.0f;
    private bool slowingDown = false; // To track if fish is slowing down
    private float slowDownEndTime = 0f; // Time when the fish can resume normal speed
    private float slowDownDuration = 3f; // Duration for which fish swims slowly

    private Vector3 _swimDirection;

    void Start()
    {
        if (tankCenterGoal == null)
        {
            Debug.LogError("The tankCenterGoal parameter is required but is null.");
            return;
        }

        bodyTransform = transform.Find("Body");
        randomOffset = Random.value;

        if (tankBoundsObject)
        {
            tankBounds = tankBoundsObject.GetComponent<BoxCollider>();
            if (!tankBounds)
            {
                Debug.LogError("The tankBoundsObject does not have a BoxCollider.");
            }
        }
        else
        {
            Debug.LogError("Please assign a GameObject with a BoxCollider to tankBoundsObject in the inspector.");
        }
    }

    private void Update()
    {
        Wiggle();

        if (isPredator && targetFish)
        {
            float distanceToTarget = Vector3.Distance(transform.position, targetFish.position);
            if (distanceToTarget <= maxChaseDistance)
            {
                Chase(targetFish);
            }
            else
            {
                targetFish = null;
            }
        }
        else if (isFleeing)
        {
            Flee();
        }
        else
        {
            Wander();
            AvoidObstacles();
        }

        RestrictToTankBounds();
        UpdatePosition();
        _swimDirection = transform.forward;
    }

    public void SetFleeState(bool state)
    {
        isFleeing = state;
    }

    void Flee()
    {
        if (targetFish)
        {
            Vector3 fleeDirection = (transform.position - targetFish.position).normalized;
            _swimDirection = fleeDirection;
        }
          
        swimSpeed = swimSpeedMax;
        transform.position += _swimDirection * swimSpeed * Time.deltaTime;
    }

    void Wiggle()
    {
        float speedPercent = swimSpeed / swimSpeedMax;
        float minWiggleSpeed = 12f;
        float maxWiggleSpeed = minWiggleSpeed + 1f;
        float wiggleSpeed = Mathf.Lerp(minWiggleSpeed, maxWiggleSpeed, speedPercent);
        float angle = Mathf.Sin((Time.time + randomOffset) * wiggleSpeed) * 5f;
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

        float rotationRate = nearBounds ? maxTurnRateY : Time.deltaTime / 2f;
        transform.rotation = Quaternion.Slerp(transform.rotation, goalLookRotation, rotationRate);
    }

    void AvoidObstacles()
    {
        RaycastHit hit;
        obstacleDetected = Physics.Raycast(transform.position, transform.forward, out hit, obstacleSensingDistance);

        if (obstacleDetected)
        {
            Vector3 reflectionVector = Vector3.Reflect(transform.forward, hit.normal);
            float goalPointMinDistanceFromHit = 1f;
            Vector3 reflectedPoint = hit.point + reflectionVector * Mathf.Max(hit.distance, goalPointMinDistanceFromHit);
            Vector3 goalDirection = reflectedPoint - transform.position + Random.insideUnitSphere * 2f;
            goalLookRotation = Quaternion.LookRotation(goalDirection);
            float dangerLevel = Mathf.Pow(1 - (hit.distance / obstacleSensingDistance), 4f);
            dangerLevel = Mathf.Max(0.01f, dangerLevel);
            float turnRate = maxTurnRateY * dangerLevel;
            Quaternion rotation = Quaternion.Slerp(transform.rotation, goalLookRotation, Time.deltaTime * turnRate);
            transform.rotation = rotation;
        }
    }

    private void RestrictToTankBounds()
    {
        if (tankBounds)
        {
            Vector3 closestPoint = tankBounds.ClosestPoint(transform.position);
            float distanceToBoundary = Vector3.Distance(transform.position, closestPoint);

            if (distanceToBoundary < boundaryThreshold)
            {
                nearBounds = true;

                // Push away from the closest boundary point
                Vector3 redirection = (transform.position - closestPoint).normalized;
                _swimDirection = Vector3.Lerp(_swimDirection, redirection, 0.2f);
            }
            else
            {
                nearBounds = false;
            }
        }
    }

    private void UpdatePosition()
    {
        Vector3 nextPosition = transform.position + _swimDirection * swimSpeed * Time.deltaTime;

        if (tankBounds)
        {
            // If the fish is near the bounds, adjust the swim direction
            if (IsNearBounds(nextPosition))
            {
                AdjustSwimDirectionAwayFromBounds();
            }

            // Adjust the position if it's outside the bounds
            nextPosition = AdjustPositionToStayInsideBounds(nextPosition);
        }

        transform.position = nextPosition;
    }

    private void AdjustSwimDirectionAwayFromBounds()
    {
        Vector3 toCenter = (tankCenterGoal.position - transform.position).normalized;
        _swimDirection = Vector3.Lerp(_swimDirection, toCenter, 0.3f);
    }

    private Vector3 AdjustPositionToStayInsideBounds(Vector3 position)
    {
        Vector3 localPos = tankBounds.transform.InverseTransformPoint(position) - tankBounds.center;

        float halfX = tankBounds.size.x * 0.5f;
        float halfY = tankBounds.size.y * 0.5f;
        float halfZ = tankBounds.size.z * 0.5f;

        localPos.x = Mathf.Clamp(localPos.x, -halfX, halfX);
        localPos.y = Mathf.Clamp(localPos.y, -halfY, halfY);
        localPos.z = Mathf.Clamp(localPos.z, -halfZ, halfZ);

        return tankBounds.transform.TransformPoint(localPos + tankBounds.center);
    }


    private bool IsNearBounds(Vector3 position)
    {
        Vector3 localPos = tankBounds.transform.InverseTransformPoint(position) - tankBounds.center;
        localPos = new Vector3(Mathf.Abs(localPos.x), Mathf.Abs(localPos.y), Mathf.Abs(localPos.z));

        float threshold = 0.1f;
        if (localPos.x > tankBounds.size.x * 0.5f - threshold || localPos.y > tankBounds.size.y * 0.5f - threshold || localPos.z > tankBounds.size.z * 0.5f - threshold)
            return true;

        return false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & fishLayer) != 0)
        {
            targetFish = null;
        }
    }

    void Chase(Transform target)
    {
        Vector3 chaseDirection = (target.position - transform.position).normalized;
        if (Vector3.Distance(transform.position, target.position) < 2f)
        {
            swimSpeed = chaseSpeed * 2;
        }
        else
        {
            swimSpeed = chaseSpeed;
        }
        transform.position += chaseDirection * swimSpeed * Time.deltaTime;
        transform.LookAt(target);
    }

    void FleeFrom(Vector3 dangerPosition)
    {
        Vector3 fleeDirection = (transform.position - dangerPosition).normalized;
        _swimDirection = fleeDirection;
        swimSpeed = chaseSpeed;
        transform.position += fleeDirection * swimSpeed * Time.deltaTime;
    }
}
