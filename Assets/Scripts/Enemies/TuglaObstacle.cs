using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuglaObstacle : MonoBehaviour
{
    ScoreManager scoreManager;
    public Rigidbody[] rigidbodies;
    public GameObject tuglaDuman;
    public float radius;

	private void Awake()
	{
        scoreManager = FindObjectOfType<ScoreManager>();
	}
	public void DoDestroy(int force)
	{
        foreach (var rb in rigidbodies)
        {
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
                rb.AddExplosionForce(force, transform.position, radius, 15, ForceMode.Impulse);
            }
        }
        gameObject.GetComponent<Collider>().enabled = false;
        scoreManager.AddCoins(gameObject.transform.position, 10);
        gameObject.layer = LayerMask.NameToLayer("Enemy");
        tuglaDuman.SetActive(true);
    }
}
