using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class EnemyTest : MonoBehaviour,I_Entity
{
    public float Hp = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [PunRPC]
    public void damaged(Vector3 pos,int damage = 0)
    {
        Debug.Log("Èå¾Ó ¾ÆÆÄ¿ë¤Ð");
        Hp -= damage;
        if (Hp <= 0)
        {
            Debug.Log("Áê±Ý");
            Destroy(gameObject);
            SpawnerTest.instance.Spawn(1);
        }
    }

    public void die(Action action)
    {
        throw new NotImplementedException();
    }
}
