using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    //�ϴ� �Ѿ� �߻� �����..
    //������ �Ѿ� �̵�,źâ,��������.
    public GameObject Bullet;
    public int MaxBullet;
    public int CurrentBullet;
    // Start is called before the first frame update
    void Start()
    {
        CurrentBullet = MaxBullet;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Fire() {
        //�̰Ű������� ���߿� ������Ƽ�� bool ������ �������� �ϴ°� �� ��������
        if (CurrentBullet > 0) {
            GameObject bullet = Instantiate(Bullet, transform.position, transform.rotation);
            bullet.transform.forward = transform.forward;
            CurrentBullet--;
        }

    }
    public void Reload() {
        CurrentBullet = MaxBullet;
    }
}
