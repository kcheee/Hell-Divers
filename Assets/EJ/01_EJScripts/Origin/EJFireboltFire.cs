using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EJFireboltFire : MonoBehaviourPun
{
    //rocket����
    bool isFireDone = true;
    int FireBoltCount = 6;

    //rocketPos
    public Transform FirePos;
    float rotX;
    float rotZ;
    Vector3 FireOriginAngle;

    GameObject fireBoltImpact;
    public GameObject fireBoltFactory;
    public GameObject fireBoltExploImpactFactory;

    // Start is called before the first frame update
    void Start()
    {
        FireOriginAngle = FirePos.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            if (isFireDone)
            {
                StartCoroutine(MakeFireBolt(null));
                //MakeFireBoltTest();
            }
        }
    }

    //����� �ϴµ� �Ⱥ��̳�..
    void MakeFireBoltTest()
    {
        GameObject firebolt = Instantiate(fireBoltExploImpactFactory);
        firebolt.transform.position = FirePos.transform.position;
    }

    #region MakeFireBolt �ڷ�ƾ �Լ�
    public IEnumerator MakeFireBolt(System.Action<int> complete)
    {
        isFireDone = false;

        for (int i = 0; i < FireBoltCount; i++)
        {
            GameObject fireBolt = Instantiate(fireBoltFactory);
            fireBolt.transform.position = FirePos.position;
            fireBolt.transform.forward = FirePos.transform.forward;

            ONLeftArmAnim();
            bodyReactionAnim();

            EJBossSFX.instance.PlaymachineGunSFX();

            //X�� ������ +�� ������, -�� �Ʒ� ����
            //Z�� �¿���� +�� ���� ���� ������, -�� ���� ���� ����

            FirePos.Rotate(new Vector3(rotX, 0, rotZ), Space.Self);
            rotZ -= 3;
            rotX += 1;

            //print("RocketPos�� ���� �ޱ���" + RocketPos.transform.rotation);

            yield return new WaitForSeconds(0.7f);

        }

        FirePos.localEulerAngles = FireOriginAngle;
        rotX = 0;
        rotZ = 0;

        //print("�������� �չ����� ���� �������� ���ƿԽ��ϴ�");

        isFireDone = true;

        if (complete != null)
        {
            complete(1);
        }
        yield return null;
    }
#endregion

    #region Anim�Լ���
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
}
