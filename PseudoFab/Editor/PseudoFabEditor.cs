// PseudoFabEditor.cs
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.SceneManagement;

static class PseudoFab {
    const string PREF_ENABLED = "PseudoFab.Enabled";
    const string PREF_COLOR = "PseudoFab.ColorRGBA";

    public static bool Enabled {
        get => EditorPrefs.GetBool(PREF_ENABLED, true);
        set => EditorPrefs.SetBool(PREF_ENABLED, value);
    }

    public static Color DefaultColor {
        get {
            var hex = EditorPrefs.GetString(PREF_COLOR, "#8CF2E673"); // light teal with alpha
            Color c; ColorUtility.TryParseHtmlString(hex, out c);
            if (c.a <= 0f) c = new Color(0.55f, 0.95f, 0.90f, 0.45f);
            return c;
        }
        set => EditorPrefs.SetString(PREF_COLOR, "#" + ColorUtility.ToHtmlStringRGBA(value));
    }

    [MenuItem("Tools/Pseudofab/Enabled", priority = 10)]
    static void ToggleEnabled() => Enabled = !Enabled;

    [MenuItem("Tools/Pseudofab/Enabled", true)]
    static bool ToggleEnabledValidate() {
        Menu.SetChecked("Tools/Pseudofab/Enabled", Enabled);
        return true;
    }

    [MenuItem("Tools/Pseudofab/Mark Selected %#g", priority = 11)] // Ctrl/Cmd+Shift+G
    static void MarkSelected() {
        foreach (var obj in Selection.gameObjects) {
            var m = obj.GetComponent<PseudoFabMarker>();
            if (!m) m = Undo.AddComponent<PseudoFabMarker>(obj);
            Undo.RecordObject(m, "Set Pseudofab Color");
            m.color = DefaultColor;
            m.redirectHierarchy = true;
            EditorUtility.SetDirty(m);
        }
    }

    [MenuItem("Tools/Pseudofab/Clear From Selected", priority = 12)]
    static void ClearFromSelected() {
        foreach (var obj in Selection.gameObjects) {
            var m = obj.GetComponent<PseudoFabMarker>();
            if (m) Undo.DestroyObjectImmediate(m);
        }
    }

    [MenuItem("Tools/Pseudofab/Settings…", priority = 13)]
    static void OpenSettings() => PseudoFabSettingsWindow.ShowWindow();
}

class PseudoFabSettingsWindow : EditorWindow {
    public static void ShowWindow() {
        var win = GetWindow<PseudoFabSettingsWindow>("Pseudofab");
        win.minSize = new Vector2(260, 120);
        win.Show();
    }

    void OnGUI() {
        GUILayout.Space(6);
        var enabled = PseudoFab.Enabled;
        var newEnabled = EditorGUILayout.ToggleLeft("Enabled", enabled);
        if (newEnabled != enabled) PseudoFab.Enabled = newEnabled;

        GUILayout.Space(6);
        var c = PseudoFab.DefaultColor;
        var newC = EditorGUILayout.ColorField(new GUIContent("Default Color"), c);
        if (newC != c) PseudoFab.DefaultColor = newC;

        GUILayout.Space(6);
        if (GUILayout.Button("Apply Default Color to Selected Pseudofabs")) {
            foreach (var obj in Selection.gameObjects) {
                var m = obj.GetComponent<PseudoFabMarker>();
                if (m) {
                    Undo.RecordObject(m, "Apply Pseudofab Color");
                    m.color = PseudoFab.DefaultColor;
                    EditorUtility.SetDirty(m);
                }
            }
        }
    }
}

[InitializeOnLoad]
static class PseudoFabHooks {
    static bool _guard;

    static PseudoFabHooks() {
        EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
        Selection.selectionChanged += OnSelectionChanged;
        EditorApplication.playModeStateChanged += _ => EditorApplication.RepaintHierarchyWindow();

        customIcon = AssetDatabase.LoadAssetAtPath<Texture2D>(customIconPath);
    }

    private static readonly string customIconPath = "Assets/Basics/PseudoFab/PseudoFab teal.png";
    private static Texture2D customIcon;

    // Colors
    private static readonly Color32 rowColor = new Color32(56, 56, 56, 255);     // dark gray background
    private static readonly Color32 tealText = new Color32(169, 205, 200, 255); // teal text

    /*
    static PseudoFabHooks() {
        EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
        customIcon = AssetDatabase.LoadAssetAtPath<Texture2D>(customIconPath);
    }*/

