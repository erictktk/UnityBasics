/// <summary>
/// Basics TMP Installer Window: shows banner, detects TMP, promotes .txt → .cs into Assets
/// </summary>

using UnityEngine;
using UnityEditor;
using System;
using System.IO;

public class BasicsTMPInstallerWindow : EditorWindow {
    private const string HeaderImagePath = "Assets/Basics/unity basics.png";
    private const string SourceFolder = "Assets/Basics/TMP_Templates"; // put .txt here
    private const string DestinationRoot = "Assets";                      // always root

    private Texture2D headerImage;

    [MenuItem("Basics/Config")]
    public static void Open() {
        var win = GetWindow<BasicsTMPInstallerWindow>("Install/Config");
        win.minSize = new Vector2(560, 360);
        win.Show();
    }

    private void OnEnable() {
        headerImage = AssetDatabase.LoadAssetAtPath<Texture2D>(HeaderImagePath);
    }

    private void OnGUI() {
        DrawHeaderImage();



        GUILayout.Space(8);

        GUILayout.Label("Text Mesh Pro Support: ", EditorStyles.boldLabel);
        bool tmpPresent = HasTMP();

        EditorGUILayout.LabelField("TextMeshPro:", tmpPresent ? "Detected" : "Not Found",
            EditorStyles.boldLabel);

        using (new EditorGUI.DisabledScope(!tmpPresent)) {
            if (GUILayout.Button("Install Text Mesh Pro Support", GUILayout.Height(28))) {
                InstallTxtAsCs();
            }
        }

        if (!tmpPresent) {
            EditorGUILayout.HelpBox("TMP not detected. Install via Package Manager first.",
                MessageType.Warning);
        }
    }

    private void DrawHeaderImage() {
        if (!headerImage) return;
        float ppp = Mathf.Max(1f, EditorGUIUtility.pixelsPerPoint);
        float w = headerImage.width / ppp;
        float h = headerImage.height / ppp;
        var r = new Rect((position.width - w) * 0.5f, 10f, w, h);
        GUI.DrawTexture(r, headerImage, ScaleMode.StretchToFill, true);
        GUILayout.Space(h + 20f);
    }

    private void InstallTxtAsCs() {
        if (!AssetDatabase.IsValidFolder(SourceFolder)) {
            Debug.LogWarning($"[TMP Installer] Source not found: {SourceFolder}");
            return;
        }

        var txtFiles = Directory.GetFiles(SourceFolder, "*.txt", SearchOption.AllDirectories);
        if (txtFiles.Length == 0) {
            Debug.Log("[TMP Installer] No .txt templates found.");
            return;
        }

        int count = 0;
        foreach (var txt in txtFiles) {
            string dest = Path.Combine(DestinationRoot,
                Path.GetFileNameWithoutExtension(txt) + ".cs");
            File.Copy(txt, dest, overwrite: true);
            count++;
            Debug.Log($"[TMP Installer] Promoted {txt} → {dest}");
        }

        AssetDatabase.Refresh();
        Debug.Log($"[TMP Installer] Installed {count} scripts into {DestinationRoot}");
    }

    private static bool HasTMP() {
        var t = Type.GetType("TMPro.TMP_Text, Unity.TextMeshPro");
        if (t != null) return true;
        foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            if (asm.GetType("TMPro.TMP_Text") != null) return true;
        return false;
    }
}
