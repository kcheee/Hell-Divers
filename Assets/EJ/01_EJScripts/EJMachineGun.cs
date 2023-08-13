using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJMachineGun : MonoBehaviour
{
    //machinGunFire변수
    bool isMachineDone = true;
    float machineGunDelayTime = 0.1f;

    //machineGun Pos 변수
    public Transform machineGunPos;
    Vector3 originMachineAngle;

    //machineGun Effect
    //public GameObject machineImpactFactory;
    GameObject machineGunImpact;
    public GameObject machineGunFactory;

    // Start is called before the first frame update
    void Start()
    {
        originMachineAngle = machineGunPos.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (isMachineDone)
            {
                StartCoroutine(MachineGunFire());
            }
        }
    }
   
    public IEnumerator MachineGunFire()
    {
        RaycastHit machineGunHitInfo;

        //machineGun 변수
        int machineCount = 8;
        int machineGunRotateZadd = -5;

        isMachineDone = false;

        for (int i = 0; i < machineCount; i++)
        {
            if (Physics.Raycast(machineGunPos.position, machineGunPos.up, out machineGunHitInfo, float.MaxValue))
            {                             
                Debug.DrawRay(machineGunPos.position, machineGunPos.up, Color.red);

                //01.machineGunEffect Enqueue
                //machineGunImpact = EJObjectPoolMgr.instance.GetmachineGunImpactQueue();
                GameObject machineGunImpact = Instantiate(machineGunFactory);

                //02.machineGunEffect position
                machineGunImpact.transform.position = machineGunHitInfo.point;
                machineGunImpact.transform.forward = machineGunHitInfo.normal;
                machineGunImpact.transform.localScale = Vector3.one * 3;
                //machineGunImpact.transform.parent = machineGunHitInfo.transform;

                print("머신건이 닿은 곳은 "+ machineGunHitInfo.point);

                //03.일정 시간 후 Dequeue해줄 필요 없이 particle destroy
                /*  if (machineGunImpact.activeSelf)
                {
                    yield return new WaitForSeconds(3f);
                    EJObjectPoolMgr.instance.ReturnmachineGunImpactQueue(machineGunImpact);
                }*/
            }           

            //04. machineGunPos가 일정량만큼 Z축 회전
            machineGunPos.Rotate(new Vector3(0, 0, machineGunRotateZadd), Space.Self);
            
            yield return new WaitForSeconds(machineGunDelayTime);
        }

        //04. machineGunPos Angle 초기화
        machineGunPos.localEulerAngles = originMachineAngle;
        isMachineDone = true;      
    }
}
