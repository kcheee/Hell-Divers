using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EJFlarebullet : MonoBehaviourPun
{
    #region flarebullet변수
    float flareBulletSpeed = 50f;
    float destroyTime = 1f;
    float bombRadius = 3;

    public GameObject floorEffectFactory;
    //public PhotonView tankPv;

    Rigidbody rb;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(DestroySelf());
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.up * flareBulletSpeed;
    }

    // Update is called once per frame
    void Update()
    {

    }


    #region 01. Trigger효과
    private void OnTriggerEnter(Collider other)
    {
        //print("FlareBullet이 trigger로 닿은 것은" + other.gameObject);

        if (other.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            print(">>>>>Floor에 닿았다");

            //RPC로 안보인다.
            ShowBulletImpact();
            //photonView.RPC(nameof(ShowBulletImpact), RpcTarget.All);
            //photonView.RPC(nameof(test), RpcTarget.All);
        }

        if (other.gameObject.tag == "Player")
        {
            //player 피격 pos 전달 필요
            //other.transform.GetComponent<PhotonView>().RPC("damaged", RpcTarget.All, transform.position + Vector3.down * 1.6f, 3);

            PlayerDamage();
        }
    }

    #endregion

    [PunRPC]
    void test()
    {
        Debug.Log("제발 부타");
    }

    #region 02. ShowBulletImpact함수
    [PunRPC]
   public void ShowBulletImpact()
    {
        print(">>>>>ShowBulletImpact함수 실행");

        //GameObject floorEffect = Instantiate(floorEffectFactory, transform.position, Quaternion.identity);
        GameObject floorEffect = PhotonNetwork.Instantiate("mDAX_Electricity_Ground_Explosion_00", transform.position, Quaternion.identity);

        Debug.Log("gausCannon이 바닥에 닿았을 때 생기는 효과는" + floorEffect);

        //floorEffect.transform.localScale = Vector3.one * 2;
        Destroy(gameObject, 2.5f);
    }


    #endregion

    #region 안쓰는 함수
    IEnumerator DestroySelf4Trigger(Collider other)
    {
        //tankPv.RPC("ShowGausCannonImpact", RpcTarget.All, transform.position,other.transform.up);

        //HP테스트 필요
        if (other.gameObject.tag == "Player")
        {
            other.transform.GetComponent<PhotonView>().RPC("damaged", RpcTarget.All, other.gameObject.transform.position + Vector3.up , 3);
        }      

        flareBulletSpeed = 0;
        yield return new WaitForSeconds(destroyTime);

        Destroy(gameObject);
    }
    #endregion


    #region 03. bombRadius에 들어온 PlayerDamage함수
    void PlayerDamage()
    {
        //bomb반경 안의 player damage 
        RaycastHit[] bombHits = Physics.SphereCastAll(transform.position, bombRadius, Vector3.up, 0f, LayerMask.GetMask("Player"));

        print("bomb에 맞은 것은 " + bombHits[0].transform.gameObject.name);

        foreach (RaycastHit hitObj in bombHits)
        {
            hitObj.transform.GetComponent<PhotonView>().RPC("damaged", RpcTarget.All, hitObj.point, 3);
        }

    }
    #endregion
}
