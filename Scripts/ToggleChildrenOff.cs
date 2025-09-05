using UnityEngine;

namespace Basics {
    public class ToggleChildrenOff : MonoBehaviour {
        public enum Mode { AllAtOnce, Sequential }
        public Mode mode = Mode.AllAtOnce;
        public float delayBetween = 1f;

        private Transform[] children;
        private float timer;
        private int currentIndex;

        void Start() {
            int count = transform.childCount;
            children = new Transform[count];
            for (int i = 0; i < count; i++)
                children[i] = transform.GetChild(i);
        }

        void Update() {
            if (mode == Mode.AllAtOnce) {
                foreach (var child in children)
                    child.gameObject.SetActive(false);
                enabled = false;
                return;
            }

            if (currentIndex >= children.Length) {
                enabled = false;
                return;
            }

            timer += Time.deltaTime;
            if (timer >= delayBetween) {
                children[currentIndex].gameObject.SetActive(false);
                currentIndex++;
                timer = 0f;
            }
        }
    }
}
