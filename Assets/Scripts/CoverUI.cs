using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

[RequireComponent(typeof(Image))]
public class CoverUI : MonoBehaviour
{
	private float m_BuffRate = 0.95f;
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
		image.enabled = false;
	}

	public void FadeIn (float duration)
	{
		image.enabled = true;
		image.color = new Color(0, 0, 0, 1);
		image.DOFade(0, duration * m_BuffRate).SetEase(Ease.OutQuad);
	}

	public void FadeOut (float duration)
	{
		image.enabled = true;
		image.color = new Color(0, 0, 0, 0);
		image.DOFade(1, duration * m_BuffRate).SetEase(Ease.InQuad);
	}
}
