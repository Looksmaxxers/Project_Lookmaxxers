using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    
    [SerializeField] private Vector3 offset;

    private Slider healthSlider;
    private Slider staminaSlider;
    private EnemyStats stats;
    private Camera camera;

    private void Awake()
    {
        stats = GetComponent<EnemyStats>();
        camera = Camera.main;

        healthSlider = canvas.transform.GetChild(1).GetComponent<Slider>();
        staminaSlider = canvas.transform.GetChild(0).GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        healthSlider.value = stats.curHealth / stats.maxHealth;
        staminaSlider.value = stats.curStamina / stats.maxStamina;
        if (stats.curHealth <= 0)
        {
            canvas.gameObject.SetActive(false);
        }

        canvas.transform.rotation = camera.transform.rotation;
        canvas.transform.position = transform.position + offset;
    }
}
