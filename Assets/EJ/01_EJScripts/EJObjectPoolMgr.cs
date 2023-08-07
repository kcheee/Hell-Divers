using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJObjectPoolMgr : MonoBehaviour
{
    public static EJObjectPoolMgr instance;

    //GausCanon
    public GameObject gausCanonImpactPrefab;
    public Queue<GameObject> gausCononImpactQueue = new Queue<GameObject>();
    int gausCannonNum = 15;

    //MachineGun
    public GameObject machineGunImpactPrefab;
    public Queue<GameObject> machineGunImpactQueue = new Queue<GameObject>();
    int machineGunNum = 5;

    //Bomb
    public GameObject bombPrefab;
    public Queue<GameObject> bombQueue = new Queue<GameObject>();

    public GameObject bombImpactPrefab;
    public Queue<GameObject> bombImpactQueue = new Queue<GameObject>();

    int bombNum = 4;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        //gausCanon 미리 생성 후 꺼두기
        for (int i = 0; i < gausCannonNum; i++)
        {
            GameObject gausCannonImpactObject = Instantiate(gausCanonImpactPrefab, Vector3.zero, Quaternion.identity);
            gausCononImpactQueue.Enqueue(gausCannonImpactObject);
            gausCannonImpactObject.SetActive(false);
        }

        //MachineGun 미리 생성 후 꺼두기
        for (int i = 0; i < machineGunNum; i++)
        {
            GameObject machineGunImpactObject = Instantiate(machineGunImpactPrefab, Vector3.zero, Quaternion.identity);
            bombQueue.Enqueue(machineGunImpactObject);
            machineGunImpactObject.SetActive(false);
        }

        //bomb 미리 생성 후 꺼두기
        for (int i = 0; i < bombNum; i++)
        {
            GameObject bombObject = Instantiate(bombPrefab, Vector3.zero, Quaternion.identity);
            bombQueue.Enqueue(bombObject);
            bombObject.SetActive(false);

            GameObject bombImpactObject = Instantiate(bombImpactPrefab, Vector3.zero, Quaternion.identity);
            bombImpactQueue.Enqueue(bombImpactObject);
            bombImpactObject.SetActive(false);
        }
    }

    //gausCannon 함수
    public void InsertGausCannonImpactQueue(GameObject gausCannonImpact)
    {
        gausCononImpactQueue.Enqueue(gausCannonImpact);
        gausCannonImpact.SetActive(false);
    }

    public GameObject GetGausCannonImpactQueue()
    {
        GameObject gausCannonImpactObject = gausCononImpactQueue.Dequeue();
        gausCannonImpactObject.SetActive(true);
        return gausCannonImpactObject;
    }

    //machine Gun 함수
    public void InsertmachineGunImpactQueue(GameObject machineGunImpact)
    {
        gausCononImpactQueue.Enqueue(machineGunImpact);
        machineGunImpact.SetActive(false);
    }

    public GameObject GetmachineGunImpactQueue()
    {
        GameObject machineGunImpactObject = machineGunImpactQueue.Dequeue();
        machineGunImpactObject.SetActive(true);
        return machineGunImpactObject;
    }

    //bomb 함수
    public void InsertbombQueue(GameObject bomb)
    {
        bombQueue.Enqueue(bomb);
        bomb.SetActive(false);
    }

    public GameObject GetbombQueue()
    {
        GameObject bombObject = bombQueue.Dequeue();
        bombObject.SetActive(true);
        return bombObject;
    }

    public void InsertbombImpactQueue(GameObject bombImpact)
    {
        bombImpactQueue.Enqueue(bombImpact);
        bombImpact.SetActive(false);
    }

    public GameObject GetbombImpactQueue()
    {
        GameObject bombImpactObject = bombImpactQueue.Dequeue();
        bombImpactObject.SetActive(true);
        return bombImpactObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
