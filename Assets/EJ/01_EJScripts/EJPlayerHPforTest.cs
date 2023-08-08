using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJPlayerHPforTest : MonoBehaviour
{
    float curHP;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void HitByBodyExplosion(float damage)
    {
        curHP -= damage; 
        // ¸®¾×¼Ç 
    }
}
