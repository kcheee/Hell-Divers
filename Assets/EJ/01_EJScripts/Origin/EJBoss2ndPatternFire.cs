using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EJBoss2ndPatternFire: MonoBehaviourPun
{
    //rocketº¯¼ö
    bool isRocketDone = true;
    int RocketCount = 6;

    //rocketPos
    public Transform RocketPos;

    GameObject rocketImpact;
    public GameObject rocketFactory;

    // Start is called before the first frame update
    void Start()
    {

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
            rocket.transform.localEulerAngles = RocketPos.transform.localEulerAngles;
            rocket.transform.forward = RocketPos.transform.up;

            ONLeftArmAnim();
            bodyReactionAnim();

            EJBossSFX.instance.PlaymachineGunSFX();

            float rotY = Random.Range(-20, 20);
            float rotX = Random.Range(-105, -100);

            RocketPos.Rotate(new Vector3(rotX, rotY, 0), Space.Self);
            yield return new WaitForSeconds(0.7f);

        }
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
