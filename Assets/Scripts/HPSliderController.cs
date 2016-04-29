using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class HPSliderController : MonoBehaviour
{
	[SerializeField] Slider m_MainSlider;
	[SerializeField] Slider m_SubSlider;

	void SetSliderValue(float val)
	{
		val = Mathf.Clamp01(val);

		if(m_MainSlider)
		{
			m_MainSlider.DOValue(val, 0.1f).SetEase(Ease.InQuad);
		}

		if(m_SubSlider)
		{
			m_SubSlider.DOValue(val, 0.3f).SetDelay(0.2f).SetEase(Ease.InOutQuad);
		}
	}
}
