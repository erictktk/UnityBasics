// !!!ID: 5a7bde3dc70743aa9a2de0c07067f9c8
// RectHandleAllInOne.cs
// Drag LEFT/RIGHT/TOP/BOTTOM independently (like Unity's Rect Tool).
// Works in XY, supports Z-rotation. Min-size respected.

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class RectHandleAllInOne : MonoBehaviour {
    [Header("Rect (local XY)")]
    public Vector2 size = new Vector2(2f, 1f);     // width, height
    public Vector2 center = Vector2.zero;          // local center offset (object space)
    [Range(-180, 180)] public float rotationZ = 0f; // local Z rot for the rect visual

    [Header("Limits / Visuals")]
    public Vector2 minSize = new Vector2(0.01f, 0.01f);
    public Color lineColor = new Color(0.2f, 1f, 0.2f, 1f);

#if UNITY_EDITOR
    Matrix4x4 RectTRS =>
        transform.localToWorldMatrix *
        Matrix4x4.TRS(new Vector3(center.x, center.y, 0f), Quaternion.Euler(0, 0, rotationZ), Vector3.one);

    Vector3[] GetLocalCorners(Vector2 half) {
        return new[]{
            new Vector3(-half.x, -half.y, 0), // BL
            new Vector3(-half.x,  half.y, 0), // TL
            new Vector3( half.x,  half.y, 0), // TR
            new Vector3( half.x, -half.y, 0)  // BR
        };
    }

    void OnDrawGizmos() {
        Handles.color = lineColor;
        using (new Handles.DrawingScope(RectTRS)) {
            var h = size * 0.5f;
            Handles.DrawSolidRectangleWithOutline(GetLocalCorners(h), new Color(0, 0, 0, 0), lineColor);
        }
    }

    [CustomEditor(typeof(RectHandleAllInOne))]
    class RectEdgeHandleEditor : Editor {
        public override void OnInspectorGUI() => DrawDefaultInspector();

        void OnSceneGUI() {
            var t = (RectHandleAllInOne)target;

            Matrix4x4 M = t.RectTRS;
            Vector2 half = t.size * 0.5f;

            // Local edges (in rect's local X/Y)
            float L = -half.x, R = half.x, B = -half.y, T = half.y;

            // Local unit axes rotated by rotationZ (world-space directions)
            float rad = t.rotationZ * Mathf.Deg2Rad;
            Vector2 ux = new(Mathf.Cos(rad), Mathf.Sin(rad));   // +X (right)
            Vector2 uy = new(-Mathf.Sin(rad), Mathf.Cos(rad));  // +Y (up)

            // Edge centers in local rect space
            Vector3 Lc = new(L, 0, 0), Rc = new(R, 0, 0), Tc = new(0, T, 0), Bc = new(0, B, 0);

            // Convert to world positions
            Vector3 WL = M.MultiplyPoint3x4(Lc);
            Vector3 WR = M.MultiplyPoint3x4(Rc);
            Vector3 WT = M.MultiplyPoint3x4(Tc);
            Vector3 WB = M.MultiplyPoint3x4(Bc);

            // Drag handles constrained along their outward normals
            Vector3 nL = new(-ux.x, -ux.y, 0);
            Vector3 nR = new(ux.x, ux.y, 0);
            Vector3 nT = new(uy.x, uy.y, 0);
            Vector3 nB = new(-uy.x, -uy.y, 0);

            float hsize = HandleUtility.GetHandleSize(M.MultiplyPoint3x4(Vector3.zero)) * 0.08f;

            using (new Handles.DrawingScope(M))
                Handles.DrawSolidRectangleWithOutline(t.GetLocalCorners(half), new Color(0, 0, 0, 0), t.lineColor);

            EditorGUI.BeginChangeCheck();
            Vector3 WL2 = Handles.Slider(WL, nL, hsize, Handles.DotHandleCap, 0);
            Vector3 WR2 = Handles.Slider(WR, nR, hsize, Handles.DotHandleCap, 0);
            Vector3 WT2 = Handles.Slider(WT, nT, hsize, Handles.DotHandleCap, 0);
            Vector3 WB2 = Handles.Slider(WB, nB, hsize, Handles.DotHandleCap, 0);
            bool changed = EditorGUI.EndChangeCheck();

            // Position handle for whole rect
            EditorGUI.BeginChangeCheck();
            Vector3 Wc = M.MultiplyPoint3x4(Vector3.zero);
            Vector3 Wc2 = Handles.PositionHandle(Wc, Quaternion.identity);
            bool movedCenter = EditorGUI.EndChangeCheck();

            if (!changed && !movedCenter) return;

            Undo.RecordObject(t, "Adjust Rect");

            // Move center (object local)
            if (movedCenter) {
                Vector3 dW = Wc2 - Wc;
                Vector3 dLocal = t.transform.worldToLocalMatrix.MultiplyVector(dW);
                t.center += new Vector2(dLocal.x, dLocal.y);
                // Recompute M after moving center
                M = t.RectTRS;
                WL = M.MultiplyPoint3x4(Lc); WR = M.MultiplyPoint3x4(Rc);
                WT = M.MultiplyPoint3x4(Tc); WB = M.MultiplyPoint3x4(Bc);
            }

            if (changed) {
                // Project world deltas onto rect local +X / +Y axes
                Vector3 dWL = WL2 - WL, dWR = WR2 - WR, dWT = WT2 - WT, dWB = WB2 - WB;
                float dLx = Vector3.Dot(dWL, new Vector3(ux.x, ux.y, 0));  // +X distance
                float dRx = Vector3.Dot(dWR, new Vector3(ux.x, ux.y, 0));
                float dTy = Vector3.Dot(dWT, new Vector3(uy.x, uy.y, 0));  // +Y distance
                float dBy = Vector3.Dot(dWB, new Vector3(uy.x, uy.y, 0));

                bool movedL = dWL.sqrMagnitude > 1e-12f;
                bool movedR = dWR.sqrMagnitude > 1e-12f;
                bool movedT = dWT.sqrMagnitude > 1e-12f;
                bool movedB = dWB.sqrMagnitude > 1e-12f;

                // Update edges directly in rect local coordinates
                if (movedL) L += dLx;   // dragging right -> dLx>0 -> L increases -> width shrinks
                if (movedR) R += dRx;   // dragging right -> dRx>0 -> R increases -> width grows
                if (movedT) T += dTy;   // dragging up    -> dTy>0 -> T increases -> height grows
                if (movedB) B += dBy;   // dragging up    -> dBy>0 -> B increases -> height shrinks

                // Enforce min size by clamping the moved edge
                float minW = Mathf.Max(t.minSize.x, 1e-5f);
                float minH = Mathf.Max(t.minSize.y, 1e-5f);

                if (R - L < minW) {
                    if (movedL && !movedR) L = R - minW;
                    else R = L + minW;
                }
                if (T - B < minH) {
                    if (movedB && !movedT) B = T - minH;
                    else T = B + minH;
                }

                // New center shift in rect-local axes (midpoint), then rotate back to object local
                Vector2 dCenterRect = new Vector2((L + R) * 0.5f, (B + T) * 0.5f);
                float irad = -rad;
                Vector2 rx = new(Mathf.Cos(irad), Mathf.Sin(irad));
                Vector2 ry = new(-Mathf.Sin(irad), Mathf.Cos(irad));
                Vector2 dCenterObj = dCenterRect.x * rx + dCenterRect.y * ry;

                t.center += dCenterObj;

                // Final size from edges
                t.size = new Vector2(R - L, T - B);
            }

            EditorApplication.QueuePlayerLoopUpdate();
            SceneView.RepaintAll();
        }
    }
#endif
}
