using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EJBoss2ndPatternRocket : MonoBehaviourPun
{
    #region rocket 변수
    float rocketSpeed;
    float rocketRadius = 10;

    Rigidbody rb;
    public GameObject rocketExploImpactFactory;

    public PhotonView tankPv;
    #endregion

    void Start()
    {
        //대충 20m정도 날아감
        //rocketSpeed = Random.Range(10, 20);
        rocketSpeed = Random.Range(10, 15); 

        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * rocketSpeed, ForceMode.Impulse);
    }

    void Update()
    {
        //??? 로켓헤드 방향
        transform.forward = rb.velocity.normalized;
    }

    #region 01. Trigger효과
    //terrain과 부딪히는 것은 trigger임
    private void OnTriggerEnter(Collider other)
    {
        print("rocket이 Trigger로 부딪힌 것은" + other);

        if (other.gameObject.tag == "Floor")
        {
            if (/*PhotonNetwork.IsMasterClient*/true)
            {
                //StartCoroutine(rocketExplodebyTrigger(/*other.transform.up*/));
                ShowRocketExploImpact(transform.position, 3);

                //동기화 안되면 이거
                photonView.RPC(nameof(ShowRocketExploImpact), RpcTarget.All, transform.position, 3);

            }

            Destroy(gameObject);
            EJBossSFX.instance.PlaybombExploSFX();
        }

        if (other.gameObject.tag == "Player")
        {
            PlayerDamage();
        }
    }

    #endregion

    #region 02. ShowRocketExploImpact

    [PunRPC]
    public void ShowRocketExploImpact(Vector3 pos, /*Vector3 normal,*/ float waitTime)
    {
        print("로켓이 바닥충돌 효과가 발생했습니다");

        GameObject rocketExploImpact = Instantiate(rocketExploImpactFactory);
        //GameObject rocketExploImpact = PhotonNetwork.Instantiate("bomb+DAX_Frost_Nova_t0_0 1", pos,Quaternion.identity);

        rocketExploImpact.transform.position = pos;

        StartCoroutine(wait(rocketExploImpact, waitTime));

    }

    IEnumerator wait(GameObject rocket, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }

    #endregion

    #region PlayerDamage함수

    void PlayerDamage()
    {
        //bomb반경 안의 player damage 
        RaycastHit[] bombHits = Physics.SphereCastAll(transform.position, rocketRadius, Vector3.up, 0f, LayerMask.GetMask("Player"));

        print("Rocket에 맞은 것은 " + bombHits[0].transform.gameObject.name);

        foreach (RaycastHit hitObj in bombHits)
        {
            hitObj.transform.GetComponent<PhotonView>().RPC("damaged", RpcTarget.All, hitObj.point, 3);
        }

    }
    #endregion

}
