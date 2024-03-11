using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isPaused;
    public CanvasGroup cg;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnButtonClick()
    {
        Debug.Log("Button Clicked");
        Time.timeScale = 1f;
        cg.interactable = false;
        cg.alpha = 0.0f;
    }
}
