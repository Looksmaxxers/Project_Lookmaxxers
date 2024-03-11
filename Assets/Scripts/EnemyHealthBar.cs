using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    
    [SerializeField] private Vector3 offset;

    private EnemyStats stats;
    private Camera camera;

    private void Awake()
    {
        stats = GetComponent<EnemyStats>();
        camera = Camera.main;

    }

    // Update is called once per frame
    void Update()
    {
        slider.value = stats.curHealth / stats.maxHealth;
        if (stats.curHealth <= 0)
        {
            slider.gameObject.SetActive(false);
        }

        slider.transform.rotation = camera.transform.rotation;
        slider.transform.position = transform.position + offset;
    }
}
