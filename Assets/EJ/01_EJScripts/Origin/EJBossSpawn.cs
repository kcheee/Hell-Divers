using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;

public class EJBossSpawn : MonoBehaviourPun
{
    float currTime;
    float BossSpawnTIme = 0.1f;

    public GameObject bossSpawnFactory;
    public GameObject SpawnPoint;
    public GameObject bossTrigger;
    

    bool isBossDone = false;
    int BossCount = 0;

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
    }
    

    [PunRPC]
    void SpawnBoss()
    {
        Debug.Log("0.1f�� �������ϴ�.");
        GameObject boss = PhotonNetwork.Instantiate("Tank-2nd",SpawnPoint.transform.position, Quaternion.identity);
        //boss.transform.parent = SpawnPoint.transform;
        //boss.transform.position = SpawnPoint.transform.localPosition;
        boss.transform.position = SpawnPoint.transform.position;
        Debug.Log(boss.transform.position);
        boss.GetComponent<NavMeshAgent>().enabled = true;

        //nav�� �����ٸ�, bossFSM�� ������.�� ������ �ٲ���ϳ�?
        boss.GetComponent<BossFSM>().enabled = true;
       
        isBossDone = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isBossDone)
        {
            SpawnBoss();
            isBossDone = true;
        }
    }
}
