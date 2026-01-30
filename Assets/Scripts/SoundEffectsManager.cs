using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent (typeof(AudioSource))]
public class SoundEffectsManager : MonoBehaviour
{
    public static SoundEffectsManager instance;

    [SerializeField] private AudioSource audioSource;

    [SerializeField] private float pitchMin = 0.9f;
    [SerializeField] private float pitchMax = 1.1f;

    private void Awake() {

        if (instance == null) {
            instance = this;
        }

        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudioClip(AudioClip sound) {

        audioSource.pitch = Random.Range(pitchMin, pitchMax);
        audioSource.PlayOneShot(sound);
    }
}
