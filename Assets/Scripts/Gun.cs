using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    PlayerController playerController;
    public GameObject bullet, bulletPosition, cloneGun, currentGun;
    public Vector3 defaultPosition;
    public Vector3 defaultRotation;
    public float bulletSpeed;
    public float waitTime;
    public float time;
    public bool isClear;

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
    }
    
    public IEnumerator ThrowBullet()
    {
        playerController.ExplosionDamage(playerController.transform.position, 20);
        CancelInvoke(nameof(BulletFrequency));
        currentGun = playerController.selectGun;
        if (playerController.hitColliders.Count > 0)
        {
            if (playerController.gunType == GunType.pump && (playerController.currentRange == RangeType.PumpEnemy || playerController.currentRange == RangeType.RifleEnemy) && playerController.guns[(int)playerController.gunType] == playerController.selectGun)
            {
                Debug.Log("pump");
                for (int i = 0; i < playerController.hitColliders.Count; i++)
				{
					if (!playerController.getGunControl && SmoothFollow.Instance.state) break;
					Vector3 targetPos = playerController.hitColliders[i].transform.position;
					CreatedBullet(targetPos);
					yield return new WaitForSeconds(0.1f);
				}

			}
            else if (playerController.gunType == GunType.pump && !(playerController.currentRange == RangeType.PumpEnemy || playerController.currentRange == RangeType.RifleEnemy) && playerController.guns[(int)playerController.gunType] == playerController.selectGun)
            {
                for (int i = 0; i < 5; i++)
				{
					if (!playerController.getGunControl && SmoothFollow.Instance.state) break;
					CreatedBullet2();

					yield return new WaitForSeconds(0.1f);
				}
			}
            else if (playerController.gunType == GunType.sniper && playerController.currentRange == RangeType.SniperEnemy && playerController.guns[(int)playerController.gunType] == playerController.selectGun)
            {
                Vector3 targetPos = playerController.hitColliders[0].transform.position;
                CreatedBullet(targetPos);

            }
            else if (playerController.gunType == GunType.sniper && !(playerController.currentRange == RangeType.SniperEnemy) && playerController.guns[(int)playerController.gunType] == playerController.selectGun)
            {
                Vector3 targetPos = playerController.hitColliders[0].transform.position;
                CreatedBullet(targetPos);

            }
            else if (playerController.gunType == GunType.rocket && playerController.currentRange == RangeType.RocketEnemy && playerController.guns[(int)playerController.gunType] == playerController.selectGun)
            {
                Vector3 targetPos = playerController.hitColliders[0].transform.position;
                CreatedBullet(targetPos);

            }
            else if (playerController.gunType == GunType.rocket && !(playerController.currentRange == RangeType.RocketEnemy) && playerController.guns[(int)playerController.gunType] == playerController.selectGun)
            {
                Vector3 targetPos = playerController.hitColliders[0].transform.position;
                CreatedBullet(targetPos);
            }
        }
        else
        {
            if (playerController.gunType == GunType.pump && playerController.guns[(int)playerController.gunType] == playerController.selectGun)
            {

                for (int i = 0; i < 5; i++)
                {
                    if (!playerController.getGunControl && SmoothFollow.Instance.state) break;
                    CreatedBullet2();
                    yield return new WaitForSeconds(0.1f);
                }
            }
            else if (playerController.gunType == GunType.sniper && playerController.guns[(int)playerController.gunType] == playerController.selectGun)
            {
                CreatedBullet2();
            }
            else if (playerController.gunType == GunType.rocket && playerController.guns[(int)playerController.gunType] == playerController.selectGun)
            {
                CreatedBullet2();
            }
        }
        Invoke(nameof(BulletFrequency), waitTime);
    }

	private void CreatedBullet2()
	{
		GameObject createdBullet = Instantiate(bullet, bulletPosition.transform.position, Quaternion.identity);
		createdBullet.GetComponent<Rigidbody>().AddForce((bulletPosition.transform.forward) * bulletSpeed, ForceMode.Impulse);
	}

	private void CreatedBullet(Vector3 targetPos)
	{
		targetPos.y = bulletPosition.transform.position.y;
		Vector3 dir = bulletPosition.transform.position - targetPos;
		dir = -dir.normalized;
		GameObject createdBullet = Instantiate(bullet, bulletPosition.transform.position, Quaternion.identity);
		createdBullet.GetComponent<Rigidbody>().AddForce(dir * bulletSpeed, ForceMode.Impulse);
	}

	public void ThrowGunAnimation()
    {
        CancelInvoke(nameof(BulletFrequency));
        Rigidbody cloneGunRigidbody;
        GameObject clone = Instantiate(cloneGun, gameObject.transform.position, Quaternion.identity);
        cloneGunRigidbody = clone.GetComponent<Rigidbody>();
        clone.transform.parent = null;
        cloneGunRigidbody.isKinematic = false;
        cloneGunRigidbody.AddForce(new Vector3(-1, 1, 1) * 10000 * Time.deltaTime);
        gameObject.SetActive(false);
        Destroy(clone, 2f);
    }
    public void BulletFrequency()
    {
        if (playerController.getGunControl && currentGun == playerController.selectGun) playerController.animator.SetTrigger(playerController.gunType.ToString() + "fire");
    }


}
