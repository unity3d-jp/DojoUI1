using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Fugaku
{
	[AddComponentMenu("UI/Effects/Subdivide")]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(MaskableGraphic))]
	public class Subdivide : BaseMeshEffect
	{
		[SerializeField, RangeAttribute(1, 16)]
		private int m_NumberX = 1;
		public int numberX
		{
			get { return m_NumberX; }
			set
			{
				m_NumberX = value;
				if (graphic != null)
					graphic.SetVerticesDirty();
			}
		}

		[SerializeField, RangeAttribute(1, 16)]
		private int m_NumberY = 1;
		public int numberY
		{
			get { return m_NumberY; }
			set
			{
				m_NumberY = value;
				if (graphic != null)
					graphic.SetVerticesDirty();
			}
		}

		[System.NonSerialized]
		private RectTransform m_Rect;
		private RectTransform rectTransform
		{
			get
			{
				if (m_Rect == null)
					m_Rect = GetComponent<RectTransform>();
				return m_Rect;
			}
		}

		protected Subdivide(){}

		#if UNITY_EDITOR
		protected override void OnValidate()
		{
			numberX = m_NumberX;
			numberY = m_NumberY;
			base.OnValidate();
		}
		#endif

		public override void ModifyMesh(VertexHelper vh)
		{
			if (!IsActive())
				return;

			var output = ListPool<UIVertex>.Get();
			vh.GetUIVertexStream(output);
			ApplyVerts(output, m_NumberX, m_NumberY);
			vh.Clear();
			vh.AddUIVertexTriangleStream(output);
			ListPool<UIVertex>.Release(output);
		}

		protected void ApplyVerts(List<UIVertex> verts, int numX, int numY)
		{
			UIVertex vt;

			int initVertsNumber = verts.Count;
			var neededCpacity = initVertsNumber * numX * numY;
			if (verts.Capacity < neededCpacity)
				verts.Capacity = neededCpacity;

			Vector2 sizeDelta = rectTransform.sizeDelta;

			// 1辺の大きさ
			Vector2 size = new Vector2(sizeDelta.x / numX, sizeDelta.y / numY);

			// UVの一編の割合
			Vector2 uvSize = new Vector2(verts[4].uv0.x - verts[0].uv0.x, verts[1].uv0.y - verts[0].uv0.y);

			// ずらす値
			Vector2 pivot = rectTransform.pivot - new Vector2(0.5f, 0.5f);
			Vector2 offset = (size - sizeDelta) / 2 + new Vector2(pivot.x * size.x, pivot.y * size.y) - new Vector2(sizeDelta.x * pivot.x, sizeDelta.y * pivot.y);
			Vector3 basePoint = Vector3.zero;

			// まず全てのセル（板ポリ）を作る
			for (int i = 0; i < initVertsNumber; ++i)
			{
				vt = verts[i];

				//頂点の編集
				Vector3 v = vt.position;
				v.x /= numX;
				v.y /= numY;
				v.x += offset.x;
				v.y += offset.y;
				vt.position = v;

				//UV座標の編集
				Vector3 uv = vt.uv0;
				int num = i % initVertsNumber;

				if (num == 0)
				{
					uv = vt.uv0;
					basePoint = uv;

				} else if (num == 1) { 

					uv = basePoint + new Vector3(0, 1f / numY * uvSize.y, 0);

				} else if(num == 2 || num == 3) {

					uv = basePoint + new Vector3(1f / numX * uvSize.x, 1f / numY * uvSize.y, 0);

				} else if (num == 4) {

					uv = basePoint + new Vector3(1f / numX * uvSize.x, 0, 0);

				} else if (num == 5) {

					uv = basePoint;
				}

				vt.uv0 = uv;
				verts[i] = vt;
			}

			// 2つ目の以降を配置しなおす
			for (int i = initVertsNumber; i < neededCpacity; ++i)
			{
				int vertsNumber = i % initVertsNumber;
				int group = i / initVertsNumber;
				vt = verts[vertsNumber];
				verts.Add(vt);

				//頂点の編集
				Vector3 v = vt.position;
				v.x += group % numX * size.x;
				v.y += group / numX * size.y;
				vt.position = v;

				//UV座標の編集
				Vector3 uv = vt.uv0;
				uv += new Vector3((float)(group % numX) / numX * uvSize.x, (float)(group / numX) / numY * uvSize.y, 0);
				vt.uv0 = uv;

				verts[i] = vt;
			}
		}
	}
}
