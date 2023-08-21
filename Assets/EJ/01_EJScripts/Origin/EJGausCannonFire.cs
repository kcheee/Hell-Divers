using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJGausCannonFire : MonoBehaviour
{
    //cannonFire ����
    int cannonCount = 25;
    bool isCannonDone = true;
    float cannonDelayTime = 0.02f; //0.02f

    //cannon Position ����
    public Transform cannonPos;
    Vector3 originCannonAngle;
    //public GameObject cannonImpactFactory;
    GameObject cannonImpact;
    public GameObject gausCannonMuzzleFactory;

    //����line ����
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
                StartCoroutine(CannonFire(null));
            }
        }
    }

    public IEnumerator CannonFire(System.Action<int> complete)
    {
        RaycastHit cannonHitInfo;

        //cannonFire Angle ����
        int cannonPosZDir = -1;
        float cannonPosX = 0;
        float cannonPosXadd = 0.1f;

        //�̹� ���� �ȿ� �������Ƿ�
        //isCannonDone = True �� �ڵ� ���� ������ �ٽ� ������ ���ϰ�
        isCannonDone = false;

        for (int i = 0; i < cannonCount; i++)
        {
            //cannonPosition Angle����
            if (i % 4 == 0)
            {
                cannonPosX += cannonPosXadd;
                cannonPosZDir *= -1;
            }

            //lineRenderer ������
            cannonLine.SetPosition(0, cannonPos.position);

            //cannonMuzzleImpact ����
            GameObject gausCannonMuzzleImpact = Instantiate(gausCannonMuzzleFactory);
            gausCannonMuzzleImpact.transform.localScale = Vector3.one *0.5f;
            gausCannonMuzzleImpact.transform.position = cannonPos.transform.position;
            gausCannonMuzzleImpact.transform.up = cannonPos.transform.up;

            //floor�� ����� �Ͼ� ����FX
            if (Physics.Raycast(cannonPos.position, cannonPos.up, out cannonHitInfo, float.MaxValue))
            {
                Debug.DrawRay(cannonPos.position, cannonPos.up);

                cannonImpact = EJObjectPoolMgr.instance.GetGausCannonImpactQueue();
                
                //02. ���� ���� Effect
                cannonImpact.transform.position = cannonHitInfo.point;
                cannonImpact.transform.localScale = Vector3.one * 3;
                cannonImpact.transform.up = cannonHitInfo.normal;
                //cannonImpact.transform.parent = cannonHitInfo.transform;

                //LineRenderer ������
                cannonLine.SetPosition(1, cannonHitInfo.point);
                cannonLine.enabled = true;

                EJObjectPoolMgr.instance.ReturnGausCannonImpactQueue(cannonImpact);
                
            }

            cannonPos.Rotate(new Vector3(cannonPosX, 0, 5 * cannonPosZDir), Space.Self);

            print("gausCannon�� ���� ����" + cannonHitInfo.point);
            
            yield return new WaitForSeconds(cannonDelayTime);
        }

        //CannonPos �ʱ�ȭ
        cannonPos.transform.localEulerAngles = originCannonAngle;
        cannonLine.enabled = false;
        isCannonDone = true;

        if (complete != null)
        {
            complete(2);
        }
    }       
}