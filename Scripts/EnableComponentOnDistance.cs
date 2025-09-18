// !!!ID: 74e4143f046149fc80f01099531e1be2
using UnityEngine;

/// <summary>
/// Enables or disables a specified component based on the distance to another Transform.
/// Can trigger once or continuously loop depending on the 'loop' setting.
/// </summary>
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
