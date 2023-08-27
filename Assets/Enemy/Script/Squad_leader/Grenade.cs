using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;
public class Grenade : MonoBehaviourPun
{

    public GameObject smoke;
    public GameObject bombEft;
    //public GameObject BombParticle2;
    ParticleSystem p;

    private void Start()
    {
        // 메인캠에 달려있는 오브젝트 가져옴
        //cam.transform.GetComponent<DOTweenAnimation>().DOPlay();
        p = smoke.GetComponent<ParticleSystem>();
    }


    [PunRPC] 
    void pun_delay()
    {
        StartCoroutine(delay());
    }


    IEnumerator delay()
    {
        yield return new WaitForSeconds(1);
            //smoke.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        Camera.main.transform.DOShakePosition(0.3f, 0.5f);
        // 데미지 함수 넣어야 함.
        Collider[] cols = Physics.OverlapSphere(transform.position, 5);

        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].CompareTag("Player"))
            {
                cols[i].GetComponent<PhotonView>().RPC("damaged", RpcTarget.All,
                new Vector3(cols[i].transform.position.x, cols[i].transform.position.y+1, cols[i].transform.position.z), 2);
            }
            if (cols[i].CompareTag("Enemy"))
            {
                cols[i].GetComponent<PhotonView>().RPC("damaged", RpcTarget.All,
                new Vector3(cols[i].transform.position.x, cols[i].transform.position.y + 1, cols[i].transform.position.z), 2);
            }

        }
        Instantiate(bombEft,transform.position, Quaternion.identity);

        Destroy(gameObject,0.25f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Floor")
        {
            StartCoroutine(delay());
        }
    }
}
