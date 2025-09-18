using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

[Serializable]
public class ScriptInfo {
    public int Number;
    public string Name;
    public string Id;
    public List<string> Tags;
}

[Serializable]
public class ScriptInfoList {
    public List<ScriptInfo> Items;
}

public static class TagSearch {
    private static readonly Dictionary<string, HashSet<string>> AliasToBases = BuildAliasIndex();

    // Load scripts.json from a TextAsset (drop into Resources)
    public static List<ScriptInfo> Load(TextAsset jsonAsset) {
        var wrapper = JsonUtility.FromJson<ScriptInfoList>(jsonAsset.text);
        return wrapper.Items ?? new List<ScriptInfo>();
    }

    public static List<ScriptInfo> Search(string query, IEnumerable<ScriptInfo> scripts, bool requireAll = false) {
        var tokens = (query ?? "").ToLowerInvariant()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries);

        var baseTags = new HashSet<string>();
        foreach (var t in tokens) {
            if (AliasToBases.TryGetValue(t, out var bases))
                foreach (var b in bases) baseTags.Add(b);
        }

        if (baseTags.Count == 0) return new List<ScriptInfo>();

        return scripts.Where(s => {
            var tags = (s.Tags ?? new List<string>())
                        .Select(x => x.ToLowerInvariant()).ToHashSet();

            return requireAll
                ? baseTags.All(tags.Contains)  // AND
                : baseTags.Any(tags.Contains); // OR
        }).ToList();
    }

    private static Dictionary<string, HashSet<string>> BuildAliasIndex() {
        var map = new Dictionary<string, HashSet<string>>();
        foreach (var kv in TagAliases.MAP) {
            var baseKey = kv.Key.ToLowerInvariant();

            if (!map.ContainsKey(baseKey))
                map[baseKey] = new HashSet<string>();
            map[baseKey].Add(baseKey);

            foreach (var alias in kv.Value) {
                var a = alias.ToLowerInvariant();
                if (!map.ContainsKey(a))
                    map[a] = new HashSet<string>();
                map[a].Add(baseKey);
            }
        }
        return map;
    }
}
