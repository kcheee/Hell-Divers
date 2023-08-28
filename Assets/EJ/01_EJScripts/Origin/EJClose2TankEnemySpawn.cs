using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJClose2TankEnemySpawn : MonoBehaviour
{
    public SpawnManager spawnManager;


    Animator anim;
    float gaze;
    private Transform closestObject;
    float distance;

    bool flag = false;
    bool delayflag = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator component_off()
    {
        // 플래그
        // 스폰
        spawnManager.SpawnFlag = false;
        spawnManager.enabled = false;

        yield return new WaitForSeconds(7);
        GetComponent<EJClose2TankEnemySpawn>().enabled = false;
    }
}
