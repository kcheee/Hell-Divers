using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerTest1 : MonoBehaviour
{
    //Test Text
    public Transform trBody;
    public float speed = 5;
    public Gun currentGun;
    public Gun mainGun;
    public Gun subGun;
    public bool reload = false;
    Animator anim;

    //현재 가까이 다가가서 활성화 되어있는 오브젝트
    public AudioClip testclip;
    public AudioClip testclip2;
    public GameObject stratagemObj;
    public Stratagems current_stratagem;


    public Stratagems C_Stratagem {
        get { return current_stratagem; }
        set { current_stratagem = value;
            //스트라타잼 애니메이션 후 잡는다.
            GameObject stratagemobj = Instantiate(stratagemObj, trBody.position + Vector3.up ,trBody.rotation);
            Stratagems stratagem = stratagemobj.GetComponent<Stratagems>();
            stratagem = value;
            Rigidbody arbody = stratagemobj.GetComponent<Rigidbody>();
            arbody.AddForce(trBody.forward * 7 + trBody.up * 5, ForceMode.Impulse);
        }
    }

    public I_StratagemObject currentGemObj;

    public Code_InputManager code_input;
    public StratagemManager stratagemManager;
    public PlayerHP playerHp;
    public enum PlayerState { 
        Live,Die
    }
    public PlayerState currentState = PlayerState.Live;
    void Start()
    {
        anim = trBody.GetComponent<Animator>();
        //test
       // PlayerUI.instance.ManganizeText.text = currentGun.currentManganize.ToString();
        //PlayerUI.instance.BulletText.text = currentGun.maxBullet + " / " + currentGun.currentBullet;

        //만약, mine 이라면
        //add component.
        code_input = gameObject.AddComponent<Code_InputManager>();
        stratagemManager = GetComponent<StratagemManager>();
        playerHp = GetComponent<PlayerHP>();
        playerHp.Ondie = () => {
            if (currentState != PlayerState.Die) {
                anim.SetTrigger("Die"); 
                currentState = PlayerState.Die;
                PlayerManager.instace.PlayerList.Remove(this);
            }
        
        };
    }

    void Update()
    {
        if (currentState == PlayerState.Die) {
            return;
        }
        //test
        //PlayerUI.instance.ManganizeText.text = currentGun.currentManganize.ToString();
        //PlayerUI.instance.BulletText.text = currentGun.maxBullet + " / " + currentGun.currentBullet;


        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");



        Vector3 dir = Vector3.right * h + Vector3.forward * v;

        //
        dir.Normalize();
        speed = 4;
        anim.SetFloat("Horizontal", h);
        anim.SetFloat("Vertical", v);
        anim.SetFloat("speed", dir.magnitude);
        anim.SetFloat("RunSpeed", speed);


        //컨트롤 키를 눌렀을때 스트라타잼 입력을 받고싶다.
        if (Input.GetKey(KeyCode.LeftControl))
        {
            PlayerUI.instance.StratagemImage.gameObject.SetActive(true);
            //입력 코드를 입력할때
            code_input.input(() => {
                code_input.IsInput = !stratagemManager.Isreturn;
                //(원래 여기서 사운드 하는거 아님. 테스트임.)
                SoundManager.instance.Play(testclip2);
                //코드가 진짜 코드와 맞는지 계속 확인해준다.
                int count = code_input.KeyInputList.Count - 1;
                List<KeyType.Key> list = code_input.KeyInputList;
                Stratagems stratagem = stratagemManager.CompareCode(list, count);
                if (stratagem)
                {
                    C_Stratagem = stratagem;
                    code_input.init();
                    stratagemManager.init();
                }

            }); //end lambda.
            return;
        } //end Input.
        //만약에 움직이고 있다면(sqr은 루트 ㄴㄴ)
        if (dir.sqrMagnitude > 0)
        {
            //SmoothDemp?
           // transform.forward = dir;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = 10;
                anim.SetFloat("RunSpeed", speed);
            }

            // 이동 방향과 Body 의 오른쪽 벡터의 사이각을 구하자
            float angle = Vector3.Angle(dir, trBody.right);

            //만약에 각도가 90보다 작으면 오른쪽을 회전
            //원작에 회전이 생각보다 빠르게 된다.
            if (! (Vector3.Magnitude(trBody.forward - dir) < 0.1)) {
                if (angle < 90)
                {
                    trBody.Rotate(new Vector3(0, 5, 0));
                    //Debug.Log(angle);
                }
                //그렇지않으면 왼쪽으로 회전
                else
                {
                    //Debug.Log(angle);
                    trBody.Rotate(new Vector3(0, -5, 0));
                }
            }           
            //trBody.forward = Vector3.Lerp(trBody.forward, dir,Time.deltaTime * 20);

        }
        Aiming();

        /*Vector3 playerPosition = transform.position;
        Camera mainCamera = Camera.main;
        float moveSpeed = 10;
        // 카메라 시야의 경계를 구합니다.
        float cameraDistance = mainCamera.transform.position.y - transform.position.y;
        float cameraHalfHeight = Mathf.Tan(mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad) * cameraDistance;
        float cameraHalfWidth = cameraHalfHeight * mainCamera.aspect;

        // 플레이어의 이동 입력을 받습니다.
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // 플레이어의 위치를 업데이트합니다.
        float newPosX = playerPosition.x + moveHorizontal * moveSpeed * Time.deltaTime;
        float newPosZ = playerPosition.z + moveVertical * moveSpeed * Time.deltaTime;

        // 카메라 시야 내에 제한합니다.
        float clampedX = Mathf.Clamp(newPosX, mainCamera.transform.position.x - cameraHalfWidth, mainCamera.transform.position.x + cameraHalfWidth);
        float clampedZ = Mathf.Clamp(newPosZ, mainCamera.transform.position.z - cameraHalfHeight, mainCamera.transform.position.z + cameraHalfHeight);

        // 제한된 위치로 플레이어의 위치를 업데이트합니다.
        transform.position = new Vector3(clampedX, playerPosition.y, clampedZ);*/



        transform.position += dir * speed * Time.deltaTime;
        
        if (Input.GetMouseButton(0) && !reload)
        {
            CancelInvoke("ResetSpread");
            //anim.SetTrigger("Fire2");
            bool IsAnim = currentGun.Fire();
            
            
        }
        if (Input.GetMouseButtonUp(0))
        {
            CancelInvoke("ResetSpread");
            anim.SetBool("Fire", false);
            Invoke("ResetSpread",0.5f);
            
        }


        //재장전 키를 눌렀고 Gun한테 장전을 할수 있는지 물어본다.
        if (Input.GetKey(KeyCode.R) && currentGun.ReloadAble()) {
            //애니메이션이 끝나고 장전이 실행된다.
            //장전 - > iDLE
            anim.SetTrigger("Reload");
            reload = true;
            SoundManager.instance.Play(testclip);
        }
       

        //1번을 누르면 메인 무기
        if (Input.GetKey(KeyCode.Alpha1)) {
            ChangeGun(mainGun);
        }
        //2번을 누르면 보조 무기
        if (Input.GetKey(KeyCode.Alpha2))
        {
            ChangeGun(subGun);
        }



        //땠으면
        //전부 초기화 한다.
        if (Input.GetKeyUp(KeyCode.LeftControl)) {
            PlayerUI.instance.StratagemImage.gameObject.SetActive(false);
            code_input.init();
            stratagemManager.init();
        }


        if (Input.GetKeyDown(KeyCode.E) && currentGemObj != null) {
            currentGemObj.Add();
        }

    }

    public void ChangeGun(Gun gun) {
        currentGun.gameObject.SetActive(false);
        currentGun = gun;
        currentGun.gameObject.SetActive(true);
    }
    public void test() {
        anim.SetBool("Fire", false);
    }

    public void Reloading() {
        currentGun.Reload();
    }

    public void Aiming() {
        
        if (Input.GetButtonUp("Fire2"))
        {
            anim.SetBool("PistolAiming", false);
            anim.SetBool("RifleAiming", false);
        }
        //마우스 우클릭
        if (Input.GetButton("Fire2"))
        {


            

            //만약 총을 들고있다면 
            //조준 애니메이션을 실행합니다.

            //만약 현재 총기가 라이플이라면 라이플 애니메이션을 실행하고
            //현재 총기가 피스톨이라면 피스톨 애니메이션을 실행한다.
            switch (currentGun.gunType)
            {
                case Gun.GunType.Rifle:
                    anim.SetBool("RifleAiming", true);

                    break;
                case Gun.GunType.Pistol:
                    anim.SetBool("PistolAiming",true);

                    break;
                
            }
            //anim.SetBool("Aiming", true);

            speed = 1;
            //마우스 위치를
            Vector3 msPos = Input.mousePosition;

            //스크린 위치로 바꾸고 거기에 해당하는 레이를 만든다.
            Ray ray = Camera.main.ScreenPointToRay(msPos);
            RaycastHit hitInfo;
            //레이를 쏜다.
            if (Physics.Raycast(ray, out hitInfo))
            {

                //맞은곳 - 자신의 위치를 target으로 한다. y값은 사용하지 않으니 0으로 한다.
                Vector3 target = hitInfo.point - transform.position;
                target.y = 0;
                //일단 이렇게 하고 수정합시당.
                trBody.forward = target;
                //trBody.forward = Vector3.Lerp(trBody.forward,target,Time.deltaTime * 5);
            }

            Vector2 myDir = new Vector2(trBody.forward.x, trBody.forward.z);
            anim.SetFloat("MyHorizontal", myDir.x);
            anim.SetFloat("MyVertical", myDir.y);

        }

    }


    public void ResetSpread() {
        currentGun.ResetSpread();
    }
}
