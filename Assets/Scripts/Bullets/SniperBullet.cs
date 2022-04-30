using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperBullet : MonoBehaviour
{
	public GameObject sniperBulletParticle;
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Enemy"))
		{
			HumanoidEnemy humanoidEnemy = other.gameObject.GetComponent<HumanoidEnemy>();
			humanoidEnemy.DoRagdoll(10);
			Destroy(gameObject, 0.3f);
		}
		else if (other.CompareTag("TuglaObstacle"))
		{
			TuglaObstacle tuglaObstacle = other.gameObject.GetComponent<TuglaObstacle>();
			tuglaObstacle.DoDestroy(3);
			Destroy(gameObject, 0.3f);
		}
		else if (other.CompareTag("BetonObstacle"))
		{
			Debug.Log("Sniper");
			GameObject bulletParticle = Instantiate(sniperBulletParticle, new Vector3(transform.position.x, transform.position.y, transform.position.z - 2f), Quaternion.identity);
			Destroy(bulletParticle, 3); // rocket de bunu globale alýp kapat.
			Destroy(gameObject);
		}
	}
}
