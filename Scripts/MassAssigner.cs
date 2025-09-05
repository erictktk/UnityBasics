using UnityEngine;
using System;
using System.Reflection;
using System.Collections.Generic;

/*
!!! Summary
MassAssigner is a Unity editor utility that applies randomized float values to a specified field
or property across all child components of the same type as a given reference component.
- Uses reflection to find fields or properties with the given name.
- Random values are seeded and only re-applied if the seed changes.
- Works in edit mode for quick bulk adjustments in the inspector.
*/


namespace Basics {
    [ExecuteInEditMode]
    public class MassAssigner : MonoBehaviour {
        public Component referenceComponent; // For determining type
        public string parameterName;

        public float minValue = 0f;
        public float maxValue = 1f;
        public int seed = 0;

        private int previousSeed = int.MinValue;

        void OnValidate() {
            if (referenceComponent == null || string.IsNullOrEmpty(parameterName))
                return;

            if (seed == previousSeed)
                return;

            ApplyRandomValues();
            previousSeed = seed;
        }

        void ApplyRandomValues() {
            Type targetType = referenceComponent.GetType();
            UnityEngine.Random.InitState(seed);

            Component[] targets = GetComponentsInChildren(targetType, true);

            foreach (Component comp in targets) {
                if (comp == null)
                    continue;

                float randomValue = UnityEngine.Random.Range(minValue, maxValue);
                bool success = TrySetFloat(comp, parameterName, randomValue);

                if (!success)
                    Debug.LogWarning($"Failed to set '{parameterName}' on {comp.name} ({targetType.Name})");
            }
        }

        bool TrySetFloat(Component comp, string name, float val) {
            Type type = comp.GetType();
            FieldInfo field = type.GetField(name);
            if (field != null && field.FieldType == typeof(float)) {
                field.SetValue(comp, val);
                return true;
            }

            PropertyInfo prop = type.GetProperty(name);
            if (prop != null && prop.PropertyType == typeof(float) && prop.CanWrite) {
                prop.SetValue(comp, val, null);
                return true;
            }

            return false;
        }
    }
}

