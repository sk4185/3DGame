using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CIntroManager : MonoBehaviour {

    UIWidget _pressAnyKey;
    UIWidget _mainMenu;
    public GameObject _mainMenuObj;
    public GameObject _pressAnyObj;
    private void Start()
    {
        _pressAnyKey = _pressAnyObj.GetComponent<UIWidget>();
        _mainMenu = _mainMenuObj.GetComponent<UIWidget>();
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
