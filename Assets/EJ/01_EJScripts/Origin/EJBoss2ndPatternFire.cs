using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EJBoss2ndPatternFire: MonoBehaviourPun
{
    //rocket����
    bool isRocketDone = true;
    int RocketCount = 6;

    //rocketPos
    public Transform RocketPos;
    float rotX;
    float rotZ;
    Vector3 RocketOriginAngle;

    GameObject rocketImpact;
    public GameObject rocketFactory;

    // Start is called before the first frame update
    void Start()
    {
        //���� ȸ���� ��Ƶ� ����
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

    public IEnumerator MakeRocket(System.Action<int> complete)
    {
        isRocketDone = false;

        for (int i = 0; i < RocketCount; i++)
        {
            GameObject rocket = Instantiate(rocketFactory);
            rocket.transform.position = RocketPos.position;
            //rocket.transform.localEulerAngles = RocketPos.transform.localEulerAngles;
            rocket.transform.forward = RocketPos.transform.up;

            ONLeftArmAnim();
            bodyReactionAnim();

            EJBossSFX.instance.PlaymachineGunSFX();

            //rotX = Random.Range(-10, 10);
            //rotZ = Random.Range(-10, 10);
            
            //X�� ������ +�� ������, -�� �Ʒ� ����
            //Z�� �¿���� +�� ���� ���� ������, -�� ���� ���� ����
            

            RocketPos.Rotate(new Vector3(rotX, 0, rotZ), Space.Self);
            rotZ -= 3;
            rotX += 1;

            //print("RocketPos�� ���� �ޱ���" + RocketPos.transform.rotation);

            yield return new WaitForSeconds(0.7f);

        }
        RocketPos.localEulerAngles = RocketOriginAngle;
        rotX = 0;
        rotZ = 0;

        
        print("�������� �չ����� ���� �������� ���ƿԽ��ϴ�");

        isRocketDone = true;


        if (complete != null)
        {
            complete(1);
        }      
        yield return null;
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
