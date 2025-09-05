using UnityEngine;

namespace Basics {
	public class EnableComponentOnDistance : MonoBehaviour {
		public Behaviour componentToEnable;
		public Transform other;
		public float distance = 1f;
		public bool loop = false;

		private bool triggered;

		void Start() {
			if (componentToEnable != null)
				componentToEnable.enabled = false;
		}

		void Update() {
			if (componentToEnable == null || other == null) return;

			float d = Vector3.Distance(transform.position, other.position);
			if (d <= distance && (!triggered || loop)) {
				componentToEnable.enabled = true;
				triggered = true;
				if (!loop) enabled = false;
			}
			else if (loop && d > distance) {
				componentToEnable.enabled = false;
				triggered = false;
			}
		}
	}
}
