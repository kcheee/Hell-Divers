using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Gun : MonoBehaviourPun
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
    //currentBullet
    public int Current_Bullet
    {
        get { return currentBullet; }
        set {
            currentBullet = value;
            PlayerUI.instance.BulletText.text = value.ToString();
        }
    }

    public int maxManganize;
    int currentManganize;
    //currentManganize
    public int Current_Manganize
    {
        get { return currentManganize; }
        set { currentManganize = value;
            PlayerUI.instance.ManganizeText.text = value.ToString();
        }
    }
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

    //최소 탄퍼짐 & 최대 탄퍼짐
    public float min_spreadRange = 1;
    public float max_spreadRange = 5;
    //탄퍼짐 증가율
    public float add_spreadRange = 200;

    // Start is called before the first frame update
    void Start()
    {
        //Max로 해줍니다!
        Current_Bullet = maxBullet;
        Current_Manganize = maxManganize;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Fire의 같은경우 한번 더 플레이 해 보니까
    //Ray로 되있는것같다.
    public bool Fire(int rand) {
        //이거같은경우는 나중에 프로퍼티로 bool 변수를 빼가지고 하는게 더 나을려나
        bool fire = Current_Bullet > 0 && isFire;
        //bool returnfire = currentBullet > 0 && (WaitAnim ? isFire && WaitAnim : true);
        //총알이 있니?
        if (fire) {
            //발사했을때, 탄퍼짐 범위를 증가시킨다.
            current_spreadRange += 0.01f * add_spreadRange;
            //Debug.LogError("계산된 탄퍼짐!" + current_spreadRange);
            //탄퍼짐 범위를 제한한다.
            current_spreadRange = Mathf.Clamp(current_spreadRange, min_spreadRange, max_spreadRange);

            //총의 탄퍼짐 값을 구한다.
            //자신의 앞방향에서 자신의 오른쪽  백터에서 방향값을 곱하고 범위값을 곱한 백터를 더한다.
            Vector3 spread = transform.forward + transform.right * rand * 0.01f * current_spreadRange;
            //Debug.LogError("이것이 탄퍼짐이다." + spread);

            GameObject fireEft = Instantiate(FireEft, FirePos.position, Quaternion.identity);
            GameObject muzzleEft = Instantiate(MuzzleEft, FirePos.position, Quaternion.identity);
            //fireEft.transform.parent = null;
            //fireEft.transform.forward = transform.forward ;
            fireEft.transform.rotation = Quaternion.LookRotation(spread, Vector3.up);
            muzzleEft.transform.forward = FirePos.forward;
            //if (PhotonNetwork.IsMasterClient)

            animtest.ResetTrigger("Fire2");
            animtest.SetTrigger("Fire2");
            isFire = false;
            StartCoroutine(FireWait());

            //그리고 Ray를 발사한다,.(정확하게 하자면, 총구 위치에서 Ray가 됨.)
            //그리고 사거리.
            //근데 Ray는 직선이니까 하나를 더 쏘거나 아니면 그냥 각도를 내리거나. 1번째 방법이 
            //더 낫겠지.(ㄴㄴ 그냥 이펙트인거같음!)
            //노말백터\


            Debug.DrawRay(transform.position, spread * MaxDistance, Color.red, 1);

            //정확한 동기화를 위해서 마스터만 실행한다.
            if (PhotonNetwork.IsMasterClient) {
                Ray ray = new Ray(transform.position, spread);
                //Debug.Log(transform.forward);


                RaycastHit hit;
                // 나중에 layer로 설정
                if (Physics.Raycast(ray, out hit, MaxDistance))
                {
                    Debug.Log("Hit" + hit.collider.gameObject.name);
                    PhotonView view = hit.collider.gameObject.GetComponent<PhotonView>();
                    I_Entity entity = hit.collider.gameObject.GetComponent<I_Entity>();
                    if (entity != null)
                    {
                        view.RPC("damaged",RpcTarget.All, hit.point, 10);
                        //entity.damaged(10);
                        //기본적으로 포톤은 커스텀 타입을 지원해주지 않는다.
                        //따라서 직렬화/비직렬화를 직접 구현을 해야하는데, 시간이 오래걸릴것으로 예상됨 따라서 
                        //그냥 HP를 동기화 하면 되는데? 
                        //photonView.RPC(nameof(Damage),RpcTarget.All,list);
                    }
/*                    if (hit.collider.tag == "Enemy")
                    {
                        hit.collider.GetComponent<Enemy_Fun>().E_Hit(hit.point);
                    }*/
                }
                Current_Bullet--;
            }
        }
            
        return isFire;
    }

    [PunRPC]
    public void Damage(int damage) {
        Debug.Log("DAMAGED!!");
        
    }   
    private void OnEnable()
    {
        isFire = true;
    }


    //여기는 자동 Reload 가 없습니다
    public bool ReloadAble() {
        //탄창이 0보다 크고 현재 총알이 최대 총알보다 작을때 장전을 할수있음!
        if (Current_Manganize > 0 && Current_Bullet < maxBullet) {

            return true;
        }
        return false;
    }

    public void Reload() {
        Debug.Log("Reloading!!!");
        Current_Bullet = maxBullet;
        Current_Manganize--;
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
