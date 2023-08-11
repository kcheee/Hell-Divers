using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Initiate : MonoBehaviour
{
    private void Start()
    {
        gameObject.GetComponent<Animator>().Play("Appear");
    }
    private void Update()
    {
        
    }
}
