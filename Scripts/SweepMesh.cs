using UnityEngine;
using System.Collections.Generic;

namespace Basics.Mesh {
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class SweepMesh : MonoBehaviour {
        public enum Plane { XY, XZ, YZ }
        public Plane plane = Plane.XY;

        public List<Vector3> points = new List<Vector3>();
        public Vector3 segmentStart = new Vector3(0, -0.5f, 0);
        public Vector3 segmentEnd = new Vector3(0, 0.5f, 0);

        public Material material;

        MeshFilter mf;
        MeshRenderer mr;

        void Awake() {
            mf = GetComponent<MeshFilter>();
            mr = GetComponent<MeshRenderer>();
            if (material) mr.sharedMaterial = material;
        }

        void Start() => GenerateMesh();

        public void GenerateMesh() {
            if (points == null || points.Count < 2) return;
            int n = points.Count;
            Vector3 segDir = (segmentEnd - segmentStart).normalized;

            var verts = new Vector3[n * 2];
            var uvs = new Vector2[n * 2];
            var tris = new int[(n - 1) * 6];

            for (int i = 0; i < n; i++) {
                Vector3 p = points[i];
                Vector3 tan = (i < n - 1 ? points[i + 1] - p : p - points[i - 1]);
                switch (plane) {
                    case Plane.XY: tan = new Vector3(tan.x, tan.y, 0); break;
                    case Plane.XZ: tan = new Vector3(tan.x, 0, tan.z); break;
                    case Plane.YZ: tan = new Vector3(0, tan.y, tan.z); break;
                }
                tan.Normalize();
                var rot = Quaternion.FromToRotation(segDir, tan);
                verts[i * 2] = p + rot * segmentStart;
                verts[i * 2 + 1] = p + rot * segmentEnd;
                float u = (float)i / (n - 1);
                uvs[i * 2] = new Vector2(u, 0);
                uvs[i * 2 + 1] = new Vector2(u, 1);
            }

            for (int i = 0; i < n - 1; i++) {
                int v = i * 2, t = i * 6;
                tris[t] = v;
                tris[t + 1] = v + 2;
                tris[t + 2] = v + 1;
                tris[t + 3] = v + 1;
                tris[t + 4] = v + 2;
                tris[t + 5] = v + 3;
            }

            var mesh = new UnityEngine.Mesh { name = "SweepMesh" };
            mesh.vertices = verts;
            mesh.uv = uvs;
            mesh.triangles = tris;
            mesh.RecalculateNormals();
            mf.mesh = mesh;
        }
    }
}

