using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CIntroManager : MonoBehaviour {


    private void Start()
    {
        StartCoroutine("GoLobbySceneCoroutine");
    }


    // 로비 씬으로 이동하는 코루틴
    private IEnumerator GoLobbySceneCoroutine()
    {
        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene("Lobby");
    }
}
