using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnManager : MonoBehaviourPun
{
    #region 싱글톤
    static public SpawnManager Instance;
    private void Awake()
    {
        Instance = this; 
    }

    #endregion

    public GameObject[] spawnPos;
    public GameObject[] spawnMob;
    public int spawnmaxcount = 10;  // 스폰 맥스 카운트
    public float spawnInterval = 1.0f;  // 소환 간격
    public bool SpawnFlag = false;
    private int currentSpawnCount = 0;  // 현재 스폰된 횟수

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (SpawnFlag)
        {
            int randomPosIndex = Random.Range(0, spawnPos.Length);  // 랜덤한 스폰 위치 인덱스
            int randomMobIndex = Random.Range(0, 4);  // 랜덤한 몹 인덱스

            // 랜덤한 위치에 몹 스폰
            string mobPrefabName = "";

            switch (randomMobIndex)
            {
                case 0: mobPrefabName = "Initiate_E-main"; break;
                case 1: mobPrefabName = "Squad-Leader-Main"; break;
                case 2: mobPrefabName = "Hound-main"; break;
                case 3: mobPrefabName = "Immolator-main-Flame"; break;
            }

            PhotonNetwork.Instantiate(mobPrefabName, spawnPos[randomPosIndex].transform.position, Quaternion.identity);
            currentSpawnCount++;  // 스폰 횟수 증가

            yield return new WaitForSeconds(spawnInterval);
        }

        //Debug.Log("탈출");
    }

    // 몬스터가 사망했을 때 호출되는 함수
    public void MonsterDied()
    {
        currentSpawnCount--; // 스폰 횟수 감소
        if (currentSpawnCount < 0)
        {
            currentSpawnCount = 0;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            MonsterDied();
        }
    }
}