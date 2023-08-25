using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerHP : MonoBehaviourPun,I_Entity
{
    public System.Action Ondie;
    Animator anim;
    public GameObject DamageEft;
    public GameObject DieEft;
    public enum State { Live, Wounded,Die }
    public State current_State;
    public int HP
    {
        get { return hp; }
        set { hp = value; 
            if (hp < 0) {
                //photonView.RPC(nameof(die), RpcTarget.All, Ondie);
                
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
        if(HP > 0)
        {
            HP -= damage;
            Instantiate(DamageEft, pos, Quaternion.identity);
            
        }
        else
        {
            if (current_State != State.Die) {
                die(Ondie);
                current_State = State.Die;
            }
            
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void die(Action action)
    {
        Instantiate(DieEft, transform.position, Quaternion.identity);
        action();

        Invoke("activeFalse", 3f);
    }
    public void activeFalse()
    {
        gameObject.SetActive(false);
        
    }
}
