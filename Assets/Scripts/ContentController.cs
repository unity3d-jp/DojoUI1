using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class ContentController : MonoBehaviour
{
	public void SetLock ()
	{
		GetComponent<CanvasGroup>().interactable = false;
	}
	
	public void SetUnlock ()
	{
		GetComponent<CanvasGroup>().interactable = true;
	}

}
