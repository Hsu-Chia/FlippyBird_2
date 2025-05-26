using System;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class GameOverWindow_Andy : MonoBehaviour {
    [SerializeField] private GameObject contentRoot;
    private Text scoreText;

    private void Awake()
    {
        Debug.Log("🟩 GameOverWindow.Awake()");
        scoreText = transform.Find("contentRoot/scoreText").GetComponent<Text>();

        transform.Find("contentRoot/retryBtn").GetComponent<Button_UI>().ClickFunc = () => {
            Loader_Andy.Load(Loader_Andy.Scene.GameScene);
        };
        SoundManager_Andy.AddButtonSounds(transform.Find("contentRoot/retryBtn").GetComponent<Button_UI>());
        transform.Find("contentRoot/mainMenuBtn").GetComponent<Button_UI>().ClickFunc = () => {
            Loader_Andy.Load(Loader_Andy.Scene.MainMenu);
        };
        SoundManager_Andy.AddButtonSounds(transform.Find("contentRoot/mainMenuBtn").GetComponent<Button_UI>());

        Hide(); // ✅ 現在只隱藏內容，而不是整個物件
    }

    private void Start()
    {
        Debug.Log("🟩 GameOverWindow.Start()");
        var bird = Bird_Andy.GetInstance();
        if (bird == null)
        {
            Debug.LogError("❌ 找不到 Bird_Andy 實例，無法訂閱事件！");
            return;
        }

        bird.OnDied += Bird_OnDied;
    }

    private void Bird_OnDied(object sender, EventArgs e)
    {
        Debug.Log("🟥 GameOverWindow 收到死亡事件，應該顯示 UI");
        scoreText.text = Level_Andy.GetInstance().GetPipesPassedCount().ToString();
        Show();
    }

    private void Hide()
    {
        contentRoot.SetActive(false); // ✅ 不關掉整個 GameObject，只關內容
    }

    private void Show()
    {
        contentRoot.SetActive(true);
    }
}