using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    PlayerController playerController;
    ScoreManager scoreManager;

    public Animator animator;
    public GameObject enemyBullet, enemyBulletPosition, enemyConfetti, sniperBulletParticle, rifleBulletParticle, enemyBloodParticle;
    public GameObject spine;
    public GameObject thighl;
    public GameObject thighr;
    public GameObject rootTransform;
    public GameObject pelvis;


    public LayerMask layer;

    public float randomValue;
    public Rigidbody[] rigidbodies;
    public float radius;

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        scoreManager = FindObjectOfType<ScoreManager>();
        animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SniperBullet") && gameObject.CompareTag("Enemy"))
        {
            scoreManager.AddCoins(other.gameObject.transform.position, 1);
            Ragdoll();
            Random.InitState(System.DateTime.Now.Millisecond * (int)transform.position.z);
            pelvis.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-20, 20), Random.Range(-10, 20), Random.Range(0, 10)) * 10, ForceMode.Impulse);
            gameObject.layer = LayerMask.NameToLayer("Enemy");
            gameObject.GetComponent<Collider>().enabled = false;
            enemyBloodParticle.SetActive(true);
        }
        else if (other.CompareTag("SniperBullet") && playerController.currentRange == RangeType.SniperObstacle)
        {
            Debug.Log("Sniper");
            scoreManager.AddCoins(other.gameObject.transform.position, 10);
            for (int i = 0; i < playerController.hitColliders.Count; i++)
            {
                playerController.hitColliders[i].GetComponent<EnemyController>().gameObject.layer = LayerMask.NameToLayer("Enemy");
                playerController.hitColliders[i].GetComponent<EnemyController>().gameObject.GetComponent<Collider>().enabled = false;
                playerController.hitColliders[i].GetComponent<EnemyController>().enemyConfetti.SetActive(true);
                foreach (var rb in playerController.hitColliders[i].GetComponent<EnemyController>().rigidbodies)
                {
                    if (rb != null)
                    {
                        rb.isKinematic = false;
                        rb.useGravity = true;
                        rb.AddExplosionForce(100, other.transform.position, radius, 3);
                    }
                }
            }
        }
        else if ((other.CompareTag("SniperBullet") || other.CompareTag("PumpBullet")) && playerController.currentRange == RangeType.RocketEnemy)
        {

            //if (other.CompareTag("SniperBullet"))
            //{
            //    GameObject bulletParticle = Instantiate(sniperBulletParticle, new Vector3(other.gameObject.transform.position.x, other.gameObject.transform.position.y, other.gameObject.transform.position.z - 2.5f), Quaternion.identity);
            //    Destroy(bulletParticle, 2);
            //}
            //else if (other.CompareTag("PumpBullet"))
            //{
            //    GameObject bulletParticle = Instantiate(sniperBulletParticle, new Vector3(other.gameObject.transform.position.x, other.gameObject.transform.position.y, other.gameObject.transform.position.z - 1.5f), Quaternion.identity);
            //    Destroy(bulletParticle, 2);
            //}
            

            //Destroy(other.gameObject);

        }
        else if (other.CompareTag("SniperBullet") && playerController.currentRange == RangeType.RifleEnemy)
        {
            scoreManager.AddCoins(other.gameObject.transform.position, 10);
            gameObject.layer = LayerMask.NameToLayer("Enemy");
            enemyConfetti.SetActive(true);
            foreach (var rb in rigidbodies)
            {
                if (rb != null)
                {
                    rb.isKinematic = false;
                    rb.useGravity = true;
                    rb.AddExplosionForce(10, transform.position, radius, 15, ForceMode.Impulse);
                }
            }
            gameObject.GetComponent<Collider>().enabled = false;
        }

        else if (other.CompareTag("PumpBullet") && gameObject.CompareTag("Enemy"))
        {
            scoreManager.AddCoins(other.gameObject.transform.position, 1);
            Ragdoll();
            Random.InitState(System.DateTime.Now.Millisecond * (int)transform.position.z);
            pelvis.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-20, 20), Random.Range(-10, 20), Random.Range(0, 10)) * 10, ForceMode.Impulse);
            Destroy(other.gameObject);
            gameObject.layer = LayerMask.NameToLayer("Enemy");
            gameObject.GetComponent<Collider>().enabled = false;
            enemyBloodParticle.SetActive(true);
        }
        else if (other.CompareTag("PumpBullet") && (playerController.currentRange == RangeType.RifleEnemy || playerController.currentRange == RangeType.SniperObstacle || playerController.currentRange == RangeType.RocketEnemy))
        {
            scoreManager.AddCoins(other.gameObject.transform.position, 10);
            gameObject.layer = LayerMask.NameToLayer("Enemy");
            gameObject.GetComponent<Collider>().enabled = false;
            enemyConfetti.SetActive(true);
            foreach (var rb in rigidbodies)
            {
                if (rb != null)
                {
                    rb.isKinematic = false;
                    rb.useGravity = true;
                    rb.AddExplosionForce(250, other.transform.position, radius, 3);
                }
            }
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("RocketBullet") && (playerController.currentRange == RangeType.RifleEnemy || playerController.currentRange == RangeType.SniperObstacle || playerController.currentRange == RangeType.RocketEnemy))
        {
            if(playerController.currentRange == RangeType.RocketEnemy)
            {
                gameObject.GetComponent<Animation>().Play("Barikat");
                gameObject.GetComponent<Collider>().enabled = false;
            }
            scoreManager.AddCoins(other.gameObject.transform.position, 10);
            enemyConfetti.SetActive(true);
            gameObject.layer = LayerMask.NameToLayer("Enemy");
            foreach (var rb in rigidbodies)
            {
                if (rb != null)
                {
                    rb.isKinematic = false;
                    rb.useGravity = true;
                    rb.AddExplosionForce(10, transform.position, radius, 15, ForceMode.Impulse);
                }
            }
            gameObject.GetComponent<Collider>().enabled = false;
            Instantiate(GM.Instance.rocketParticle, other.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("RocketBullet") && playerController.currentRange == RangeType.RocketObstacle)
        {
            scoreManager.AddCoins(other.gameObject.transform.position, 10);
            EnemyController enemy;
            enemy = playerController.hitColliders[playerController.hitColliders.Count - 1].GetComponent<EnemyController>();
            enemy.enemyConfetti.SetActive(true);
            enemy.gameObject.layer = LayerMask.NameToLayer("Enemy");
            enemy.GetComponent<Collider>().enabled = false;

            foreach (var rb in playerController.hitColliders[playerController.hitColliders.Count - 1].GetComponent<EnemyController>().rigidbodies)
            {
                if (rb != null)
                {
                    rb.isKinematic = false;
                    rb.useGravity = true;
                    rb.AddExplosionForce(10, transform.position, radius, 1, ForceMode.Impulse);
                }
            }
            for (int i = 0; i < playerController.hitColliders.Count - 1; i++)
            {
                enemy = playerController.hitColliders[i].GetComponent<EnemyController>();
                if (enemy.animator != null)
                {
                    enemy.animator.enabled = false;
                }
                enemy.spine.transform.parent = enemy.rootTransform.transform;
                enemy.thighl.transform.parent = enemy.rootTransform.transform;
                enemy.thighr.transform.parent = enemy.rootTransform.transform;
                enemy.gameObject.layer = LayerMask.NameToLayer("Enemy");
                enemy.gameObject.GetComponent<Collider>().enabled = false;

                foreach (var item2 in enemy.rigidbodies)
                {
                    item2.useGravity = true;
                    item2.isKinematic = false;
                }
                Random.InitState(System.DateTime.Now.Millisecond * (int)transform.position.z);
                enemy.pelvis.GetComponent<Rigidbody>().AddExplosionForce(200, transform.position, 100, 10, ForceMode.Impulse);
                enemy.pelvis.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-10, 10), 0, 0) * 1, ForceMode.Impulse);
                enemy.GetComponent<Collider>().enabled = false;
                enemy.enemyBloodParticle.SetActive(true);
            }
            Instantiate(GM.Instance.rocketParticle, other.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("RocketBullet") && playerController.currentRange == RangeType.SniperEnemy)
        {
            scoreManager.AddCoins(other.gameObject.transform.position, 3);
            int index = playerController.hitColliders.IndexOf(gameObject.GetComponent<EnemyController>().GetComponent<Collider>());
            for (int i = 0; i < playerController.hitColliders.Count - (playerController.hitColliders.Count - 3); i++)
            {
                EnemyController enemy = playerController.hitColliders[index - i].GetComponent<EnemyController>();
                enemy.enemyBloodParticle.SetActive(true);
                if (enemy.animator != null)
                {
                    enemy.animator.enabled = false;
                }
                enemy.spine.transform.parent = playerController.hitColliders[index - i].GetComponent<EnemyController>().rootTransform.transform;
                enemy.thighl.transform.parent = playerController.hitColliders[index - i].GetComponent<EnemyController>().rootTransform.transform;
                enemy.thighr.transform.parent = playerController.hitColliders[index - i].GetComponent<EnemyController>().rootTransform.transform;
                enemy.gameObject.layer = LayerMask.NameToLayer("Enemy");
                enemy.gameObject.GetComponent<Collider>().enabled = false;

                foreach (var item2 in enemy.rigidbodies)
                {
                    item2.useGravity = true;
                    item2.isKinematic = false;
                }
                Random.InitState(System.DateTime.Now.Millisecond * (int)transform.position.z);
                enemy.pelvis.GetComponent<Rigidbody>().AddExplosionForce(250, transform.position, 100, 15, ForceMode.Impulse);
                enemy.pelvis.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-10, 10), 0, 0) * 10, ForceMode.Impulse);
            }
            playerController.selectGun.GetComponent<Gun>().isClear = true;
            Instantiate(GM.Instance.rocketParticle, other.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            gameObject.layer = LayerMask.NameToLayer("Enemy");
        }
        else if (other.CompareTag("RocketBullet") && playerController.currentRange == RangeType.PumpEnemy)
        {
            scoreManager.AddCoins(other.gameObject.transform.position, 3);
            int count = playerController.hitColliders.Count - 2;
            if (playerController.hitColliders.Count <= 2) count = playerController.hitColliders.Count;
            for (int i = 0; i < count; i++)
            {
                EnemyController enemy = playerController.hitColliders[i].GetComponent<EnemyController>();
                enemy.enemyBloodParticle.SetActive(true);
                if (enemy.animator != null)
                {
                    enemy.animator.enabled = false;
                }
                enemy.spine.transform.parent = playerController.hitColliders[i].GetComponent<EnemyController>().rootTransform.transform;
                enemy.thighl.transform.parent = playerController.hitColliders[i].GetComponent<EnemyController>().rootTransform.transform;
                enemy.thighr.transform.parent = playerController.hitColliders[i].GetComponent<EnemyController>().rootTransform.transform;
                enemy.gameObject.layer = LayerMask.NameToLayer("Enemy");
                enemy.gameObject.GetComponent<Collider>().enabled = false;

                foreach (var item2 in playerController.hitColliders[i].GetComponent<EnemyController>().rigidbodies)
                {
                    item2.useGravity = true;
                    item2.isKinematic = false;
                }
                Random.InitState(System.DateTime.Now.Millisecond * (int)transform.position.z);
                enemy.pelvis.GetComponent<Rigidbody>().AddExplosionForce(250, transform.position, 100, 15, ForceMode.Impulse);
                enemy.pelvis.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-10, 10), 0, 0) * 10, ForceMode.Impulse);
            }
            playerController.selectGun.GetComponent<Gun>().isClear = true;
            Instantiate(GM.Instance.rocketParticle, other.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            gameObject.layer = LayerMask.NameToLayer("Enemy");
        }


    }
    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.DrawWireSphere(transform.position, radius);
    //    Gizmos.color = Color.red;
    //}

    public void Ragdoll()
    {
        if (animator != null)
            animator.enabled = false;

        spine.transform.parent = rootTransform.transform;
        thighl.transform.parent = rootTransform.transform;
        thighr.transform.parent = rootTransform.transform;

        foreach (var item in rigidbodies)
        {
            item.useGravity = true;
            item.isKinematic = false;
        }
    }
    public void EnemyFire()
    {
        if (!SmoothFollow.Instance.state)
        {
            GameObject createdBullet = Instantiate(enemyBullet, enemyBulletPosition.transform.position, Quaternion.identity);
            createdBullet.transform.DOMove(playerController.transform.position, 0.1f);
        }

    }

}
