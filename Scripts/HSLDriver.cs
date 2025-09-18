// !!!ID: d476d719d87046e29f93a60820ed1d9d
using UnityEngine;


/// <summary>
/// Drives HSL shader properties on a material in real time  
/// Updates tint color, hue shift, saturation, and luminance values  
/// Executes both in edit mode and play mode  
/// </summary>

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

