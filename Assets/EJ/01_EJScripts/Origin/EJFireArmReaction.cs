using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EJFireArmReaction : MonoBehaviour
{
    public GameObject rightArm;
    public GameObject leftArm;

    // 이동 순서: pos1 - pos2 - pos3 - pos1
    Vector3 pos1origin;
    Vector3 pos2;
    Vector3 pos3;

    // 위치 체크
    bool isPos1 = false;
    bool isPos2 = false;
    bool isPos3 = false;

    // Start is called before the first frame update
    void Start()
    {
        pos1origin = rightArm.transform.localPosition;
        pos2 = rightArm.transform.localPosition + new Vector3(0, 1, 1);
        pos3 = rightArm.transform.localPosition + new Vector3(0, 1, -0.6f);
    }

    // Update is called once per frame
    void Update()
    {
        {
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                RightArmmove();
            }
        }


        void RightArmmove()
        {
            rightArm.transform.DOMove(pos2, 0.2f).OnComplete(() =>
              {
                  rightArm.transform.DOMove(pos3, 0.4f).OnComplete(() =>
                {
                    rightArm.transform.DOMove(pos1origin, 0.2f).OnComplete(() =>
                    {
                        Debug.Log("ddddd");
                        rightArm.transform.position = pos1origin;
                    });
                });
              });

        }

        IEnumerator MovingArm()
        {
            //01. distance 거리가 가깝다면
            //02. coroutine
            //03. delegate로 실행이 끝나면

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
                rightArm.transform.localPosition = Vector3.Lerp(rightArm.transform.localPosition, pos1origin, 0.3f);
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
                    rightArm.transform.localPosition = Vector3.Lerp(rightArm.transform.localPosition, pos1origin, 0.3f);
                    isPos1 = true;
                }

                yield return null;
            }
        }

    }
    public void ONRightArmAnim()
    {

        GameObject.FindWithTag("rigthArm").GetComponentInChildren<Animator>().enabled = true;
    }

    public void OFFRightArmAnim()
    {
        GameObject.FindWithTag("rigthArm").GetComponentInChildren<Animator>().enabled = false;
    }
    public void ONLeftArmAnim()
    {
        GameObject.FindWithTag("leftArm").GetComponentInChildren<Animator>().enabled = true;
    }
    public void OFFLeftArmAnim()
    {
        GameObject.FindWithTag("leftArm").GetComponentInChildren<Animator>().enabled = false;
    }
}
