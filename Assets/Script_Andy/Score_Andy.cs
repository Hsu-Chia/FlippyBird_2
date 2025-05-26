using System;
using UnityEngine;

public static class Score_Andy
{
    public static void Start()
    {
        // 高分只重置一次，可視需求取消註解
        // ResetHighscore();

        Bird_Andy bird = Bird_Andy.GetInstance();
        if (bird != null)
        {
            bird.OnDied -= Bird_OnDied;
            bird.OnDied += Bird_OnDied;
        }
        else
        {
            Debug.LogError("❌ Score_Andy 無法綁定 Bird 的 OnDied，因為 instance 為 null");
        }
    }

    private static void Bird_OnDied(object sender, EventArgs e)
    {
        Level_Andy level = Level_Andy.GetInstance();
        if (level != null)
        {
            int score = level.GetPipesPassedCount();
            bool isNew = TrySetNewHighScore(score);
            Debug.Log(isNew ? $"🎉 新高分: {score}" : $"☑ 本次分數: {score}，未超過高分: {GetHighscore()}");
        }
        else
        {
            Debug.LogError("❌ 無法取得 Level_Andy instance");
        }
    }

    public static int GetHighscore()
    {
        return PlayerPrefs.GetInt("highscore", 0);
    }

    public static bool TrySetNewHighScore(int score)
    {
        int currentHighscore = GetHighscore();
        if (score > currentHighscore)
        {
            PlayerPrefs.SetInt("highscore", score);
            PlayerPrefs.Save();
            return true;
        }
        return false;
    }

    public static void ResetHighscore()
    {
        PlayerPrefs.SetInt("highscore", 0);
        PlayerPrefs.Save();
        Debug.Log("🔄 高分重置為 0");
    }
}