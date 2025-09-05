using UnityEngine;

public class ConditionalHideAttribute : PropertyAttribute {
    public string conditionField;
    public int compareValue; // for enums or ints
    public bool useCompareValue;

    // bool version (show if true)
    public ConditionalHideAttribute(string conditionField) {
        this.conditionField = conditionField;
        this.useCompareValue = false;
    }

    // enum/int version (show if == compareValue)
    public ConditionalHideAttribute(string conditionField, int compareValue) {
        this.conditionField = conditionField;
        this.compareValue = compareValue;
        this.useCompareValue = true;
    }
}
