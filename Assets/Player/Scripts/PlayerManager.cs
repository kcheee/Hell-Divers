using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instace;
    //4명의 포톤뷰를 가지고 있고 public List<Photonview>
    //리스트에서 자신의 포톤뷰를 가져와서 
    //RPC 함수를 실행한다.

    //아니면 그냥 Player List를 가져와도 되겠다.
    //Player의 상태를 비교해서? 
    //죽은 플레이어를 소환한다.

    public List<PlayerTest1> PlayerList = new List<PlayerTest1>();
    //RPC 함수 실행?

    // Start is called before the first frame update
    void Start()
    {
        instace = this;
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
