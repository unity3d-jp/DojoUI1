//----------------------------------------------
// Typeface Animator 
// Copyright © 2015 Fugaku
//----------------------------------------------

using UnityEditor;
using UnityEngine;

/// <summary>
/// Tools for the editor
/// </summary>

public static class TypefaceTools
{
	static public void RegisterUndo (string name, params Object[] objects)
	{
		if (objects != null && objects.Length > 0)
		{
			UnityEditor.Undo.RecordObjects(objects, name);
			
			foreach (Object obj in objects)
			{
				if (obj == null) continue;
				EditorUtility.SetDirty(obj);
			}
		}
	}

	static public bool DrawHeader (string key, ref bool target)
	{
		GUILayout.Space(5f);
		GUI.backgroundColor = new Color(1f, 1f, 1f);
		
		GUILayout.BeginHorizontal();
		string text = "<b><size=12>" + key + "</size></b>";
		string textColor = (EditorGUIUtility.isProSkin) ? "eeeeee" : "333333";
		if (target) text = "<color=#" + textColor + "ff>\u25BC " + text + "</color>";
		else text = "<color=#" + textColor + "66>\u25BA " + text + "</color>";
		if (!GUILayout.Toggle(true, text, "dragtabbright", GUILayout.Height(20f))) target = !target;
		GUILayout.EndHorizontal();
		
		return target;
	}
}