using UnityEngine;

public class LoaderUpdate_Andy : MonoBehaviour
{
    void Start()
    {
        // 等 1 秒模擬載入時間
        Invoke(nameof(ContinueToTargetScene), 1f);
    }


    // Update is called once per frame
    void ContinueToTargetScene()
    {
        Loader_Andy.LoadTargetScene();
    }
}
