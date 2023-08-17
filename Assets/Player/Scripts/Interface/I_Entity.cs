using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_Entity
{
    //데미지를 입을 때 호출할 함수
    public void damaged(int damage = 0);
    //죽었을 때 호출할 함수
    public void die(System.Action action);

}
