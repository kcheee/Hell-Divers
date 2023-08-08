using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJGausCannon : MonoBehaviour
{
    //cannonFire 변수
    int cannonCount = 25;
    bool isCannonDone = true;

    //cannon Position 변수
    public Transform cannonPos;
    Vector3 originCannonAngle;
    public GameObject cannonImpactFactory;
    GameObject cannonImpact;

    //궤적line 변수
    public LineRenderer cannonLine;

    // Start is called before the first frame update
    void Start()
    {
        originCannonAngle = cannonPos.localEulerAngles;
        cannonLine = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (isCannonDone)
            {
                StartCoroutine(CannonFire());
            }
        }
    }

    IEnumerator CannonFire()
    {
        RaycastHit cannonHitInfo;

        //cannonFire 변수
        int cannonPosZDir = -1;
        float cannonPosX = 0;
        float cannonPosXadd = 0.1f;

        //이미 조건 안에 들어왔으므로, 또 눌렀을 때 다시 들어오지 못하게 하기 위해서
        isCannonDone = false;

        for (int i = 0; i < cannonCount; i++)
        {
            //cannonPosition Angle값들
            if (i % 4 == 0)
            {
                cannonPosX += cannonPosXadd;
                cannonPosZDir *= -1;
            }

            //lineRenderer1
            cannonLine.SetPosition(0, cannonPos.position);

            if (Physics.Raycast(cannonPos.position, cannonPos.up, out cannonHitInfo, float.MaxValue))
            {
                cannonImpact = EJObjectPoolMgr.instance.GetGausCannonImpactQueue();               

                cannonImpact.transform.position = cannonHitInfo.point;
                cannonImpact.transform.forward = cannonHitInfo.normal;
                cannonImpact.transform.parent = cannonHitInfo.transform;

                cannonLine.SetPosition(1, cannonHitInfo.point);
                cannonLine.enabled = true;
            }

            //line을 이어지게 수정
            cannonPos.Rotate(new Vector3(cannonPosX, 0, 5 * cannonPosZDir), Space.Self);

            yield return new WaitForSeconds(0.02f);
        }

        cannonPos.transform.localEulerAngles = originCannonAngle;
        cannonLine.enabled = false;
        isCannonDone = true;
    }
}
