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
    //public GameObject bombFactory;
    GameObject bomb;

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
            bomb = EJObjectPoolMgr.instance.GetbombQueue();

            bomb.transform.position = bombPos.position;
            bomb.transform.up = bombPos.transform.up;

            //01.bombTrail을 만들어서 자식으로 붙인다.
            GameObject trail = new GameObject("trail");
            var tr = trail.AddComponent<TrailRenderer>();
            tr.time = 0.5f;
            trail.transform.parent = bomb.transform;
            trail.transform.localPosition = Vector3.zero;

            yield return new WaitForSeconds(0.5f);
        }

        isBombDone = true;        
    }   
}