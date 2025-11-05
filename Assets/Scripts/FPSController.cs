using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    public int targetFPS = 60;

    void Awake()
    {
        QualitySettings.vSyncCount = 0; // Disable VSync to use targetFrameRate
        Application.targetFrameRate = targetFPS;
    }

}
