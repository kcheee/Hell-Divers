using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJGausCannonFire : MonoBehaviour
{
    //cannonFire 변수
    int cannonCount = 25;
    bool isCannonDone = true;
    float cannonDelayTime = 0.02f; //0.02f

    //cannon Position 변수
    public Transform cannonPos;
    Vector3 originCannonAngle;
    //public GameObject cannonImpactFactory;
    GameObject cannonImpact;
    public GameObject gausCannonMuzzleFactory;

    //궤적line 변수
    LineRenderer cannonLine;

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

    public IEnumerator CannonFire()
    {
        RaycastHit cannonHitInfo;

        //cannonFire Angle 변수
        int cannonPosZDir = -1;
        float cannonPosX = 0;
        float cannonPosXadd = 0.1f;

        //이미 조건 안에 들어왔으므로
        //isCannonDone = True 전 코드 실행 전까지 다시 들어오지 못하게
        isCannonDone = false;

        for (int i = 0; i < cannonCount; i++)
        {
            //cannonPosition Angle조정
            if (i % 4 == 0)
            {
                cannonPosX += cannonPosXadd;
                cannonPosZDir *= -1;
            }

            //lineRenderer 시작점
            cannonLine.SetPosition(0, cannonPos.position);

            //cannonMuzzleImpact 생성
            GameObject gausCannonMuzzleImpact = Instantiate(gausCannonMuzzleFactory);
            gausCannonMuzzleImpact.transform.localScale = Vector3.one *0.5f;
            gausCannonMuzzleImpact.transform.position = cannonPos.transform.position;
            gausCannonMuzzleImpact.transform.up = cannonPos.transform.up;

            //floor에 생기는 하얀 연기FX
            if (Physics.Raycast(cannonPos.position, cannonPos.up, out cannonHitInfo, float.MaxValue))
            {
                Debug.DrawRay(cannonPos.position, cannonPos.up);

                cannonImpact = EJObjectPoolMgr.instance.GetGausCannonImpactQueue();
                
                //02. 맞은 곳에 Effect
                cannonImpact.transform.position = cannonHitInfo.point;
                cannonImpact.transform.localScale = Vector3.one * 3;
                cannonImpact.transform.up = cannonHitInfo.normal;
                //cannonImpact.transform.parent = cannonHitInfo.transform;

                //LineRenderer 도착점
                cannonLine.SetPosition(1, cannonHitInfo.point);
                cannonLine.enabled = true;

                EJObjectPoolMgr.instance.ReturnGausCannonImpactQueue(cannonImpact);
                
            }

            cannonPos.Rotate(new Vector3(cannonPosX, 0, 5 * cannonPosZDir), Space.Self);

            print("gausCannon이 맞은 곳은" + cannonHitInfo.point);
            
            yield return new WaitForSeconds(cannonDelayTime);
        }

        //CannonPos 초기화
        cannonPos.transform.localEulerAngles = originCannonAngle;
        cannonLine.enabled = false;
        isCannonDone = true;

        BossFSM.XLflag = false;
    }       
}
