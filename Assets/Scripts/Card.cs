using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

[RequireComponent(typeof(RectTransform))]
public class Card : MonoBehaviour
{
	[SerializeField] private Image m_Body;
	[SerializeField] private Image m_ActiveBackground;
	[SerializeField] private Image m_Cover;
	[SerializeField] private Image m_UnderCard;
	[SerializeField] private Image m_Face;
	[SerializeField] private Shadow m_FaceShadow;
	[SerializeField] private TypefaceAnimator m_NameText;
	[SerializeField] private Text m_HpmpText;
	[SerializeField] private Color m_TextColor;
	[SerializeField] private Color m_TextColorActive;
	private UIManager.Member m_Data;
	public UIManager.Member data
	{
		get { return m_Data; }
	}

	private Color m_CardColor;
	private Color m_ActiveColor;
	const float m_ShowThreshold = -1140;
	const float m_HideThreshold = -1260;
	const float m_Height = 250;
	public static float height
	{
		get { return m_Height; }
	}

	const float m_ThresholdHorizontal = 150;
	const float m_DelayRate = 0.1f;
	private State m_CurrentState = State.Defection;
	public State currentState
	{
		get { return m_CurrentState; }
		set { m_CurrentState = value; }
	}

	public enum State
	{
		Defection, Join, Deleted
	}

	private HorizontalScrollMode m_currentHorizontalScrollMode = HorizontalScrollMode.Non;
	private enum HorizontalScrollMode
	{
		Non,
		Right
	}

	private bool m_IsActive = true;
	public bool isActive
	{
		get { return m_IsActive; }
		private set {
			m_IsActive = value;
			if(m_Body)
				m_Body.gameObject.SetActive(m_IsActive);
			if(m_Cover)
				m_Cover.gameObject.SetActive(m_IsActive);
		}
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

	private State SetJoin()
	{
		m_ActiveBackground.gameObject.SetActive(true);
		if(m_FaceShadow)
			m_FaceShadow.enabled = true;
		if(m_NameText)
			m_NameText.GetComponent<Text>().color = m_TextColorActive;
		if(m_HpmpText)
			m_HpmpText.color = m_TextColorActive;
		return State.Join;
	}

	private State SetDefection()
	{
		m_ActiveBackground.gameObject.SetActive(false);
		if(m_FaceShadow)
			m_FaceShadow.enabled = false;
		if(m_NameText)
			m_NameText.GetComponent<Text>().color = m_TextColor;
		if(m_HpmpText)
			m_HpmpText.color = m_TextColor;
		return State.Defection;
	}

	//初期化してアニメーション実行
	public void SetUp(int order, UIManager.Member data, Color cardColor, Color activeColor)
	{
		m_Data = data;
		data.selected = false;

		if(m_NameText)
		{
			m_NameText.GetComponent<Text>().text = m_Data.name;
		}

		if(m_HpmpText)
		{
			m_HpmpText.text = "HP " + m_Data.hp + " / MP " + m_Data.mp;
		}

		if(m_Face)
		{
			m_Face.sprite = m_Data.large;
			m_Face.SetNativeSize();
		}

		this.m_CardColor = cardColor;
		this.m_ActiveColor = activeColor;

		m_Body.color = cardColor;
		m_ActiveBackground.color = activeColor;
		m_UnderCard.color = cardColor;

		isActive = false;
		currentState = SetDefection();
		m_Body.transform.localPosition = Vector3.zero;

		if(rectTransform.anchoredPosition.y > m_HideThreshold)
		{
			PlayAnimation(order);
		}
	}

	//出現アニメーション
	void PlayAnimation(int delayOrder)
	{
		isActive = true;

		m_Cover.color = new Color(0, 0, 0, 0.6f);
		rectTransform.localEulerAngles = new Vector3(75, 0, 0);

		m_Cover.DOFade(0, 0.8f).SetDelay(delayOrder * m_DelayRate);
		transform.DOLocalRotate(Vector3.zero, 1.0f).SetDelay(delayOrder * m_DelayRate).SetEase(Ease.OutElastic);
	}

	//上下のスクロール
	public void SetRotation(float contentY, Vector2 velocity)
	{
		float currentY = contentY + rectTransform.anchoredPosition.y;

		//下にスクロールしていく場合、閾値をまたいだタイミングでTweenアニメーションを行う
		if(velocity.y > 0) {
			
			if(currentY > m_ShowThreshold && !m_IsActive)
			{
				PlayAnimation(0);
			}

		//上にスクロールしていく場合、閾値をまたいだタイミングでTweenアニメーションを行う
		} else if(velocity.y < 0) {
			
			if(currentY < m_HideThreshold && m_IsActive)
			{
				isActive = false;
			}
		}
	}

	//左右のスクロール開始時に呼ばれる
	public void StartDragHorizontal(Card.State state)
	{
		m_CurrentState = state;

		switch (m_CurrentState) {
		case State.Defection:
			m_UnderCard.color = m_ActiveColor;
			break;
		case State.Join:
			m_UnderCard.color = m_CardColor;
			break;
		}
	}

	//左右のスクロール中に呼ばれ続ける
	public void DragHorizontal(Vector2 deltaPosition, bool isMemberMax)
	{
		//戦闘参加メンバーがマックスの場合、アクティブでなければスワイプ不可
		if(isMemberMax && m_CurrentState != State.Join) return;

		//基本右にしか行けないようにする
		m_Body.rectTransform.anchoredPosition += deltaPosition;
		m_Body.rectTransform.anchoredPosition = new Vector2(Mathf.Max(m_Body.rectTransform.anchoredPosition.x, 0), 0);

		if(m_currentHorizontalScrollMode == HorizontalScrollMode.Non)
		{
			if(m_Body.rectTransform.anchoredPosition.x > 0)
			{
				m_currentHorizontalScrollMode = HorizontalScrollMode.Right; 
			}
		}

		// アクティブを表すパネルの表示
		switch (m_CurrentState) {
		case State.Defection:
			if(m_Body.rectTransform.anchoredPosition.x > m_ThresholdHorizontal)
			{
				SetJoin();
			} else {
				SetDefection();
			}
			break;
		case State.Join:
			SetDefection();
			break;
		}
	}

	//左右のスクロールから戻るアニメーションを行い、新しいStateを返す
	public Card.State GetNextStateAfterAnimation()
	{
		Card.State result = m_CurrentState;
		float duration = 0.4f;
		float targetX = 0;

		m_currentHorizontalScrollMode = HorizontalScrollMode.Non;

		switch (m_CurrentState) {
		case State.Defection:

			//閾値以上のドラッグかつメンバーがマックスに到達していなければカードがアクティブになる
			if(m_Body.rectTransform.anchoredPosition.x > m_ThresholdHorizontal)
			{
				targetX = m_ThresholdHorizontal;
				result = SetJoin();
			} else {
				targetX = 0;
				result = SetDefection();
			}
			break;
		case State.Join:
			targetX = 0;
			result = SetDefection();
			break;
		}

		//トゥイーンアニメーション
		m_Body.transform.DOLocalMoveX(targetX, duration, true).SetEase(Ease.OutBack);

		//テキストが揺れる
		m_NameText.Play();

		return result;
	}
}
