using UnityEngine;
using UnityEngine.UI;
using System.Reflection;


// Summary:
// HueRotateUI cycles the hue of a UI element’s color over time.
// - Supports two modes:
//   1. Graphic: Directly modifies a UnityEngine.UI.Graphic component’s color.
//   2. Reflection: Uses reflection to modify a specified Color property or field on another component.
// - Fields include configurable delay, speed, and the reflection field/property name.
// - Rotates hue continuously in Update() after the delay has elapsed.

namespace Basics {
    public class HueRotateUI : MonoBehaviour {
        public enum TargetMode { Graphic, Reflection }

        public float delay = 2f;
        public TargetMode mode = TargetMode.Graphic;
        public string reflectionFieldName = "color";
        public float speed = .1f;

        public Graphic targetGraphic;
        public Component reflectionTarget;

        private PropertyInfo propInfo;
        private FieldInfo fieldInfo;
        private float elapsed;

        void Start() {
            if (mode == TargetMode.Graphic && targetGraphic == null)
                targetGraphic = GetComponent<Graphic>();

            if (mode == TargetMode.Reflection && reflectionTarget == null)
                reflectionTarget = GetComponent<Component>();

            if (mode == TargetMode.Reflection && reflectionTarget != null) {
                var type = reflectionTarget.GetType();
                propInfo = type.GetProperty(reflectionFieldName);
                fieldInfo = type.GetField(reflectionFieldName);
            }
        }

        void Update() {
            elapsed += Time.deltaTime;
            if (elapsed < delay) return;

            if (mode == TargetMode.Graphic) {
                if (targetGraphic == null) return;
                Color col = targetGraphic.color;
                RotateColor(ref col);
                targetGraphic.color = col;
            }
            else if (mode == TargetMode.Reflection) {
                if (reflectionTarget == null) return;

                Color col = default;
                if (propInfo != null && propInfo.PropertyType == typeof(Color))
                    col = (Color)propInfo.GetValue(reflectionTarget);
                else if (fieldInfo != null && fieldInfo.FieldType == typeof(Color))
                    col = (Color)fieldInfo.GetValue(reflectionTarget);
                else
                    return;

                RotateColor(ref col);

                if (propInfo != null && propInfo.CanWrite)
                    propInfo.SetValue(reflectionTarget, col);
                else if (fieldInfo != null)
                    fieldInfo.SetValue(reflectionTarget, col);
            }
        }

        void RotateColor(ref Color col) {
            Color.RGBToHSV(col, out float h, out float s, out float v);
            h = (h + speed * Time.deltaTime) % 1f;
            if (h < 0) h += 1f;
            col = Color.HSVToRGB(h, s, v);
        }
    }
}
