using UnityEngine;

public class OptionManager : MonoBehaviour
{
    [Range(0, 2)]
    public int vSyncCount;
    public int maxFps;

    void Start()
    {
        QualitySettings.vSyncCount = vSyncCount;
        Application.targetFrameRate = maxFps;
    }

    void Update()
    {
        
    }
}
