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

    //1. fire coroutine안에서 muzzle, hitpoint 두가지 이펙트를 켜고 끄고 싶다.
    //2. 생성된 후, 일정 시간이 지나면 끄고 싶다. 

    //1-1. 코루틴 안에 또 코루틴을 사용?
    //1-2. 한 루틴으로 한다고 하면 yield return 어떻게 집어 넣을지? 모르겠음.
    //코루틴이 실행되고 있고, 임팩트마다 사라지는 스크립트를 넣어서 붙여주면 된다.

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

            //lineRenderer
            cannonLine.SetPosition(0, cannonPos.position);

            GameObject gausCannonMuzzleImpact = EJObjectPoolMgr.instance.GetGausCannonMuzzleImpactQueue();
            //01.총구에 Effect?????????????????????????????????
            //!!StartCoroutine(GausCannonMuzzleImpact());

            if (Physics.Raycast(cannonPos.position, cannonPos.up, out cannonHitInfo, float.MaxValue))
            {
                cannonImpact = EJObjectPoolMgr.instance.GetGausCannonImpactQueue();
                
                //02. 맞은 곳에 Effect
                cannonImpact.transform.position = cannonHitInfo.point;
                cannonImpact.transform.forward = cannonHitInfo.normal;
                cannonImpact.transform.parent = cannonHitInfo.transform;

                //LineRenderer
                cannonLine.SetPosition(1, cannonHitInfo.point);
                cannonLine.enabled = true;
            }

            cannonPos.Rotate(new Vector3(cannonPosX, 0, 5 * cannonPosZDir), Space.Self);

            yield return new WaitForSeconds(0.02f);
        }

        //02.For문이 다 돌고 나면 모두 한번에 꺼지도록??????????????????????????????????
        //!!EJObjectPoolMgr.instance.ReturnGausCannonImpactQueue(cannonImpact);

        //CannonPos 초기화
        cannonPos.transform.localEulerAngles = originCannonAngle;
        cannonLine.enabled = false;
        isCannonDone = true;
    }

    //01.총구에 Effect
    //muzzle을 키고 끄는 코루틴을 또 새로?????????
    IEnumerator GausCannonMuzzleImpact()
    {
        GameObject gausCannonMuzzleImpact = EJObjectPoolMgr.instance.GetGausCannonMuzzleImpactQueue();
        yield return new WaitForSeconds(0.2f);
        EJObjectPoolMgr.instance.ReturnGausCannonMuzzleImpactQueue(gausCannonMuzzleImpact);
        yield return null;
    }
        
}
