using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private EnemyStats stats;
    [SerializeField] private Camera camera;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    private void Awake()
    {
        stats = GetComponent<EnemyStats>();
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
        slider.transform.position = target.position + offset;
    }
}
