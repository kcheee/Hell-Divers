using DG.Tweening;
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
    bool startlerp=false;
    bool CheckShake = false;
    private void OnEnable()
    {
        StartCoroutine(startlerpCam());
    }
    IEnumerator startlerpCam()
    {

        yield return new WaitForSeconds(1);
        DOTween.To(() => camY, x => camY = x, 5, 1);
        startlerp=true;
    }
    // Start is called before the first frame update
    void Start()
    {

        //Target = GameObject.FindWithTag("Player").transform; 
    }

    // Update is called once per frame
    void Update()
    {
        if (startlerp)
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

            transform.position = Vector3.Lerp(pos, target, Time.smoothDeltaTime * 5);
        }

        // �׽�Ʈ��
        if (Input.GetKeyDown(KeyCode.Mouse0)) 
        {
            CheckShake = true;        
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            CheckShake = false;
        }
        if(CheckShake)
        {
            Camera.main.transform.DOShakePosition(0.2f, 0.1f, 10, 90);
        }     
    }




}
