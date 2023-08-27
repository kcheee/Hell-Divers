using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using Photon.Pun;

public class StratagemManager : MonoBehaviour
{
    //스트라타잼 4개를 보유하고있다!
    static int maxStratagem;
    //보유하고 있는 스트라타잼
    public List<Stratagems> current_Stratagems = new List<Stratagems>();
    //활성화가 되어있는 스트라타잼
    public List<Stratagems> active_Stratagems = new List<Stratagems>();

    public bool Isreturn = false;

    public PlayerInfoObj PlayerInfoUI;
    public GameObject UIObject;
    public GameObject UIObject2;

    public System.Action<int,int,bool> active_Action;


    public PhotonView photonView;
    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        if (!photonView.IsMine) {
            return;
        }
        int index = 0;
        //active_Stratagems = current_Stratagems.ToList();
        current_Stratagems.ForEach((s) =>
        {
            GameObject go = null;
            //지금은 급하니 땜빵을 하자
            switch (index) {
                case 0:
                    go = Instantiate(UIObject, PlayerInfoUI.Stratagem_Panel);
                    break;
                case 1:
                    go = Instantiate(UIObject2, PlayerInfoUI.Stratagem_Panel);
                    break;
            }
            
            StratagemUICode CodeUI = go.GetComponent<StratagemUICode>();
            //자기 자신의 코드를 생성하고 셋팅.(예정)
            CodeUI.GetImage.sprite = s.Stratagem_Image;
            s.myStratagemUI = CodeUI;
            //foreach (KeyType.Key key in s.CallCode)
            //{
            //    컬코드에 따라서 생성되는 이미지 UI
            //    Image img = new Image();
            //    img.sprite = Code.GetTexture(key)
            //    Code.Code_images.Add();
            //}

            //PlayerInfoUI.codes.Add(Code);


            active_Stratagems.Add(s);
            index++;
        });
          
        

    }
    public Stratagems CompareCode(List<KeyType.Key> list,int index) {
        if (Isreturn) { return null; }

        KeyType.Key key = list[index];
        //내가 가지고 있는 스트라타잼들의 코드가 
        //지금 입력받은 코드와 일치하느냐
        
        for(int i = 0; i < active_Stratagems.Count; i++) { //이거때문에 있는거에서 해야한다니까..오류어류./
            Debug.LogError("몇번 실행되었느냐!" + active_Stratagems.Count);
            Stratagems Stratagem = active_Stratagems[i];
            //인덱스가 크다 : 문제가 있다.
            //하지만 인덱스로 비교를 안해도 그냥 다 비교했을때 같은게 ㅇ벗다면 
            if (Stratagem.CallCode.Count -1 < index) {
                Debug.LogError("Return!!!!");
                continue;
            }
            //지금 들어온 입력값과 인덱스가 스트라타잼의 코드와 입력값과 같니?
            if (Stratagem.CallCode[index] == key)
            {
                //너 활성화 (나중수정)
                if (!active_Stratagems.Contains(Stratagem)) {
                    active_Stratagems.Add(Stratagem);
                    Debug.Log("너 활성화");
                }
                
                //PlayerInfoUI.codes[i].Code_images[index].color = Color.red;
                //인덱스가 같고 코드의 길이가 들어온 인덱스와 같으면
                //스킬을 사용한다.
                Debug.Log(Stratagem.CallCode.Count - 1 + " " + index + 1);
                Stratagem.myStratagemUI.Code_images[index].color = Color.yellow;
                if (Stratagem.CallCode.Count - 1 == index) {
                    Debug.Log("스킬사용!!");
                    //인덱스를 초기화 하고싶다.
                    list.Clear();
                    active_Stratagems.Clear();
                    Stratagem.myStratagemUI.ResetColor();
                    return Stratagem;
                }
            }
            else {
                i = -1; //초기화
                //너 비활성화
                Stratagem.myStratagemUI.ResetColor();
                //PlayerInfoUI.codes[i].ResetColor();
                active_Stratagems.Remove(Stratagem);

                Debug.Log("너 비활성화" + active_Stratagems.Count);

            }
        }

        //여기서 그냥 너 이제 입력하지마 라고 알려주자.
        if (active_Stratagems.Count <= 0) {
            Isreturn = true;
        }
        return null;

    }

    public void init() {
        active_Stratagems.Clear();
        Isreturn = false;
        current_Stratagems.ForEach((s) =>
        {
            active_Stratagems.Add(s);
            s.myStratagemUI.ResetColor();
            
        }
            
            
        );
    }


}
