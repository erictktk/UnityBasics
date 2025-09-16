// !!!ID: 8921ddf4b9ce4905bd14149f60c4241f
using UnityEngine;

/// <summary>
/// Custom property attribute for conditionally hiding fields in the Unity Inspector.
/// Supports two modes:
/// - Boolean field: shows the property if the condition field is true.
/// - Integer/Enum field: shows the property if the condition field equals a given compareValue.
/// </summary>
public class ConditionalHideAttribute : PropertyAttribute
{
    public string conditionField;
    public int compareValue; // for enums or ints
    public bool useCompareValue;

    // bool version (show if true)
    public ConditionalHideAttribute(string conditionField)
    {
        this.conditionField = conditionField;
        this.useCompareValue = false;
    }

    // enum/int version (show if == compareValue)
    public ConditionalHideAttribute(string conditionField, int compareValue)
    {
        this.conditionField = conditionField;
        this.compareValue = compareValue;
        this.useCompareValue = true;
    }
}
