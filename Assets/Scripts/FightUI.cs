using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class FightUI : MonoBehaviour
{
	[SerializeField] RectTransform m_target;
	public bool onShack = false;

	void OnEnable ()
	{
		onShack = false;
		StartCoroutine(OnShakeCoroutine());
	}

	IEnumerator OnShakeCoroutine()
	{
		Vector2 defaultPosition = m_target.anchoredPosition;

		yield return new WaitUntil(() => onShack);
		Sequence sequence = DOTween.Sequence();
		sequence.Append(m_target.DOAnchorPos(defaultPosition + new Vector2(0, -100), 0.1f, true).SetEase(Ease.OutQuad));
		sequence.Append(m_target.DOAnchorPos(defaultPosition, 1.5f, true).SetEase(Ease.OutElastic));
	}
}
