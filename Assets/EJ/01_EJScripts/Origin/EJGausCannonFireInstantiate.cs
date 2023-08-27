using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

//RPC�� ���ٸ� ���� ���·� �����θ� �ȴ�.

public class EJGausCannonFireInstantiate : MonoBehaviourPun
{
    #region cannonFire����
    //cannonFire ����
    int cannonCount = 16;
    bool isCannonDone = true;
    float cannonDelayTime = 0.05f; //0.02f

    //cannon Position ����
    public Transform cannonPos;
    Vector3 originCannonAngle;
    //public GameObject cannonImpactFactory;
    GameObject cannonImpact;
    public GameObject gausCannonMuzzleFactory;
    public GameObject gausCannonPrefabFactory;

    public GameObject floorEffectFactory;

    //PhotonView
    PhotonView photonview;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        originCannonAngle = cannonPos.localEulerAngles;
        photonview = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        //test��
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            if (isCannonDone)
            {
                StartCoroutine(CannonFire(null));

                //!!!!RPC�Լ��� photonView�� ���ؼ� ȣ���ؾ��ϴµ� startcoroutine�� ��� �����ϴ� ����? 
                //photonview.RPC(nameof(StartCannonFirebyRPC), RpcTarget.All);
                //StartCoroutine(photonview.RPC("CannonFirebyRPC"), RpcTarget.All);
            }
        }
    }


    #region ����) CannonFire �ڷ�ƾ �Լ�

    public IEnumerator CannonFire(System.Action<int> complete)
    {
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

            //cannonMuzzleImpact ����
            GameObject gausCannonMuzzleImpact = Instantiate(gausCannonMuzzleFactory);
            gausCannonMuzzleImpact.transform.localScale = Vector3.one * 2;
            gausCannonMuzzleImpact.transform.position = cannonPos.transform.position;
            gausCannonMuzzleImpact.transform.up = cannonPos.transform.up;
            gausCannonMuzzleImpact.transform.localEulerAngles = cannonPos.transform.parent.localEulerAngles;

            ONRightArmAnim();
            ONBodyAnim();

            //SFX
            EJBossSFX.instance.PlaygausCannonSFX();

            //gausCannon�Һ��� ������. 
            GameObject gausCannonPrefab = Instantiate(gausCannonPrefabFactory);
            gausCannonPrefab.transform.position = cannonPos.transform.position;
            gausCannonPrefab.transform.up = cannonPos.transform.forward;

            //���̶� ���� ���ư��� �ʹ�.
            Vector3 originAngle = transform.localEulerAngles;    

            //cannonPos.Rotate(new Vector3(cannonPosX, 0, 7 * cannonPosZDir)+originAngle, Space.Self);
            cannonPos.Rotate(new Vector3(cannonPosX, 7 * cannonPosZDir, 0 ), Space.Self);
           
            yield return new WaitForSeconds(cannonDelayTime);
            //OFFRightArmAnim();
        }

        //CannonPos �ʱ�ȭ
        cannonPos.transform.localEulerAngles = originCannonAngle;
        isCannonDone = true;

        if (complete != null)
        {
            complete(2);
        }
    }
    #endregion


    [PunRPC]
    void StartCannonFirebyRPC()
    {
        StartCoroutine(CannonFire(null));
    }

    #region CannonFire ���� �Լ��� RPC�� ���� �ʿ�� ���� �ʳ�
    [PunRPC]
    public IEnumerator CannonFirebyRPC(System.Action<int> complete)
    {
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

            //cannonMuzzleImpact ����
            GameObject gausCannonMuzzleImpact = Instantiate(gausCannonMuzzleFactory);
            gausCannonMuzzleImpact.transform.localScale = Vector3.one * 5;
            gausCannonMuzzleImpact.transform.position = cannonPos.transform.position;
            gausCannonMuzzleImpact.transform.up = cannonPos.transform.up;
            gausCannonMuzzleImpact.transform.localEulerAngles = cannonPos.transform.parent.localEulerAngles;

            ONRightArmAnim();
            ONBodyAnim();

            //SFX
            EJBossSFX.instance.PlaygausCannonSFX();

            //gausCannon�Һ��� ������. 
            GameObject gausCannonPrefab = Instantiate(gausCannonPrefabFactory);
            gausCannonPrefab.transform.position = cannonPos.transform.position;
            gausCannonPrefab.transform.up = cannonPos.transform.up;

            //���̶� ���� ���ư��� �ʹ�.
            Vector3 originAngle = transform.localEulerAngles;

            cannonPos.Rotate(new Vector3(cannonPosX, 0, 5 * cannonPosZDir) + originAngle, Space.Self);

            yield return new WaitForSeconds(cannonDelayTime);
            //OFFRightArmAnim();
        }

        //CannonPos �ʱ�ȭ
        cannonPos.transform.localEulerAngles = originCannonAngle;
        isCannonDone = true;

        if (complete != null)
        {
            complete(2);
        }
    }
    #endregion

    #region Anim
    public Animator rightArmReaction;
    public Animator bodyReaction;

    public void ONRightArmAnim()
    {
        rightArmReaction.SetTrigger("Fire");
            //GameObject.FindWithTag("rigthArm").GetComponentInChildren<Animator>().enabled = true;
    }

    public void ONBodyAnim()
    {
        bodyReaction.SetTrigger("HeadReaction");
    }
    #endregion

}
