using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] private float curHealth;
    [SerializeField] private float maxHealth;
    [SerializeField] private float curStamina;
    [SerializeField] private float maxStamina;
    [SerializeField] private int flaskNum;
    [SerializeField] private Scrollbar HSlider;
    [SerializeField] private Scrollbar SSlider;
    [SerializeField] private TMP_Text FlaskNumDisplay;
    [SerializeField] private float staminaRecoverScalar;

    [SerializeField] private string weapon;

    // Start is called before the first frame update
    void Start()
    {
        flaskNum = 5;
    }

    public void damageCharacterHealth(float damageValue)
    {
        float netHealth = curHealth - damageValue;
        curHealth = netHealth >= 0 ? netHealth : 0;
    }

    public void setCharacterHealth(float health)
    {
        curHealth = health;
    }

    public void spendStamina(float spentValue)
    {
        float netStamina = curStamina - spentValue;
        curStamina = netStamina >= 0 ? netStamina : 0;
    }

    // Update is called once per frame
    void Update()
    {
        HSlider.size = (curHealth / maxHealth);
        SSlider.size = (curStamina / maxStamina);
        FlaskNumDisplay.text = flaskNum.ToString();


    }
}
