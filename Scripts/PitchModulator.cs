// !!!ID: 929ecb3f10134c8094e4ba0671a65627
using UnityEngine;

/// <summary>
/// Continuously modulates the pitch of an AudioSource using an AnimationCurve.
/// - Ensures the AudioSource is playing and looping on Start.
/// - Uses cycleDuration to determine the time span of one curve cycle.
/// - Applies pitch offset scaled by the given factor.
/// </summary>
public class PitchModulator : MonoBehaviour
{
    public AudioSource audioSource;
    public AnimationCurve pitchCurve = AnimationCurve.EaseInOut(0, 1, 1, 1);
    public float cycleDuration = 2f;
    public float scale = 1f;

    void Start()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        if (!audioSource.isPlaying) audioSource.Play();
        audioSource.loop = true;
    }

    void Update()
    {
        float t = (Time.time % cycleDuration) / cycleDuration;
        float basePitch = pitchCurve.Evaluate(t);
        audioSource.pitch = 1f + basePitch * scale;
    }
}

