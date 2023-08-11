using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EJPlayerHPforTest : MonoBehaviour
{
    public static EJPlayerHPforTest instance;

    public Image HPBar;
    public TextMeshProUGUI HPText;

    float curHP;
    float maxHP = 120;
    float HPbarFill;

    Camera mainCam;

    private void Awake()
    {
        instance = this;
        mainCam = Camera.main;
    }
    // Start is called before the first frame update
    void Start()
    {
        HP = maxHP;
        HPbarFill = HPBar.fillAmount;
        HPbarFill = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (HP < 0)
        {
            Destroy(gameObject);
        }
    }

    public void DamageHP(float damage)
    {
        if (HP > 0)
        {
            curHP -= damage;
            HPbarFill = 1- 1 / HP * damage;
        }
    }

    public float HP
    {
        get
        {
            return curHP;           
        }
        set
        {
            curHP = value;
            HPText.text = $"{value}";                   
        }
    }
}
