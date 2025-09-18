// !!!ID: 181ea385c0244472a3f097c22fa63d6e
// GenericAnimationEditor.cs
using UnityEditor;
using UnityEngine;

namespace Basic {
    [CustomEditor(typeof(GenericAnimation))]
    public class GenericAnimationEditor : Editor {
        private bool _showAllFramerates;

        public override void OnInspectorGUI() {
            serializedObject.Update();

            var namesProp = serializedObject.FindProperty(nameof(GenericAnimation.animationNames));
            var framesProp = serializedObject.FindProperty(nameof(GenericAnimation.animationFrames));
            var modeProp = serializedObject.FindProperty(nameof(GenericAnimation.framerateMode));
            var uniFpsProp = serializedObject.FindProperty(nameof(GenericAnimation.universalFramerate));
            var multiFpsProp = serializedObject.FindProperty(nameof(GenericAnimation.multipleFramerates));

            EditorGUILayout.PropertyField(namesProp, true);
            EditorGUILayout.PropertyField(framesProp, true);
            EditorGUILayout.PropertyField(modeProp);

            var mode = (GenericAnimation.FramerateMode)modeProp.enumValueIndex;
            if (mode == GenericAnimation.FramerateMode.Universal)
                EditorGUILayout.PropertyField(uniFpsProp);
            else
                EditorGUILayout.PropertyField(multiFpsProp, true);

            _showAllFramerates = EditorGUILayout.Foldout(_showAllFramerates, "Show All Framerate Fields");
            if (_showAllFramerates) {
                EditorGUILayout.PropertyField(uniFpsProp);
                EditorGUILayout.PropertyField(multiFpsProp, true);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
