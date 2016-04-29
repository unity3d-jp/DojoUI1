using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
	[SerializeField] private CoverUI coverUI;
	[SerializeField] private ContentCategoly m_CurrentContentCategoly = ContentCategoly.WorldMap;
	[SerializeField] private ContentController[] m_Contents;
	public Member[] members;

	public Member enemy;
	private Member m_CurrentMember;
	public Member currentMember
	{
		get { return m_CurrentMember; }
		set { m_CurrentMember = value; }
	}

	private float m_FadeInDuration = 0.5f;
	private float m_FadeOutDuration = 0.2f;

	public enum ContentCategoly
	{
		WorldMap = 0, 
		PartiyEdit = 1,
		Character = 2,
		Battle = 3
	}

	[System.Serializable]
	public class Member
	{
		[SerializeField] private string m_Name;
		[SerializeField] private int m_HP;
		[SerializeField] private int m_MP;
		[SerializeField] private int m_Attack;
		[SerializeField] private int m_Defence;
		[SerializeField, TextArea(1, 5)] private string m_Comment;
		[SerializeField] private Sprite m_Large;
		[SerializeField] private Sprite m_Small;
		[SerializeField] private Sprite m_Battle;
		public bool m_Selected;

		public string name
		{
			get { return m_Name; }
		}

		public int hp
		{
			get { return m_HP; }
		}

		public int mp
		{
			get { return m_MP; }
		}

		public int attack
		{
			get { return m_Attack; }
		}

		public int defence
		{
			get { return m_Defence; }
		}

		public string comment
		{
			get { return m_Comment; }
		}

		public Sprite large
		{
			get { return m_Large; }
		}

		public Sprite small
		{
			get { return m_Small; }
		}

		public Sprite battle
		{
			get { return m_Battle; }
		}

		public bool selected
		{
			get { return m_Selected; }
			set { m_Selected = value; }
		}
	}

	void Awake()
	{
		if(members != null)
		{
			currentMember = members[0];
		}

		foreach (var content in m_Contents)
		{
			content.gameObject.SetActive(false);
		}
	}

	void Start()
	{
		StartCoroutine(SwitchContent(m_CurrentContentCategoly));
	}

	IEnumerator SwitchContent(ContentCategoly nextContent)
	{
		if(m_CurrentContentCategoly != nextContent)
		{
			coverUI.FadeOut(m_FadeOutDuration);
			yield return new WaitForSeconds(m_FadeOutDuration);

			m_Contents[(int)m_CurrentContentCategoly].gameObject.SetActive(false);
			m_CurrentContentCategoly = nextContent;
			m_Contents[(int)m_CurrentContentCategoly].gameObject.SetActive(true);

			yield return new WaitForSeconds(m_FadeInDuration);
			coverUI.FadeIn(m_FadeInDuration);

		} else {
			
			m_Contents[(int)m_CurrentContentCategoly].gameObject.SetActive(true);
			yield break;
		}
	}

	public void SwitchContentToWorldMap()
	{
		StartCoroutine(SwitchContent(ContentCategoly.WorldMap));
	}

	public void SwitchContentToPartiyEdit()
	{
		StartCoroutine(SwitchContent(ContentCategoly.PartiyEdit));
	}

	public void SwitchContentToCharacter()
	{
		StartCoroutine(SwitchContent(ContentCategoly.Character));
	}

	public void SwitchContentToBattle()
	{
		StartCoroutine(SwitchContent(ContentCategoly.Battle));
	}

	//戦闘開始時にバトルメンバーを取得
	public void GetBattleMembers(Action<Member> action)
	{
		for(int i = 0; i < members.Length; i++)
		{
			Member member = members[i];

			if(member.selected)
			{
				action(member);
			}
		}
	}
}
