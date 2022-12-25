using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionManager : MonoBehaviour
{
    public int maxFps;

    void Start()
    {
        
    }

    void Update()
    {
        Application.targetFrameRate = maxFps;
    }
}
