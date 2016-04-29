using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class ListScroll : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
	[SerializeField] private Card m_CardTemplate;
	[SerializeField] private ScrollRect m_ScrollRect;
	[SerializeField] private Camera m_UICamera;
	[SerializeField] private Header m_Header;
	[SerializeField] private SelectedMembers m_BattleMembers;
	[SerializeField] private RectTransform m_Footer;
	[SerializeField] private Color m_CardColorBegin;
	[SerializeField] private Color m_CardColorEnd;
	[SerializeField] private Color m_CardColorActiveBegin;
	[SerializeField] private Color m_CardColorActiveEnd;
	const int MAX_ACTIVE_COUNT = 3;
	const float CLICK_ACCURACY = 10.0f;

	private ScrollMode m_currentScrollMode = ScrollMode.Non;
	private enum ScrollMode
	{
		Non, Vertical, Horizontal
	}

	private Card[] m_Cards;
	private static Card m_CurrentCard;
	public static Card CurrentCard
	{
		get { return m_CurrentCard; }
	}

	private static UIManager uiManager
	{
		get { return FindObjectOfType<UIManager>(); }
	}

	private bool isMemberMax
	{
		get {
			int activeCount = m_BattleMembers.GetActiveCount();
			return ( activeCount >= MAX_ACTIVE_COUNT ) ? true : false;
		}
	}

	void OnEnable ()
	{
		// 初回だけの処理
		if(m_Cards == null || m_Cards.Length <= 0)
		{
			m_Cards = new Card[uiManager.members.Length];
			m_Cards[0] = m_CardTemplate;
			m_Cards[0].gameObject.name = "Card 0";

			for (int i = 1; i < uiManager.members.Length; i++)
			{
				Card card = Instantiate(m_CardTemplate);
				card.rectTransform.SetParent(m_ScrollRect.content, false);
				card.rectTransform.anchoredPosition += new Vector2(0, i * Card.height * -1);
				card.gameObject.name = "Card " + i;
				m_Cards[i] = card;
			}
		}

		// カードの初期化はアクティブのたびに呼び出す
		for (int i = 0; i < m_Cards.Length; i++)
		{
			float colorRate = (float)i / m_Cards.Length;
			Color cardColor = Color.Lerp(m_CardColorBegin, m_CardColorEnd, colorRate);
			Color activeColor = Color.Lerp(m_CardColorActiveBegin, m_CardColorActiveEnd, colorRate);
			m_Cards[i].SetUp(i, uiManager.members[i], cardColor, activeColor);
		}

		//メンバーリストも初期化
		m_BattleMembers.Initialize();

		//フッタボタンの表示・非表示
		CheckFooterButton();
	}

	//スクロールの位置をリセット
	public void ResetContentPosition()
	{
		Vector2 contentPosition = m_ScrollRect.content.anchoredPosition;
		m_ScrollRect.onValueChanged.Invoke(new Vector2(0, -contentPosition.y));
		m_ScrollRect.content.anchoredPosition += new Vector2(0, -contentPosition.y);
	}

	// スワイプ開始時にタップしたカードを特定
	public void OnBeginDrag(PointerEventData data)
	{
		for (int i = 0; i < m_Cards.Length; i++)
		{
			bool isOnTap = RectTransformUtility.RectangleContainsScreenPoint(m_Cards[i].rectTransform, data.position, m_UICamera);

			if(isOnTap)
			{
				m_CurrentCard = m_Cards[i];
			}
		}
	}

	// スワイプ中
	public void OnDrag(PointerEventData data)
	{
		if (!m_CurrentCard) return;

		switch (m_currentScrollMode)
		{
			// まだScrollMode.Nonの場合（つまり指を置いたばかりの場合）、最初のフレームの移動値で縦のスクロールか横のスクロールかを決める
			case ScrollMode.Non:
			
			// 上下スクロール
			if((Mathf.Abs(data.delta.x) < Mathf.Abs(data.delta.y)))
			{
				// 上下スクロールの場合は、ScrollRectによるスクロール
				m_currentScrollMode = ScrollMode.Vertical;
				m_ScrollRect.vertical = true;

			// 左右スクロール
			} else if((Mathf.Abs(data.delta.x) > Mathf.Abs(data.delta.y))) {

				// 左右スクロールの場合は自前実装したスクロールを使うため、ScrollRectを使用不可に（でないと左右同時スクロールが可能になってしまってユーザビリティが下がる）
				m_currentScrollMode = ScrollMode.Horizontal;
				m_ScrollRect.vertical = false;
				m_CurrentCard.StartDragHorizontal(m_CurrentCard.currentState);
			}
			break;

			// 左右スクロールの処理
			case ScrollMode.Horizontal:
			Vector2 localPositionAfter;
			Vector2 localPositionBefor;

			RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), data.position, m_UICamera, out localPositionAfter);
			RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), data.position - data.delta, m_UICamera, out localPositionBefor);

			if(m_CurrentCard != null)
			{
				m_CurrentCard.DragHorizontal(new Vector2((localPositionAfter - localPositionBefor).x, 0), isMemberMax);
			}

			break;
		}
	}

	// 上下スクロールの処理はScrollRectのイベントから呼ぶ
	public void OnScrollingVertical(Vector2 data)
	{
		float contentY = m_ScrollRect.content.anchoredPosition.y;

		for (int i = 0; i < m_Cards.Length; i++)
		{
			m_Cards[i].SetRotation(contentY, m_ScrollRect.velocity);
		}
	}

	// スワイプ終了
	public void OnEndDrag(PointerEventData data)
	{
		if(m_currentScrollMode == ScrollMode.Horizontal)
			OnGoBackToDefaultPosition();
		
		m_currentScrollMode = ScrollMode.Non;

		//ヘッダがリターンを示せば、元のコンテンツである地図に戻る
		if(m_Header.isReturn)
		{
			uiManager.SwitchContentToWorldMap();
		}
	}

	// 左右スワイプだった場合の処理
	void OnGoBackToDefaultPosition()
	{
		if(m_CurrentCard != null)
		{
			Card.State prev = m_CurrentCard.currentState;
			m_CurrentCard.currentState = m_CurrentCard.GetNextStateAfterAnimation();

			//ステートが実質的に変化ナシなら処理もナシ
			if(prev == m_CurrentCard.currentState) return;

			if(m_CurrentCard.currentState == Card.State.Join)
			{
				m_BattleMembers.AddMember(m_CurrentCard);

			} else if (m_CurrentCard.currentState == Card.State.Defection) {
				
				m_BattleMembers.DeleteMember(m_CurrentCard);
			}

			m_CurrentCard = null;
		}

		CheckFooterButton();
	}

	public void OnPointerClick(PointerEventData data)
	{
		for (int i = 0; i < m_Cards.Length; i++)
		{
			bool isOn = RectTransformUtility.RectangleContainsScreenPoint(m_Cards[i].rectTransform, data.position, m_UICamera);

			if(isOn)
			{
				if((data.pressPosition - data.position).magnitude < CLICK_ACCURACY)
				{
					uiManager.currentMember = m_Cards[i].data;
					uiManager.SwitchContentToCharacter();
				}
			}
		}
	}

	void CheckFooterButton()
	{
		//メンバーが3人以上アクティブでバトルに突入可能になる
		m_Footer.gameObject.SetActive(false);
		if(isMemberMax)
		{
			m_Footer.gameObject.SetActive(true);
		}
	}
}
