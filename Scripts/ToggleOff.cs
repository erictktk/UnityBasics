using UnityEngine;


namespace Basics {
    public class ToggleOff : MonoBehaviour {
        public Component targetComponent;
        public float delay = 1f;

        void OnEnable() {
            if (targetComponent != null)
                Invoke(nameof(DisableComponent), delay);
        }

        void DisableComponent() {
            if (targetComponent != null)
                targetComponent.GetType().GetProperty("enabled")?.SetValue(targetComponent, false, null);
        }
    }
}

