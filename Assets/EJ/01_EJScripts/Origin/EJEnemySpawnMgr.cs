using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EJEnemySpawnMgr : MonoBehaviourPun
{
    static public EJEnemySpawnMgr Instance;

    private void Awake()
    {
        Instance = this;
    }

    public GameObject[] spawnPos;
    public GameObject[] spawnMob;
    public int spawnmaxcount = 10;  // ���� �ƽ� ī��Ʈ
    public float spawnInterval = 1.0f;  // ��ȯ ����
    public bool SpawnFlag = false;
    private int currentSpawnCount = 0;  // ���� ������ Ƚ��


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnRoutine());
        print("enemySpawnMgr ��ŸƮ �Լ� ����");
    }

    private IEnumerator SpawnRoutine()
    {
        print("SpawnRoutine������");
        while (SpawnFlag)
        {
            int randomPosIndex = Random.Range(0, spawnPos.Length);  // ������ ���� ��ġ �ε���
            int randomMobIndex = Random.Range(0, 4);  // ������ �� �ε���

            // ������ ��ġ�� �� ����
            string mobPrefabName = "";

            switch (randomMobIndex)
            {
                case 0: mobPrefabName = "Initiate_E-main"; break;
                case 1: mobPrefabName = "Squad-Leader-Main"; break;
                case 2: mobPrefabName = "Hound-main"; break;
                case 3: mobPrefabName = "Immolator-main-Flame"; break;
            }

            PhotonNetwork.Instantiate(mobPrefabName, spawnPos[randomPosIndex].transform.position, Quaternion.identity);
            currentSpawnCount++;  // ���� Ƚ�� ����

            yield return new WaitForSeconds(spawnInterval);
        }

        //Debug.Log("Ż��");
    }

    // ���Ͱ� ������� �� ȣ��Ǵ� �Լ�
    public void MonsterDied()
    {
        currentSpawnCount--; // ���� Ƚ�� ����
        if (currentSpawnCount < 0)
        {
            currentSpawnCount = 0;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            MonsterDied();
        }
    }
}
