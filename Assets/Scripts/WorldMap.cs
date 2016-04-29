using UnityEngine;
using System.Collections;

public class WorldMap : MonoBehaviour
{
	private WorldMapBattlePoint m_CurrentPoint = null;

	public void SetBattlePoint(WorldMapBattlePoint point)
	{
		m_CurrentPoint = point;
	}

	public void SetNull()
	{
		if(m_CurrentPoint != null)
		{
			m_CurrentPoint.CloseAnimation();
			m_CurrentPoint = null;
		}
	}
}
