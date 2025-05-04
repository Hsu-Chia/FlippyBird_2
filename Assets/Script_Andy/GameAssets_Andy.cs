using UnityEngine;

public class GameAssets_Andy : MonoBehaviour
{
    private static GameAssets_Andy _instance;

    public static GameAssets_Andy GetInstance()
    {
        return _instance;
    }

    private void Awake()
    {
        _instance = this;
        
    }
    public Sprite pipeHeadSprite;
    public Transform pfPipeHead;
    public Transform pfPipeBody;
}
