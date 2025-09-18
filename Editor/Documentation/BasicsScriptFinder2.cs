// !!!ID: 1cd60119e8bf45af9357a9460995f3ee
// File: BasicsScriptFinder.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BasicsScriptFinder2 : EditorWindow {
    private const string DefaultSearchFolder = "Assets/Basics";
    private const string HeaderImagePath = "Assets/Basics/unity basics.png";
    private const string DefaultTagsJsonPath = "Assets/Basics/tags.txt";

    [Serializable]
    private struct Match {
        public string path;
        public string fileName;
        public UnityEngine.Object asset;
    }

    // ----- UI state
    private string searchTerm = "";
    private string lastSearched = "";
    private bool autoSearch = true;
    private bool useTagsSearch = false;
    private string tagsJsonPath = DefaultTagsJsonPath;

    private Vector2 scroll;
    private Texture2D headerImage;

    // ----- Results
    private readonly List<Match> results = new List<Match>();

    // ----- Strategies
    private ISeekStrategy nameStrategy;
    private TagSeekStrategy tagStrategy; // keep ref to expose reload

    // ====== Editor Window ======
    [MenuItem("Basics/Search2")]
    public static void Open() {
        var win = GetWindow<BasicsScriptFinder2>("Basics Script Search2");
        win.minSize = new Vector2(560, 420);
        win.Show();
    }

    private void OnEnable() {
        headerImage = AssetDatabase.LoadAssetAtPath<Texture2D>(HeaderImagePath);
        nameStrategy = new NameSeekStrategy();
        tagStrategy = new TagSeekStrategy();
    }

    private void OnGUI() {
        DrawHeaderImage();

        using (new EditorGUILayout.HorizontalScope()) {
            EditorGUILayout.LabelField($"Search in \"{DefaultSearchFolder}\"", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            autoSearch = GUILayout.Toggle(autoSearch, "Auto Search", EditorStyles.miniButton);
        }

        // Toggle: Use Tags Search
        using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox)) {
            useTagsSearch = EditorGUILayout.ToggleLeft("Use Tags Search", useTagsSearch);
            if (useTagsSearch) {
                EditorGUI.indentLevel++;
                tagsJsonPath = EditorGUILayout.TextField("Tags JSON Path", tagsJsonPath);
                using (new EditorGUILayout.HorizontalScope()) {
                    if (GUILayout.Button("Reload Tags", GUILayout.Width(110))) {
                        tagStrategy.Reload(tagsJsonPath);
                        // do not auto-run search here; user can type or press Search
                    }
                    if (GUILayout.Button("Open JSON", GUILayout.Width(110))) {
                        var asset = AssetDatabase.LoadAssetAtPath<TextAsset>(tagsJsonPath);
                        if (asset) AssetDatabase.OpenAsset(asset);
                        else Debug.LogWarning($"[Basics Script Finder] Could not open: {tagsJsonPath}");
                    }
                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.HelpBox("Search terms are space-separated; terms map through TagAliases (aliases → base tags). Matches if any tag overlaps.", MessageType.None);
                EditorGUI.indentLevel--;
            }
            else {
                EditorGUILayout.HelpBox("Filename search (case-insensitive, substring).", MessageType.None);
            }
        }

        // Query row
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

    // ====== UI helpers ======
    private void DrawHeaderImage() {
        if (!headerImage) return;

        float ppp = Mathf.Max(1f, EditorGUIUtility.pixelsPerPoint);
        float w = headerImage.width / ppp;
        float h = headerImage.height / ppp;

        var r = new Rect((position.width - w) * 0.5f, 10f, w, h);
        GUI.DrawTexture(r, headerImage, ScaleMode.StretchToFill, true);

        GUILayout.Space(h + 16f);
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

    // ====== Search orchestration ======
    private void RunSearch() {
        results.Clear();

        if (!AssetDatabase.IsValidFolder(DefaultSearchFolder)) {
            Debug.LogWarning($"[Basics Script Finder] Folder not found: {DefaultSearchFolder}");
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
            var strategy = useTagsSearch
                ? (ISeekStrategy)tagStrategy.WithRoot(DefaultSearchFolder, tagsJsonPath)
                : (ISeekStrategy)nameStrategy.WithRoot(DefaultSearchFolder);

            foreach (var m in strategy.Seek(q)) {
                results.Add(m);
            }
            Repaint();
        }
        catch (Exception ex) {
            Debug.LogError($"[Basics Script Finder] Error: {ex}");
        }
    }

    // ====== Strategy Interfaces & Implementations ======
    private interface ISeekStrategy {
        ISeekStrategy WithRoot(string searchFolder, string extra = null);
        IEnumerable<Match> Seek(string query);
    }

    /// <summary>Filename substring search (original behavior).</summary>
    private class NameSeekStrategy : ISeekStrategy {
        private string searchFolder;

        public ISeekStrategy WithRoot(string searchFolder, string extra = null) {
            this.searchFolder = searchFolder;
            return this;
        }

        public IEnumerable<Match> Seek(string query) {
            string[] guids = AssetDatabase.FindAssets("t:Script", new[] { searchFolder });
            var q = query.ToLowerInvariant();

            foreach (var guid in guids) {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                if (!string.Equals(Path.GetExtension(path), ".cs", StringComparison.OrdinalIgnoreCase))
                    continue;

                string file = Path.GetFileNameWithoutExtension(path);
                if (file.ToLowerInvariant().Contains(q)) {
                    var asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
                    yield return new Match { path = path, fileName = file, asset = asset };
                }
            }
        }
    }

    /// <summary>Tag search: load scripts.json, expand terms via TagAliases, match by tag overlap, resolve assets in project.</summary>
    private class TagSeekStrategy : ISeekStrategy {
        [Serializable]
        private class ScriptInfo {
            public int Number;
            public string Name;
            public string Id;
            public List<string> Tags;
        }

        [Serializable]
        private class ScriptInfoList {
            public List<ScriptInfo> Items;
        }

        private string searchFolder;
        private string jsonPath;
        private List<ScriptInfo> db = new List<ScriptInfo>();
        private Dictionary<string, HashSet<string>> aliasToBases; // alias -> base(s)

        public TagSeekStrategy() {
            aliasToBases = BuildAliasIndex();
        }

        public ISeekStrategy WithRoot(string searchFolder, string extra = null) {
            this.searchFolder = searchFolder;
            if (!string.IsNullOrEmpty(extra) && extra != jsonPath) {
                jsonPath = extra;
                // soft-load (lazy) — don't fail window if missing; will try at Seek time
            }
            return this;
        }

        public void Reload(string path) {
            jsonPath = path;
            LoadDb();
        }

        public IEnumerable<Match> Seek(string query) {
            if (db == null || db.Count == 0) LoadDb();

            // tokens → base tags via aliases
            var tokens = (query ?? "").ToLowerInvariant().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var baseTags = new HashSet<string>();
            foreach (var t in tokens) {
                if (aliasToBases.TryGetValue(t, out var bases))
                    foreach (var b in bases) baseTags.Add(b);
            }
            if (baseTags.Count == 0) yield break;

            // Filter scripts: any overlap
            var matches = db.Where(s => {
                var tags = (s.Tags ?? new List<string>()).Select(x => x.ToLowerInvariant()).ToHashSet();
                return baseTags.Any(tags.Contains);
            });

            // Resolve each script to an asset inside searchFolder
            foreach (var s in matches) {
                var nameNoExt = Path.GetFileNameWithoutExtension(s.Name ?? "");
                if (string.IsNullOrEmpty(nameNoExt)) continue;

                // limit to folder; fallback to global
                string[] guids = AssetDatabase.FindAssets($"{nameNoExt} t:Script", new[] { searchFolder });
                if (guids == null || guids.Length == 0) guids = AssetDatabase.FindAssets($"{nameNoExt} t:Script");

                foreach (var guid in guids) {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    if (!string.Equals(Path.GetFileNameWithoutExtension(path), nameNoExt, StringComparison.OrdinalIgnoreCase))
                        continue;
                    var asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
                    yield return new Match { path = path, fileName = nameNoExt, asset = asset };
                    break; // one result per script entry
                }
            }
        }

        private void LoadDb() {
            db = new List<ScriptInfo>();
            if (string.IsNullOrEmpty(jsonPath)) return;

            // Try load as Unity asset first
            var textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(jsonPath);
            string json = textAsset ? textAsset.text : null;

            // If not found, fallback to absolute path
            if (string.IsNullOrEmpty(json)) {
                string abs = Path.Combine(Directory.GetParent(Application.dataPath).FullName, jsonPath);
                if (!File.Exists(abs)) {
                    Debug.LogWarning($"Tags JSON not found: {jsonPath}");
                    return;
                }
                json = File.ReadAllText(abs);
            }

            // Wrap array JSON if needed
            if (json.TrimStart().StartsWith("[")) {
                json = "{ \"Items\": " + json + "}";
            }

            var wrapper = JsonUtility.FromJson<ScriptInfoList>(json);
            db = wrapper?.Items ?? new List<ScriptInfo>();
        }


        private static Dictionary<string, HashSet<string>> BuildAliasIndex() {
            var map = new Dictionary<string, HashSet<string>>();
            foreach (var kv in TagAliases.MAP) {
                var baseKey = kv.Key.ToLowerInvariant();

                if (!map.TryGetValue(baseKey, out var self))
                    map[baseKey] = self = new HashSet<string>();
                self.Add(baseKey);

                foreach (var alias in kv.Value) {
                    var a = alias.ToLowerInvariant();
                    if (!map.TryGetValue(a, out var set))
                        map[a] = set = new HashSet<string>();
                    set.Add(baseKey);
                }
            }
            return map;
        }
    }
}
