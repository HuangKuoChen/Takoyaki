using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class GameManger : MonoBehaviour {

    public static GameManger Instance;
    public GameObject SingleBtn, DoubleBtn, PauseBtn, ContinueBtn, MenuBtn, ExitBtn, HighScoreBtn, HelpBtn;
    public GameObject HighScore, Help;
    public AudioSource BackgroundMusic, MenuMusic, EndMusic, ButtonSound;
    public Takoyaki[] TakoyakiObj = new Takoyaki[15];
    public Text TimerText, GameScoreText, HighScoreText;
    public bool play, pause;
    public int CountdownTime, Score;
    public float GameTime; 

    // Use this for initialization
    void Start () {
        SingleBtn.SetActive(true);
        DoubleBtn.SetActive(true);
        ExitBtn.SetActive(true);
        ContinueBtn.SetActive(false);
        MenuBtn.SetActive(false);
        PauseBtn.SetActive(false);
        HighScoreBtn.SetActive(true);
        HelpBtn.SetActive(true);
        Instance = this;
        //Reset();
        TimerText.text = "";
        GameScoreText.text = "Score";
        MenuMusic.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (play && !pause)
        {
            GameTime += Time.deltaTime;         //int.Parse(Time.deltaTime.ToString());
            CountdownTime = 120 - Mathf.FloorToInt(GameTime);
            if (CountdownTime > 0)
                TimerText.text = CountdownTime.ToString();
            else
            {
                TimerText.text = "0";
                GameOver();
            }
                
        } else if(!play)
            TimerText.text = "";
        //if (Input.GetMouseButtonDown(0))
    }

    public void ViewHelp()
    {
        Help.SetActive(true);
    }

    public void CloseHelp()
    {
        Help.SetActive(false);
    }

    public void AddScore(int s)
    {
        Score += s;
        GameTime -= 5;
        GameScoreText.text = "Score  "+Score;
    }

    public void ViewHighScore()
    {
        HighScoreText.text = "";
        for (int i = 1; i < 11; i++)
        {
            HighScoreText.text += i.ToString().PadLeft(4- i.ToString().Length)+ "." + String.Format("{0,7}\n", PlayerPrefs.GetInt("NO" + i));
        }
        HighScore.SetActive(true);
    }

    public void CloseHighScore()
    {
        HighScore.SetActive(false);
    }

    public void SortHighScore()
    {
        // Initialize High Score
        if (!PlayerPrefs.HasKey("NO1"))
        {
            for (int i = 1; i < 11; i++)
            {
                PlayerPrefs.SetInt("NO" + i, 0);
                PlayerPrefs.Save();
            }
        }

        int compare = Score;
        for (int i = 1; i < 11; i++)
        {
            int temp = PlayerPrefs.GetInt("NO" + i);
            if(compare > temp)
            {
                Debug.Log("No " + i + " = " + compare);
                PlayerPrefs.SetInt("NO" + i, compare);
                PlayerPrefs.Save();
                compare = temp;
            } else
                Debug.Log("No " + i + " = "+temp);
        }
    }

    public void Reset()
    {
        play = false;
        pause = false;
        Score = 0;
        GameScoreText.text = "Score";
        for (int i = 0; i < 15; i++)
        {
            TakoyakiObj[i].ClearAll();
        }
    }

    public void Pause()
    {
        SingleBtn.SetActive(false);
        DoubleBtn.SetActive(false);
        ExitBtn.SetActive(true);
        ContinueBtn.SetActive(true);
        MenuBtn.SetActive(true);
        PauseBtn.SetActive(false);
        HighScoreBtn.SetActive(true);
        HelpBtn.SetActive(true);
        pause = true;
        ButtonSound.Play();
    }

    public void Continue()
    {
        SingleBtn.SetActive(false);
        DoubleBtn.SetActive(false);
        ExitBtn.SetActive(false);
        ContinueBtn.SetActive(false);
        MenuBtn.SetActive(false);
        PauseBtn.SetActive(true);
        HighScoreBtn.SetActive(false);
        HelpBtn.SetActive(false);
        pause = false;
        ButtonSound.Play();
    }

    public void Menu()
    {
        SingleBtn.SetActive(true);
        DoubleBtn.SetActive(true);
        ExitBtn.SetActive(true);
        ContinueBtn.SetActive(false);
        MenuBtn.SetActive(false);
        PauseBtn.SetActive(false);
        HighScoreBtn.SetActive(true);
        HelpBtn.SetActive(true);
        Reset();
        BackgroundMusic.Stop();
        MenuMusic.Play();
        ButtonSound.Play();
    }

    public void GameStart()
    {
        SingleBtn.SetActive(false);
        DoubleBtn.SetActive(false);
        ExitBtn.SetActive(false);
        ContinueBtn.SetActive(false);
        MenuBtn.SetActive(false);
        PauseBtn.SetActive(true);
        HighScoreBtn.SetActive(false);
        HelpBtn.SetActive(false);
        Reset();
        play = true;
        ButtonSound.Play();
        MenuMusic.Stop();
        EndMusic.Stop();
        BackgroundMusic.Play();
        GameTime = 0;
    }

    public void GameOver()
    {
        SingleBtn.SetActive(true);
        DoubleBtn.SetActive(true);
        ExitBtn.SetActive(true);
        ContinueBtn.SetActive(false);
        MenuBtn.SetActive(false);
        PauseBtn.SetActive(false);
        HighScoreBtn.SetActive(true);
        HelpBtn.SetActive(true);
        Customer.Instance.ClearAll();
        play = false;
        EndMusic.Play();
        MenuMusic.Play();
        BackgroundMusic.Stop();
        SortHighScore();
    }

    // Exit application
    public void Exit()
    {
        Application.Quit();
    }
}
