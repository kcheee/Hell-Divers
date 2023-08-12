using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public float camY = 10; //고정   
    public float followSpeed = 10;
    Transform Target;
    // Start is called before the first frame update
    void Start()
    {
        Target = GameObject.FindWithTag("Player").transform; 
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        pos.y = 0;
        Vector3 target = Target.position;
        target.y = 0;
        //내 위치와 타겟의 위치의 y를 0으로 설정하고
        //둘의 거리를 잰다.
        float distance = Vector3.Distance(pos, target);

        //다시 원래 y로 돌려놓음.
        pos.y = camY;
        target.z += -4;
        target.y = camY;

        //밑으로 갈때는 -7이니까 아래인것. 나중에 고쳥야진
        Debug.Log(distance);
        //거리에 따라서 스피드가 달라지도록 설정한다.
        transform.position = Vector3.Lerp(pos, target, Time.smoothDeltaTime * distance );
    }
}
