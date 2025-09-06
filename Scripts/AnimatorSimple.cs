// !!!ID: 760693e2d4264f3ebcc4b4736794dffc
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Cycles through a list of sprites, either on a constant timer or custom per-frame timings.
/// Supports pause/resume. Intended for use with Image or SpriteRenderer.
/// </summary>
namespace Basic {
    public class AnimatorSimple : MonoBehaviour {
        public enum TimingMode { Constant, Custom }
        public enum AnimationType { Image, SpriteRenderer }
        public enum OffsetMode { None, Manual, Random }
        public enum TimeOffset { None, Manual, Random }

        [Tooltip("Animation type: Image or SpriteRenderer.")]
        public AnimationType animationType = AnimationType.SpriteRenderer;

        [Tooltip("Timing mode: use a single constant time or a list of custom times per sprite.")]
        public TimingMode timingMode = TimingMode.Constant;

        [Tooltip("Offset mode: None, Manual (set offset), or Random (randomize offset).")]
        public OffsetMode offsetMode = OffsetMode.None;


        public TimeOffset timeOffsetMode = TimeOffset.None;
        public float timeOffset = 0f;

        [Tooltip("List of sprites to animate through.")]
        public List<Sprite> sprites = new List<Sprite>();

        [Tooltip("If using Constant timing mode, this is the time per sprite frame.")]
        public float constantTime = 0.2f;

        [Tooltip("If using Custom timing mode, these times define how long each sprite frame stays.")]
        public List<float> customTimes = new List<float>();

        [Range(0f, 3f)]
        [Tooltip("Multiplier applied to all custom times.")]
        public float customTimeMultiplier = 1f;

        [Tooltip("Toggle to pause/resume the sprite animation.")]
        public bool paused = false;

        private Image image;
        private SpriteRenderer spriteRenderer;
        private int index;
        private float timer;

        void Awake() {
            if (animationType == AnimationType.SpriteRenderer) {
                spriteRenderer = GetComponent<SpriteRenderer>();
            }
            else if (animationType == AnimationType.Image) {
                image = GetComponent<Image>();
            }

            int offset = 0;
            if (offsetMode == OffsetMode.Manual) {
                offset = Mathf.Clamp(index, 0, sprites.Count - 1);
            }
            else if (offsetMode == OffsetMode.Random) {
                offset = Random.Range(0, sprites.Count);
            }

            if (animationType == AnimationType.SpriteRenderer)
                spriteRenderer.sprite = sprites[offset];
            else if (animationType == AnimationType.Image)
                image.sprite = sprites[offset];

            index = offset;
            timeOffset = 0f;

            if (timeOffsetMode == TimeOffset.Manual) {
                // implement later if needed
            }
            else if (timeOffsetMode == TimeOffset.Random) {
                timeOffset = Random.Range(0f, constantTime);
            }

            if (timingMode == TimingMode.Constant)
                timer = constantTime + timeOffset;
            else if (timingMode == TimingMode.Custom && customTimes.Count > 0)
                timer = timeOffset + customTimes[0];
        }

        void Update() {
            if (paused || sprites.Count == 0) return;

            timer -= Time.deltaTime;



            if (timer <= 0f) {
                index = (index + 1) % sprites.Count;

                if (animationType == AnimationType.SpriteRenderer)
                    spriteRenderer.sprite = sprites[index];
                else if (animationType == AnimationType.Image)
                    image.sprite = sprites[index];

                float currentTime = timingMode == TimingMode.Constant
                    ? constantTime
                    : customTimes[Mathf.Clamp(index, 0, customTimes.Count - 1)] * customTimeMultiplier;

                timer = currentTime;
            }
        }
    }
}
