using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerHP : MonoBehaviourPun,I_Entity
{
    public System.Action Ondie;
    Animator anim;

    public int HP
    {
        get { return hp; }
        set { hp = value; 
            if (hp < 0) {
                //photonView.RPC(nameof(die), RpcTarget.All, Ondie);
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
    [PunRPC]
    public void damaged(Vector3 pos ,int damage)
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
        Debug.Log("ししししししししし!!");
        action();
        Invoke("activeFalse", 3f);
    }
    public void activeFalse()
    {
        gameObject.SetActive(false);
        
    }
}
