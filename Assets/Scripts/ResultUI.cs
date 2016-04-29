using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class ResultUI : MonoBehaviour
{
	[SerializeField] RectTransform[] m_stars;
	public bool trigger = false;

	private RectTransform m_Rect;
	public RectTransform rectTransform
	{
		get {
			if (m_Rect == null)
				m_Rect = GetComponent<RectTransform>();
			return m_Rect;
		}
	}

	void OnEnable ()
	{
		trigger = false;

		foreach (var star in m_stars)
		{
			star.gameObject.SetActive(false);
		}

		StartCoroutine(OnShakeCoroutine());
	}

	IEnumerator OnShakeCoroutine()
	{
		yield return new WaitUntil(() => trigger);

		Sequence sequence = DOTween.Sequence();

		for (int i = 0; i < m_stars.Length; i++)
		{
			var star = m_stars[i];
			star.gameObject.SetActive(true);
			star.localScale = new Vector3(0, 0, 1);
			sequence.Append(star.DOScale(1, 0.3f).SetEase(Ease.OutBack));
			sequence.Join(star.DOShakeAnchorPos(0.25f, 120, 20, 60, true));
			sequence.Join(rectTransform.DOShakeAnchorPos(0.25f, 60, 20, 60, true));
		}
	}
}
