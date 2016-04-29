using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GraphicCaster : Graphic
{
	protected override void OnPopulateMesh (VertexHelper vh)
	{
		base.OnPopulateMesh (vh);
		vh.Clear();
	}

	#if UNITY_EDITOR
	[CustomEditor(typeof(GraphicCaster))]
	class GraphicCastEditor : Editor
	{
		public override void OnInspectorGUI() {}
	}
	#endif
} 
