using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

[RequireComponent(typeof(Image))]
public class BattleMembersStage : MonoBehaviour
{
	[SerializeField] Image m_Shadow;
	[SerializeField] ParticleSystem m_EffectParticle;
	private Vector3 m_DefaultPosition;

	private bool m_IsActive = false;
	public bool isActive
	{
		get { return m_IsActive; }
	}

	public Sprite sprite
	{
		get { return image.sprite; }
	}

	private Image m_Image;
	public Image image
	{
		get {
			if (m_Image == null)
				m_Image = GetComponent<Image>();
			return m_Image;
		}
	}

	void Awake ()
	{
		m_DefaultPosition = transform.localPosition;
	}

	void OnEnable ()
	{
		m_Shadow.enabled = false;
	}

	public void Show(Sprite sprite)
	{
		m_IsActive = true;
		image.enabled = true;
		image.sprite = sprite;
		image.SetNativeSize();

		// アニメーション
		float duration = 0.15f;
		transform.localPosition = m_DefaultPosition + new Vector3(0, 200, 0);
		transform.DOLocalMove(m_DefaultPosition, duration, true).SetEase(Ease.InOutQuad);

		if(m_EffectParticle)
			m_EffectParticle.Play();

		m_Shadow.enabled = true;
	}

	public void Hide()
	{
		// アニメーション
		float duration = 0.1f;
		transform.DOLocalMove(m_DefaultPosition + new Vector3(0, 200, 0), duration, true).SetEase(Ease.InQuad).OnComplete(() =>
			{
				m_IsActive = false;
				image.enabled = false;
			});
		
		m_Shadow.enabled = false;
	}

	public void HideImmediately()
	{
		m_IsActive = false;
		image.enabled = false;
	}
}
