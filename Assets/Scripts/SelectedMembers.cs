using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(RectTransform))]
public class SelectedMembers : MonoBehaviour
{
	[SerializeField] private ScrollRect m_ScrollRect;
	[SerializeField] private BattleMembersStage[] m_Stages;
	private int m_ActiveCount = 0;

	private RectTransform m_Rect;
	public RectTransform rectTransform
	{
		get {
			if (m_Rect == null)
				m_Rect = GetComponent<RectTransform>();
			return m_Rect;
		}
	}

	private static UIManager uiManager
	{
		get { return FindObjectOfType<UIManager>(); }
	}

	void Start (){}

	public void Initialize()
	{
		m_ActiveCount = 0;

		foreach (var stage in m_Stages)
		{
			stage.HideImmediately();
		}
	}

	public void SetPosition(Vector2 data)
	{
		rectTransform.anchoredPosition = new Vector2(0, Mathf.Min(m_ScrollRect.content.anchoredPosition.y, 0) - 160);
	}

	public void AddMember(Card card)
	{
		for (int i = 0; i < m_Stages.Length; i++)
		{
			if(!m_Stages[i].isActive)
			{
				m_Stages[i].Show(card.data.small);
				m_ActiveCount ++;
				card.data.selected = true;
				return;
			}
		}
	}

	public void DeleteMember(Card card)
	{
		int num = FindAtDelete(card.data.small);
		if(num < 0) return;
		m_Stages[num].Hide();
		m_ActiveCount --;
		card.data.selected = false;
	}

	int FindAtDelete(Sprite sprite)
	{
		for (int i = 0; i < m_Stages.Length; i++)
		{
			if(m_Stages[i].isActive && sprite == m_Stages[i].sprite)
			{
				return i;
			}
		}

		return -1;
	}

	public int GetActiveCount()
	{
		return m_ActiveCount;
	}
}