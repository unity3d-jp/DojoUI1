using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace Fugaku
{
	[CustomEditor (typeof(Deformer))]
	public class DeformerInspector : Editor
	{
		ReorderableList reorderableList;

		void OnEnable ()
		{
			var prop = serializedObject.FindProperty ("m_ForceFields");
			reorderableList = new ReorderableList (serializedObject, prop);
			reorderableList.drawElementCallback = (rect, index, isActive, isFocused) => {
				var element = prop.GetArrayElementAtIndex (index);
				rect.height += -4;
				rect.y += 2;
				EditorGUI.PropertyField (rect, element);
			};
		}

		public override void OnInspectorGUI ()
		{
			serializedObject.Update ();

			GUILayout.Space(5f);

			// リスト・配列の変更可能なリストの表示
			reorderableList.DoLayoutList();

			serializedObject.ApplyModifiedProperties();
		}
	}
}