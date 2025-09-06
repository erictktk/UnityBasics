// !!!ID: b748869d0dc44efcbf155cfe368bc563
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

public static class ScriptIdAdder {
    private const string TargetFolder = "Assets/Basics"; // change if needed

    [MenuItem("Basics/Add Random IDs To Scripts")]
    public static void AddIds() {
        string[] files = Directory.GetFiles(TargetFolder, "*.cs", SearchOption.AllDirectories);

        foreach (var file in files) {
            string[] lines = File.ReadAllLines(file);

            // Skip if already has ID
            if (lines.Length > 0 && lines[0].StartsWith("// !!!ID:"))
                continue;

            string newId = System.Guid.NewGuid().ToString("N"); // random id

            using (StreamWriter writer = new StreamWriter(file, false)) {
                writer.WriteLine($"// !!!ID: {newId}");
                foreach (var line in lines)
                    writer.WriteLine(line);
            }

            Debug.Log($"Added ID to {file}");
        }

        AssetDatabase.Refresh();
    }
}
#endif
