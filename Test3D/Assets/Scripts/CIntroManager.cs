using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GameData;


public class CIntroManager : MonoBehaviour {

    UIWidget _pressAnyKey;
    UIWidget _mainMenu;
    public GameObject _mainMenuObj;
    public GameObject _pressAnyObj;

    public GameObject changeIdPanel;             // 아이디 변경 판넬
    public InputField changeIdInputField;        // 아이디 변경 인풋필드
    public Button changeIdSubmitButton;          // 변경 버튼
    private CUserInfo userInfo;

    private void Awake()
    {
        userInfo = GameObject.Find("UserInfo").GetComponent<CUserInfo>();
        changeIdSubmitButton.onClick.AddListener(OnClickChangeIdSubmitButton);            // 아이디 변경 버튼 클릭 이벤트 연결
    }

    private void Start()
    {
        _pressAnyKey = _pressAnyObj.GetComponent<UIWidget>();
        _mainMenu = _mainMenuObj.GetComponent<UIWidget>();

        GuestLogin();
    }

    // 게스트 버튼 클릭 이벤트
    private void GuestLogin()
    {
        string _devId = SystemInfo.deviceUniqueIdentifier;       // 디바이스 고유 정보를 참조함

        JoinOrLogin(_devId);                                     // 가입 및 로그인 처리를 수행함
    }

    // 가입 및 로그인 처리를 수행함
    private void JoinOrLogin(string _userId = "")
    {
        StartCoroutine("JoinOrLoginNetCoroutine", _userId);      // 가입 및 로그인 수행
    }

    // 가입 및 로그인 처리하는 코루틴
    private IEnumerator JoinOrLoginNetCoroutine(string _userId)
    {
        // 가입 및 로그인 url 작성
        string _url = CSocialNetworkManager.baseUrl + "/user_account.php";

        // 유저 아이디값을 POST 파라미터로 설정해 줌
        WWWForm _form = new WWWForm();
        _form.AddField("device", _userId);

        // 가입 및 로그인 요청을 수행함
        WWW _www = new WWW(_url, _form);

        // 통신을 지연을 대기함
        yield return _www;

        if (_www.error == null)
        {
            Debug.Log("data -> " + _www.text);

            // 응답 받은 json 문자열을 Dictionary 객체로 변환함
            Dictionary<string, object> _responseData
                = MiniJSON.jsonDecode(_www.text) as Dictionary<string, object>;

            string _result_code = _responseData["result_code"].ToString().Trim();

            if (_result_code.Equals("LOGIN_SUCCESS"))
            {
                // 유저 정보를 셋팅함
                userInfo.SetUserInfo(_responseData);

                // 닉네임을 아직 안 정했으면
                if(userInfo.nickName.Equals("0"))
                {
                    changeIdPanel.SetActive(true);
                }
                else
                    _pressAnyObj.SetActive(true);
            }
            else
            {
                Debug.Log("게임 서버 로그인에 실패함");
            }
        }
        else
        {
            Debug.Log("error -> " + _www.error);
        }
    }

    // 아이디 변경 버튼 이벤트 메소드
    private void OnClickChangeIdSubmitButton()
    {
        if (changeIdInputField.text.Equals(null) || changeIdInputField.text.Equals(""))    // 인풋필드에 아무것도 넣지 않았으면
        {
            Debug.Log("제대로 입력해주세요.");
        }
        else
        {
            StartCoroutine("UpdateUserIDCoroutine", changeIdInputField.text);         // 유저 아이디 변경 코루틴 실행
        }
    }

    // 아이디 변경 코루틴
    private IEnumerator UpdateUserIDCoroutine(string userId)
    {
        string _url = CSocialNetworkManager.baseUrl + "/nickname_update.php";               // 서버 url
        string _devId = SystemInfo.deviceUniqueIdentifier;                                     // 디바이스 번호

        WWWForm _form = new WWWForm();

        _form.AddField("device", _devId);
        _form.AddField("nickname", userId);

        WWW _www = new WWW(_url, _form);

        yield return _www;

        if (_www.error == null)
        {
            Debug.Log("data -> " + _www.text);

            Dictionary<string, object> _responseData
                = MiniJSON.jsonDecode(_www.text) as Dictionary<string, object>;

            string _result = _responseData["result_code"].ToString().Trim();

            if (_result.Equals("UPDATE_SUCCESS"))
            {
                Dictionary<string, object> _accountInfoMap = _responseData["account_info"] as
                    Dictionary<string, object>;

                int _rand = Random.Range(0, 1000);

                userInfo.nickName = _accountInfoMap["nickname"].ToString() + _rand;

                changeIdPanel.SetActive(false);
                _pressAnyObj.SetActive(true);
            }
            else
            {
                Debug.Log("동일한 아이디가 존재합니다.");
            }
        }
        else
        {
            Debug.Log("error -> " + _www.error);
        }
    }

    // 로비 씬으로 이동하는 코루틴
    private IEnumerator GoLobbySceneCoroutine()
    {
        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene("LoadingScene");
    }

    // 인트로 시작후 출력되는 Press To start 화면 클릭시
    public void PressAnyKeySpriteClicked()
    {
        // pressAnykey 위젯 닫고 메인메뉴 open
        StartCoroutine(FadeIn(_mainMenu, _pressAnyKey));
    }

    // 게임 시작 버튼이 클릭됐을 때 로딩씬 먼저 로드
    public void OnPlayBtnClicked()
    {
        StartCoroutine("GoLobbySceneCoroutine");
    }

    // 랭킹 버튼 클릭 됐을 때
    public void OnRankingBtnClick()
    {

    }

    // Fadeout 이펙트 코루틴
    IEnumerator FadeOut(UIWidget widget)
    {
        widget.alpha = 1.0f;
        while (widget.alpha > 0.0f)
        {
            yield return null;
            widget.alpha -= Time.deltaTime * 2.0f;
            
        }
        widget.gameObject.SetActive(false);
    }

    // FadeIn 이펙트 코루틴
    IEnumerator FadeIn(UIWidget openWidget, UIWidget closeWidget)
    {
        yield return StartCoroutine("FadeOut", closeWidget);
        openWidget.gameObject.SetActive(true);
        openWidget.alpha = 0.0f;
        while(openWidget.alpha < 1.0f)
        {
            yield return null;
            openWidget.alpha += Time.deltaTime * 2.0f;
        }

       
    }
}
