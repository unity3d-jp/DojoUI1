using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ClickParticleEffect : MonoBehaviour, IPointerClickHandler
{
	[SerializeField] ParticleSystem m_Particle;
	[SerializeField] Camera m_Camera;

	public void OnPointerClick(PointerEventData eventData)
	{
		StartCoroutine(PlayEffectCoroutine());
	}

	IEnumerator PlayEffectCoroutine()
	{
		ParticleSystem particle = GameObject.Instantiate(m_Particle) as ParticleSystem;
		particle.transform.SetParent(transform);

		Vector2 localPoint;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), Input.mousePosition, m_Camera, out localPoint);
		particle.transform.localPosition = new Vector3(localPoint.x, localPoint.y, 0);

		yield return new WaitWhile(() => particle.IsAlive());

		Destroy(particle.gameObject);
	}
}
