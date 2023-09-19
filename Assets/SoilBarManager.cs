using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SoilBarManager : MonoBehaviour
{
    public Image soilBar;
    public BagController bagController;
    public ParticleSystem smokeEffect;
    public AudioSource audioSource;
    public AudioClip smokeSound;
    private Camera _cam;
    private bool isDestroyed = false;

    private void Start()
    {
        _cam = Camera.main;
        if (smokeEffect != null)
        {
            smokeEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            var main = smokeEffect.main;
            main.stopAction = ParticleSystemStopAction.Callback;
        }
    }

    private void LateUpdate()
    {
        if (isDestroyed) return;

        if (bagController.soilAmount <= 0)
        {
            isDestroyed = true;

            if (smokeEffect != null)
            {
                smokeEffect.gameObject.SetActive(true);
                smokeEffect.transform.position = bagController.transform.position;
                smokeEffect.Play();

                // Deactivate after playing
                smokeEffect.gameObject.SetActive(false);

                // Reactivate for future use
                smokeEffect.gameObject.SetActive(true);
            }

            if (audioSource != null && smokeSound != null)
            {
                audioSource.PlayOneShot(smokeSound);
            }

            Destroy(bagController.gameObject);
            return;
        }

        float fillAmount = (bagController.soilAmount >= 30) ? 1.0f : bagController.soilAmount / 30.0f;
        soilBar.fillAmount = fillAmount;

        Vector3 bagPosition = bagController.transform.position;
        soilBar.transform.position = bagPosition + new Vector3(0, 0.15f, -0.1f);

        Vector3 toCamera = _cam.transform.position - soilBar.transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(-toCamera, _cam.transform.up);
        soilBar.transform.rotation = lookRotation;
    }
}
