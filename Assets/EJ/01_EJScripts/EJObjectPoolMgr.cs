using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJObjectPoolMgr : MonoBehaviour
{
    public static EJObjectPoolMgr instance;

    //GausCanon
    public GameObject gausCanonImpactPrefab;
    public Queue<GameObject> gausCononImpactQueue = new Queue<GameObject>();
    int gausCannonNum = 25;

    //MachineGun
    public GameObject machineGunImpactPrefab;
    public Queue<GameObject> machineGunImpactQueue = new Queue<GameObject>();
    int machineGunNum = 5;

    //Bomb
    public GameObject bombPrefab;
    public Queue<GameObject> bombQueue = new Queue<GameObject>();

    public GameObject bombImpactPrefab;
    public Queue<GameObject> bombImpactQueue = new Queue<GameObject>();

    int bombNum = 10;

    //BodyExplosion
    public GameObject bodyExploImpactPrefab;
    public Queue<GameObject> bodyExploImpactQueue = new Queue<GameObject>();

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        //gausCanon �̸� ���� �� ���α�
        for (int i = 0; i < gausCannonNum; i++)
        {
            GameObject gausCannonImpactObject = Instantiate(gausCanonImpactPrefab, Vector3.zero, Quaternion.identity);
            gausCononImpactQueue.Enqueue(gausCannonImpactObject);
            gausCannonImpactObject.SetActive(false);
        }

        //MachineGun �̸� ���� �� ���α�
        for (int i = 0; i < machineGunNum; i++)
        {
            GameObject machineGunImpactObject = Instantiate(machineGunImpactPrefab, Vector3.zero, Quaternion.identity);
            machineGunImpactQueue.Enqueue(machineGunImpactObject);
            machineGunImpactObject.SetActive(false);
        }

        //bomb �̸� ���� �� ���α�
        for (int i = 0; i < bombNum; i++)
        {
            GameObject bombObject = Instantiate(bombPrefab, Vector3.zero, Quaternion.identity);
            bombQueue.Enqueue(bombObject);
            bombObject.SetActive(false);

            GameObject bombImpactObject = Instantiate(bombImpactPrefab, Vector3.zero, Quaternion.identity);
            bombImpactQueue.Enqueue(bombImpactObject);
            bombImpactObject.SetActive(false);
        }

        //bodyExplosion �̸� ���� �� ���α�
        GameObject bodyexploImpactObject = Instantiate(bodyExploImpactPrefab, Vector3.zero, Quaternion.identity);
        bodyExploImpactQueue.Enqueue(bodyexploImpactObject);
        bodyexploImpactObject.SetActive(false);
    }

    //gausCannon �Լ�
    //Queue Storage�� Enqueue(�־�д�)
    public void ReturnGausCannonImpactQueue(GameObject gausCannonImpact)
    {
        gausCononImpactQueue.Enqueue(gausCannonImpact);
        gausCannonImpact.SetActive(false);
    }
    //Queue Storage���� Dequeue(������)
    public GameObject GetGausCannonImpactQueue()
    {
        GameObject gausCannonImpactObject = gausCononImpactQueue.Dequeue();
        gausCannonImpactObject.SetActive(true);
        return gausCannonImpactObject;
    }

    //machine Gun �Լ�
    //Queue Storage�� Enqueue(�־�д�)
    public void ReturnmachineGunImpactQueue(GameObject machineGunImpact)
    {
        machineGunImpactQueue.Enqueue(machineGunImpact);
        machineGunImpact.SetActive(false);
    }
    //Queue Storage���� Dequeue(������)
    public GameObject GetmachineGunImpactQueue()
    {
        GameObject machineGunImpactObject = machineGunImpactQueue.Dequeue();
        machineGunImpactObject.SetActive(true);
        return machineGunImpactObject;
    }

    //bomb �Լ�
    //Queue Storage�� Enqueue(�־�д�)
    public void ReturnbombQueue(GameObject bomb)
    {
        bombQueue.Enqueue(bomb);
        bomb.SetActive(false);
    }
    //Queue Storage���� Dequeue(������)
    public GameObject GetbombQueue()
    {
        GameObject bombObject = bombQueue.Dequeue();
        bombObject.SetActive(true);
        return bombObject;
    }
    //Queue Storage�� Enqueue(�־�д�)
    public void ReturnbombImpactQueue(GameObject bombImpact)
    {
        bombImpactQueue.Enqueue(bombImpact);
        bombImpact.SetActive(false);
    }
    //Queue Storage���� Dequeue(������)
    public GameObject GetbombImpactQueue()
    {
        GameObject bombImpactObject = bombImpactQueue.Dequeue();
        bombImpactObject.SetActive(true);
        return bombImpactObject;
    }

    //bodyExplosion �Լ�
    //Queue Storage�� Enqueue(�־�д�)
    public void ReturnbodyExploImpactQueue(GameObject bodyExploImpact)
    {
        bodyExploImpactQueue.Enqueue(bodyExploImpact);
        bodyExploImpact.SetActive(false);
    }
    //Queue Storage���� Dequeue(������)
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
