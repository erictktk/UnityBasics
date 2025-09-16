// !!!ID: d4e302ec6c504214b857f1f36047e17b
// GenericAnimation.cs
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Generic sprite animation component for 2D objects.
/// - Stores multiple animations, each with a name and sprite list.
/// - Supports two framerate modes:
///     - Universal: all animations use the same fps.
///     - Multiple: each animation has its own fps.
/// - Automatically cycles frames and updates the SpriteRenderer.
/// - Provides Play(name) to switch animations by name.
/// - Allows adding sprites to animations at runtime.
/// </summary>
namespace Basic
{
    [System.Serializable]
    public class SpriteListWrapper
    {
        public List<Sprite> sprites = new List<Sprite>();
    }

    public class GenericAnimation : MonoBehaviour
    {
        [Tooltip("Names for each animation")]
        public List<string> animationNames = new List<string>();

        [Tooltip("Sprite lists for each animation (must match animationNames.Count)")]
        public List<SpriteListWrapper> animationFrames = new List<SpriteListWrapper>();

        public enum FramerateMode { Universal, Multiple }
        [Tooltip("Universal: one fps for all; Multiple: each entry has its own fps")]
        public FramerateMode framerateMode = FramerateMode.Universal;

        [Tooltip("FPS used when in Universal mode")]
        public float universalFramerate = 12f;

        [Tooltip("Per-animation FPS when in Multiple mode (must match animationFrames.Count)")]
        public List<float> multipleFramerates = new List<float>();

        private SpriteRenderer sr;
        private int currentAnimIndex;
        private int currentFrame;
        private float timer;

        void OnValidate()
        {
            if (animationNames.Count != animationFrames.Count)
                Debug.LogError("animationNames.Count must match animationFrames.Count");
            if (framerateMode == FramerateMode.Multiple && multipleFramerates.Count != animationFrames.Count)
                Debug.LogError("multipleFramerates.Count must match animationFrames.Count");
        }

        void Start()
        {
            sr = GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            if (animationFrames == null || animationFrames.Count == 0) return;
            var wrapper = animationFrames[currentAnimIndex];
            if (wrapper == null || wrapper.sprites.Count == 0) return;

            float fps = framerateMode == FramerateMode.Universal
                ? universalFramerate
                : multipleFramerates[currentAnimIndex];

            timer += Time.deltaTime;
            if (timer >= 1f / fps)
            {
                timer -= 1f / fps;
                currentFrame = (currentFrame + 1) % wrapper.sprites.Count;
                sr.sprite = wrapper.sprites[currentFrame];
            }
        }

        /// 
        /// Switch to the animation with the given name.
        public void Play(string name)
        {
            int i = animationNames.IndexOf(name);
            if (i >= 0 && i < animationFrames.Count)
            {
                currentAnimIndex = i;
                currentFrame = 0;
                timer = 0f;
            }
        }


        /// Add a sprite at runtime to the specified animation index.
        public void AddSpriteToAnimation(int index, Sprite s)
        {
            if (index < 0 || index >= animationFrames.Count) return;
            if (animationFrames[index] == null)
                animationFrames[index] = new SpriteListWrapper();
            animationFrames[index].sprites.Add(s);
        }
    }
}
