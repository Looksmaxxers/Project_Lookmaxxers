using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{

    private bool paused;

    //// Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        paused = false;
    }

    //// Update is called once per frame
    void Update()
    {
        if (paused) {
            if (Input.GetKeyUp(KeyCode.Escape)) {
                Time.timeScale = 1;
                paused = false;
            }
        } else {
            if (Input.GetKeyUp(KeyCode.Escape)) {
                Time.timeScale = 0;
                paused = true;
            }
        }
    }
}
