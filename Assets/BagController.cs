using UnityEngine;

public class BagController : MonoBehaviour
{
    public Transform tankBounds;
    public Camera mainCamera;
    public float rotationSpeed = 60f;
    public float minRotation = 0f;
    public float maxRotation = 45f;
    public float lerpSpeed = 5f;
    public float elevationSpeed = 0.1f;
    public float maxElevationAboveInitial = 2f;
    public ParticleSystem substrateEmitter;
    public SubstrateAccumulator substrateAccumulator;
    public float minPouringAngle = 20f;
    public float maxPouringAngle = 90f;
    public AudioSource pourSound;

    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private float initialElevation;
    private bool isPouring = false;
    private float accumulatedXRotation;
    private float targetVolume = 0f;
    private float fadeSpeed = 5f;
    public float soilAmount = 100.0f;

    private void Start()
    {
        pourSound = GetComponent<AudioSource>();
        pourSound.playOnAwake = false;  // Disable play on awake

        if (pourSound == null)
        {
            Debug.LogError("AudioSource not found!");
        }

        targetPosition = transform.position;
        targetRotation = transform.rotation;
        initialElevation = transform.position.y;
        substrateEmitter.Stop();
        accumulatedXRotation = transform.rotation.eulerAngles.x;
        pourSound = GetComponent<AudioSource>();

        if (pourSound == null)
        {
            Debug.LogError("AudioSource not found!");
        }
    }

    private void Update()
    {
        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
        float newElevation = targetPosition.y - scrollDelta * elevationSpeed;
        newElevation = Mathf.Clamp(newElevation, initialElevation, initialElevation + maxElevationAboveInitial);
        targetPosition.y = newElevation;

        float rotationChangeX = Input.GetKey(KeyCode.W) ? -rotationSpeed : (Input.GetKey(KeyCode.S) ? rotationSpeed : 0f);
        accumulatedXRotation += rotationChangeX * Time.deltaTime;
        accumulatedXRotation = Mathf.Clamp(accumulatedXRotation, minRotation, maxRotation);

        float rotationChangeY = Input.GetKey(KeyCode.A) ? -rotationSpeed : (Input.GetKey(KeyCode.D) ? rotationSpeed : 0f);
        float accumulatedYRotation = transform.rotation.eulerAngles.y + rotationChangeY * Time.deltaTime;

        targetRotation = Quaternion.Euler(accumulatedXRotation, accumulatedYRotation, targetRotation.eulerAngles.z);

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            if (hit.collider.transform == tankBounds)
            {
                targetPosition = new Vector3(hit.point.x, targetPosition.y, hit.point.z);
            }
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * lerpSpeed);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        if (accumulatedXRotation > minPouringAngle && accumulatedXRotation < maxPouringAngle)
        {
            if (!isPouring)
            {
                substrateEmitter.Play();
                isPouring = true;
                targetVolume = 1f; // Set target volume to max
            }
        }
        else
        {
            if (isPouring)
            {
                substrateEmitter.Stop();
                isPouring = false;
                targetVolume = 0f; // Set target volume to zero
            }
        }

        if (accumulatedXRotation > minPouringAngle && accumulatedXRotation < maxPouringAngle && soilAmount > 0)
        {
            if (!isPouring)
            {
                substrateEmitter.Play();
                isPouring = true;
                targetVolume = 1f; // Set target volume to max
            }

            // Decrease the remaining soil
            soilAmount -= Time.deltaTime;  // Modify this line according to how fast you want the soil to decrease
            if (soilAmount < 0)
            {
                soilAmount = 0;
                substrateEmitter.Stop();
                isPouring = false;
                targetVolume = 0f; // Set target volume to zero
            }
        }
        else
        {
            if (isPouring)
            {
                substrateEmitter.Stop();
                isPouring = false;
                targetVolume = 0f; // Set target volume to zero
            }
        }

        // Fade volume
        if (pourSound != null)
        {
            pourSound.volume = Mathf.Lerp(pourSound.volume, targetVolume, Time.deltaTime * fadeSpeed);

            // Start playing the sound if it's not already playing
            if (pourSound.volume > 0.01f && !pourSound.isPlaying)
            {
                pourSound.Play();
            }
            // Stop playing the sound if it has faded out
            else if (pourSound.volume <= 0.01f && pourSound.isPlaying)
            {
                pourSound.Stop();
            }
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("Particle Collision Detected");

        if (other.CompareTag("Base"))
        {
            Debug.Log("Particle collided with Base");
            Vector3 collisionPoint = other.transform.position;
            substrateAccumulator.Accumulate(collisionPoint);
        }
    }
}
