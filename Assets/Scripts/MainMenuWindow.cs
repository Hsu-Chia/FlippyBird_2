/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class MainMenuWindow : MonoBehaviour {

    private void Awake() {
       var playBtn = transform.Find("playBtn").GetComponent<Button_UI>();
        playBtn.ClickFunc = () => { Loader.Load(Loader.Scene.GameScene); };
        SoundManager_Andy.AddButtonSounds(playBtn); // ✅ 明確使用 SoundManager_Andy 版本

        var quitBtn = transform.Find("quitBtn").GetComponent<Button_UI>();
        quitBtn.ClickFunc = () => { Application.Quit(); };
        SoundManager_Andy.AddButtonSounds(quitBtn); // ✅ 明確使用 SoundManager_Andy 版本
    }

}
