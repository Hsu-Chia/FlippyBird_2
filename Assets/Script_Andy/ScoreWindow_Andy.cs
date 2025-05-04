using System;
using UnityEngine;
using UnityEngine.UI;

public class ScoreWindow_Andy : MonoBehaviour
{
    private Text scoreText;
    private void Awake()
    {
       scoreText = transform.Find("scoreText").GetComponent<Text>();
    }

    private void Update()
    {
        scoreText.text = Level_Andy.GetInstance().GetPipesPassedCount().ToString();
    }
}
