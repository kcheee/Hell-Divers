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
    //public GameObject bombHead;

    //bombMuzzleFX
    public GameObject bombMuzzleFactory;

    //bombReaction

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
                StartCoroutine(MakeBomb(null));
            }
        }
    }

    public IEnumerator MakeBomb(System.Action<int> complete)
    {
        isBombDone = false;
        for (int i = 0; i < bombCount; i++)
        {
            //bomb ����
            bomb = EJObjectPoolMgr.instance.GetbombQueue();

            bomb.transform.position = bombPos.position;
            bomb.transform.up = bombPos.transform.up;

            //�����ϸ鼭 �ݵ� ����� ���ڸ��� �ϳ���!
            bombHeadReaction();

            //bombMuzzle ����
            GameObject bombMuzzleImpact = Instantiate(bombMuzzleFactory);

            bombMuzzleImpact.transform.position = bombPos.position;
            bombMuzzleImpact.transform.localEulerAngles =bombPos.transform.parent.localEulerAngles;
            bombMuzzleImpact.transform.localScale = Vector3.one ;
            bombMuzzleImpact.transform.up = bombPos.transform.forward;

            //��Ÿ��
            yield return new WaitForSeconds(0.5f);
            BossFSM.Sflag = false;
        }

        isBombDone = true;

        if(complete != null)
        {
            complete(0);
        }
    }

    //Coroutine ���ÿ� �߻��ϵ��� ��� �ϴ���?
    void bombHeadReaction()
    {
        //bombHead.transform.position = new Vector3(0, 0, 1);
        //bombHead.transform.position = Vector3.Lerp(bombHead.transform.position, Vector3.one, 0.7f);
    }

}