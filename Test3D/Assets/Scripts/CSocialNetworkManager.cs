using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSocialNetworkManager : MonoBehaviour {

    // 계정 타입
    public enum ACCOUNT_TYPE { GEUST, GOOGLE };
    public static ACCOUNT_TYPE accountType = ACCOUNT_TYPE.GEUST;

    // 기본 게임 서버 URL
    public static string baseUrl = "http://52.36.36.101/Test3D";

    // Use this for initialization
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
