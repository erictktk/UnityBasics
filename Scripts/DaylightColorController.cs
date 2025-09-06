// !!!ID: 8941f8ed6f54414a9c0d36580feb0dae
using UnityEngine;


namespace Basics {
    public class DaylightColorController : MonoBehaviour {
        public enum TargetMode { SpriteRenderer, Material }
        public TargetMode targetMode;

        public SpriteRenderer spriteRenderer;
        public Material targetMaterial;

        [Range(0f, 1f)] public float timeOfDay = 0f;
        public Gradient dayColorGradient;

        void Update() {
            Color newColor = dayColorGradient.Evaluate(timeOfDay);

            switch (targetMode) {
                case TargetMode.SpriteRenderer:
                    if (spriteRenderer != null)
                        spriteRenderer.color = newColor;
                    break;

                case TargetMode.Material:
                    if (targetMaterial != null)
                        targetMaterial.SetColor("_Color", newColor);
                    break;
            }
        }
    }
}
