using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class DragDownToReturn : MonoBehaviour, IEndDragHandler
{
	[SerializeField] private Header m_Header;

	private static UIManager uiManager
	{
		get { return FindObjectOfType<UIManager>(); }
	}

	public void OnEndDrag(PointerEventData data)
	{
		//ヘッダがリターンを示せば、元のコンテンツである地図に戻る
		if(m_Header.isReturn)
		{
			uiManager.SwitchContentToPartiyEdit();
		}
	}
}
