using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace Fugaku
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Deformer))]
	public class BySpeed : MonoBehaviour
	{
		[SerializeField] CircleForceField forceField;
		[SerializeField, RangeAttribute(0.01f, 1)] float m_Following = 0.5f;
//		[SerializeField, RangeAttribute(0, 1)] float m_ForcePositionFromCenter = 0.5f;

		Vector3 m_Delay;
		Vector3 m_ChaserPosition;
		//Vector3 m_ForcePosition;
		bool m_IsMoving = false;

		void Start()
		{
			m_ChaserPosition = transform.localPosition;
		}

		Vector3 speed;
		const float spring = 0.5f;

		void LateUpdate()
		{
			m_IsMoving = true;

			// バネ運動
			/*
			Vector3 ax = (transform.localPosition - m_ChaserPosition) * spring;
			speed += ax * Time.deltaTime * 200f * (0.1f + m_Following);
			speed *= Mathf.Pow(0.5f, Time.deltaTime * 10f);
			m_ChaserPosition += speed * Time.deltaTime;
			*/

			// バネ係数なし運動
			m_ChaserPosition = Vector3.Lerp(m_ChaserPosition, transform.localPosition, Time.deltaTime * (1.5f / m_Following));

			m_Delay = transform.localPosition - m_ChaserPosition;

			if(m_Delay.magnitude > 0.1f)
			{
				// 移動
			//	Vector3 max = m_Delay.normalized * forceField.localRadius * 1.0f;
			//	m_ForcePosition = Vector3.Lerp(max, Vector3.zero, m_ForcePositionFromCenter);
				forceField.rectTransform.localPosition = -m_Delay;
				forceField.force = (m_ChaserPosition - transform.localPosition).magnitude / forceField.localRadius * 2.5f;

				// 回転
				transform.eulerAngles = Vector3.forward * (m_Delay.x + m_Delay.y) * Time.smoothDeltaTime * 10;

			} else {

				m_IsMoving = false;
				m_ChaserPosition = transform.localPosition;
			}

		}

		void OnDrawGizmosSelected()
		{
			if(Application.isPlaying && m_IsMoving)
			{
				Color gizmoColor = new Color(0.2f, 0.8f, 0);
				Gizmos.color = gizmoColor;
				Gizmos.DrawLine(transform.position, transform.TransformPoint(-m_Delay));
				GizmoUtility.DrawCircle(transform.position, 10f, gizmoColor);
				GizmoUtility.DrawCircle(transform.TransformPoint(-m_Delay), 10f, gizmoColor);
			}
		}
	}
}