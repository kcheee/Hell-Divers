using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_StratagemObject
{
    
    //스트라타잼 오브젝트는 모두 E키를 눌러서 상호작용한다.
    //E키를 눌렀을때 호출하는 함수.
    void Add();
    
    //사용하거나 파괴할때
    void I_Destroy();

    //사거리 안에 들어왔을때
/*    void OnActive(PlayerTest1 player);
    //사거리에서 나올때 실행되는 함수
    void OnDeactive(PlayerTest1 player);*/
}   
