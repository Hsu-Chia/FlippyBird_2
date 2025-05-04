using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class GameHandler_Andy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("GameHandler Started!");
        
        GameObject gameObject = new GameObject("Pipe",typeof(SpriteRenderer));
        gameObject.GetComponent<SpriteRenderer>().sprite =GameAssets_Andy.GetInstance().pipeHeadSprite; 

    }
}
