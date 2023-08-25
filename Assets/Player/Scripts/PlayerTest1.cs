using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
public class PlayerTest1 : MonoBehaviourPun,IPunObservable
{
    //Test Text
    public Transform trBody;
    public Transform RightHand;
    public Text NickNameText;

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

    //던지려고 하는 물체
    public GameObject throwObject;


    public PlayerInfoObj PlayerInfoUI;
    public Stratagems C_Stratagem {
        get { return current_stratagem; }
        set { current_stratagem = value;
            //스트라타잼 애니메이션 후 잡는다.
            //anim.SetTrigger("Grenade");

            photonView.RPC(nameof(PlayAnim), RpcTarget.All, "Grenade");
            photonView.RPC(nameof(test1), RpcTarget.All, C_Stratagem.id);
            //currentGun.gameObject.SetActive(false);
            //GameObject stratagemobj = PhotonNetwork.Instantiate("Stratagem",trBody.position,Quaternion.identity);  //Instantiate(stratagemObj, trBody.position + Vector3.up ,trBody.rotation);'

            //Stratagems stratagem = stratagemobj.GetComponent<Stratagems>();
            //stratagem = value;

        }
    }

    //문제 : 스트라타잼 코드를 입력 후 던지는데 
    //내 PC에서는 무엇을 던졌는지 아는데
    //상대 PC에서는 무엇을 던지는지 몰라서 스트라타잼 ID를 같이 보낸다.
    //하지만,누적되는것이 마음에 들지 않는다.,
    string id;
    [PunRPC]
    public void test1(string s) {
        id = s;
    }



    //던지는 애니메이션이 끝난 후 이 함수가 실행된다. 
    public void FireGrenade() {
        Debug.Log(id);
        Throw(id);
    }
    [PunRPC]
    public void Throw(string name) {
        //모두 Throw애니메이션을 하지만 마스터 클라이언트에서만 생성
        //하지만 PhotonInstantiate라서 모든 PC에서 생성하니까 별 문제가 없다.
        //문제는 위치를 동기화하는것
        if (PhotonNetwork.IsMasterClient)
        {
            GameObject stratagemobj = PhotonNetwork.Instantiate(name, RightHand.position, Quaternion.identity); //Instantiate((GameObject)Resources.Load("str01"), RightHand.position, Quaternion.identity); //
            //그러니까 이놈의 포톤뷰를 가져와서 모든 PC에 RPC를 한다! 
            PhotonView view = stratagemobj.GetComponent<PhotonView>();
            //스트라타잼을 던지는 사람의 forward와 up 방향을 받아서 물리적으로 힘을 부여함
            //RPC를 최소화하는게 목적이나, 스트라타잼은 많이 호출되지 않음.
            view.RPC("Throw", RpcTarget.All, trBody.forward,trBody.up);

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

    private void Awake()
    {
        
        //PlayerManager.instace.action();
    }
    void Start()
    {
        if(NickNameText)
            NickNameText.text = photonView.Owner.NickName;
        //생성할때 오너의 닉네임을 가지고
        PlayerInfoUI =  PlayerManager.instace.JoinUI(photonView.Owner.NickName);
        //플레이어는 자신의 UI를 알고있으면 RPC로 다 되는거임

        ch = GetComponent<CharacterController>();
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
                PlayerManager.instace.DeathList.Add(this);
            }
        
        };


        if (photonView.IsMine) {
            PlayerManager.instace.action = null;
        }
        PlayerManager.instace.Addlist(this);
    }

    Vector3 last;

    public int layer = 0;

    Coroutine currCoroutine;
    private void LateUpdate()
    {
        last = transform.position;
    }
    float h = 0;
    float v = 0;
    private Vector3 targetPsition;
    private Quaternion targetRotation;

    public CharacterController ch;

