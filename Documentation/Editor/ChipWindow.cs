using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;

public class ChipManager {
    public HashSet<string> Selected = new HashSet<string>();
    public event Action<string, bool> OnChipToggled; // (tag, isSelected)

    public bool IsSelected(string tag) => Selected.Contains(tag);

    public void SetSelected(string tag, bool v) {
        if (v) Selected.Add(tag); else Selected.Remove(tag);
        OnChipToggled?.Invoke(tag, v);
    }
}

public class ChipWindow : EditorWindow {
    private readonly List<string> tags = new List<string> { "Enemy", "Player", "Boss", "Collectible", "Quest", "Rare" };
    private ChipManager manager;

    // Colors (tweak as you like)
    private Color onBg = new Color(0.20f, 0.50f, 1.00f);
    private Color offBg = new Color(0.90f, 0.90f, 0.90f);
    private Color onTxt = Color.white;
    private Color offTxt = Color.black;

    [MenuItem("Tools/Chips (IMGUI)")]
    public static void Open() => GetWindow<ChipWindow>("Chips (IMGUI)");

    private void OnEnable() {
        manager = new ChipManager();
        manager.OnChipToggled += (tag, val) => Debug.Log($"Chip toggled: {tag} -> {(val ? "ON" : "OFF")}");
    }

    private void OnGUI() {
        GUILayout.Label("Tag Filters", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        foreach (var tag in tags) {
            bool active = manager.IsSelected(tag);
            bool newVal = DrawChip(manager, tag, active, onBg, offBg, onTxt, offTxt);
            if (newVal != active) manager.SetSelected(tag, newVal);
        }
        EditorGUILayout.EndHorizontal();

        // Example: show selected
        EditorGUILayout.Space(4);
        EditorGUILayout.LabelField("Selected:", string.Join(", ", manager.Selected));
    }

    /// <summary>
    /// Reusable pill-like toggle. Notifies manager on change.
    /// </summary>
    private static bool DrawChip(ChipManager manager, string text, bool active,
                                 Color activeBg, Color inactiveBg, Color activeText, Color inactiveText) {
        // Local cached 1x1 textures per color (avoids recreating every frame)
        Texture2D BgTex(Color c) {
            // Static cache lives for the domain
            return _TexCache.Get(c);
        }

        var style = new GUIStyle("Button") {
            margin = new RectOffset(2, 2, 2, 2),
            padding = new RectOffset(10, 10, 4, 4),
            alignment = TextAnchor.MiddleCenter
        };

        style.normal.textColor = active ? activeText : inactiveText;
        style.normal.background = BgTex(active ? activeBg : inactiveBg);

        // Rounded look via font/spacing (IMGUI has no true corner radius); keep it simple
        return GUILayout.Toggle(active, text, style, GUILayout.ExpandWidth(false));
    }

    // ---- Tiny texture cache helper ----
    private static class _TexCache {
        private static readonly Dictionary<Color, Texture2D> cache = new Dictionary<Color, Texture2D>();

        public static Texture2D Get(Color c) {
            if (cache.TryGetValue(c, out var tex) && tex) return tex;
            tex = MakeTex(1, 1, c);
            cache[c] = tex;
            return tex;
        }

        private static Texture2D MakeTex(int w, int h, Color col) {
            var t = new Texture2D(w, h) { hideFlags = HideFlags.HideAndDontSave };
            var px = new Color[w * h];
            for (int i = 0; i < px.Length; i++) px[i] = col;
            t.SetPixels(px);
            t.Apply();
            return t;
        }
    }
}
