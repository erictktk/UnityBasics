/*
 * TypewriterFromSourceText (UI)
 * -----------------------------
 * A UI typewriter effect for TextMeshProUGUI that has no midword breaks.
 * Copies from source
 *
 * Features:
 * - Works with UI TMP_Text components.
 * - Captures baked text on enable or manually.
 * - Play(), Stop(), and SkipToEnd() controls.
 * - Adjustable speed, start delay, scaled/unscaled time.
 * - Optional punctuation pauses for natural rhythm.
 * - UnityEvents for typing start/finish hooks.
 *
 * Implementation:
 * - Forces TMP layout on the source.
 * - Inserts '\n' at the source’s computed line breaks.
 * - Reveals text gradually via maxVisibleCharacters.
 */

using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System.Text;
using System.Collections.Generic;

public class TypewriterFromSource : MonoBehaviour {
    [Header("References")]
    public TMP_Text source;
    public TMP_Text target;

    [Header("Typing")]
    public bool captureOnEnable = true;
    public bool autoStart = true;
    public float charactersPerSecond = 45f;
    public float startDelay = 0f;
    public bool useUnscaledTime = true;

    [Header("Pauses")]
    public bool punctuationPauses = true;
    public string punctuation = ".,;:!?";
    public float punctuationExtraDelay = 0.12f;

    [Header("Events")]
    public UnityEvent onTypingStarted;
    public UnityEvent onTypingFinished;

    string _snapshot = "";
    Coroutine _routine;
    int _visibleCountTarget;

    void Reset() {
        target = GetComponent<TMP_Text>();
    }

    void OnEnable() {
        if (target == null) target = GetComponent<TMP_Text>();
        if (captureOnEnable) CaptureSnapshot();
        if (autoStart) Play();
    }

    // --- Always bake line breaks from the source ---
    public void CaptureSnapshot() {
        var s = source != null ? source : target;
        if (s == null) { _snapshot = ""; return; }

        s.ForceMeshUpdate();
        var ti = s.textInfo;
        var original = s.text;

        List<int> insertAt = new List<int>(ti.lineCount - 1);
        for (int li = 0; li < ti.lineCount - 1; li++) {
            var line = ti.lineInfo[li];
            int lastVis = line.lastVisibleCharacterIndex;
            if (lastVis < 0) continue;
            var ci = ti.characterInfo[lastVis];
            int afterChar = ci.index + 1;
            insertAt.Add(afterChar);
        }

        var sb = new StringBuilder(original);
        for (int i = insertAt.Count - 1; i >= 0; i--) {
            int pos = insertAt[i];
            if (pos >= 0 && pos <= sb.Length) sb.Insert(pos, '\n');
        }
        _snapshot = sb.ToString();
    }

    public void SetSourceAndStart(TMP_Text newSource) {
        source = newSource;
        CaptureSnapshot();
        Play();
    }

    public void Play() {
        if (target == null) return;
        if (string.IsNullOrEmpty(_snapshot)) CaptureSnapshot();
        Stop();
        _routine = StartCoroutine(TypeRoutine());
    }

    public void Stop() {
        if (_routine != null) {
            StopCoroutine(_routine);
            _routine = null;
        }
    }

    public void SkipToEnd() {
        Stop();
        if (target == null) return;
        target.text = _snapshot;
        target.maxVisibleCharacters = int.MaxValue;
        onTypingFinished?.Invoke();
    }

    IEnumerator TypeRoutine() {
        onTypingStarted?.Invoke();

        target.text = _snapshot;
        target.ForceMeshUpdate();
        _visibleCountTarget = target.textInfo.characterCount;
        target.maxVisibleCharacters = 0;

        if (startDelay > 0f) {
            if (useUnscaledTime) yield return new WaitForSecondsRealtime(startDelay);
            else yield return new WaitForSeconds(startDelay);
        }

        float cps = Mathf.Max(0.0001f, charactersPerSecond);
        float secPerChar = 1f / cps;

        int shown = 0;
        while (shown < _visibleCountTarget) {
            shown++;
            target.maxVisibleCharacters = shown;

            if (punctuationPauses) {
                int visIndex = Mathf.Clamp(shown - 1, 0, target.textInfo.characterCount - 1);
                var charInfo = target.textInfo.characterInfo[visIndex];
                if (charInfo.character != '\0' && punctuation.IndexOf(charInfo.character) >= 0) {
                    if (useUnscaledTime) yield return new WaitForSecondsRealtime(secPerChar + punctuationExtraDelay);
                    else yield return new WaitForSeconds(secPerChar + punctuationExtraDelay);
                    continue;
                }
            }

            if (useUnscaledTime) yield return new WaitForSecondsRealtime(secPerChar);
            else yield return new WaitForSeconds(secPerChar);
        }

        onTypingFinished?.Invoke();
        _routine = null;
    }
}
