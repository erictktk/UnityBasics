using UnityEngine;
using UnityEditor;

namespace Basics {
    [CustomEditor(typeof(ParameterDriverTransform))]
    public class ParameterDriverTransformEditor : Editor {

        SerializedProperty resetOnDisable;
        SerializedProperty targetType, parameter, driverType, transitionType, useInitialValue;

        SerializedProperty sc_Rate, sc_InitVal, sc_Delay;
        SerializedProperty m_Period, m_MinVal, m_MaxVal, m_Delay, m_Offset;
        SerializedProperty p_Delay, p_Period, p_MinVal, p_MaxVal, p_TimeOffset;
        SerializedProperty sw_Delay, sw_InitVal, sw_MaxVal;
        SerializedProperty ss_Delay, ss_Duration, ss_InitVal, ss_FinalVal;
        SerializedProperty rs_Delay, rs_Period, rs_MinVal, rs_MaxVal, rs_Scale;

        void OnEnable() {
            resetOnDisable = serializedObject.FindProperty("resetOnDisable");

            targetType = serializedObject.FindProperty("targetType");
            parameter = serializedObject.FindProperty("parameter");
            driverType = serializedObject.FindProperty("driverType");
            transitionType = serializedObject.FindProperty("transitionType");
            useInitialValue = serializedObject.FindProperty("useInitialValue");

            sc_Rate = serializedObject.FindProperty("sc_Rate");
            sc_InitVal = serializedObject.FindProperty("sc_InitVal");
            //sc_
            sc_Delay = serializedObject.FindProperty("sc_Delay");

            m_Period = serializedObject.FindProperty("m_Period");
            m_MinVal = serializedObject.FindProperty("m_MinVal");
            m_MaxVal = serializedObject.FindProperty("m_MaxVal");
            m_Delay = serializedObject.FindProperty("m_Delay");
            m_Offset = serializedObject.FindProperty("m_Offset");

            p_Delay = serializedObject.FindProperty("p_Delay");
            p_Period = serializedObject.FindProperty("p_Period");
            p_MinVal = serializedObject.FindProperty("p_MinVal");
            p_MaxVal = serializedObject.FindProperty("p_MaxVal");
            p_TimeOffset = serializedObject.FindProperty("p_TimeOffset");

            sw_Delay = serializedObject.FindProperty("sw_Delay");
            sw_InitVal = serializedObject.FindProperty("sw_InitVal");
            sw_MaxVal = serializedObject.FindProperty("sw_MaxVal");

            ss_Delay = serializedObject.FindProperty("ss_Delay");
            ss_Duration = serializedObject.FindProperty("ss_Duration");
            ss_InitVal = serializedObject.FindProperty("ss_InitVal");
            ss_FinalVal = serializedObject.FindProperty("ss_FinalVal");

            rs_Delay = serializedObject.FindProperty("rs_Delay");
            rs_Period = serializedObject.FindProperty("rs_Period");
            rs_MinVal = serializedObject.FindProperty("rs_MinVal");
            rs_MaxVal = serializedObject.FindProperty("rs_MaxVal");
            rs_Scale = serializedObject.FindProperty("rs_Scale");
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();

            EditorGUILayout.PropertyField(resetOnDisable);

            EditorGUILayout.LabelField("Target & Parameter", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(targetType);
            EditorGUILayout.PropertyField(parameter);

            var tgt = (ParameterDriverTransform.TargetType)targetType.enumValueIndex;
            var param = (ParameterDriverTransform.Parameter)parameter.enumValueIndex;

            if (tgt == ParameterDriverTransform.TargetType.Transform &&
                (param == ParameterDriverTransform.Parameter.AnchoredPosX ||
                 param == ParameterDriverTransform.Parameter.AnchoredPosY)) {
                EditorGUILayout.HelpBox("AnchoredPosX and AnchoredPosY require a RectTransform target.", MessageType.Error);
            }
            else if (tgt == ParameterDriverTransform.TargetType.RectTransform &&
                (param == ParameterDriverTransform.Parameter.PosX ||
                 param == ParameterDriverTransform.Parameter.PosY ||
                 param == ParameterDriverTransform.Parameter.PosZ)) {
                EditorGUILayout.HelpBox("PosX/Y/Z require a Transform target, not RectTransform.", MessageType.Error);
            }
            else {
                EditorGUILayout.HelpBox("", MessageType.None);
            }

            EditorGUILayout.PropertyField(useInitialValue);




            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Driver Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(driverType);
            EditorGUILayout.PropertyField(transitionType);

            EditorGUILayout.Space();
            var type = (ParameterDriverTransform.DriverType)driverType.enumValueIndex;

            switch (type) {
                case ParameterDriverTransform.DriverType.ShiftConstant:
                    EditorGUILayout.LabelField("Shift Constant", EditorStyles.boldLabel);
                    Draw("Initial Value", sc_InitVal);
                    Draw("Delay", sc_Delay);
                    Draw("Rate", sc_Rate);
                    break;
                case ParameterDriverTransform.DriverType.Modulo:
                    EditorGUILayout.LabelField("Modulo", EditorStyles.boldLabel);
                    Draw("Delay", m_Delay);
                    Draw("Period", m_Period);
                    Draw("Min Value", m_MinVal);
                    Draw("Max Value", m_MaxVal);
                    Draw("Offset", m_Offset);
                    break;
                case ParameterDriverTransform.DriverType.Periodic:
                    EditorGUILayout.LabelField("Periodic", EditorStyles.boldLabel);
                    Draw("Delay", p_Delay);
                    Draw("Period", p_Period);
                    Draw("Min Value", p_MinVal);
                    Draw("Max Value", p_MaxVal);
                    Draw("Time Offset", p_TimeOffset);
                    break;
                case ParameterDriverTransform.DriverType.Switch:
                    EditorGUILayout.LabelField("Switch", EditorStyles.boldLabel);
                    Draw("Delay", sw_Delay);
                    Draw("Initial Value", sw_InitVal);
                    Draw("Final Value", sw_MaxVal);
                    break;
                case ParameterDriverTransform.DriverType.StepSmooth:
                    EditorGUILayout.LabelField("Step Smooth", EditorStyles.boldLabel);
                    Draw("Delay", ss_Delay);
                    Draw("Duration", ss_Duration);
                    Draw("Initial Value", ss_InitVal);
                    Draw("Final Value", ss_FinalVal);
                    break;
                case ParameterDriverTransform.DriverType.RandomStep:
                    EditorGUILayout.LabelField("Random Step", EditorStyles.boldLabel);
                    Draw("Delay", rs_Delay);
                    Draw("Period", rs_Period);
                    Draw("Min Value", rs_MinVal);
                    Draw("Max Value", rs_MaxVal);
                    Draw("Scale", rs_Scale);
                    break;
            }

            serializedObject.ApplyModifiedProperties();
        }

        void Draw(string label, SerializedProperty prop) {
            EditorGUILayout.PropertyField(prop, new GUIContent(label));
        }
    }
}

