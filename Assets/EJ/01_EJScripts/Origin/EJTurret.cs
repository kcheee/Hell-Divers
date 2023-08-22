using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EJTurret : MonoBehaviourPun
{
    //turretAnim
    public Animator turretHeadReaction;

    //fire����
    float attackRange = 5;
    public LayerMask playerLayer;
    int fireCount;
    //int maxFireCount = 5;

    GameObject target;

    public GameObject turretBulletFactory;
    public Transform turretFirePos;
    public GameObject turretMuzzleFactory;
   
    //Head Rotate
    public GameObject turretHead;
    float ry = 0;
    float rotateAngle = 20;
    public int power;

    //Player Object
    //Transform NearPlayer;
    GameObject NearPlayer;
    float Turret2PlayerDistance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //player ����
        NearPlayer =  GameObject.FindWithTag("Player");

        Debug.Log("Player��" + NearPlayer);

        #region overlapSphere�� ã��
        //NearPlayer = FindClosestObject();

        //Collider[] players = Physics.OverlapSphere(transform.position, attackRange, playerLayer);

        //player ���� �� Fire
        //if (players.Length > 0)
        //{
        //    Invoke(nameof(fireCountReset), 1f);

        //    for (int i = 0; i < players.Length; i++)
        //    {
        //        if (fireCount < maxFireCount)
        //        {
        //            target = players[i].gameObject;
        //            turretHead.transform.LookAt(target.transform);

        //            StartCoroutine(UpdateTurretFire());
        //        }
        //    }
        //}else
        //{
        //    print("Idle�����Դϴ�");
        //    UpdateIdle();
        //}
        #endregion

        float Turret2PlayerDistance = Vector3.Distance(transform.position, NearPlayer.transform.position);

        //NearPlayer�� distance���� �����ٸ�
        if (Turret2PlayerDistance < attackRange)
        {
            turretHead.transform.LookAt(NearPlayer.transform);
            //photonView.RPC(nameof(StartUpdateTurretFire), RpcTarget.All);

            Debug.Log("Turret���� �Ÿ���"+Turret2PlayerDistance);
        }
        else 
        {
            UpdateIdle();
        }
        if(Input.GetKeyDown(KeyCode.L))
        {
            photonView.RPC(nameof(StartUpdateTurretFire), RpcTarget.All);
        }
    }

    bool isRightRotate = false;
    public void UpdateIdle()
    {

        if (isRightRotate)
        {
            ry += rotateAngle * Time.deltaTime;
            if (ry >= 90)
            {
                isRightRotate = false;
            }
        }else
        {
            ry -= rotateAngle * Time.deltaTime;
            if (ry <= -90)
            {
                isRightRotate = true;
            }
        }
        turretHead.transform.localEulerAngles = new Vector3(0, ry, 0);
    }

    // �ѹ��� ����
    bool isTurretDone = false;
    // �ڷ�ƾ���� Update
    float delayTime = 1f;
    // Ż���Ʈ ����: while������ distance���� �־����� �� ������ �̹� ������ Idle���·� ���� ����

    [PunRPC]
    public void StartUpdateTurretFire()
    {
        Debug.Log("Log �� ����!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        StartCoroutine(UpdateTurretFire());
    }

    [PunRPC]
    public IEnumerator UpdateTurretFire()
    {
        if (!isTurretDone)
        {
            isTurretDone = true;
            GameObject bullet = Instantiate(turretBulletFactory, turretFirePos.transform.position, Quaternion.identity);

            //forward���� �����ִ� ��??
            Instantiate(turretMuzzleFactory, turretFirePos.transform.position, Quaternion.identity).transform.parent = transform;
            
            bullet.transform.parent = transform;

            ONturretHeadAnim();

            bullet.GetComponent<Rigidbody>().AddForce(turretFirePos.forward*power, ForceMode.Impulse);

            yield return new WaitForSeconds(delayTime);
            isTurretDone = false;
        }                   
    }


    //turretHeadAnim
    public void ONturretHeadAnim()
    {
        turretHeadReaction.SetTrigger("turretFire");
    }    


    public void fireCountReset()
    {
        fireCount = 0;
    }


    // ����� �÷��̾� ã�� �Լ�.
    public Transform FindClosestObject()
    {
        Transform closest = PlayerManager.instace.PlayerList[0].transform;
        float closestDistance = Vector3.Distance(transform.position, closest.position);

        foreach (PlayerTest1 obj in PlayerManager.instace.PlayerList)
        {
            float distance = Vector3.Distance(transform.position, obj.transform.position);
            if (distance < closestDistance)
            {
                closest = obj.transform;
                closestDistance = distance;
            }
        }

        return closest;
    }
}
