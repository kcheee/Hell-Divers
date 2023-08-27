using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

//RPC로 쓴다면 원래 상태로 돌려두면 된다.

public class EJGausCannonFireInstantiate : MonoBehaviourPun
{
    #region cannonFire변수
    //cannonFire 변수
    int cannonCount = 16;
    bool isCannonDone = true;
    float cannonDelayTime = 0.05f; //0.02f

    //cannon Position 변수
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
        //test용
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            if (isCannonDone)
            {
                StartCoroutine(CannonFire(null));

                //!!!!RPC함수는 photonView를 통해서 호출해야하는데 startcoroutine은 어떻게 적용하는 거지? 
                //photonview.RPC(nameof(StartCannonFirebyRPC), RpcTarget.All);
                //StartCoroutine(photonview.RPC("CannonFirebyRPC"), RpcTarget.All);
            }
        }
    }


    #region 원본) CannonFire 코루틴 함수

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
            gausCannonMuzzleImpact.transform.localScale = Vector3.one * 2;
            gausCannonMuzzleImpact.transform.position = cannonPos.transform.position;
            gausCannonMuzzleImpact.transform.up = cannonPos.transform.up;
            gausCannonMuzzleImpact.transform.localEulerAngles = cannonPos.transform.parent.localEulerAngles;

            ONRightArmAnim();
            ONBodyAnim();

            //SFX
            EJBossSFX.instance.PlaygausCannonSFX();

            //gausCannon불빛이 나간다. 
            //GameObject gausCannonPrefab = PhotonNetwork.Instantiate("flarebullet_squadLeader_EJ", cannonPos.transform.position, Quaternion.identity);
            GameObject gausCannonPrefab = Instantiate(gausCannonPrefabFactory);
            gausCannonPrefab.transform.position = cannonPos.transform.position;
            gausCannonPrefab.transform.up = cannonPos.transform.forward;

            //몸이랑 같이 돌아가고 싶다.
            Vector3 originAngle = transform.localEulerAngles;    

            //cannonPos.Rotate(new Vector3(cannonPosX, 0, 7 * cannonPosZDir)+originAngle, Space.Self);
            cannonPos.Rotate(new Vector3(cannonPosX, 7 * cannonPosZDir, 0 ), Space.Self);
           
            yield return new WaitForSeconds(cannonDelayTime);
            //OFFRightArmAnim();
        }

        //CannonPos 초기화
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

    #region CannonFire 원본 함수를 RPC로 만들 필요는 없지 않나
    [PunRPC]
    public IEnumerator CannonFirebyRPC(System.Action<int> complete)
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

            ONRightArmAnim();
            ONBodyAnim();

            //SFX
            EJBossSFX.instance.PlaygausCannonSFX();

            //gausCannon불빛이 나간다. 
            GameObject gausCannonPrefab = Instantiate(gausCannonPrefabFactory);
            gausCannonPrefab.transform.position = cannonPos.transform.position;
            gausCannonPrefab.transform.up = cannonPos.transform.up;

            //몸이랑 같이 돌아가고 싶다.
            Vector3 originAngle = transform.localEulerAngles;

            cannonPos.Rotate(new Vector3(cannonPosX, 0, 5 * cannonPosZDir) + originAngle, Space.Self);

            yield return new WaitForSeconds(cannonDelayTime);
            //OFFRightArmAnim();
        }

        //CannonPos 초기화
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
