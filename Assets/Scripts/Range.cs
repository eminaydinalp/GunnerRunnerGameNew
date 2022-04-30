using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum RangeType { RocketEnemy, SniperEnemy, PumpEnemy, RifleEnemy, SniperObstacle, RocketObstacle, Empty};

public class Range : MonoBehaviour
{
    public GameObject[] enemies;
    public RangeType rangeType;

    private void OnTriggerEnter(Collider other)
    {
      
        if (other.CompareTag("Player") && gameObject.CompareTag("BigRange"))
        {
            other.gameObject.GetComponent<PlayerController>().ChangeEnum(rangeType);
        }
        if (other.CompareTag("Player") && gameObject.CompareTag("SmallRange"))
        {
            StartCoroutine(OpenEnemyFire());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().ChangeEnum(RangeType.Empty);
        }
    }
    public IEnumerator OpenEnemyFire()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            GameObject item = enemies[i];
            item.gameObject.GetComponent<HumanoidEnemy>().animator.SetBool("EnemyGun", true);
            yield return new WaitForSeconds(0.2f);
        }
    }

}
