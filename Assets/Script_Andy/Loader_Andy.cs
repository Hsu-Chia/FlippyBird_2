using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader_Andy
{
    public enum Scene
    {
        GameScene,
        LoadingScene,
    }
    private static Scene targetScene;

    public static void Load(Scene scene)
    {
        // 記住要載入的最終場景
        targetScene = scene;

        // 先載入 LoadingScene
        SceneManager.LoadScene("Scenes_Andy/Loading");
    }

    public static void LoadTargetScene()
    {
        // 在 LoadingScene 執行時呼叫，實際切去真正場景
        switch (targetScene)
        {
            case Scene.GameScene:
                SceneManager.LoadScene("Scenes_Andy/GameScene");
                break;
            default:
                Debug.LogError("❌ Unknown target scene: " + targetScene);
                break;
        }
    }
}
