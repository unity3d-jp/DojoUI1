using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace Fugaku
{
	[ExecuteInEditMode]
	[AddComponentMenu("UI/Effects/CircleForceField")]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(RectTransform))]
	public class CircleForceField : UIBehaviour
	{
		[SerializeField, RangeAttribute(-1, 1)] float m_Force  = 0;
		public float force
		{
			get {
				if(!IsActive()) return 0;
				return m_Force;
			}
			set {
				m_Force = value;
			}
		}

		[SerializeField, RangeAttribute(0, 1)] float m_TargetSize  = 0.5f;
		public float targetSize
		{
			get { return m_TargetSize; }
			set {
				m_TargetSize = value;
			}
		}

		[SerializeField] AnimationCurve m_ForceCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
		public AnimationCurve forceCurve
		{
			get { return m_ForceCurve; }
		}

		[System.NonSerialized] private RectTransform m_Rect;
		public RectTransform rectTransform
		{
			get
			{
				if (m_Rect == null)
					m_Rect = GetComponent<RectTransform>();
				return m_Rect;
			}
		}

		public float radius
		{
			get {
				Vector3[] corners = new Vector3[4];
				rectTransform.GetWorldCorners(corners);
				float width = Mathf.Abs(corners[0].x - corners[2].x);
				float height = Mathf.Abs(corners[0].y - corners[2].y);
				return Mathf.Max(width, height) / 2;
			}
		}

		public float localRadius
		{
			get {
				Vector3[] corners = new Vector3[4];
				rectTransform.GetLocalCorners(corners);
				float width = Mathf.Abs(corners[0].x - corners[2].x);
				float height = Mathf.Abs(corners[0].y - corners[2].y);
				return Mathf.Max(width, height) / 2;
			}
		}

		void OnDrawGizmosSelected()
		{
			// エリアの表示
			Color color = new Color(0.7f, 0.9f, 0.2f);
			GizmoUtility.DrawCircle(transform.position, 3, color);
			GizmoUtility.DrawCircle(transform.position, radius, color);

			// オフセットの表示
			color = new Color(0.6f, 0.6f, 0.6f);
			GizmoUtility.DrawCircle(transform.position, radius * m_TargetSize, color);

			// フォースフィールドの表示
			if(m_Force > 0)
			{
				color = new Color(0.9f, 0.4f, 0.6f);
				float val = Mathf.Lerp(m_TargetSize, 1, m_Force);
				GizmoUtility.DrawCircle(transform.position, radius * val, color);

			} else if (m_Force < 0) {
				
				color = new Color(0.5f, 0.7f, 0.95f);
				float val = Mathf.Lerp(-m_TargetSize, -1, -m_Force);
				GizmoUtility.DrawCircle(transform.position, radius * val, color);
			}
		}
	}
}