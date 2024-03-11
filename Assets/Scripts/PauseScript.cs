using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    public bool paused;

    private void Start()
    {
        Time.timeScale = 0;
        paused = true;
    }

    private void Update()
    {
        if (paused)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                Time.timeScale = 1;
                paused = false;
            }
        } else
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                Time.timeScale = 0;
                paused = true;
            }
        }
    }
}
