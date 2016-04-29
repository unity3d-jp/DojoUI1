using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class WorldMapBattlePoint : MonoBehaviour
{
	[SerializeField] private RectTransform m_Circle1;
	[SerializeField] private RectTransform m_Circle2;
	[SerializeField] private RectTransform m_Circle3;
	[SerializeField] private RectTransform m_Balloon;

	private bool isOpen = false;

	private static UIManager uiManager
	{
		get { return FindObjectOfType<UIManager>(); }
	}

	void Awake()
	{
		m_Balloon.gameObject.SetActive(false);
	}
	
	public void OnClickBattlePoint()
	{
		if(!isOpen)
		{
			isOpen = true;
			OpenAnimation();

		} else {

			uiManager.SwitchContentToPartiyEdit();
		}
	}

	private void OpenAnimation()
	{
		//ボタンのアニメーション
		m_Circle1.DOSizeDelta(new Vector2(200, 200), 0.6f, true).SetEase(Ease.InOutBack);
		m_Circle2.DOSizeDelta(new Vector2(200, 200), 0.6f, true).SetEase(Ease.InOutBack).SetDelay(0.1f);
		m_Circle3.DOSizeDelta(new Vector2(200, 200), 0.6f, true).SetEase(Ease.InOutBack).SetDelay(0.2f);

		//バルーン表示＆アニメーション
		m_Balloon.gameObject.SetActive(true);
		m_Balloon.localScale = Vector3.zero;
		m_Balloon.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack).SetDelay(0.5f);
	}

	public void CloseAnimation()
	{
		if(isOpen)
		{
			isOpen = false;

			//バルーン表示＆アニメーション
			m_Balloon.localScale = Vector3.one;
			m_Balloon.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InExpo).OnComplete(() => { m_Balloon.gameObject.SetActive(false); });

			//ボタンのアニメーション
			m_Circle3.DOSizeDelta(new Vector2(0, 0), 0.6f, true).SetEase(Ease.InOutBack).SetDelay(0.3f);
			m_Circle2.DOSizeDelta(new Vector2(48, 48), 0.6f, true).SetEase(Ease.InOutBack).SetDelay(0.4f);
			m_Circle1.DOSizeDelta(new Vector2(120, 120), 0.6f, true).SetEase(Ease.InOutBack).SetDelay(0.5f);
		}
	}
}
