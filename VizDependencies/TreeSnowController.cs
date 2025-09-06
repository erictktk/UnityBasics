// !!!ID: c4436fe55b8e4a7790a96a93814ca049
using UnityEngine;
using System.Collections.Generic;

public class TreeSnowController : MonoBehaviour {
    [SerializeField] private string noiseTexProperty = "_NoiseTex_ST";
    [SerializeField] private string luminanceAddProperty = "_Add";
    [SerializeField] private float offsetRange = 10f;
    [SerializeField] private float amplitude = 0f;
    [SerializeField] private bool useBatching = false;

    private float lastAmplitude = float.NaN;

    class Entry {
        public Renderer renderer;
        public Vector4 baseNoiseST;
        public float baseAdd;
        public Material materialInstance;
        public MaterialPropertyBlock mpb;
    }

    private List<Entry> entries = new List<Entry>();

    void Start() {
        foreach (Transform child in transform) {
            Renderer r = child.GetComponent<Renderer>();
            if (r == null) continue;

            Entry e = new Entry();
            e.renderer = r;

            if (useBatching) {
                e.mpb = new MaterialPropertyBlock();
                r.GetPropertyBlock(e.mpb);

                Vector4 st = e.mpb.GetVector(noiseTexProperty);
                if (st == Vector4.zero) st = new Vector4(1, 1, 0, 0);
                st.z = Random.Range(-offsetRange, offsetRange);
                st.w = Random.Range(-offsetRange, offsetRange);
                e.baseNoiseST = st;
                e.mpb.SetVector(noiseTexProperty, st);

                float originalAdd = r.sharedMaterial.GetFloat(luminanceAddProperty);
                e.baseAdd = originalAdd;
                e.mpb.SetFloat(luminanceAddProperty, originalAdd + amplitude);

                r.SetPropertyBlock(e.mpb);
            }
            else {
                Material mat = r.material;
                Vector4 st = mat.GetVector(noiseTexProperty);
                if (st == Vector4.zero) st = new Vector4(1, 1, 0, 0);
                st.z = Random.Range(-offsetRange, offsetRange);
                st.w = Random.Range(-offsetRange, offsetRange);
                mat.SetVector(noiseTexProperty, st);
                e.baseNoiseST = st;

                float originalAdd = mat.GetFloat(luminanceAddProperty);
                e.baseAdd = originalAdd;
                mat.SetFloat(luminanceAddProperty, originalAdd + amplitude);
                e.materialInstance = mat;
            }

            entries.Add(e);
        }

        lastAmplitude = amplitude;
    }

    void Update() {
        if (Mathf.Approximately(amplitude, lastAmplitude)) return;

        foreach (var e in entries) {
            float updated = e.baseAdd + amplitude;

            if (useBatching) {
                e.mpb.SetFloat(luminanceAddProperty, updated);
                e.renderer.SetPropertyBlock(e.mpb);
            }
            else {
                e.materialInstance.SetFloat(luminanceAddProperty, updated);
            }
        }

        lastAmplitude = amplitude;
    }
}
