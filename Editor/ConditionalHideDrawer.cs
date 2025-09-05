#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ConditionalHideAttribute))]
public class ConditionalHideDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        if (ShouldShow(property)) {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return ShouldShow(property)
            ? EditorGUI.GetPropertyHeight(property, label, true)
            : -EditorGUIUtility.standardVerticalSpacing;
    }

    private bool ShouldShow(SerializedProperty property) {
        ConditionalHideAttribute cond = (ConditionalHideAttribute)attribute;
        SerializedProperty conditionProp = property.serializedObject.FindProperty(cond.conditionField);

        if (conditionProp == null)
            return true; // fallback = always show

        if (cond.useCompareValue) {
            return conditionProp.propertyType == SerializedPropertyType.Enum ||
                   conditionProp.propertyType == SerializedPropertyType.Integer
                ? conditionProp.intValue == cond.compareValue
                : true;
        }
        else {
            return conditionProp.propertyType == SerializedPropertyType.Boolean
                ? conditionProp.boolValue
                : true;
        }
    }
}
#endif
