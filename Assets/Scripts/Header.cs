using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using Fugaku;

[RequireComponent(typeof(RectTransform))]
public class Header : MonoBehaviour
{
	[SerializeField] private ScrollRect m_ScrollRect;
	[SerializeField] private TypefaceAnimator m_HeaderText;
	[SerializeField] private Button m_ReturnButton;
	[SerializeField] private Button m_HamburgerButton;
	[SerializeField] private CircleForceField m_ForceField;
	[SerializeField] private string m_TitleDefault = "PARTY EDIT";
	[SerializeField] private string m_TitleReturn = "RELEASE TO RETURN";

	private float initHeight;
	private Vector2 showReturnButtonPosition = new Vector2(50, 0);
	private Vector2 hideReturnButtonPosition = new Vector2(-50, 0);
	private Vector2 showHamburgerButtonPosition = new Vector2(0, 0);
	private Vector2 hideHamburgerButtonPosition = new Vector2(160, 0);
	private float m_ThresholdReturn = -240;
	private bool m_IsReturn = false;

	public bool isReturn
	{
		get { return m_IsReturn; }
	}

	private RectTransform m_Rect;
	public RectTransform rectTransform
	{
		get {
			if (m_Rect == null)
				m_Rect = GetComponent<RectTransform>();
			return m_Rect;
		}
	}

	void Start ()
	{
		initHeight = rectTransform.sizeDelta.y;
		m_ReturnButton.GetComponent<RectTransform>().anchoredPosition = hideReturnButtonPosition;
		m_HamburgerButton.GetComponent<RectTransform>().anchoredPosition = showHamburgerButtonPosition;
	}

	public void SetHeaderHeight(Vector2 data)
	{
		float deltaY = Mathf.Min(m_ScrollRect.content.localPosition.y, 0);
		rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, initHeight - deltaY);

		float force = deltaY / -1500;
		m_ForceField.force = Mathf.Min(force, 0.2f);

		if(deltaY < m_ThresholdReturn) {

			if(!m_IsReturn) {
				m_IsReturn = true;
				m_HeaderText.GetComponent<Text>().text = m_TitleReturn;
				m_HeaderText.Play();
				m_ReturnButton.GetComponent<RectTransform>().DOAnchorPos(showReturnButtonPosition, 0.2f, true).SetEase(Ease.OutBack);
				m_HamburgerButton.GetComponent<RectTransform>().DOAnchorPos(hideHamburgerButtonPosition, 0.2f, true).SetEase(Ease.OutBack);
			}

		} else {
			
			if(m_IsReturn) {
				m_IsReturn = false;
				m_HeaderText.GetComponent<Text>().text = m_TitleDefault;
				m_HeaderText.Play();
				m_ReturnButton.GetComponent<RectTransform>().DOAnchorPos(hideReturnButtonPosition, 0.2f, true).SetEase(Ease.OutBack);
				m_HamburgerButton.GetComponent<RectTransform>().DOAnchorPos(showHamburgerButtonPosition, 0.2f, true).SetEase(Ease.OutBack);
			}
		}
	}
}