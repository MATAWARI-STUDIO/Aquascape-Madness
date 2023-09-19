using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioClip kachingSound;
    public AudioClip insufficientFundsSound;
    public AudioClip clickSoundClip;
    public AudioClip hoverSoundClip;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
    }

    public void PlayKachingSound()
    {
        audioSource.PlayOneShot(kachingSound);
    }

    public void PlayInsufficientFundsSound()
    {
        audioSource.PlayOneShot(insufficientFundsSound);
    }

    public void PlayClickSound()
    {
        audioSource.PlayOneShot(clickSoundClip);
    }

    public void PlayHoverSound()
    {
        audioSource.PlayOneShot(hoverSoundClip);
    }
}
