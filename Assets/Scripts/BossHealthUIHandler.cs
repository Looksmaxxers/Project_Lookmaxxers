using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using UnityEngine.UI;
using TMPro;

public class BossHealthUIHandler : MonoBehaviour
{
    private CanvasGroup cg;
    public bool nearBoss;
    public PlayerRange pr;
    public Scrollbar EntityHealth;
    public mikeAi mike;
    public TMP_Text BossName;

    // Start is called before the first frame update
    void Start()
    {
        cg = GetComponent<CanvasGroup>();
        nearBoss = false;
        cg.alpha = 0.0f;
        BossName.text = mike.bossName;
    }

    // Update is called once per frame
    void Update()
    {
        EntityHealth.size = (mike.currHP / mike.maxHP);

        nearBoss = pr.isNearEntity;
        if (nearBoss)
        {
            cg.alpha = 1.0f;
        } else
        {
            cg.alpha = 0.0f;
        }
    }
}
