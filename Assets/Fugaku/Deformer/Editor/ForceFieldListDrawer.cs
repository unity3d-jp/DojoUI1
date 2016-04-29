using UnityEngine;
using UnityEditor;

namespace Fugaku
{
	[CustomPropertyDrawer(typeof(Deformer.ForceField))]
	public class ForceFieldListDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			float defaultWidth = position.width;
			position.width = 18;
			bool currentIsOn = EditorGUI.Toggle(position, property.FindPropertyRelative("isOn").boolValue);
			property.FindPropertyRelative("isOn").boolValue = currentIsOn;

			if(currentIsOn)
			{
			}

			position.x += position.width;
			position.width = defaultWidth - 18;
			EditorGUI.PropertyField(position, property.FindPropertyRelative("forceField"), GUIContent.none);
		}
	}
}