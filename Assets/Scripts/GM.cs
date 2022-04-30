
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GM : MonoBehaviour
{
    PlayerController playerController;
    public GameObject confetti, levelPassed, gameOver, ingamesScreen, tutorialPanel, level,rocketParticle, finger;
    public int currentLevel;
    public Text level_text;
    public bool testMode;
    public static GM Instance;
    public bool firstDown;
    public Slider slider;

    public Transform startPoint;
    public Transform endPoint;
    private float currentDistance, totalDistance = 0;

    private void Awake()
    {
        Instance = this;
        playerController = FindObjectOfType<PlayerController>();
    }
    void Start()
    {
        Application.targetFrameRate = 60;
        currentLevel = PlayerPrefs.GetInt("Level", 0);
        if (!testMode) OpenCurrentLevel();

		startPoint = GameObject.FindWithTag("Player").transform;
		endPoint = GameObject.FindWithTag("Finish").transform;
		totalDistance = Vector3.Distance(startPoint.position, endPoint.position);
		if (currentLevel == 0)
		{
			InvokeRepeating(nameof(TutorialPanel), 0, 0.5f);
		}
	}

    // Update is called once per frame
    void Update()
    {
		currentDistance = Vector3.Distance(playerController.transform.position, endPoint.transform.position);
		slider.value = 1 - (currentDistance / totalDistance);
	}
   
   
    private void OpenCurrentLevel()
    {
        level_text.text = "" + (currentLevel + 1);
        if (currentLevel > 4)
        {
            Random.InitState(System.DateTime.Now.Millisecond);
            currentLevel = Random.Range(1, 5);
            level = (GameObject)Instantiate(Resources.Load("Level" + currentLevel));
        }
        else
        {
            level = (GameObject)Instantiate(Resources.Load("Level" + currentLevel));
        }
    }

    public void seviyeAtlama()
    {
        PlayerPrefs.SetInt("Tutorial", 1);
        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Restart()
    {
         SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }
    public void Fail()
    {
        StartCoroutine(nameof(GameOver));
    }
    public void Win()
    {
        StartCoroutine(nameof(CompleteLevel));
    }
    public IEnumerator GameOver()
    {
        CancelInvoke(nameof(TutorialPanel));
        yield return new WaitForSeconds(.1f);
        //Player.Instance.started = false;
        //Player.Instance.speed = 0;
        ingamesScreen.transform.gameObject.SetActive(false);
        gameOver.SetActive(true);
        levelPassed.SetActive(false);
        tutorialPanel.SetActive(false);
       // StartCoroutine(OyunaBasla());
        
    }

    IEnumerator OyunaBasla()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    IEnumerator KonfettiPatlat()
    {
        yield return new WaitForSeconds(0.3f);
        confetti.SetActive(true);
    }

    public IEnumerator CompleteLevel()
    {
        CancelInvoke(nameof(TutorialPanel));
        yield return new WaitForSeconds(0.1f);
        ingamesScreen.SetActive(false);
        StartCoroutine(KonfettiPatlat());
        levelPassed.SetActive(true);
        gameOver.SetActive(false);
        tutorialPanel.SetActive(false);
    }
    public void TutorialPanel()
    {
        
        if (playerController.currentRange == RangeType.SniperEnemy)
        {
            if (playerController.gunType == GunType.sniper)
			{
                tutorialPanel.SetActive(false);
            }
			else
			{
                tutorialPanel.SetActive(true);
                finger.transform.localPosition = new Vector3(-231, -54, 148);
            }
        }
        else if (playerController.currentRange == RangeType.PumpEnemy)
        {
            if (playerController.gunType == GunType.pump)
            {
                tutorialPanel.SetActive(false);
            }
            else
            {
                tutorialPanel.SetActive(true);
                finger.transform.localPosition = new Vector3(273, -54, 148);
            }
            
        }
        else if (playerController.currentRange == RangeType.RocketEnemy)
        {
            if (playerController.gunType == GunType.rocket)
            {
                tutorialPanel.SetActive(false);
            }
            else
            {
                tutorialPanel.SetActive(true);
                finger.transform.localPosition = new Vector3(16, -54, 148);
            }
            
        }
    }
    public void AwakeKodunaYazilacakGameAnalitks()
    {
        //Instance nin bi alt satırına yapıştır.
        /* GameAnalytics.Initialize();
       GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, PlayerPrefs.GetInt("Level").ToString());
       if (FB.IsInitialized)
       {
           FB.ActivateApp();
       }
       else
       {
           //Handle FB.Init
           FB.Init(() =>
           {
               FB.ActivateApp();
           });
       }*/
    }
    /* public void Titret()
     {
         MMVibrationManager.Haptic(HapticTypes.LightImpact, true, this);
     }*/
    // InvokeRepeating(nameof(Titret), 0, 0.1f);
   // CancelInvoke(nameof(Titret));
}
