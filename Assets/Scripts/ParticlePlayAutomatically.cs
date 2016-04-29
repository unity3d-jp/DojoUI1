using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class ParticlePlayAutomatically : MonoBehaviour {

	void OnEnable()
	{
		GetComponent<ParticleSystem>().Play();
	}
}
