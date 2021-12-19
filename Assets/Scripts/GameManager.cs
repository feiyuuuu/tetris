using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject gameOverText;
    public Text scoreText;
    public Text gametimeText;

    [HideInInspector]
    public int score;
    private float currentSessionStartTime;
    private float currentSessionTime;

    public GameObject pause;

    [HideInInspector]
    public AudioSource audioSource;
    public AudioClip audioClip_Transform;
    public AudioClip audioClip_DestoryOneRow;

    private void Awake()
    {
        if(Instance==null)
            Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        currentSessionStartTime = Time.time;
    }


    private void Update()
    {
        UpdateGameInfo();
    }

    private void UpdateGameInfo()
    {
        scoreText.text = score.ToString();
        currentSessionTime = Time.time - currentSessionStartTime;
        gametimeText.text = currentSessionTime.ToString("F1")+"S";
    }

    public void GameOver()
    {
        gameOverText.SetActive(true);
        pause.SetActive(false);
        Time.timeScale = 0;
    }

    public void OnClickBtn_Pause()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            pause.transform.GetChild(0).GetComponent<Text>().text = "START";
        }
        else
        {
            Time.timeScale = 1;
            pause.transform.GetChild(0).GetComponent<Text>().text = "PAUSE";
        }
    }

    public void OnClickBtn_Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void OnClickBtn_Quit()
    {
        Application.Quit();
    }
}
