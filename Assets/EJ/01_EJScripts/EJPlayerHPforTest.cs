using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJPlayerHPforTest : MonoBehaviour
{
    public static EJPlayerHPforTest instance;

    public GameObject HPBarFactory;

    float curHP;
    float maxHP = 120;

    Camera mainCam;

    private void Awake()
    {
        instance = this;
        mainCam = Camera.main;
    }
    // Start is called before the first frame update
    void Start()
    {
        GameObject HPbar = Instantiate(HPBarFactory, transform.position, Quaternion.identity);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetHP(float damage)
    {
        if (HP > 0)
        {
            curHP -= damage;
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
        }
    }
}
