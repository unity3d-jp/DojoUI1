using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Image))]
public class ImageModifier : BaseMeshEffect
{
	[SerializeField]
	private Vector2 m_Offset0;
	public Vector2 offset0
	{
		get { return m_Offset0; }
		set {
			m_Offset0 = value;
			if (graphic != null)
				graphic.SetVerticesDirty();
		}
	}

	[SerializeField]
	private Vector2 m_Offset1;
	public Vector2 offset1
	{
		get { return m_Offset1; }
		set {
			m_Offset1 = value;
			if (graphic != null)
				graphic.SetVerticesDirty();
		}
	}

	[SerializeField]
	private Vector2 m_Offset2;
	public Vector2 offset2
	{
		get { return m_Offset2; }
		set {
			m_Offset2 = value;
			if (graphic != null)
				graphic.SetVerticesDirty();
		}
	}

	[SerializeField]
	private Vector2 m_Offset3;
	public Vector2 offset3
	{
		get { return m_Offset3; }
		set {
			m_Offset3 = value;
			if (graphic != null)
				graphic.SetVerticesDirty();
		}
	}


#if UNITY_EDITOR
	protected override void OnValidate()
	{
		base.OnValidate();
	}
#endif
	
	protected override void OnEnable()
	{
		base.OnEnable();
	}

	protected override void OnDisable ()
	{
		base.OnDisable();
	}

	public override void ModifyMesh(VertexHelper vertexHelper)
	{
		List<UIVertex> list = new List<UIVertex>();
		vertexHelper.GetUIVertexStream(list);

		UIVertex uiVertex = list[0];
		uiVertex.position = list[0].position + new Vector3(offset3.x, offset3.y, 0);
		list[0] = uiVertex;

		uiVertex = list[1];
		uiVertex.position = list[1].position + new Vector3(offset0.x, offset0.y, 0);
		list[1] = uiVertex;

		uiVertex = list[2];
		uiVertex.position = list[2].position + new Vector3(offset1.x, offset1.y, 0);
		list[2] = uiVertex;

		uiVertex = list[3];
		uiVertex.position = list[3].position + new Vector3(offset1.x, offset1.y, 0);
		list[3] = uiVertex;

		uiVertex = list[4];
		uiVertex.position = list[4].position + new Vector3(offset2.x, offset2.y, 0);
		list[4] = uiVertex;

		uiVertex = list[5];
		uiVertex.position = list[5].position + new Vector3(offset3.x, offset3.y, 0);
		list[5] = uiVertex;

		vertexHelper.Clear();
		vertexHelper.AddUIVertexTriangleStream(list);
	}
}
