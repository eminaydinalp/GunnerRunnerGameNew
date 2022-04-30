using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
public enum GunType { rocket, sniper, pump, empty };
public class PlayerController : MonoBehaviour
{
    public GameObject[] guns;

    public GameObject stickMan, selectGun, oldSelectGun, bloodParticle;

    public Animator animator;

    public Vector3 targetScale;

    public GunType gunType;
    public GunType oldGunType;
    public RangeType currentRange;

    public float playerSpeed = 7;
    public bool getGunControl;
    public int gunTypeSelect;
    public bool isJump;
    public bool isSecond;
    public bool isButton;

    public List<Collider> hitColliders;
    public LayerMask layer;

    public GameObject spine;
    public GameObject thighl;
    public GameObject thighr;
    public GameObject rootTransform;
    public GameObject pelvis;

    public Rigidbody[] rigidbodies;

    public GameObject rocketImage, sniperImage, rifleImage;
    private void Awake()
    {
        animator = stickMan.GetComponent<Animator>();
    }
    private void Start()
    {
        hitColliders = new List<Collider>();
        isJump = false;
        getGunControl = false;
        gunType = GunType.empty;
    }
    private void Update()
    {
        transform.position += Vector3.forward * playerSpeed * Time.deltaTime;
    }

    IEnumerator SpeedFix()
    {
        yield return new WaitForSeconds(1);
        playerSpeed = 5;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyBullet"))
        {
            Destroy(other.gameObject, 0.2f);
            StartCoroutine(Ragdoll());
        }
        else if (other.CompareTag("Finish"))
        {
            animator.SetBool("Finish", true);
            foreach (GunType val in Enum.GetValues(typeof(GunType)))
            {
                if (val != GunType.empty) animator.SetBool(val.ToString(), false);
            }
            playerSpeed = 0;
            GM.Instance.Win();
        }
        else if (other.CompareTag("ZirhObstacle") || other.CompareTag("BetonObstacle") || other.CompareTag("TuglaObstacle"))
        {
            StartCoroutine(Ragdoll());
        }
        else if (other.CompareTag("JumpAnim"))
        {
            isJump = true;
            animator.SetTrigger("Jump");
            getGunControl = true;
            playerSpeed = 10;
            StartCoroutine(SpeedFix());
        }
        else if (other.CompareTag("SlideAnim"))
        {
            animator.SetTrigger("Slide");
            getGunControl = true;
        }
    }

    public void ExplosionDamage(Vector3 center, float radius)
    {
        Collider[] Colliders = Physics.OverlapSphere(center, radius, layer);
        hitColliders.Clear();
        foreach (var item in Colliders)
        {
            hitColliders.Add(item);
        }
    }

    public void UiRocketButton(int selectGunType)
    {
        if ((GunType)selectGunType != this.gunType)
        {
            getGunControl = false;
            this.gunType = (GunType)selectGunType;

            animator.SetBool(this.gunType.ToString(), true);

            foreach (GunType val in Enum.GetValues(typeof(GunType)))
            {
                if (val != this.gunType) animator.SetBool(val.ToString(), false);
            }

        }

        if (gunType == GunType.rocket)
        {
            rocketImage.SetActive(true);
            sniperImage.SetActive(false);
            rifleImage.SetActive(false);
        }
        else if (gunType == GunType.sniper)
        {
            sniperImage.SetActive(true);
            rocketImage.SetActive(false);
            rifleImage.SetActive(false);
        }
        else if (gunType == GunType.pump)
        {
            rifleImage.SetActive(true);
            sniperImage.SetActive(false);
            rocketImage.SetActive(false);
        }
        isButton = true;
    }

    public void GetGun()
    {
        isButton = false;
        ExplosionDamage(transform.position, 20);

        if (oldSelectGun == null)
        {
            oldSelectGun = guns[(int)gunType];
        }
        else
        {
            oldSelectGun = selectGun;
            isSecond = true;
        }
        selectGun = guns[(int)gunType];

        targetScale = selectGun.transform.localScale;
        selectGun.transform.localScale = Vector3.zero;
        selectGun.SetActive(true);
        if (this.gunType == GunType.pump)
            selectGun.transform.DOScale(targetScale, 0.3f).OnComplete(() => PumpRotation());

        else
        {
            stickMan.transform.DOKill();
            stickMan.transform.DORotate(new Vector3(0, 19, 0), 0.2f);
            selectGun.transform.DOScale(targetScale, 0.3f);
        }        
    }
    
    public void GunControl()
    {
        getGunControl = true;
    }

    public void PumpRotation()
    {
        stickMan.transform.DORotate(new Vector3(0, 12, 0), 0.5f).OnComplete(() => stickMan.transform.DORotate(new Vector3(0, 30, 0), 1f).SetLoops(-1, LoopType.Yoyo));
    }
    public void ChangeEnum(RangeType changedEnum)
    {
        currentRange = changedEnum;
    }
    public IEnumerator Ragdoll()
    {
        gameObject.GetComponent<Collider>().enabled = false;

        if (animator != null)
            animator.enabled = false;

        spine.transform.parent = rootTransform.transform;
        thighl.transform.parent = rootTransform.transform;
        thighr.transform.parent = rootTransform.transform;

        foreach (var item in rigidbodies)
        {
            item.useGravity = true;
            item.isKinematic = false;
            item.velocity = Vector3.zero;
        }
        stickMan.transform.DOKill();
        SmoothFollow.Instance.state = true;
        playerSpeed = 0;
        bloodParticle.SetActive(true);
        yield return new WaitForSeconds(1f);
        GM.Instance.Fail();
    }
    public void AfterJumpAndSlide()
	{
        Gun gun = selectGun.GetComponent<Gun>();
        CancelInvoke(nameof(gun.BulletFrequency));
		if (isSecond && isButton)
		{
            getGunControl = true;
            oldSelectGun = selectGun;
            selectGun = guns[(int)gunType];
            selectGun.SetActive(true);
            oldSelectGun.SetActive(false);
        }
    }
}
