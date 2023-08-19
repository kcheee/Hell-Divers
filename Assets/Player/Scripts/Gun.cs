using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    //일단 총알 발사 방식이..
    //아직은 총알 이동,탄창,재장전만.
    public GameObject Bullet;

    //총구
    public Transform FirePos;
    public GameObject FireEft;
    public GameObject MuzzleEft;

    public int maxBullet;
    public int currentBullet;

    public int maxManganize;
    public int currentManganize;
    //총의 사거리다.
    public float MaxDistance;

    public float fireTime;

    public bool isFire = true;

    public float damage;

    public bool WaitAnim;

    public Animator animtest;

    //원작에서는 두가지로 나뉘는것같음
    public enum GunType { Rifle,Pistol }
    public GunType gunType;

    //탄퍼짐의 범위.
    public float current_spreadRange;
    public float min_spreadRange = 1;
    public float max_spreadRange = 5;
    //탄퍼짐 증가율
    public float add_spreadRange = 200;

    // Start is called before the first frame update
    void Start()
    {
        //Max로 해줍니다!
        currentBullet = maxBullet;
        currentManganize = maxManganize;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Fire의 같은경우 한번 더 플레이 해 보니까
    //Ray로 되있는것같다.
    public bool Fire(int rand) {

        //이거같은경우는 나중에 프로퍼티로 bool 변수를 빼가지고 하는게 더 나을려나
        bool fire = currentBullet > 0 && isFire;
        //bool returnfire = currentBullet > 0 && (WaitAnim ? isFire && WaitAnim : true);
        //총알이 있니?
        if (fire) {
            //발사했을때, 탄퍼짐 범위를 증가시킨다.
            current_spreadRange += 0.01f * add_spreadRange;
            Debug.LogError("계산된 탄퍼짐!" + current_spreadRange);
            //탄퍼짐 범위를 제한한다.
            current_spreadRange = Mathf.Clamp(current_spreadRange, min_spreadRange, max_spreadRange);


            animtest.ResetTrigger("Fire2");
            animtest.SetTrigger("Fire2");
            isFire = false;
            StartCoroutine(FireWait());

            //그리고 Ray를 발사한다,.(정확하게 하자면, 총구 위치에서 Ray가 됨.)
            //그리고 사거리.
            //근데 Ray는 직선이니까 하나를 더 쏘거나 아니면 그냥 각도를 내리거나. 1번째 방법이 
            //더 낫겠지.(ㄴㄴ 그냥 이펙트인거같음!)
            //노말백터\

            //총의 탄퍼짐 값을 구한다.
            //자신의 앞방향에서 자신의 오른쪽  백터에서 방향값을 곱하고 범위값을 곱한 백터를 더한다.
            Vector3 spread = transform.forward + transform.right * rand * 0.01f * current_spreadRange;
            Debug.LogError("이것이 탄퍼짐이다." + spread);
            Debug.DrawRay(transform.position, spread * MaxDistance, Color.red, 1);

            Ray ray = new Ray(transform.position,spread);
            //Debug.Log(transform.forward);
            GameObject fireEft = Instantiate(FireEft, FirePos.position,Quaternion.identity);
            GameObject muzzleEft = Instantiate(MuzzleEft, FirePos.position, Quaternion.identity);
            //fireEft.transform.parent = null;
            //fireEft.transform.forward = transform.forward ;
            fireEft.transform.rotation = Quaternion.LookRotation(spread,Vector3.up);
            muzzleEft.transform.forward = FirePos.forward;
            
            RaycastHit hit;
            // 나중에 layer로 설정
            if (Physics.Raycast(ray, out hit,MaxDistance)) {
                Debug.Log("Hit" + hit.collider.gameObject.name);
                I_Entity entity =  hit.collider.gameObject.GetComponent<I_Entity>();
                if (entity != null) {
                    entity.damaged(10);
                }
                if (hit.collider.tag == "Enemy")
                {
                    hit.collider.GetComponent<Enemy_Fun>().E_Hit(hit.point);
                }
                EnemyTest enemy = hit.collider.gameObject.GetComponent<EnemyTest>();
                if (enemy)
                {
                    enemy.Damaged(damage);
                }
            }
            currentBullet--;
        }
        return isFire;
    }

    private void OnEnable()
    {
        isFire = true;
    }


    //여기는 자동 Reload 가 없습니다
    public bool ReloadAble() {
        //탄창이 0보다 크고 현재 총알이 최대 총알보다 작을때 장전을 할수있음!
        if (currentManganize > 0 && currentBullet < maxBullet) {

            return true;
        }
        return false;
    }

    public void Reload() {
        Debug.Log("Reloading!!!");
        currentBullet = maxBullet;
        currentManganize--;
    }

    IEnumerator FireWait() {
        //animtest.ResetTrigger("Fire2");
        yield return new WaitForSeconds(fireTime);
        
        isFire = true;
    }

    public void ResetSpread() {
        current_spreadRange = 0;
    }
}
