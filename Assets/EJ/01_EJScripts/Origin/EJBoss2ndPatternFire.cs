using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EJBoss2ndPatternFire: MonoBehaviourPun
{
    #region rocket 변수
    //rocket변수
    bool isRocketDone = true;
    int RocketCount = 6;

    //rocketPos
    public Transform RocketPos;
    float rotX;
    float rotZ;
    Vector3 RocketOriginAngle;

    GameObject rocketImpact;
    public GameObject rocketFactory;
    public GameObject rocketExploImpactFactory;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //원래 회전값 담아둘 변수
        RocketOriginAngle = RocketPos.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            if (isRocketDone)
            {
                StartCoroutine(MakeRocket(null));
            }
        }
    }

    #region 01. MakeRocket Coroutine 함수
    public IEnumerator MakeRocket(System.Action<int> complete)
    {
        isRocketDone = false;

        for (int i = 0; i < RocketCount; i++)
        {
            GameObject rocket = Instantiate(rocketFactory);
            rocket.transform.position = RocketPos.position;
            rocket.transform.forward = RocketPos.transform.up;

            ONLeftArmAnim();
            bodyReactionAnim();

            EJBossSFX.instance.PlaymachineGunSFX();
            
            //X는 윗방향 +가 윗방향, -가 아래 방향
            //Z는 좌우방향 +가 보스 기준 오른쪽, -가 보스 기준 왼쪽
            
            RocketPos.Rotate(new Vector3(rotX, 0, rotZ), Space.Self);
            rotZ -= 3;
            rotX += 1;

            //print("RocketPos의 로컬 앵글은" + RocketPos.transform.rotation);

            yield return new WaitForSeconds(0.7f);

        }

        RocketPos.localEulerAngles = RocketOriginAngle;
        rotX = 0;
        rotZ = 0;
       
        //print("로켓포스 앞방향이 원래 방향으로 돌아왔습니다");

        isRocketDone = true;

        if (complete != null)
        {
            complete(1);
        }      
        yield return null;
    }
    #endregion

    #region Anim함수들
    //Anim
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

    #endregion

    #region 02. 바닥충돌FX함수

    //매개변수(3)
    //pos: rocket 스크립트에서 Trigger 충돌 위치
    //normal: rocket 스크립트에서 Trigger 충돌의 normal방향
    //waitTime: 일정 시간 후 사라질 타이밍

    [PunRPC]
    public void ShowRocketExploImpact(Vector3 pos, /*Vector3 normal,*/ float waitTime)
    {
        print("로켓이 바닥충돌 효과가 발생했습니다");

        GameObject rocketExploImpact = Instantiate(rocketExploImpactFactory);

        rocketExploImpact.transform.position = pos;
        //rocketExploImpact.transform.localScale = Vector3.one * 3;
        //rocketExploImpact.transform.forward = normal;

        StartCoroutine(wait(rocketExploImpact, waitTime));
    }

    //매개변수(2)
    //rocket: 사라지게 할 오브젝트
    //waitTime: 기다릴 시간

    IEnumerator wait(GameObject rocket, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }
    #endregion
}
