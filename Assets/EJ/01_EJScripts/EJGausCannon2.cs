using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJGausCannon2 : MonoBehaviour
{
    //fire
    int fireCount = 15;
    bool isCannonDone = true;

    //fireTransform
    public Transform cannonPos;
    Vector3 originAngle;
    public GameObject cannonImpactFactory;

    //궤적 변수
    public LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        originAngle = cannonPos.localEulerAngles;
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
        RaycastHit hitInfo;

        //fire변수
        int cannonPosZDir = -1;
        float cannonPosX = 0;
        float cannonPosXadd = 0.1f;

        //이미 조건 안에 들어왔으므로, 또 눌렀을 때 다시 들어오지 못하게 하기 위해서
        isCannonDone = false;

        for (int i = 0; i < fireCount; i++)
        {
            if (i % 4 == 0)
            {
                cannonPosX += cannonPosXadd;
                cannonPosZDir *= -1;
            }

            if (Physics.Raycast(cannonPos.position, cannonPos.up, out hitInfo, float.MaxValue))
            {
                GameObject cannonImpact = Instantiate(cannonImpactFactory);

                cannonImpact.transform.position = hitInfo.point;
                cannonImpact.transform.forward = hitInfo.normal;
                cannonImpact.transform.parent = hitInfo.transform;
            }

            cannonPos.Rotate(new Vector3(cannonPosX, 0, 5 * cannonPosZDir), Space.Self);
            yield return new WaitForSeconds(0.1f);
        }
        isCannonDone = true;
        cannonPos.transform.localEulerAngles = originAngle;
    }
}
