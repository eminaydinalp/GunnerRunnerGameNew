using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject WinPanel;
    public GameObject LosePanel;
    public GameObject WinConfetti;
    public GameObject rocketParticle;


    private void Awake()
    {
        instance = this;
    }

    public void ShowWinPanel()
    {
        WinPanel.SetActive(true);
        WinConfetti.SetActive(true);
    }
    public void ShowLosePanel()
    {
        LosePanel.SetActive(true);
        //Time.timeScale = 0;
    }
    public void LoadScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
    


}
