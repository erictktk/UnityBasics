// !!!ID: 25501a128c374c029c1aa6b34c81c105
using UnityEngine;

namespace Basics {
    public class ToggleOn : MonoBehaviour {
        public Component targetComponent;
        public float delay = 1f;

        void OnEnable() {
            if (targetComponent != null)
                Invoke(nameof(EnableComponent), delay);
        }

        void EnableComponent() {
            if (targetComponent != null)
                targetComponent.GetType().GetProperty("enabled")?.SetValue(targetComponent, true, null);
        }
    }
}
