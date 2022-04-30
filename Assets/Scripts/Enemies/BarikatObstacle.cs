using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarikatObstacle : MonoBehaviour
{
	public GameObject barikatDuman;
	public void BarikatDestroy()
	{
		GetComponent<Animation>().Play("Barikat");
		GetComponent<Collider>().enabled = false;
		barikatDuman.SetActive(true);
	}
}
