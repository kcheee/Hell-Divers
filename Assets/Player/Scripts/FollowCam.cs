using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public float camY = 10; //����   
    public float followSpeed = 10;
    public float camZ = 5;
    public Transform Target;
    public bool Iscam;
    // Start is called before the first frame update
    void Start()
    {

        //Target = GameObject.FindWithTag("Player").transform; 
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerManager.instace.PlayerList.Count == 0) return;

        Vector3 target = Vector3.zero;
        //�÷��̾� ����Ʈ���� �÷��̾ ������ �´�.
        foreach (PlayerTest1 player in PlayerManager.instace.PlayerList)
        {
            //�� �÷��̾���� ���͸� ���Ѵ�.
            target += player.transform.position;
        }
        //�� �÷��̾���� ������ �߰��� ���Ѵ�.
        if (PlayerManager.instace.PlayerList.Count != 1)
        {
            target /= PlayerManager.instace.PLAYER_LIST.Count;
        }

        //Debug.LogError(test);

        Vector3 pos = transform.position;
        pos.y = 0;

        target.y = 0;
        //�� ��ġ�� Ÿ���� ��ġ�� y�� 0���� �����ϰ�
        //���� �Ÿ��� ���.
        float distance = Vector3.Distance(pos, target);

        //�ٽ� ���� y�� ��������.
        pos.y = camY;
        target.y = camY;
        target.z += camZ;

        if (!Iscam)
        {
            target.x = pos.x;
            Iscam = true;
        }
        //���͸� �����Ѵ�.

        transform.position = Vector3.Lerp(pos, target, Time.smoothDeltaTime * 10);

    }
}
