using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;

public class EJBossSpawn : MonoBehaviourPun
{
    float currTime;
    //float BossSpawnTIme = 0.1f;

    public GameObject bossSpawnFactory;
    public GameObject SpawnPoint;
    public GameObject bossTrigger;
    

    bool isBossDone = false;
    //int BossCount = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //currTime += Time.deltaTime;

        //if (Input.GetKeyDown(KeyCode.H))
            //SpawnBoss();

        //if (EJBossHP.instance.HP <= 0)
        //{
        //    InstantiateDeathFX();
        //}

    }
    

    void SpawnBoss()
    {
        Debug.Log("0.1f가 지났습니다.");

        //
        GameObject boss = PhotonNetwork.Instantiate("Tank-2nd",SpawnPoint.transform.position, Quaternion.identity);
        //boss.transform.parent = SpawnPoint.transform;
        //boss.transform.position = SpawnPoint.transform.localPosition;
        boss.transform.position = SpawnPoint.transform.position;
        Debug.Log(boss.transform.position);
        boss.GetComponent<NavMeshAgent>().enabled = true;

        //nav가 켜졌다면, bossFSM이 켜진다.로 조건을 바꿔야하나?
        boss.GetComponent<BossFSM>().enabled = true;
       
        isBossDone = true;
    }


    //보스의 위치를 가져오려면..?
    bool deathexploDone = false;
    void InstantiateDeathFX()
    {
        if (!deathexploDone)
        {
            Transform boss = GetComponent<EJBossHP>().gameObject.transform;

            print("DeathFX가 실행되었습니다");
            GameObject bodyexloImpact = PhotonNetwork.Instantiate("DeadExplo", boss.position, Quaternion.identity);
            bodyexloImpact.transform.localScale = Vector3.one * 30;

            bodyexloImpact.SetActive(true);

            deathexploDone = true;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (PhotonNetwork.IsMasterClient)
        {

        if (!isBossDone)
        {
            SpawnBoss();
            isBossDone = true;
        }
        }
    }
}
