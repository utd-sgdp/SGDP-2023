#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Game.SoundSystem.Editor
{
    [CustomPropertyDrawer(typeof(RangedFloat), true)]
    public class RangedFloatDrawer : PropertyDrawer
    {
        private const float RangeBoundsLabelWidth = 50f;
        private const float Gap = 4f;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            label = EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, label);

            SerializedProperty minProp = property.FindPropertyRelative("MinValue");
            SerializedProperty maxProp = property.FindPropertyRelative("MaxValue");

            float minValue = Mathf.Round(minProp.floatValue * 10000) * 0.0001f;
            float maxValue = Mathf.Round(maxProp.floatValue * 10000) * 0.0001f;

            float rangeMin = 0;
            float rangeMax = 1;

            var ranges = (MinMaxRangeAttribute[])fieldInfo.GetCustomAttributes(typeof(MinMaxRangeAttribute), true);
            if (ranges.Length > 0) {
                rangeMin = ranges[0].Min;
                rangeMax = ranges[0].Max;
            }

            var rangeBoundsLabel1Rect = new Rect(position) { width = RangeBoundsLabelWidth - Gap };
            position.xMin += RangeBoundsLabelWidth + Gap;

            var rangeBoundsLabel2Rect = new Rect(position);
            rangeBoundsLabel2Rect.xMin = rangeBoundsLabel2Rect.xMax - RangeBoundsLabelWidth + Gap;
            position.xMax -= RangeBoundsLabelWidth + Gap;


            EditorGUI.BeginChangeCheck();

            var minValue2 = EditorGUI.DelayedFloatField(rangeBoundsLabel1Rect, minValue);
            if (position.xMax > position.xMin + 10) {
                EditorGUI.MinMaxSlider(position, ref minValue, ref maxValue, rangeMin, rangeMax);
            }
            var maxValue2 = EditorGUI.DelayedFloatField(rangeBoundsLabel2Rect, maxValue);

            if (EditorGUI.EndChangeCheck()) {
                var actualMin = minProp.floatValue;
                var actualMax = maxProp.floatValue;
                float newMin = actualMin;
                float newMax = actualMax;

                // Min changed by slider
                if (CheckDifferent(actualMin, minValue)) {
                    newMin = Mathf.Clamp(minValue, rangeMin, rangeMax);
                    if (newMin > actualMax) newMax = newMin;
                }
                // Min changed by float field
                if (CheckDifferent(actualMin, minValue2)) {
                    newMin = Mathf.Clamp(minValue2, rangeMin, rangeMax);
                    if (newMin > actualMax) newMax = newMin;
                }

                // Max changed by slider
                if (CheckDifferent(actualMax, maxValue)) {
                    newMax = Mathf.Clamp(maxValue, rangeMin, rangeMax);
                    if (newMax < actualMin) newMin = newMax;
                }
                // Max changed by float field
                if (CheckDifferent(actualMax, maxValue2)) {
                    newMax = Mathf.Clamp(maxValue2, rangeMin, rangeMax);
                    if (newMax < actualMin) newMin = newMax;
                }

                minProp.floatValue = newMin;
                maxProp.floatValue = newMax;
            }

            EditorGUI.EndProperty();
        }

        private static bool CheckDifferent(float value1, float value2) {
            return Mathf.Abs(value1 - value2) > 0.00001f;
        }
    }
}
#endif