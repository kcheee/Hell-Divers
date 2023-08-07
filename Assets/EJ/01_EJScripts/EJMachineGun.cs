using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJMachineGun : MonoBehaviour
{
    //machinGunFire변수
    bool isMachineDone = true;

    //machineGun Pos 변수
    public Transform machineGunPos;
    Vector3 originMachineAngle;
    public GameObject machineImpactFactory;

    //궤적 Trail 변수
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

        //machineGun 변수
        int machineCount = 8;
        int machineGunRotateZadd = -5;

        isMachineDone = false;

        for (int i = 0; i < machineCount; i++)
        {
            if (Physics.Raycast(machineGunPos.position, machineGunPos.up, out machineGunHitInfo, float.MaxValue))
            {
                GameObject machineGunImpact = Instantiate(machineImpactFactory);

                machineGunImpact.transform.position = machineGunHitInfo.point;
                machineGunImpact.transform.forward = machineGunHitInfo.normal;
                machineGunImpact.transform.parent = machineGunHitInfo.transform;

                Destroy(machineGunImpact, 1);
            }           

            machineGunPos.Rotate(new Vector3(0, 0, machineGunRotateZadd), Space.Self);
            yield return new WaitForSeconds(0.1f);
        }

        machineGunPos.localEulerAngles = originMachineAngle;
        isMachineDone = true;      
    }
}
