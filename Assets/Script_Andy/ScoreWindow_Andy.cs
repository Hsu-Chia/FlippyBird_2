using System;
using UnityEngine;
using UnityEngine.UI;

public class ScoreWindow_Andy : MonoBehaviour
{
    private Text highscoreText;
    private Text scoreText;
    private void Awake()
    {
       scoreText = transform.Find("scoreText").GetComponent<Text>();
       highscoreText = transform.Find("highscoreText").GetComponent<Text>();
    }

    private void Start()
    {
        highscoreText.text = "HighScore"+ Score_Andy.GetHighscore().ToString();
    }

    private void Update()
    {
        scoreText.text = Level_Andy.GetInstance().GetPipesPassedCount().ToString();
    }
}
