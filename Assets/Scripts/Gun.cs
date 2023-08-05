using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    //일단 총알 발사 방식이..
    //아직은 총알 이동,탄창,재장전만.
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
        //이거같은경우는 나중에 프로퍼티로 bool 변수를 빼가지고 하는게 더 나을려나
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
