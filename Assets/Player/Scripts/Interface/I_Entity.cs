using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_Entity
{
    //�������� ���� �� ȣ���� �Լ�
    public void damaged(int damage = 0);
    //�׾��� �� ȣ���� �Լ�
    public void die(System.Action action);

}
