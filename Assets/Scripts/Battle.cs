using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class Battle : MonoBehaviour
{
	[SerializeField] private BattleMember[] m_Members;
	[SerializeField] private RectTransform m_FightUI;
	[SerializeField] private RectTransform m_ResultUI;
	private bool m_IsFighting = false;

	private BattleMember m_Enemy;
	private BattleMember enemy
	{
		get {
			
			if (m_Enemy == null)
			{
				for (int i = 0; i < m_Members.Length; i++)
				{
					if(m_Members[i].characterType == BattleMember.CharacterType.Enemy)
					{
						m_Enemy = m_Members[i];
						return m_Members[i];
					}
				}

				Debug.LogError("No Enemy Character");
				return null;
			}

			return m_Enemy;
		}
	}

	private BattleMember TargetMember(BattleMember attacker)
	{
		if(attacker.characterType == BattleMember.CharacterType.Player)
		{
			return enemy;
		}

		int num = Random.Range(1, m_Members.Length);
		return m_Members[num];
	}

	private static UIManager uiManager
	{
		get { return FindObjectOfType<UIManager>(); }
	}

	private void ShuffleMembers()
	{
		for (int i = 0; i < m_Members.Length; i++)
		{
			BattleMember temp = m_Members[i];
			int randomIndex = Random.Range(0, m_Members.Length);
			m_Members[i] = m_Members[randomIndex];
			m_Members[randomIndex] = temp;
		}
	}

	void Awake()
	{
		m_FightUI.gameObject.SetActive(false);
		m_ResultUI.gameObject.SetActive(false);
	}

	void OnEnable()
	{
		//エネミーの初期化
		m_Members[0].Initialize(uiManager.enemy);

		//味方メンバーの初期化
		int count = 0;
		uiManager.GetBattleMembers((UIManager.Member member) => {
			m_Members[++count].Initialize(member);
		});

		if(count != 3)
		{
			Debug.LogError("Battle Member Number is incorrect");
		}

		//イントロ開始
		StartCoroutine(IntroCoroutine());
	}

	//イントロアニメーション
	private IEnumerator IntroCoroutine()
	{
		//イントロアニメーション
		m_FightUI.gameObject.SetActive(true);
		m_ResultUI.gameObject.SetActive(false);

		yield return new WaitForSeconds(4.0f);

		//本編
		StartCoroutine(BattleCoroutine());

	}

	//バトル本編
	private IEnumerator BattleCoroutine()
	{
		m_IsFighting = true;

		while (true)
		{
			for (int i = 0; i < m_Members.Length; i++)
			{
				if(!m_IsFighting) { yield break; }

				m_Members[i].Attack(m_Members[i].attack);
				TargetMember(m_Members[i]).Defence(m_Members[i].attack);

				yield return new WaitForSeconds(1.0f);
			}
		}
	}

	public void OnBeatEnemy()
	{
		StartCoroutine(BeatEnemyCoroutine());
	}

	IEnumerator BeatEnemyCoroutine()
	{
		m_IsFighting = false;

		yield return new WaitForSeconds(1.0f);

		//勝利アニメーション
		m_ResultUI.gameObject.SetActive(true);

		yield return new WaitForSeconds(5.0f);

		uiManager.SwitchContentToPartiyEdit();
	}
}
