using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class Character : MonoBehaviour
{
	[SerializeField] private ScrollRect m_ScrollRect;
	[SerializeField] private Image m_MainImage;
	[SerializeField] private Text m_NameText;
	[SerializeField] private Text m_HpmpText;
	[SerializeField] private Text m_CommentText;

	private static UIManager uiManager
	{
		get { return FindObjectOfType<UIManager>(); }
	}

	void OnEnable ()
	{
		UIManager.Member member = uiManager.currentMember;

		if(m_NameText)
		{
			m_NameText.text = member.name;
		}

		if(m_HpmpText)
		{
			m_HpmpText.text = "HP " + member.hp + " / MP " + member.mp;
		}

		if(m_CommentText)
		{
			m_CommentText.text = member.comment;
		}

		m_MainImage.sprite = member.large;

		ShowAnimation();
	}

	void ShowAnimation()
	{
		RectTransform m_Main = m_MainImage.GetComponent<RectTransform>();
		m_Main.anchoredPosition = new Vector2(0, -1000);
		m_Main.DOAnchorPosY(0, 0.5f, true).SetEase(Ease.OutExpo);
	}

	public void SetMainImageSize(Vector2 data)
	{
		float deltaY = Mathf.Min(m_ScrollRect.content.localPosition.y, 0);
		float scale = deltaY / -1000;
		m_MainImage.rectTransform.localScale = new Vector3(1 + scale, 1 + scale, 1);
	}
}
