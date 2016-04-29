//----------------------------------------------
// Typeface Animator 
// Copyright © 2015 Fugaku
//----------------------------------------------

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TypefaceAnimator), true)]
public class TypefaceAnimatorEditor : Editor
{
	public override void OnInspectorGUI ()
	{
		GUILayout.Space(5f);
		//base.OnInspectorGUI();
		DrawProperties();
	}
	
	protected void DrawProperties ()
	{
		TypefaceAnimator ta = target as TypefaceAnimator;
		
		EditorGUIUtility.labelWidth = 120f;
		GUI.changed = false;

		TypefaceAnimator.TimeMode timeMode = (TypefaceAnimator.TimeMode)EditorGUILayout.EnumPopup("Time Mode", ta.timeMode);

		if(timeMode == TypefaceAnimator.TimeMode.Time)
		{
			GUILayout.BeginHorizontal();
			float duration = EditorGUILayout.FloatField("Duration", ta.duration, GUILayout.Width(160f));
			GUILayout.Label("seconds");
			GUILayout.EndHorizontal();

			if (GUI.changed)
			{
				TypefaceTools.RegisterUndo("Time Change", ta);
				ta.duration =  Mathf.Max(duration, 0.01f);
				EditorUtility.SetDirty(ta);
			}

		} else if (timeMode == TypefaceAnimator.TimeMode.Speed) {

			float speed = EditorGUILayout.FloatField("Speed", ta.speed, GUILayout.Width(160f));

			if (GUI.changed)
			{
				TypefaceTools.RegisterUndo("Speed Change", ta);
				ta.speed = Mathf.Max(speed, 0.01f);
				EditorUtility.SetDirty(ta);
			}
		}

		GUILayout.BeginHorizontal();
		float delay = EditorGUILayout.FloatField("Start Delay", ta.delay, GUILayout.Width(160f));
		GUILayout.Label("seconds");
		GUILayout.EndHorizontal();
		
		TypefaceAnimator.Style style = (TypefaceAnimator.Style)EditorGUILayout.EnumPopup("Play Style", ta.style);
		bool playOnAwake = EditorGUILayout.Toggle("Play On Awake", ta.playOnAwake);
		float progress = EditorGUILayout.Slider("Progress", ta.progress, 0f, 1f);

		if (GUI.changed)
		{
			TypefaceTools.RegisterUndo("Base Change", ta);
			ta.timeMode = timeMode;
			ta.delay = Mathf.Max(delay, 0f);
			ta.style = style;
			ta.playOnAwake = playOnAwake;
			ta.progress = progress;
			EditorUtility.SetDirty(ta);
		}
		
		if (TypefaceTools.DrawHeader("Position", ref ta.usePosition))
		{
			ta.usePosition = true;
			
			Vector3 from = EditorGUILayout.Vector3Field("From", ta.positionFrom);
			Vector3 to = EditorGUILayout.Vector3Field("To", ta.positionTo);
			AnimationCurve positionAnimationCurve = EditorGUILayout.CurveField("Animation Curve", ta.positionAnimationCurve);
			float positionSeparation = EditorGUILayout.Slider("Separation", ta.positionSeparation, 0f, 1f);
			
			if (GUI.changed)
			{
				TypefaceTools.RegisterUndo("Position Change", ta);
				ta.positionFrom = from;
				ta.positionTo = to;
				ta.positionAnimationCurve = positionAnimationCurve;
				ta.positionSeparation = positionSeparation;
				EditorUtility.SetDirty(ta);
			}
		}
		
		if (TypefaceTools.DrawHeader("Rotation", ref ta.useRotation))
		{
			ta.useRotation = true;
			
			float from = EditorGUILayout.FloatField("From", ta.rotationFrom);
			float to = EditorGUILayout.FloatField("To", ta.rotationTo);
			Vector2 pivot = EditorGUILayout.Vector2Field("Pivot", ta.rotationPivot);
			AnimationCurve rotationAnimationCurve = EditorGUILayout.CurveField("Animation Curve", ta.rotationAnimationCurve);
			float rotationSeparation = EditorGUILayout.Slider("Separation", ta.rotationSeparation, 0f, 1f);
			
			if (GUI.changed)
			{
				TypefaceTools.RegisterUndo("Rotaion Change", ta);
				ta.rotationFrom = from;
				ta.rotationTo = to;
				ta.rotationPivot = new Vector2(Mathf.Clamp01(pivot.x), Mathf.Clamp01(pivot.y));
				ta.rotationAnimationCurve = rotationAnimationCurve;
				ta.rotationSeparation = rotationSeparation;
				EditorUtility.SetDirty(ta);
			}
		}
		
		if (TypefaceTools.DrawHeader("Scale", ref ta.useScale))
		{
			ta.useScale = true;

			var boldtext = new GUIStyle (GUI.skin.label);
			boldtext.fontStyle = FontStyle.Bold;
			bool scaleSyncXY = EditorGUILayout.Toggle("Sync XY", ta.scaleSyncXY);

			if(scaleSyncXY)
			{
				float from = EditorGUILayout.FloatField("From", ta.scaleFrom);
				float to = EditorGUILayout.FloatField("To", ta.scaleTo);
				Vector2 pivot = EditorGUILayout.Vector2Field("Pivot", ta.scalePivot);
				AnimationCurve scaleAnimationCurve = EditorGUILayout.CurveField("Animation Curve", ta.scaleAnimationCurve);
				float scaleSeparation = EditorGUILayout.Slider("Separation", ta.scaleSeparation, 0f, 1f);
				
				if (GUI.changed)
				{
					TypefaceTools.RegisterUndo("Scale Change", ta);
					ta.scaleSyncXY = scaleSyncXY;
					ta.scaleFrom = from;
					ta.scaleTo = to;
					ta.scalePivot = new Vector2(Mathf.Clamp01(pivot.x), Mathf.Clamp01(pivot.y));
					ta.scaleAnimationCurve = scaleAnimationCurve;
					ta.scaleSeparation = scaleSeparation;
					EditorUtility.SetDirty(ta);
				}
				
			} else {

				EditorGUILayout.LabelField("Scalse X", boldtext);
				float from = EditorGUILayout.FloatField("      From", ta.scaleFrom);
				float to = EditorGUILayout.FloatField("      To", ta.scaleTo);
				Vector2 pivot = EditorGUILayout.Vector2Field("      Pivot", ta.scalePivot);
				AnimationCurve scaleAnimationCurve = EditorGUILayout.CurveField("      Animation Curve", ta.scaleAnimationCurve);
				EditorGUILayout.LabelField("Scalse Y", boldtext);
				float fromY = EditorGUILayout.FloatField("      From", ta.scaleFromY);
				float toY = EditorGUILayout.FloatField("      To", ta.scaleToY);
				Vector2 pivotY = EditorGUILayout.Vector2Field("      Pivot", ta.scalePivotY);
				AnimationCurve scaleAnimationCurveY = EditorGUILayout.CurveField("      Animation Curve", ta.scaleAnimationCurveY);
				float scaleSeparation = EditorGUILayout.Slider("Separation", ta.scaleSeparation, 0f, 1f);

				if (GUI.changed)
				{
					TypefaceTools.RegisterUndo("Scale Change", ta);
					ta.scaleSyncXY = scaleSyncXY;
					ta.scaleFrom = from;
					ta.scaleTo = to;
					ta.scalePivot = new Vector2(Mathf.Clamp01(pivot.x), Mathf.Clamp01(pivot.y));
					ta.scaleAnimationCurve = scaleAnimationCurve;
					ta.scaleFromY = fromY;
					ta.scaleToY = toY;
					ta.scalePivotY = new Vector2(Mathf.Clamp01(pivotY.x), Mathf.Clamp01(pivotY.y));
					ta.scaleAnimationCurveY = scaleAnimationCurveY;
					ta.scaleSeparation = scaleSeparation;
					EditorUtility.SetDirty(ta);
				}
			}
			
		}
		
		if (TypefaceTools.DrawHeader("Alpha", ref ta.useAlpha))
		{
			ta.useAlpha = true;
			
			float from = EditorGUILayout.Slider("From", ta.alphaFrom, 0f, 1f);
			float to = EditorGUILayout.Slider("To", ta.alphaTo, 0f, 1f);
			AnimationCurve alphaAnimationCurve = EditorGUILayout.CurveField("Animation Curve", ta.alphaAnimationCurve);
			float alphaSeparation = EditorGUILayout.Slider("Separation", ta.alphaSeparation, 0f, 1f);
			
			if (GUI.changed)
			{
				TypefaceTools.RegisterUndo("Alpha Change", ta);
				ta.alphaFrom = from;
				ta.alphaTo = to;
				ta.alphaAnimationCurve = alphaAnimationCurve;
				ta.alphaSeparation = alphaSeparation;
				EditorUtility.SetDirty(ta);
			}
		}
		
		if (TypefaceTools.DrawHeader("Color", ref ta.useColor))
		{
			ta.useColor = true;
			
			Color from = EditorGUILayout.ColorField("From", ta.colorFrom);
			Color to = EditorGUILayout.ColorField("To", ta.colorTo);
			AnimationCurve colorAnimationCurve = EditorGUILayout.CurveField("Animation Curve", ta.colorAnimationCurve);
			float colorSeparation = EditorGUILayout.Slider("Separation", ta.colorSeparation, 0f, 1f);
			
			if (GUI.changed)
			{
				TypefaceTools.RegisterUndo("Color Change", ta);
				ta.colorFrom = from;
				ta.colorTo = to;
				ta.colorAnimationCurve = colorAnimationCurve;
				ta.colorSeparation = colorSeparation;
				EditorUtility.SetDirty(ta);
			}
		}

		EditorGUILayout.PropertyField(serializedObject.FindProperty("onStart"), new GUIContent("On Start"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("onComplete"), new GUIContent("On Complete"));
	}
}
