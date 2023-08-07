using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//bomb에 trail붙이기
public class EJBombFire : MonoBehaviour
{
    //bomb
    bool isBombDone = true;
    Rigidbody rb;
    int bombCount = 4;

    //bombPos
    public Transform bombPos;
    Vector3 originBombAngle;
    public GameObject bombFactory;

    // Start is called before the first frame update
    void Start()
    {
        originBombAngle = bombPos.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (isBombDone)
            {
                StartCoroutine(MakeBomb());
            }
        }
    }

    IEnumerator MakeBomb()
    {
        isBombDone = false;

        for (int i = 0; i < bombCount; i++)
        {
            GameObject bomb = Instantiate(bombFactory);

            bomb.transform.position = bombPos.position;
            bomb.transform.up = bombPos.transform.up;
            yield return new WaitForSeconds(0.5f);
        }

        isBombDone = true;
        
    }   
}