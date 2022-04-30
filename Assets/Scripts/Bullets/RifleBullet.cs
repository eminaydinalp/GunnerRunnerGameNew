using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleBullet : MonoBehaviour
{
	public GameObject rifleBulletParticle;
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Enemy"))
		{
			HumanoidEnemy humanoidEnemy = other.gameObject.GetComponent<HumanoidEnemy>();
			humanoidEnemy.DoRagdoll(2);
			Destroy(gameObject);
		}
		else if (other.CompareTag("TuglaObstacle"))
		{
			TuglaObstacle tuglaObstacle = other.gameObject.GetComponent<TuglaObstacle>();
			tuglaObstacle.DoDestroy(2);
			Destroy(gameObject);
		}
		else if (other.CompareTag("BetonObstacle"))
		{
			Debug.Log("Rifle");
			GameObject bulletParticle = Instantiate(rifleBulletParticle, new Vector3(transform.position.x, transform.position.y, transform.position.z - 1f), Quaternion.identity);
			Destroy(bulletParticle, 3); // rocket de bunu globale alýp kapat.
			Destroy(gameObject);
		}
	}
}
