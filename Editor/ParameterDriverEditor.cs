// !!!ID: 88b935b054f54086aa2c447673a842fb
using UnityEngine;
using UnityEditor;

namespace Basics {
    [CustomEditor(typeof(ParameterDriver))]
    public class ParameterDriverEditor : Editor {
        
        SerializedProperty behaviour, component, parameterName;
        SerializedProperty driverType, transitionType;

        SerializedProperty stopAfter;
        SerializedProperty setInitAfterDelay;
        SerializedProperty sc_Rate, sc_InitVal, sc_Delay;
        SerializedProperty m_Period, m_MinVal, m_MaxVal, m_Delay, m_Offset;
        SerializedProperty p_Delay, p_Period, p_MinVal, p_MaxVal, p_TimeOffset;
        SerializedProperty sw_Delay, sw_InitVal, sw_MaxVal;
        SerializedProperty ss_Delay, ss_Duration, ss_InitVal, ss_FinalVal;
        SerializedProperty rs_Delay, rs_Period, rs_MinVal, rs_MaxVal, rs_Scale;

        void OnEnable() {
            behaviour = serializedObject.FindProperty("behaviour");
            component = serializedObject.FindProperty("component");
            parameterName = serializedObject.FindProperty("parameterName");
            driverType = serializedObject.FindProperty("driverType");
            transitionType = serializedObject.FindProperty("transitionType");


            stopAfter = serializedObject.FindProperty("stopAfter");
            setInitAfterDelay = serializedObject.FindProperty("setInitAfterDelay");


            sc_Rate = serializedObject.FindProperty("sc_Rate");
            sc_InitVal = serializedObject.FindProperty("sc_InitVal");
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

            EditorGUILayout.LabelField("Behaviour or Component:", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(behaviour);
            EditorGUILayout.PropertyField(component);
            EditorGUILayout.PropertyField(parameterName);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Type:", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(driverType);
            EditorGUILayout.PropertyField(transitionType);
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(stopAfter);
            EditorGUILayout.PropertyField(setInitAfterDelay);
            EditorGUILayout.Space();


            var type = (ParameterDriver.DriverType)driverType.enumValueIndex;

            switch (type) {
                case ParameterDriver.DriverType.ShiftConstant:
                    EditorGUILayout.LabelField("Shift Constant", EditorStyles.boldLabel);
                    Draw("Initial Value", sc_InitVal);
                    Draw("Delay", sc_Delay);
                    Draw("Rate", sc_Rate);
                    break;
                case ParameterDriver.DriverType.Modulo:
                    EditorGUILayout.LabelField("Modulo", EditorStyles.boldLabel);
                    Draw("Delay", m_Delay);
                    Draw("Period", m_Period);
                    Draw("Min Value", m_MinVal);
                    Draw("Max Value", m_MaxVal);
                    Draw("Offset", m_Offset);
                    break;
                case ParameterDriver.DriverType.Periodic:
                    EditorGUILayout.LabelField("Periodic", EditorStyles.boldLabel);
                    Draw("Delay", p_Delay);
                    Draw("Period", p_Period);
                    Draw("Min Value", p_MinVal);
                    Draw("Max Value", p_MaxVal);
                    Draw("Time Offset", p_TimeOffset);
                    break;
                case ParameterDriver.DriverType.Switch:
                    EditorGUILayout.LabelField("Switch", EditorStyles.boldLabel);
                    Draw("Delay", sw_Delay);
                    Draw("Initial Value", sw_InitVal);
                    Draw("Final Value", sw_MaxVal);
                    break;
                case ParameterDriver.DriverType.StepSmooth:
                    EditorGUILayout.LabelField("Step Smooth", EditorStyles.boldLabel);
                    Draw("Delay", ss_Delay);
                    Draw("Duration", ss_Duration);
                    Draw("Initial Value", ss_InitVal);
                    Draw("Final Value", ss_FinalVal);
                    break;
                case ParameterDriver.DriverType.RandomStep:
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
