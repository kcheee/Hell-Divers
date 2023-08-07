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

            //lineRenderer1
            cannonLine.SetPosition(0, cannonPos.position);

            if (Physics.Raycast(cannonPos.position, cannonPos.up, out cannonHitInfo, float.MaxValue))
            {
                GameObject cannonImpact = Instantiate(cannonImpactFactory);

                cannonImpact.transform.position = cannonHitInfo.point;
                cannonImpact.transform.forward = cannonHitInfo.normal;
                cannonImpact.transform.parent = cannonHitInfo.transform;

                cannonLine.SetPosition(1, cannonHitInfo.point);
                cannonLine.enabled = true;
            }

            //line�� �̾����� ����
            cannonPos.Rotate(new Vector3(cannonPosX, 0, 5 * cannonPosZDir), Space.Self);

            yield return new WaitForSeconds(0.02f);
        }

        cannonPos.transform.localEulerAngles = originCannonAngle;
        cannonLine.enabled = false;
        isCannonDone = true;
    }
}
