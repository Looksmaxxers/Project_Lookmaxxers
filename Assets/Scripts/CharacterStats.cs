using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] private float curHealth;
    [SerializeField] private float maxHealth;
    [SerializeField] public Scrollbar HSlider;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HSlider.size = (curHealth / maxHealth);
    }
}
