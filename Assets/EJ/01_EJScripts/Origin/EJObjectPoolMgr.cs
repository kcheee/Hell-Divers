using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EJObjectPoolMgr : MonoBehaviourPun
{

    public static EJObjectPoolMgr instance;

    //GausCanon
    public GameObject gausCanonImpactPrefab;
    public Queue<GameObject> gausCononImpactQueue = new Queue<GameObject>();
    int gausCannonNum = 25;

    public GameObject gausCanonMuzzleImpactPrefab;
    public Queue<GameObject> gausCononMuzzleImpactQueue = new Queue<GameObject>();
    int gausCannonMuzzleNum = 25;

    //MachineGun
    public GameObject machineGunImpactPrefab;
    public Queue<GameObject> machineGunImpactQueue = new Queue<GameObject>();
    int machineGunNum = 5;

    //Bomb
    public GameObject bombPrefab;
    public Queue<GameObject> bombQueue = new Queue<GameObject>();

    public GameObject bombMuzzleImpactPrefab;
    public Queue<GameObject> bombMuzzleImpactQueue = new Queue<GameObject>();

    public GameObject bombExploImpactPrefab;
    public Queue<GameObject> bombExploImpactQueue = new Queue<GameObject>();

    int bombNum = 10;

    //BodyExplosion
    public GameObject bodyExploImpactPrefab;
    public Queue<GameObject> bodyExploImpactQueue = new Queue<GameObject>();

    //ObjectPool Mgr
    public GameObject ObjectPool;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        //gausCanon 미리 생성 후 꺼두기
        for (int i = 0; i < gausCannonNum; i++)
        {
            GameObject gausCannonImpactObject = Instantiate(gausCanonImpactPrefab, Vector3.zero, Quaternion.identity);
            gausCononImpactQueue.Enqueue(gausCannonImpactObject);
            gausCannonImpactObject.SetActive(false);
            gausCannonImpactObject.transform.parent = ObjectPool.transform;
        }
        //gausCanon Muzzle 미리 생성 후 꺼두기
        for (int i = 0; i < gausCannonNum; i++)
        {
            GameObject gausCannonMuzzleImpactObject = Instantiate(gausCanonMuzzleImpactPrefab, Vector3.zero, Quaternion.identity);
            gausCononMuzzleImpactQueue.Enqueue(gausCannonMuzzleImpactObject);
            gausCannonMuzzleImpactObject.SetActive(false);
            gausCannonMuzzleImpactObject.transform.parent = ObjectPool.transform;
        }

        //MachineGun 미리 생성 후 꺼두기
        for (int i = 0; i < machineGunNum; i++)
        {
            GameObject machineGunImpactObject = Instantiate(machineGunImpactPrefab, Vector3.zero, Quaternion.identity);
            machineGunImpactQueue.Enqueue(machineGunImpactObject);
            machineGunImpactObject.SetActive(false);
            machineGunImpactObject.transform.parent = ObjectPool.transform;
        }

        //bomb 미리 생성 후 꺼두기
        for (int i = 0; i < bombNum; i++)
        {
            GameObject bombObject = Instantiate(bombPrefab, Vector3.zero, Quaternion.identity);
            bombQueue.Enqueue(bombObject);
            bombObject.SetActive(false);
            bombObject.transform.parent = ObjectPool.transform;

            GameObject bombMuzzleImpactObject = Instantiate(bombMuzzleImpactPrefab, Vector3.zero, Quaternion.identity);
            bombMuzzleImpactQueue.Enqueue(bombMuzzleImpactObject);
            bombMuzzleImpactObject.SetActive(false);
            bombMuzzleImpactObject.transform.parent = ObjectPool.transform;

            GameObject bombExploImpactObject = Instantiate(bombExploImpactPrefab, Vector3.zero, Quaternion.identity);
            bombExploImpactQueue.Enqueue(bombExploImpactObject);
            bombExploImpactObject.SetActive(false);
            bombExploImpactObject.transform.parent = ObjectPool.transform;
        }

        //bodyExplosion 미리 생성 후 꺼두기
        GameObject bodyexploImpactObject = Instantiate(bodyExploImpactPrefab, Vector3.zero, Quaternion.identity);
        bodyExploImpactQueue.Enqueue(bodyexploImpactObject);
        bodyexploImpactObject.SetActive(false);
        bodyexploImpactObject.transform.parent = ObjectPool.transform;
    }
    [PunRPC]
    //gausCannon 함수
    //Queue Storage에 Enqueue(넣어둔다)
    public void ReturnGausCannonImpactQueue(GameObject gausCannonImpact)
    {
        gausCononImpactQueue.Enqueue(gausCannonImpact);
        gausCannonImpact.SetActive(false);
        print("returnGausCannonimpactQueue");
        
    }
    [PunRPC]
    //Queue Storage에서 Dequeue(꺼낸다)
    public GameObject GetGausCannonImpactQueue()
    {
        //particle play를 다시 해주는 코드 필요
        GameObject gausCannonImpactObject = gausCononImpactQueue.Dequeue();
        gausCannonImpactObject.SetActive(true);
        print("getGausCannonimpactQueue");
        return gausCannonImpactObject;
        
        
    }
    [PunRPC]
    //Queue Storage에 Enqueue(넣어둔다)
    public void ReturnGausCannonMuzzleImpactQueue(GameObject gausCannonMuzzleImpact)
    {
        gausCononMuzzleImpactQueue.Enqueue(gausCannonMuzzleImpact);
        gausCannonMuzzleImpact.SetActive(false);
        print("returnMuzzleQueue");


    }
    [PunRPC]
    //Queue Storage에서 Dequeue(꺼낸다)
    public GameObject GetGausCannonMuzzleImpactQueue()
    {
        GameObject gausCannonMuzzleImpactObject = gausCononMuzzleImpactQueue.Dequeue();
        gausCannonMuzzleImpactObject.SetActive(true);
        print("getMuzzleQueue");
        return gausCannonMuzzleImpactObject;
        
    }

    [PunRPC]
    //machine Gun 함수
    //Queue Storage에 Enqueue(넣어둔다)
    public void ReturnmachineGunImpactQueue(GameObject machineGunImpact)
    {
        machineGunImpactQueue.Enqueue(machineGunImpact);
        machineGunImpact.SetActive(false);

    }
    [PunRPC]
    //Queue Storage에서 Dequeue(꺼낸다)
    public GameObject GetmachineGunImpactQueue()
    {
        GameObject machineGunImpactObject = machineGunImpactQueue.Dequeue();
        machineGunImpactObject.SetActive(true);
        return machineGunImpactObject;
    }
    [PunRPC]
    //bomb 함수
    //Queue Storage에 Enqueue(넣어둔다)
    public void ReturnbombQueue(GameObject bomb)
    {
        bombQueue.Enqueue(bomb);
        bomb.SetActive(false);
    }
    [PunRPC]
    //Queue Storage에서 Dequeue(꺼낸다)
    public GameObject GetbombQueue()
    {
        GameObject bombObject = null;
        if (bombQueue.Count > 0)
        {
            bombObject = bombQueue.Dequeue();
            bombObject.SetActive(true);
        }
        else
        {
            //bombObject = Instantiate(bombPrefab, Vector3.zero, Quaternion.identity);
            //bombObject.transform.parent = ObjectPool.transform;
        }
        return bombObject;
    }
    [PunRPC]
    //Queue Storage에 Enqueue(넣어둔다)
    public void ReturnbombImpactQueue(GameObject bombImpact)
    {
        bombMuzzleImpactQueue.Enqueue(bombImpact);
        bombImpact.SetActive(false);
    }
    [PunRPC]
    //Queue Storage에서 Dequeue(꺼낸다)
    public GameObject GetbombImpactQueue()
    {
        GameObject bombImpactObject = bombMuzzleImpactQueue.Dequeue();
        bombImpactObject.SetActive(true);
        return bombImpactObject;
    }
    [PunRPC]
    //Queue Storage에 Enqueue(넣어둔다)
    public void ReturnbombExploImpactQueue(GameObject bombExploImpact)
    {
        bombExploImpactQueue.Enqueue(bombExploImpact);
        bombExploImpact.SetActive(false);
    }
    [PunRPC]
    //Queue Storage에서 Dequeue(꺼낸다)
    public GameObject GetbombExploImpactQueue()
    {
        GameObject bombExploImpactObject = bombExploImpactQueue.Dequeue();
        bombExploImpactObject.SetActive(true);
        return bombExploImpactObject;
    }
    [PunRPC]
    //bodyExplosion 함수
    //Queue Storage에 Enqueue(넣어둔다)
    public void ReturnbodyExploImpactQueue(GameObject bodyExploImpact)
    {
        bodyExploImpactQueue.Enqueue(bodyExploImpact);
        bodyExploImpact.SetActive(false);
    }
    [PunRPC]
    //Queue Storage에서 Dequeue(꺼낸다)
    public GameObject GetbodyExploImpactQueue()
    {
        GameObject bodyexploImpactObject = bodyExploImpactQueue.Dequeue();
        bodyexploImpactObject.SetActive(true);
        return bodyexploImpactObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
