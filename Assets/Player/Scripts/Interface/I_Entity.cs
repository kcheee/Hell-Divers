using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public interface I_Entity
{
    //�������� ���� �� ȣ���� �Լ�
    [PunRPC]
    public void damaged(Vector3 pos,int damage = 0);
    //�׾��� �� ȣ���� �Լ�
    public void die(System.Action action);



}
