using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

[RequireComponent(typeof(Image))]
public class BattleMember : MonoBehaviour
{
	[SerializeField] private TypefaceAnimator m_DamageText;
	[SerializeField] private CharacterType m_CharacterType = CharacterType.Player;
	public CharacterType characterType
	{
		get { return m_CharacterType; }
	}

	[SerializeField] private AttackType m_AttackType = AttackType.Sword;
	public AttackType attackType
	{
		get { return m_AttackType; }
	}

	private Vector2 defaultPosition;
	const float STEP_DISTANCE = 80;

	public enum CharacterType
	{
		Player, Enemy
	}

	public enum AttackType
	{
		Sword, Arrow, Magic, Enemy
	}

	private Vector2 stepDistance
	{
		get {
			Vector2 _step = new Vector2(STEP_DISTANCE, STEP_DISTANCE);
			if(m_CharacterType == CharacterType.Enemy) _step = new Vector2(-STEP_DISTANCE, -STEP_DISTANCE);
			return _step;
		}
	}

	private UIManager.Member m_Data;
	private int m_HP;

	public int attack
	{
		get { return m_Data.attack; }
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

	[SerializeField] private UnityEvent endBattle;

	void Awake ()
	{
		defaultPosition = rectTransform.anchoredPosition;
		m_DamageText.gameObject.SetActive(false);
	}

	public void Initialize(UIManager.Member data)
	{
		m_Data = data;
		m_HP = data.hp;

		var image = GetComponent<Image>();
		image.enabled = true;
		image.sprite = data.battle;
		image.SetNativeSize();
		SendMessage("SetSliderValue", 1);
	}

	public void Attack(int damage)
	{
		Sequence mySequence = DOTween.Sequence();
		mySequence.Append(rectTransform.DOAnchorPos((defaultPosition + stepDistance), 0.2f, true).SetEase(Ease.InOutQuad));
		mySequence.Append(rectTransform.DOAnchorPos((defaultPosition), 0.2f, true).SetDelay(0.4f).SetEase(Ease.InOutQuad));
	}

	public void Defence(int damage)
	{
		StartCoroutine(DefenceCoroutine(damage));
	}

	IEnumerator DefenceCoroutine(int damage)
	{
		m_HP -= damage;
		float percentage = (float)m_HP / (float)m_Data.hp;

		yield return new WaitForSeconds(0.2f);

		m_DamageText.gameObject.SetActive(true);
		m_DamageText.GetComponent<Text>().text = damage.ToString();
		m_DamageText.Play();

		Sequence sequence = DOTween.Sequence();
		sequence.Append(rectTransform.DOAnchorPos(defaultPosition + stepDistance, 0.05f, true).SetEase(Ease.OutQuad));
		sequence.Append(rectTransform.DOAnchorPos(defaultPosition, 0.2f, true).SetEase(Ease.OutElastic));

		yield return new WaitForSeconds(0.1f);

		SendMessage("SetSliderValue", percentage);

		yield return new WaitForSeconds(0.6f);

		m_DamageText.gameObject.SetActive(false);

		// エネミーがやられた場合
		if(m_HP <= 0 && characterType == CharacterType.Enemy)
		{
			endBattle.Invoke();

			var image = GetComponent<Image>();
			yield return new WaitForSeconds(0.2f);
			image.enabled = false;
			yield return new WaitForSeconds(0.05f);
			image.enabled = true;
			yield return new WaitForSeconds(0.05f);
			image.enabled = false;
			yield return new WaitForSeconds(0.05f);
			image.enabled = true;
			yield return new WaitForSeconds(0.05f);
			image.enabled = false;
		}
	}
}
