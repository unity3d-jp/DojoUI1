using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace Fugaku
{
	[ExecuteInEditMode]
	[AddComponentMenu("UI/Effects/Deformer")]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Subdivide))]
	public class Deformer : BaseMeshEffect
	{
		public List<ForceField> m_ForceFields;

		[System.Serializable]
		public class ForceField
		{
			public CircleForceField forceField;
			public bool isOn = false;
		}

		[System.NonSerialized] private RectTransform m_Rect;
		private RectTransform rectTransform
		{
			get
			{
				if (m_Rect == null)
					m_Rect = GetComponent<RectTransform>();
				return m_Rect;
			}
		}

		protected Deformer(){}

		#if UNITY_EDITOR
		protected override void OnValidate()
		{
			base.OnValidate();
		}
		#endif

		protected override void Start()
		{
			base.Start();
		}

		void Update()
		{
			if (graphic != null)
				graphic.SetVerticesDirty();
		}

		void OnDrawGizmosSelected()
		{
			if(m_ForceFields == null) return;
			if(m_ForceFields.Count <= 0) return;

			foreach (var force in m_ForceFields)
			{
				if(force.forceField == null) break;
				Gizmos.color = new Color(0.7f, 0.9f, 0.2f);
				Gizmos.DrawLine(transform.position, force.forceField.transform.position);
			}
		}

		public override void ModifyMesh(VertexHelper vh)
		{
			if (!IsActive())
				return;

			var output = ListPool<UIVertex>.Get();
			vh.GetUIVertexStream(output);

			DeformAll(output);

			vh.Clear();
			vh.AddUIVertexTriangleStream(output);
			ListPool<UIVertex>.Release(output);
		}

		// TODO セル個々の描画順をコントロールする方法はまだ模索中。現在はSubdivideに準拠し、左下から右上に順番に並んでいる
		void DeformAll(List<UIVertex> verts)
		{
			if(m_ForceFields == null) return;
			if(m_ForceFields.Count <= 0) return;

			foreach (var force in m_ForceFields)
			{
				if(force.isOn)
				{
					DeformEffect(force.forceField, verts);
				}
			}

		}

		void DeformEffect(CircleForceField forceField, List<UIVertex> verts)
		{
			if(forceField == null) return;
			//if(forceField.force <= 0) return;

			UIVertex vt;

			for (int i = 0; i < verts.Count; ++i)
			{
				vt = verts[i];
				Vector3 pos = vt.position;

				// ターゲットとなる場所
				Vector3 targetPosition = pos;

				// フォースフィールドの自分から見た相対座標
				Vector3 forceFieldPosition = transform.InverseTransformPoint(forceField.transform.position);

				// フォースフィールドの中心から各頂点までの距離
				Vector3 distanceFromForceCenter = forceFieldPosition - pos;

				// 引力の計算
				if(forceField.force > 0)
				{
					//フォースフィールドの中心から各頂点までの距離を半径で割り、アニメーションカーブで欠けることを見越して0.5を引いておく
					float rate = distanceFromForceCenter.magnitude / forceField.localRadius - 0.5f;

					//目指すべきターゲットの場所
					targetPosition = forceFieldPosition + pos * forceField.localRadius * 2 / Mathf.Max(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y) * forceField.targetSize;

					//ラープで均しておく
					targetPosition = Vector3.LerpUnclamped(targetPosition, pos, forceField.forceCurve.Evaluate(rate * 2));

					//最後にフォースの力を考慮
					pos = Vector3.LerpUnclamped(vt.position, targetPosition, forceField.force);


				// TODO 斥力の計算を直すべし
				} else if (forceField.force < 0) {

					if(distanceFromForceCenter.magnitude < forceField.localRadius)
					{
						//フォースフィールドの中心から各頂点までの距離を半径で割り、アニメーションカーブで欠けることを見越して0.5を引いておく
						float rate = distanceFromForceCenter.magnitude / forceField.localRadius - 0.5f;

						//目指すべきターゲットの場所
						targetPosition = forceFieldPosition + pos * forceField.localRadius * 2 / Mathf.Max(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y) * forceField.targetSize;

						//ラープで均しておく
						//targetPosition = Vector3.LerpUnclamped(targetPosition, pos, 2 - forceField.forceCurve.Evaluate(rate * 2));
						targetPosition = Vector3.LerpUnclamped(targetPosition, pos, forceField.forceCurve.Evaluate(rate * 2));

						//最後にフォースの力を考慮
						//pos = Vector3.LerpUnclamped(vt.position, targetPosition, Mathf.Abs(forceField.force));
						pos = Vector3.LerpUnclamped(vt.position, targetPosition, forceField.force);

					}
				}

				vt.position = pos;
				verts[i] = vt;
			}
		}
	}
}