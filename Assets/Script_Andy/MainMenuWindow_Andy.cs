using System;
using UnityEngine;
using CodeMonkey.Utils;
using Unity.VisualScripting;

public class MainMenuWindow_Andy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        transform.Find("playBtn").GetComponent<Button_UI>().ClickFunc = () =>
        {
            Loader_Andy.Load(Loader_Andy.Scene.GameScene); };
    }
}
