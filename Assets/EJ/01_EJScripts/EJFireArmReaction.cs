using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EJFireArmReaction : MonoBehaviour
{
    public GameObject rightArm;
    public GameObject leftArm;

    // �̵� ����: pos1 - pos2 - pos3 - pos1
    Vector3 pos1;
    Vector3 pos2;
    Vector3 pos3;

    // ��ġ üũ
    bool isPos1 = false;
    bool isPos2 = false;
    bool isPos3 = false;

    // Start is called before the first frame update
    void Start()
    {
        pos1 = rightArm.transform.localPosition;
        pos2 = new Vector3(0, 0, 1);
        pos3 = new Vector3(0, 0, -0.6f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            move();
        }
    }


    void move()
    {
        transform.DOMove(pos2, 1f).OnComplete(() =>
          {
              transform.DOMove(pos3, 1f).OnComplete(() =>
            {
                transform.DOMove(pos1, 1f).OnComplete(() =>
                {
                    Debug.Log("ddddd");
                });
            });
          });

    }

    IEnumerator MovingArm()
    {
        //01. distance �Ÿ��� �����ٸ�
        //02. coroutine
        //03. delegate�� ������ ������

        if (!isPos2 && isPos1 && isPos3)
        {
            rightArm.transform.localPosition = Vector3.Lerp(rightArm.transform.localPosition, pos2, 0.3f);
            isPos2 = true;
        }
        if (!isPos3 && isPos1 && isPos2)
        {
            rightArm.transform.localPosition = Vector3.Lerp(rightArm.transform.localPosition, pos3, 0.3f);
            isPos3 = true;
        }
        if (!isPos1 && isPos2 && isPos3)
        {
            rightArm.transform.localPosition = Vector3.Lerp(rightArm.transform.localPosition, pos1, 0.3f);
            isPos1 = true;
        }

        yield return null;
    }

    IEnumerator MovingArm2()
    {
        while (!isPos2 || !isPos3 || !isPos1)
        {
            if (!isPos2 && isPos1 && isPos3)
            {
                rightArm.transform.localPosition = Vector3.Lerp(rightArm.transform.localPosition, pos2, 0.3f);
                isPos2 = true;
            }
            else if (!isPos3 && isPos1 && isPos2)
            {
                rightArm.transform.localPosition = Vector3.Lerp(rightArm.transform.localPosition, pos3, 0.3f);
                isPos3 = true;
            }
            else if (!isPos1 && isPos2 && isPos3)
            {
                rightArm.transform.localPosition = Vector3.Lerp(rightArm.transform.localPosition, pos1, 0.3f);
                isPos1 = true;
            }

            yield return null;
        }
    }
}
