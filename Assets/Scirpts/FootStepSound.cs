using UnityEngine;

public class FootStepSound : MonoBehaviour
{
    private AudioSource audioSource;
    [Header("FootStep Sound")]
    public AudioClip[] footStepClips;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Step()
    {
        audioSource.PlayOneShot(footStepClips[Random.Range(0, footStepClips.Length)]);
    }
}
