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

    //bombMuzzleFX
    public GameObject bombMuzzleFactory;

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
        for (int i = 0; i < bombCount; i++)
        {
            //bomb 积己
            bomb = EJObjectPoolMgr.instance.GetbombQueue();

            bomb.transform.position = bombPos.position;
            bomb.transform.up = bombPos.transform.up;

            //bombMuzzle 积己
            GameObject bombMuzzleImpact = Instantiate(bombMuzzleFactory);

            bombMuzzleImpact.transform.position = bombPos.position;
            bombMuzzleImpact.transform.localScale = Vector3.one * 4;
            bombMuzzleImpact.transform.up = bombPos.transform.forward;

            //酿鸥烙
            yield return new WaitForSeconds(0.5f);
            BossFSM.Sflag = false;
        }

        isBombDone = true;        
    }
}