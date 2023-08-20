using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJBossHP : MonoBehaviour
{
    public static EJBossHP instance;

    float currentHP;
    float maxHP = 300000;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        HP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float HP
    {
        get
        {
            return currentHP;
        }
        set
        {
            currentHP = value;
        }
    }

    public void BOSS_HP_DAMAGE(float damage)
    {
        if (HP > 0)
        {
            HP -= damage;
        }
    }
}
