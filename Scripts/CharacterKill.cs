using UnityEngine;

namespace Basics {
    public class CharacterKill : MonoBehaviour {
        public GameObject prefabToSpawn;

        public enum LayerInteraction {
            None,
            Enemy,
            Custom
        }

        public LayerInteraction layerMode = LayerInteraction.None;
        public string customLayerName = "";

        void OnTriggerEnter2D(Collider2D other) {
            if (!IsLayerAllowed(other.gameObject)) return;

            if (prefabToSpawn == null) {
                Debug.LogWarning("No prefab assigned to TriggerSpawnAndDisable on " + gameObject.name);
                return;
            }

            Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
            Debug.Log("Prefab spawned and player deactivated.");

            gameObject.SetActive(false);
        }

        private bool IsLayerAllowed(GameObject obj) {
            switch (layerMode) {
                case LayerInteraction.None:
                    return true;

                case LayerInteraction.Enemy:
                    return obj.layer == LayerMask.NameToLayer("Enemy");

                case LayerInteraction.Custom:
                    if (!string.IsNullOrEmpty(customLayerName)) {
                        return obj.layer == LayerMask.NameToLayer(customLayerName);
                    }
                    break;
            }
            return false;
        }
    }
}