    private static void OnHierarchyGUI(int instanceID, Rect rect) {
        var go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        if (go == null) return;

        // Only apply to pseudofabs
        var mark = go.GetComponent<PseudoFabMarker>();
        if (!mark) return;

        if (Selection.activeInstanceID == instanceID) {
            if (customIcon != null) {
                //var iconRect = new Rect(rect.xMin + 2f, rect.yMin + 1f, 16, 16);
                var iconRect = new Rect(rect.xMin + 1, rect.yMin + 1, 13, 14);
                GUI.DrawTexture(iconRect, customIcon, ScaleMode.ScaleToFit, true);
                rect.xMin += 17f; // indent text so it doesn’t overlap
                rect.yMin -= 1f;
            }
            return;
        }

        // Draw solid background if not selected
        EditorGUI.DrawRect(rect, rowColor);

        // Draw icon if present and then text
        if (customIcon != null) {
            //var iconRect = new Rect(rect.xMin + 2f, rect.yMin + 1f, 16, 16);
            var iconRect = new Rect(rect.xMin+1, rect.yMin+1, 13, 14);
            GUI.DrawTexture(iconRect, customIcon, ScaleMode.ScaleToFit, true);
            rect.xMin += 17f; // indent text so it doesn’t overlap
            rect.yMin -= 1f;
        }

        var style = new GUIStyle(EditorStyles.label);
        style.normal.textColor = tealText;
        EditorGUI.LabelField(rect, go.name, style);
    }

    /*
    static void OnHierarchyGUI(int instanceID, Rect rect) {
        if (!PseudoFab.Enabled) return;

        /*
         "Prefab Icon" → the usual blue cube prefab icon.

        "GameObject Icon" → default gray cube.

        "d_Prefab Icon" → dark-theme version.
        */

    /*
    var go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
    if (!go) return;

    var mark = go.GetComponent<PseudoFabMarker>();
    if (!mark) return;

    // Draw overlay
    var r = rect;
    r.xMin += 0f; r.xMax -= 2f;
    r.yMin += 1f; r.yMax -= 1f;
    EditorGUI.DrawRect(r, mark.color);

    // Draw icon + text
    var icon = EditorGUIUtility.IconContent("GameObject Icon");
    var content = new GUIContent(go.name, icon.image);

    EditorStyles.label

    EditorGUI.LabelField(rect, content, EditorStyles.label);*/

    /*
    var go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
    if (!go) return;

    var mark = go.GetComponent<PseudoFabMarker>();
    if (!mark) return;

    // Teal text style
    var style = new GUIStyle(EditorStyles.label);
    style.normal.textColor = new Color32(169, 205, 200, 255);

    // Pick an icon — e.g. Unity's prefab cube
    var icon = EditorGUIUtility.IconContent("Prefab Icon");

    // Combine icon + text
    var content = new GUIContent(go.name, icon.image);

    EditorGUI.LabelField(rect, content, style);*/


    /*
    var go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
    if (!go) return;

    var mark = go.GetComponent<PseudoFabMarker>();
    if (!mark) return;

    // Example: tint full row light teal
    var teal = new Color32(169, 205, 200, 100); // semi-transparent
    EditorGUI.DrawRect(rect, teal);

    // Redraw the object name on top
    EditorGUI.LabelField(rect, go.name, EditorStyles.label);*/
    //}


    static void OnSelectionChanged() {
        if (_guard || !PseudoFab.Enabled) return;

        // Allow hierarchy clicks
        var fw = EditorWindow.focusedWindow;
        if (fw != null && fw.GetType().Name == "SceneHierarchyWindow") return;

        var src = Selection.objects;
        if (src == null || src.Length == 0) return;

        var dst = new List<Object>(src.Length);
        GameObject toCollapse = null;
        bool changed = false;

        foreach (var o in src) {
            if (o is GameObject go) {
                var mk = FindNearestMarker(go.transform);
                if (mk && mk.redirectHierarchy && mk.gameObject != go) {
                    dst.Add(mk.gameObject);
                    toCollapse = mk.gameObject;
                    changed = true;
                }
                else dst.Add(go);
            }
            else dst.Add(o);
        }

        if (!changed) return;

        
        // inside your OnSelectionChanged redirect branch
        EditorApplication.delayCall += () =>
        {
            // apply redirected selection first...
            if (_guard) return;
            _guard = true;
            Selection.objects = dst.ToArray();
            _guard = false;

            // ...then collapse the parent after hierarchy updates
            EditorApplication.delayCall += () =>
            {
                if (toCollapse != null) PseudoFabUtils.Collapse(toCollapse);
            };
        };

    }



    static PseudoFabMarker FindNearestMarker(Transform t) {
        var cur = t;
        while (cur != null) {
            var m = cur.GetComponent<PseudoFabMarker>();
            if (m) return m;
            cur = cur.parent;
        }
        return null;
    }
}
#endif
