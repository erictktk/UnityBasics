// !!!ID: 26c13e0b0e1b4d47a3d80002a2853782
using UnityEngine;

/// <summary>
/// Continuously offsets the UV coordinates of a material to create scrolling or stepped texture movement.
/// - Can run continuously with uvSpeed or in discrete steps with stepSize and stepInterval.
/// - Supports using the main texture offset or a custom shader property.
/// - Updates the material’s texture offset every frame.
/// </summary>
namespace Basics
{
    public class UVOffset : MonoBehaviour
    {
        public Renderer targetRenderer;
        public Vector2 uvSpeed = new Vector2(0.1f, 0f);
        public Vector2 stepSize = new Vector2(0.1f, 0f);
        public float stepInterval = 0.5f;
        public bool discrete = false;
        public bool useCustomProperty = false;
        public string textureProperty = "_MainTex";

        private Vector2 offset;
        private float timer;
        private int propID;

        void Start()
        {
            if (targetRenderer == null) targetRenderer = GetComponent<Renderer>();
            propID = Shader.PropertyToID(textureProperty);
            offset = useCustomProperty
                ? targetRenderer.material.GetTextureOffset(propID)
                : targetRenderer.material.mainTextureOffset;
            timer = 0f;
        }

        void Update()
        {
            if (discrete)
            {
                timer += Time.deltaTime;
                if (timer >= stepInterval)
                {
                    offset += stepSize;
                    timer -= stepInterval;
                }
            }
            else
            {
                offset += uvSpeed * Time.deltaTime;
            }

            if (useCustomProperty)
                targetRenderer.material.SetTextureOffset(propID, offset);
            else
                targetRenderer.material.mainTextureOffset = offset;
        }
    }
}
