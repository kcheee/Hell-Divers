using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJGausCannon : MonoBehaviour
{
    //cannonFire ����
    int cannonCount = 25;
    bool isCannonDone = true;

    //cannon Position ����
    public Transform cannonPos;
    Vector3 originCannonAngle;
    public GameObject cannonImpactFactory;
    GameObject cannonImpact;

    //����line ����
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

    //1. fire coroutine�ȿ��� muzzle, hitpoint �ΰ��� ����Ʈ�� �Ѱ� ���� �ʹ�.
    //2. ������ ��, ���� �ð��� ������ ���� �ʹ�. 

    //1-1. �ڷ�ƾ �ȿ� �� �ڷ�ƾ�� ���?
    //1-2. �� ��ƾ���� �Ѵٰ� �ϸ� yield return ��� ���� ������? �𸣰���.
    //�ڷ�ƾ�� ����ǰ� �ְ�, ����Ʈ���� ������� ��ũ��Ʈ�� �־ �ٿ��ָ� �ȴ�.

    IEnumerator CannonFire()
    {
        RaycastHit cannonHitInfo;

        //cannonFire ����
        int cannonPosZDir = -1;
        float cannonPosX = 0;
        float cannonPosXadd = 0.1f;

        //�̹� ���� �ȿ� �������Ƿ�, �� ������ �� �ٽ� ������ ���ϰ� �ϱ� ���ؼ�
        isCannonDone = false;

        for (int i = 0; i < cannonCount; i++)
        {
            //cannonPosition Angle����
            if (i % 4 == 0)
            {
                cannonPosX += cannonPosXadd;
                cannonPosZDir *= -1;
            }

            //lineRenderer
            cannonLine.SetPosition(0, cannonPos.position);

            GameObject gausCannonMuzzleImpact = EJObjectPoolMgr.instance.GetGausCannonMuzzleImpactQueue();
            //01.�ѱ��� Effect?????????????????????????????????
            //!!StartCoroutine(GausCannonMuzzleImpact());

            if (Physics.Raycast(cannonPos.position, cannonPos.up, out cannonHitInfo, float.MaxValue))
            {
                cannonImpact = EJObjectPoolMgr.instance.GetGausCannonImpactQueue();
                
                //02. ���� ���� Effect
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

        //02.For���� �� ���� ���� ��� �ѹ��� ��������??????????????????????????????????
        //!!EJObjectPoolMgr.instance.ReturnGausCannonImpactQueue(cannonImpact);

        //CannonPos �ʱ�ȭ
        cannonPos.transform.localEulerAngles = originCannonAngle;
        cannonLine.enabled = false;
        isCannonDone = true;
    }

    //01.�ѱ��� Effect
    //muzzle�� Ű�� ���� �ڷ�ƾ�� �� ����?????????
    IEnumerator GausCannonMuzzleImpact()
    {
        GameObject gausCannonMuzzleImpact = EJObjectPoolMgr.instance.GetGausCannonMuzzleImpactQueue();
        yield return new WaitForSeconds(0.2f);
        EJObjectPoolMgr.instance.ReturnGausCannonMuzzleImpactQueue(gausCannonMuzzleImpact);
        yield return null;
    }
        
}
