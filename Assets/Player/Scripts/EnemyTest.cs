using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : MonoBehaviour
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

    public void Damaged(float damage) {
        Debug.Log("Èå¾Ó ¾ÆÆÄ¿ë¤Ð");
        Hp-= damage;
        if (Hp <= 0) {
            Debug.Log("Áê±Ý");
            Destroy(gameObject);
            SpawnerTest.instance.Spawn(1);
        }
    }
}
