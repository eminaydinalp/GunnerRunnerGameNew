using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Anim : MonoBehaviour
{
    
    PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
    }
    
    public void ThrowGun()
    {
        //StartCoroutine(playerController.selectGun.GetComponent<Gun>().ThrowGunAnimation());
        playerController.selectGun.GetComponent<Gun>().ThrowGunAnimation();
    }
    public void ThrowBullet()
    {
        StartCoroutine(playerController.selectGun.GetComponent<Gun>().ThrowBullet());
    }
    public void BulletFrequency()
    {
        playerController.selectGun.GetComponent<Gun>().BulletFrequency();
    }
    public void GetGun()
    {
        playerController.GetGun();
    }
    public void GunControl()
    {
        playerController.GunControl();
    }
    public void AfterJumpAndSlide()
	{
       playerController.AfterJumpAndSlide();
	}
    
    
}
