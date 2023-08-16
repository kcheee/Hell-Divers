using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJGausCannonFireInstantiate : MonoBehaviour
{
    //cannonFire 변수
    int cannonCount = 25;
    bool isCannonDone = true;
    float cannonDelayTime = 0.05f; //0.02f

    //cannon Position 변수
    public Transform cannonPos;
    Vector3 originCannonAngle;
    //public GameObject cannonImpactFactory;
    GameObject cannonImpact;
    public GameObject gausCannonMuzzleFactory;
    public GameObject gausCannonPrefabFactory;


    // Start is called before the first frame update
    void Start()
    {
        originCannonAngle = cannonPos.localEulerAngles;
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

            //cannonMuzzleImpact 생성
            GameObject gausCannonMuzzleImpact = Instantiate(gausCannonMuzzleFactory);
            gausCannonMuzzleImpact.transform.localScale = Vector3.one * 5;
            gausCannonMuzzleImpact.transform.position = cannonPos.transform.position;
            gausCannonMuzzleImpact.transform.up = cannonPos.transform.up;
            gausCannonMuzzleImpact.transform.localEulerAngles = cannonPos.transform.parent.localEulerAngles;

            //gausCannon불빛이 나간다. 
            GameObject gausCannonPrefab = Instantiate(gausCannonPrefabFactory);
            gausCannonPrefab.transform.position = cannonPos.transform.position;
            gausCannonPrefab.transform.up = cannonPos.transform.up;

            //몸이랑 같이 돌아가고 싶다.
            Vector3 originAngle = transform.localEulerAngles;    

            cannonPos.Rotate(new Vector3(cannonPosX, 0, 5 * cannonPosZDir)+originAngle, Space.Self);

            yield return new WaitForSeconds(cannonDelayTime);
        }

        //CannonPos 초기화
        cannonPos.transform.localEulerAngles = originCannonAngle;
        isCannonDone = true;

        if (complete != null)
        {
            complete(2);
        }
    }
}
