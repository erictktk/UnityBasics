// PseudoFabMarker.cs
using UnityEngine;

[ExecuteAlways]
[DisallowMultipleComponent]
[AddComponentMenu("")] // hide from Add Component menu
public class PseudoFabMarker : MonoBehaviour {
    [HideInInspector] public Color color = new Color(0.55f, 0.95f, 0.90f, 0.45f); // light teal default
    [HideInInspector] public bool redirectHierarchy = true;

    void OnEnable() {
        // Keep the component invisible in the Inspector
        //hideFlags |= HideFlags.HideInInspector;
    }

    // If Unity ever clears flags (domain reloads), re-apply
    void OnValidate() {
        //hideFlags |= HideFlags.HideInInspector;
    }
}
