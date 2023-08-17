using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : MonoBehaviour,I_Entity
{
    public System.Action Ondie;
    Animator anim;

    public int HP
    {
        get { return hp; }
        set { hp = value; 
            if (hp < 0) {
                die(Ondie);
            } 
        }
    }
    public int hp;
    public int maxHp;
    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
        anim = GetComponentInChildren<Animator>();
        
    }
    public void damaged(int damage)
    {
        HP -= damage;
        Debug.Log("Player Damaged");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void die(Action action)
    {
        action();
        Destroy(gameObject, 3f);
    }
}
