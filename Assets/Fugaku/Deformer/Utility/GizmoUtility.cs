using UnityEngine;
using System.Collections;

public class GizmoUtility
{
	public static void DrawCircle(Vector3 position, float radius, Color color)
	{
		Gizmos.color = color;
		int num = 60;

		for (int i = 0; i < num; i++)
		{
			float fromAngle = Mathf.Deg2Rad * i * (360 / num);
			float toAngle = Mathf.Deg2Rad * (i + 1) * (360 / num);

			Gizmos.DrawLine(
				new Vector3(Mathf.Cos(fromAngle), Mathf.Sin(fromAngle), 0) * radius + position, 
				new Vector3(Mathf.Cos(toAngle), Mathf.Sin(toAngle), 0) * radius + position
			);
		}
	}

	public static void DrawArrow (Vector3 beginPosition, Vector3 direction, Color color)
	{
		if(direction.magnitude <= 0) return;
		
		float arrowHeadLength = 0.25f;
		float arrowHeadAngle = 20.0f;
		Vector3 endPosition = beginPosition + direction;

		Vector3 right = Quaternion.LookRotation (direction) * Quaternion.Euler (arrowHeadAngle, 0, 0) * Vector3.back;
		Vector3 left = Quaternion.LookRotation (direction) * Quaternion.Euler (-arrowHeadAngle, 0, 0) * Vector3.back;
		Vector3 up = Quaternion.LookRotation (direction) * Quaternion.Euler (0, arrowHeadAngle, 0) * Vector3.back;
		Vector3 down = Quaternion.LookRotation (direction) * Quaternion.Euler (0, -arrowHeadAngle, 0) * Vector3.back;

		Gizmos.color = color;
		Gizmos.DrawLine(beginPosition, endPosition);
		Gizmos.DrawLine(endPosition, endPosition + right * arrowHeadLength * direction.magnitude);
		Gizmos.DrawLine(endPosition, endPosition + left * arrowHeadLength * direction.magnitude);
		Gizmos.DrawLine(endPosition, endPosition + up * arrowHeadLength * direction.magnitude);
		Gizmos.DrawLine(endPosition, endPosition + down * arrowHeadLength * direction.magnitude);
	}
}
