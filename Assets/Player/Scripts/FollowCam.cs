using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public float camY = 10; //고정   
    public float followSpeed = 10;
    public float camZ = 5;
    public Transform Target;
    // Start is called before the first frame update
    void Start()
    {
        //Target = GameObject.FindWithTag("Player").transform; 
    }

    // Update is called once per frame
    void Update()
    {
        

        Vector3 target = Vector3.zero ;
        //플레이어 리스트에서 플레이어를 가지고 온다.
        foreach (PlayerTest1 player in PlayerManager.instace.PlayerList) {
            //각 플레이어들의 백터를 더한다.
            target += player.transform.position;
        }
        //각 플레이어들의 백터의 중간을 구한다.
        if (PlayerManager.instace.PlayerList.Count != 1) {
            target /= 2;
        }
            

        Vector3 pos = transform.position;
        pos.y = 0;

        target.y = 0;
        //내 위치와 타겟의 위치의 y를 0으로 설정하고
        //둘의 거리를 잰다.
        float distance = Vector3.Distance(pos, target);

        //다시 원래 y로 돌려놓음.
        pos.y = camY;        
        target.y = camY;
        target.z += camZ;


        //백터를 보간한다.
        transform.position = Vector3.Lerp(pos, target, Time.smoothDeltaTime * 10  );
    }
}
