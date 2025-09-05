using UnityEngine;
using UnityEngine.UI;

namespace Basics {
    public class ColorChannelSetter : MonoBehaviour {
        [Header("Defaults to SpriteRenderer or Image component on this GameObject")]
        public Component target; // SpriteRenderer or Image

        public bool doR = false;
        public bool doG = false;
        public bool doB = false;
        public bool doA = true;

        private float rInit;
        private float gInit;
        private float bInit;
        private float aInit;


        [Range(0f, 1f)] public float r = 1f;
        [Range(0f, 1f)] public float g = 1f;
        [Range(0f, 1f)] public float b = 1f;
        [Range(0f, 1f)] public float a = 1f;

        private void Start() {
            /*
            if (!doR){
                rInit = 
            }*/

            if (target == null) {
                target = GetComponent<SpriteRenderer>();
                if (target == null) {
                    target = GetComponent<Image>();
                }
            }

            GetInitial();
        }

        void GetInitial() {
            if (target is SpriteRenderer sr) {
                rInit = sr.color.r;
                gInit = sr.color.g;
                bInit = sr.color.b;
                aInit = sr.color.a;
            }
            else if (target is Image img) {
                rInit = img.color.r;
                gInit = img.color.g;
                bInit = img.color.b;
                aInit = img.color.a;
            }
        }

        void Update() {
            if (target is SpriteRenderer sr) {
                //sr.color = new Color(r, g, b, a);

                float actualR = doR ? r : rInit;
                float actualG = doG ? g : gInit;
                float actualB = doB ? b : bInit;
                float actualA = doA ? a : aInit;
                sr.color = new Color(actualR, actualG, actualB, actualA);
            }
            else if (target is Image img) {
                float actualR = doR ? r : rInit;
                float actualG = doG ? g : gInit;
                float actualB = doB ? b : bInit;
                float actualA = doA ? a : aInit;

                //img.color = new Color(r, g, b, a);
                img.color = new Color(actualR, actualG, actualB, actualA);
            }
        }
    }
}
