using UnityEngine;

namespace Basics {
    [ExecuteAlways]
    public class HSLDriver : MonoBehaviour {
        public Material material;
        public Color tint = Color.white;
        public float hueShift = 0f;
        public float saturation = 1f;
        public float luminance = 1f;

        void Update() {
            if (material == null) return;
            material.SetColor("_Tint", tint);
            material.SetFloat("_HueShift", hueShift);
            material.SetFloat("_Saturation", saturation);
            material.SetFloat("_Luminance", luminance);
        }
    }
}

