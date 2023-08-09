using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJMachineGun : MonoBehaviour
{
    //machinGunFire����
    bool isMachineDone = true;

    //machineGun Pos ����
    public Transform machineGunPos;
    Vector3 originMachineAngle;
    public GameObject machineImpactFactory;

    //���� Trail ����
    //public TrailRenderer trailLine;

    // Start is called before the first frame update
    void Start()
    {
        originMachineAngle = machineGunPos.localEulerAngles;
        //trailLine = GetComponent<TrailRenderer>();
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

    IEnumerator MachineGunFire()
    {
        RaycastHit machineGunHitInfo;

        //machineGun ����
        int machineCount = 8;
        int machineGunRotateZadd = -5;

        isMachineDone = false;

        for (int i = 0; i < machineCount; i++)
        {
            if (Physics.Raycast(machineGunPos.position, machineGunPos.up, out machineGunHitInfo, float.MaxValue))
            {
                GameObject machineGunImpact = EJObjectPoolMgr.instance.GetGausCannonImpactQueue();

                //EJObjectPoolMgr.instance.GetGausCannonImpactQueue();
                machineGunImpact.transform.position = machineGunHitInfo.point;
                machineGunImpact.transform.forward = machineGunHitInfo.normal;
                //machineGunImpact.transform.parent = machineGunHitInfo.transform;


                //Destroy(machineGunImpact, 1);
            }           

            machineGunPos.Rotate(new Vector3(0, 0, machineGunRotateZadd), Space.Self);
            yield return new WaitForSeconds(0.1f);
        }

        //EJObjectPoolMgr.instance.InsertGausCannonImpactQueue(machineGunImpact);

        machineGunPos.localEulerAngles = originMachineAngle;
        isMachineDone = true;      
    }
}
