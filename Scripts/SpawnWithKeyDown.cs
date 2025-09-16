// !!!ID: 5535e49545ac45d6bf77bec5b4e964a9
using UnityEngine;


/// <summary>
/// Spawns a prefab when a specified key is pressed.
/// - Key is configurable (default: Space).
/// - OffsetMode:
///     - Constant: uses a fixed offset vector.
///     - RandomUnitCircle: spawns at a random offset within a unit circle.
/// - Instantiates the prefab at (transform.position + offset) with no rotation.
/// </summary>
namespace Basics.InputHandling
{
    public class SpawnWithKeyDown : MonoBehaviour
    {
        public enum OffsetMode { Constant, RandomUnitCircle }

        public GameObject prefab;
        public KeyCode key = KeyCode.Space;
        public OffsetMode mode = OffsetMode.Constant;
        public Vector3 offset = Vector3.zero;

        void Update()
        {
            if (Input.GetKeyDown(key) && prefab != null)
            {
                Vector3 spawnOffset = mode == OffsetMode.Constant
                    ? offset
                    : (Vector2)Random.insideUnitCircle;

                Vector3 spawnPosition = transform.position + spawnOffset;
                Instantiate(prefab, spawnPosition, Quaternion.identity);
            }
        }
    }
}

