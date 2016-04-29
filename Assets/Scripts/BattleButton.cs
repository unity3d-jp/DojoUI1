using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using Fugaku;

[RequireComponent(typeof(Image))]
public class BattleButton : MonoBehaviour
{
	[SerializeField] private CircleForceField m_ForceField;
	[SerializeField] private Image m_Background;
	private float m_Duration = 0.5f;

	void OnEnable()
	{
		// ボタンのアニメーション
		Image image = GetComponent<Image>();
		transform.localEulerAngles = new Vector3(0, 0, 180);
		transform.DOLocalRotate(new Vector3(0, 0, 20), m_Duration).SetEase(Ease.OutQuart);
		image.color = new Color(1, 1, 1, 0);
		image.DOFade(1, m_Duration * 0.5f).SetEase(Ease.InQuad);

		// Force Fieldのアニメーション
		m_ForceField.transform.localPosition = new Vector3(0, 0, 00);
		m_ForceField.transform.DOLocalMoveY(-800, m_Duration, true).SetEase(Ease.OutQuad);

		// バックグラウンドの出現
		m_Background.color = new Color(1, 1, 1, 0);
		m_Background.DOFade(1, m_Duration * 0.5f).SetEase(Ease.InQuad);
	}
}
