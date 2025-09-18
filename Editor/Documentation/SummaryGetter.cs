// !!!ID: 3f50e22b1b8c4c24b8fd9a88ff9a65da
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;

public static class ScriptSummaryCollector {
    private const string TargetFolder = "Assets/Basics"; // your folder

    [MenuItem("Basics/Documentation/Collect Summaries")]
    public static void CollectSummaries() {
        string[] files = Directory.GetFiles(TargetFolder, "*.cs", SearchOption.AllDirectories);
        StringBuilder sb = new StringBuilder();

        foreach (var file in files) {
            string[] lines = File.ReadAllLines(file);
            string id = ExtractID(lines, Path.GetFileName(file));
            string summary = ExtractSummary(lines, Path.GetFileName(file));
            sb.AppendLine($"### {Path.GetFileName(file)}");
            sb.AppendLine(id);
            sb.AppendLine(summary);
            sb.AppendLine();
        }

        string outputPath = Path.Combine(TargetFolder, "ScriptSummaries.txt");
        File.WriteAllText(outputPath, sb.ToString());
        AssetDatabase.Refresh();

        Debug.Log($"Summaries collected in {outputPath}");
    }

    private static string ExtractID(string[] lines, string filename) {
        // Look for the line with "!!!ID: <id>"
        foreach (var line in lines) {
            if (line.Contains("!!!ID:")) {
                int idx = line.IndexOf("!!!ID:") + "!!!ID:".Length;
                return line.Substring(idx).Trim();
}
        }
        return $"No ID found in {filename}.";
    }

    private static string ExtractSummary(string[] lines, string filename) {
        // If there's an XML doc summary (/// <summary>), extract it
        for (int i = 0; i < lines.Length; i++) {
            if (lines[i].Contains("/// <summary>")) {
                string summary = "";
                for (int j = i + 1; j < lines.Length; j++) {
                    if (lines[j].Contains("/// </summary>"))
                        break;
                    summary += lines[j].Replace("///", "").Trim() + " ";
                }
                return summary.Trim();
            }
        }

        // Otherwise fallback
        return $"No summary found in {filename}.";
    }
}
