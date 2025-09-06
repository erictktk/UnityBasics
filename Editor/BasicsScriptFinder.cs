// !!!ID: 1cd60119e8bf45af9357a9460995f3ee
// File: BasicsScriptFinder.cs
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BasicsScriptFinder : EditorWindow {
    private const string SearchFolder = "Assets/Basics";
    private const string HeaderImagePath = "Assets/Basics/unity basics.png";

    private string searchTerm = "";
    private string lastSearched = "";
    private bool autoSearch = true;

    private Vector2 scroll;
    private Texture2D headerImage;

    [Serializable]
    private struct Match {
        public string path;
        public string fileName;
        public UnityEngine.Object asset;
    }

    private readonly List<Match> results = new List<Match>();

    private void FindRoot() {
        //look for file that's called basics_root_finder.txt
    }

    [MenuItem("Basics/Search")]
    public static void Open() {
        var win = GetWindow<BasicsScriptFinder>("Basics Script Search");
        win.minSize = new Vector2(560, 360);
        win.Show();
    }

    private void OnEnable() {
        headerImage = AssetDatabase.LoadAssetAtPath<Texture2D>(HeaderImagePath);
    }

    private void OnGUI() {
        DrawHeaderImage();

        using (new EditorGUILayout.HorizontalScope()) {
            EditorGUILayout.LabelField($"Search in \"{SearchFolder}\"", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            autoSearch = GUILayout.Toggle(autoSearch, "Auto Search", EditorStyles.miniButton);
        }

        using (new EditorGUILayout.HorizontalScope()) {
            GUI.SetNextControlName("SearchField");
            var newTerm = EditorGUILayout.TextField("Query", searchTerm);

            using (new EditorGUI.DisabledScope(string.IsNullOrWhiteSpace(newTerm))) {
                if (GUILayout.Button("Search", GUILayout.Width(80))) {
                    searchTerm = newTerm;
                    RunSearch();
                }
            }

            if (GUILayout.Button("Clear", GUILayout.Width(70))) {
                searchTerm = "";
                lastSearched = "";
                results.Clear();
            }

            if (autoSearch && newTerm != searchTerm) {
                searchTerm = newTerm;
                RunSearch();
            }
            else if (!autoSearch &&
                     Event.current.type == EventType.KeyDown &&
                     Event.current.keyCode == KeyCode.Return &&
                     GUI.GetNameOfFocusedControl() == "SearchField") {
                searchTerm = newTerm;
                RunSearch();
                GUI.FocusControl(null);
                Repaint();
            }
        }

        GUILayout.Space(6);
        DrawResults();
    }

    // Header image at its imported aspect
    private void DrawHeaderImage() {
        if (!headerImage) return;

        float ppp = Mathf.Max(1f, EditorGUIUtility.pixelsPerPoint);
        float w = headerImage.width / ppp;
        float h = headerImage.height / ppp; // keep your fix

        var r = new Rect((position.width - w) * 0.5f, 10f, w, h);
        GUI.DrawTexture(r, headerImage, ScaleMode.StretchToFill, true);

        GUILayout.Space(h + 20f);
    }

    private void DrawResults() {
        EditorGUILayout.LabelField($"Results: {results.Count}", EditorStyles.miniBoldLabel);
        GUILayout.Space(2);

        using (var sv = new EditorGUILayout.ScrollViewScope(scroll)) {
            scroll = sv.scrollPosition;

            if (results.Count == 0) {
                EditorGUILayout.HelpBox("No matches yet. Type to search (Auto Search on) or press Search.", MessageType.Info);
                return;
            }

            foreach (var m in results) {
                using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox)) {
                    EditorGUILayout.LabelField(m.fileName, EditorStyles.boldLabel);
                    EditorGUILayout.LabelField(m.path, EditorStyles.wordWrappedLabel);

                    using (new EditorGUILayout.HorizontalScope()) {
                        if (GUILayout.Button("Ping", GUILayout.Width(80)) && m.asset)
                            EditorGUIUtility.PingObject(m.asset);

                        if (GUILayout.Button("Open", GUILayout.Width(80)) && m.asset)
                            AssetDatabase.OpenAsset(m.asset);

                        GUILayout.FlexibleSpace();
                    }
                }
            }
        }
    }

    private void RunSearch() {
        results.Clear();

        if (!AssetDatabase.IsValidFolder(SearchFolder)) {
            Debug.LogWarning($"[Basics Script Finder] Folder not found: {SearchFolder}");
            return;
        }

        string q = (searchTerm ?? "").Trim();
        if (q == lastSearched && results.Count > 0) return;
        lastSearched = q;

        if (string.IsNullOrEmpty(q)) {
            Repaint();
            return;
        }

        try {
            string[] guids = AssetDatabase.FindAssets("t:Script", new[] { SearchFolder });
            foreach (var guid in guids) {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                if (!string.Equals(Path.GetExtension(path), ".cs", StringComparison.OrdinalIgnoreCase))
                    continue;

                string file = Path.GetFileNameWithoutExtension(path);
                if (file.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0) {
                    var asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
                    results.Add(new Match { path = path, fileName = file, asset = asset });
                }
            }
            Repaint();
        }
        catch (Exception ex) {
            Debug.LogError($"[Basics Script Finder] Error: {ex}");
        }
    }
}
#endif
