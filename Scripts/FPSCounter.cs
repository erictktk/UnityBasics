using UnityEngine;

/// <summary>
/// Displays an on-screen FPS counter with frame time in milliseconds and frames per second.
/// Optionally also shows the number of root GameObjects in the active scene.
/// </summary>
public class FPSCounter : MonoBehaviour {
    float deltaTime = 0.0f;
    public bool showNumRootObjects = true;

    void Update() {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    void OnGUI() {
        int w = Screen.width, h = Screen.height;
        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = Color.white;

        int count = UnityEngine.SceneManagement.SceneManager.GetActiveScene().rootCount;

        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text;
        if (!showNumRootObjects) {
            text = string.Format("{0:0.0} ms   ({1:0.} fps)", msec, fps);
        }
        else {
            text = string.Format("{0:0.0} ms   ({1:0.} fps)   {2} numRootGameObjects", msec, fps, count);
        }
        GUI.Label(rect, text, style);
    }
}
