using UnityEngine;

public class OptionManager : MonoBehaviour
{
    public int maxFps;

    void Start()
    {
        Application.targetFrameRate = maxFps;
    }

    void Update()
    {
        
    }
}
