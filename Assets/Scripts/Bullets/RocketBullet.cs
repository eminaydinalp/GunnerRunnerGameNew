using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBullet : MonoBehaviour
{
	public LayerMask layer;
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Enemy"))
		{
			Collider[] Colliders = Physics.OverlapSphere(transform.position, 1.5f, layer);
			foreach (var item in Colliders)
			{
				item.gameObject.GetComponent<HumanoidEnemy>().DoRagdoll(10);
			}
			Instantiate(GM.Instance.rocketParticle, other.transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
		else if (other.CompareTag("TuglaObstacle"))
		{
			Collider[] Colliders = Physics.OverlapSphere(transform.position, 1.5f, layer);
			foreach (var item in Colliders)
			{
				item.gameObject.GetComponent<TuglaObstacle>().DoDestroy(15);
			}
			Instantiate(GM.Instance.rocketParticle, other.transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
		else if (other.CompareTag("BetonObstacle"))
		{
			other.gameObject.GetComponent<BarikatObstacle>().BarikatDestroy();
			Instantiate(GM.Instance.rocketParticle, other.transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
		else if (other.CompareTag("ZirhObstacle"))
		{
			other.gameObject.GetComponent<TuglaObstacle>().DoDestroy(10);
			Collider[] Colliders = Physics.OverlapSphere(transform.position, 4f, layer);
			foreach (var item in Colliders)
			{
				item.gameObject.GetComponent<HumanoidEnemy>().DoRagdoll(2);
			}
			Instantiate(GM.Instance.rocketParticle, other.transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
	}
}
