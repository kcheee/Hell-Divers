using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJBombFire : MonoBehaviour
{
    //bomb
    bool isBombDone = true;
    int bombCount = 4;

    //bombPos
    public Transform bombPos;
    Vector3 originBombAngle;
    GameObject bomb;
    GameObject bombMuzzleImpact;

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

    public void Test()
    {
        Debug.Log("fds");


    }
    public IEnumerator MakeBomb()
    {
        isBombDone = false;
        Debug.Log("fds");
        for (int i = 0; i < bombCount; i++)
        {
            //bomb ����
            bomb = EJObjectPoolMgr.instance.GetbombQueue();

            bomb.transform.position = bombPos.position;
            bomb.transform.up = bombPos.transform.up;

            //bombMuzzle ����
            bombMuzzleImpact = EJObjectPoolMgr.instance.GetbombImpactQueue();

            bombMuzzleImpact.transform.position = bombPos.position;
            bombMuzzleImpact.transform.up = bombPos.transform.forward;

            yield return new WaitForSeconds(0.5f);
        }

        isBombDone = true;        
    }
}