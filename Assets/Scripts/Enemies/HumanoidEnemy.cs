using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidEnemy : MonoBehaviour
{
    ScoreManager scoreManager;
    PlayerController playerController;
    public Animator animator;
    public GameObject spine;
    public GameObject thighl;
    public GameObject thighr;
    public GameObject rootTransform;
    public GameObject pelvis;
    public GameObject enemyBloodParticle;
    public GameObject enemyBullet, enemyBulletPosition;

    public Rigidbody[] rigidbodies;

	private void Awake()
	{
        playerController = FindObjectOfType<PlayerController>();
        scoreManager = FindObjectOfType<ScoreManager>();
        animator = GetComponent<Animator>();
	}
	public void DoRagdoll(int force)
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
        Random.InitState(System.DateTime.Now.Millisecond * (int)transform.position.z);
        pelvis.GetComponent<Rigidbody>().AddExplosionForce(200, transform.position, 100, 15, ForceMode.Impulse);
        pelvis.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-20, 20), Random.Range(-10, 20), Random.Range(0, 10)) * force, ForceMode.Impulse);
        scoreManager.AddCoins(gameObject.transform.position, 1);
        gameObject.layer = LayerMask.NameToLayer("Enemy");
        gameObject.GetComponent<Collider>().enabled = false;
        Invoke(nameof(EnemyBlood), 0.2f);
    }
    public void EnemyFire()
    {
        if (!SmoothFollow.Instance.state)
        {
            GameObject createdBullet = Instantiate(enemyBullet, enemyBulletPosition.transform.position, Quaternion.identity);
            createdBullet.transform.DOMove(playerController.transform.position, 0.1f);
        }

    }
    public void EnemyBlood()
	{
        if(pelvis.transform.position.y <= 0.1f || pelvis.transform.position.y >= -0.1f)
        {
            enemyBloodParticle.SetActive(true);
        }
		else
		{
            Invoke(nameof(EnemyBlood), 0.2f);
        }
	}
}