    Vector2 myDir;
    void Update()
    {
        ch.Move(Vector3.up * -9.81f * Time.deltaTime);
        //transform.position += Vector3.up * -9.81f * Time.deltaTime;
        myDir = new Vector2(trBody.forward.x, trBody.forward.z);
/*        Vector3 test = Camera.main.WorldToViewportPoint(transform.position);
        Vector3 pos = transform.position;
        //Debug.Log(test);
        if (test.x < 0.05) {
            test.x = 0.05f;
            //pos = Camera.main.ViewportToWorldPoint(test);
            //pos.x = 0.95f;
            pos.x += Time.deltaTime * 5;
            transform.position = pos;
            //Camera.main.GetComponent<FollowCam>().Iscam = false;
        }

        if (test.x > 0.95)
        {
            //Camera.main.GetComponent<FollowCam>().Iscam = false;
            //pos.x -= Time.deltaTime * 2;
            test.x = 0.95f;
            pos.x -= Time.deltaTime * 5;
            pos = Camera.main.ViewportToWorldPoint(test);
            //pos.x = 0.95f;
            transform.position = pos;

            return;
        }
        //Debug.Log(gameObject.name +  test);
        if (test.y < 0.05f) {
            test.y = 0.05f;
            pos = Camera.main.ViewportToWorldPoint(test);
            pos.y = 0;
            pos.z += Time.deltaTime * 5;
            transform.position = pos;
        }
        if(test.y > 0.95)
        {
            test.y = 0.95f;
            pos = Camera.main.ViewportToWorldPoint(test);
            pos.y = 0;
            pos.z -= Time.deltaTime * 5;
            transform.position = pos;
            return;
        }*/
        if (currentState == PlayerState.Die) {
            return;
        }
        //test
        //PlayerUI.instance.ManganizeText.text = currentGun.currentManganize.ToString();
        //PlayerUI.instance.BulletText.text = currentGun.maxBullet + " / " + currentGun.currentBullet;

        Vector3 dir = Vector3.right * h + Vector3.forward * v;
        dir.Normalize();
        if (photonView.IsMine)
        {
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");
            dir = Vector3.right * h + Vector3.forward * v;
            dir.Normalize();
            speed = 4;
            if (Input.GetMouseButton(0) && !reload) {
                           
                
                    
                
            }

            if (Input.GetMouseButton(0) && !reload)
            {
                if (current_stratagem)
                {
                    
                    photonView.RPC(nameof(PlayAnim), RpcTarget.All, "Throw");

                }
                else {
                    int rand = Random.Range(-1, 2);
                    photonView.RPC(nameof(Fire), RpcTarget.All, rand);
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                
                //CancelInvoke("ResetSpread");
                if(currCoroutine != null)
                {
                    StopCoroutine(currCoroutine);
                }
                anim.SetBool("Fire", false);
                //Invoke("ResetSpread", 0.5f);
                currCoroutine = StartCoroutine(ResetSpread());
                Debug.LogWarning("Invoke 호출");

            }

            //컨트롤 키를 눌렀을때 스트라타잼 입력을 받고싶다.
            if (Input.GetKey(KeyCode.LeftControl))
            {
                PlayerUI.instance.StratagemImage.gameObject.SetActive(true);
                //입력 코드를 입력할때
                code_input.input(() => {
                    code_input.IsInput = !stratagemManager.Isreturn;

                    SoundManager.instance.SfxPlay(PlayerSound.instance.GetClip(PlayerSound.P_SOUND.Input));

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


            //재장전 키를 눌렀고 Gun한테 장전을 할수 있는지 물어본다.
            if (Input.GetKeyDown(KeyCode.R) && currentGun.ReloadAble())
            {
                //애니메이션이 끝나고 장전이 실행된다.
                //장전 - > iDLE
                photonView.RPC(nameof(PlayAnim), RpcTarget.All, "Reload");
                reload = true;
                SoundManager.instance.SfxPlay(PlayerSound.instance.GetClip(PlayerSound.P_SOUND.Reloading));
            }


            //1번을 누르면 메인 무기
            if (Input.GetKey(KeyCode.Alpha1))
            {
                ChangeGun(mainGun);
            }
            //2번을 누르면 보조 무기
            if (Input.GetKey(KeyCode.Alpha2))
            {
                ChangeGun(subGun);
            }



            //땠으면
            //전부 초기화 한다.
            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                PlayerUI.instance.StratagemImage.gameObject.SetActive(false);
                code_input.init();
                stratagemManager.init();
            }

            //아이템!
            if (Input.GetKeyDown(KeyCode.E) && currentGemObj != null)
            {
                photonView.RPC(nameof(GetItem), RpcTarget.All);
            }

            if (dir.sqrMagnitude > 0)
            {
                //SmoothDemp?
                // transform.forward = dir;
                if (Input.GetKey(KeyCode.LeftShift))
                {

                    speed = 10;
                    
                }



                // 이동 방향과 Body 의 오른쪽 벡터의 사이각을 구하자
                float angle = Vector3.Angle(dir, trBody.right);

                //만약에 각도가 90보다 작으면 오른쪽을 회전
                //원작에 회전이 생각보다 빠르게 된다.
                if (!(Vector3.Magnitude(trBody.forward - dir) < 0.1))
                {
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
            //Debug.LogError(speed);
            //transform.position += dir * speed * Time.deltaTime;
            ch.Move(dir * speed * Time.deltaTime);
        }
        //End Ming
        else
        {
            transform.position = Vector3.Lerp(transform.position, targetPsition, Time.smoothDeltaTime * 20);
            trBody.rotation = Quaternion.Lerp(trBody.rotation,  targetRotation, Time.smoothDeltaTime * 20);
        }


        anim.SetFloat("Horizontal", h);
        anim.SetFloat("Vertical", v);
        anim.SetFloat("speed",dir.magnitude);
        anim.SetFloat("RunSpeed", speed);
        anim.SetFloat("MyHorizontal",myDir.x);
        anim.SetFloat("MyVertical",myDir.y);
    }

    [PunRPC]
    public void Fire(int rand) {
        photonView.RPC("Res_Spr",RpcTarget.All);
        //Debug.LogError(rand);
        currentGun.Fire(rand);
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
        
        //RPC 함수 최소화
        if (Input.GetButtonUp("Fire2"))
        {
            photonView.RPC(nameof(PlayAnim), RpcTarget.All, "RifleAiming", false);

        }
        if (Input.GetButtonDown("Fire2")) { 
            photonView.RPC(nameof(PlayAnim), RpcTarget.All, "RifleAiming", true);
        }
        //마우스 우클릭
        if (Input.GetButton("Fire2"))
        {
            //만약 총을 들고있다면 
            //조준 애니메이션을 실행합니다.

            //만약 현재 총기가 라이플이라면 라이플 애니메이션을 실행하고
            //현재 총기가 피스톨이라면 피스톨 애니메이션을 실행한다.

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


            //RPC 없어도 가능. 성능 저하 발생!!

            //  photonView.RPC(nameof(PlayAnim), RpcTarget.All, "MyHorizontal", myDir.x);
           // photonView.RPC(nameof(PlayAnim), RpcTarget.All, "MyVertical", myDir.y);
            /*anim.SetFloat("MyHorizontal", myDir.x);
            anim.SetFloat("MyVertical", myDir.y);*/

        }

    }



    //RPC는 인보크를 사용할수없으니ㄷ까(인보크가 안되어서 코루틴으로 작성 왜 안되지?)
    public IEnumerator ResetSpread() {
        yield return new WaitForSeconds(0.5f);
        Debug.LogWarning("RESETSPR인보크호출");
        photonView.RPC(nameof(ResetSpreadRPC), RpcTarget.All);
        //currCoroutine = null;
    }

    //모든 PC야 내가 이 아이템을 주웠어 네네들 다 삭제해줘!
    [PunRPC]
    public void GetItem() {
        //포톤은 마스터 클라이언트거나 내 객체만 삭제가 가능한것으로 보임 따라서 
        //마스터 PC에 있는놈만 이것을 실행하면 된다!
        if(PhotonNetwork.IsMasterClient && currentGemObj != null)
            currentGemObj.Add();
    }

    [PunRPC]
    public void ResetSpreadRPC() {
        Debug.LogWarning("RPC");
        currentGun.ResetSpread();
    }


    public enum AnimationType { 
        Trigger,Bool,Float
    }
    [PunRPC]
    public void PlayAnim(string name) {
        anim.SetTrigger(name);
    }

    [PunRPC]
    public void PlayAnim(string name,float value)
    {
        //Debug.Log("HELLO!" + value);
        anim.SetFloat(name,value);
    }

    [PunRPC]
    public void PlayAnim(string name, bool value)
    {
        anim.SetBool(name, value);
    }

    [PunRPC]
    public void Res_Spr()
    {
        CancelInvoke("ResetSpread");
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //너가 나냐
        if (stream.IsWriting)
        {
            stream.SendNext(h);
            stream.SendNext(v);
            stream.SendNext(speed);
            stream.SendNext(transform.position);
            stream.SendNext(trBody.rotation);
            
        }
        //누구냐
        else {
            
            h = (float)stream.ReceiveNext();
            v = (float)stream.ReceiveNext();
            speed = (float)stream.ReceiveNext();
            targetPsition = (Vector3)stream.ReceiveNext();
            targetRotation = (Quaternion)stream.ReceiveNext();
        }
    }


    //플레이어가 나가면 등록 종료
    private void OnDestroy()
    {
        PlayerManager.instace.PLAYER_LIST.Remove(this);
    }
}
