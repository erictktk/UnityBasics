using UnityEngine;

public class PitchModulator : MonoBehaviour {
    public AudioSource audioSource;
    public AnimationCurve pitchCurve = AnimationCurve.EaseInOut(0, 1, 1, 1);
    public float cycleDuration = 2f;
    public float scale = 1f;

    void Start() {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        if (!audioSource.isPlaying) audioSource.Play();
        audioSource.loop = true;
    }

    void Update() {
        float t = (Time.time % cycleDuration) / cycleDuration;
        float basePitch = pitchCurve.Evaluate(t);
        audioSource.pitch = 1f + basePitch * scale;
    }
}

