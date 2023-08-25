using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EJMachineGun : MonoBehaviourPun
{
    //machinGunFire����
    bool isMachineDone = true;
    float machineGunDelayTime = 0.1f;
    public int machineGunDamage = 3;

    //machineGun Pos ����
    public Transform machineGunPos;
    Vector3 originMachineAngle;

    //machineGun Effect
    //public GameObject machineImpactFactory;
    GameObject machineGunImpact;
    public GameObject machineGunFactory;

    //bool isLeftArmAnimDone = false;

    //Layer
    int PlayerLayer;
    int DefaultLayer;

    // Start is called before the first frame update
    void Start()
    {
        originMachineAngle = machineGunPos.localEulerAngles;

        PlayerLayer = LayerMask.GetMask("Player");
        DefaultLayer = LayerMask.GetMask("Default");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (isMachineDone)
            {
                StartCoroutine(MachineGunFire(null));
            }
        }


        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
        }
    }
   

    public IEnumerator MachineGunFire(System.Action<int> complete)
    {
        RaycastHit machineGunHitInfo;

        //machineGun ����
        int machineCount = 8;
        int machineGunRotateZadd = -10;

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
                machineGunImpact.transform.localScale = Vector3.one * 0.7f;
                //machineGunImpact.transform.parent = machineGunHitInfo.transform;

                //print("�ӽŰ��� ���� ���� " + machineGunHitInfo.point);

                //03.���� �ð� �� Dequeue���� �ʿ� ���� particle destroy
                /*  if (machineGunImpact.activeSelf)
                {
                    yield return new WaitForSeconds(3f);
                    EJObjectPoolMgr.instance.ReturnmachineGunImpactQueue(machineGunImpact);
                }*/

                //leftArmPos�� �ݵ��� �ش�
                //Animator�� �״ٰ� ����.

                #region  HP �پ��� �� �׽�Ʈ �ʿ�
                //machineGun hitInfo�� player��� �������� �ش�.
                //if (machineGunHitInfo.transform.tag == "Player")
                //{
                //    machineGunHitInfo.transform.GetComponent<PhotonView>().RPC("damaged", RpcTarget.All, machineGunHitInfo, 3);

                //}

                Collider[] machineGunHits =
                Physics.OverlapSphere(machineGunHitInfo.transform.position, 1f, LayerMask.GetMask("Player"));

                foreach (Collider attackedPlayer in machineGunHits)
                {
                    print("attackedPlayer��" + attackedPlayer);

                    attackedPlayer.transform.GetComponent<PhotonView>().RPC("damaged", RpcTarget.All, attackedPlayer.transform.position+Vector3.up*1.6f, machineGunDamage);
                }
                #endregion

                ONLeftArmAnim();
                bodyReactionAnim();

                EJBossSFX.instance.PlaymachineGunSFX();
            }
         
            //04. machineGunPos�� ��������ŭ Z�� ȸ��
            machineGunPos.Rotate(new Vector3(0, 0, machineGunRotateZadd), Space.Self);

            yield return new WaitForSeconds(machineGunDelayTime);

        }
        //04. machineGunPos Angle �ʱ�ȭ
        machineGunPos.localEulerAngles = originMachineAngle;
        isMachineDone = true;

        if (complete != null)
        {
            complete(1);
        }
    }

    public Animator leftArmReaction;
    public Animator bodyReaction;

    public void ONLeftArmAnim()
    {
        leftArmReaction.SetTrigger("leftFire");
    }
    public void bodyReactionAnim()
    {
        leftArmReaction.SetTrigger("HeadReaction");
    }

}
