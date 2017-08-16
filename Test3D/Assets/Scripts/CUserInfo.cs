using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CUserInfo : MonoBehaviour {

    [HideInInspector]
    public string googleId;
    [HideInInspector]
    public string deviceId;
    [HideInInspector]
    public string nickName;

    [HideInInspector]
    public int userIcon;
    [HideInInspector]
    public int gold;
    [HideInInspector]
    public int cash;
    [HideInInspector]
    public int win;
    [HideInInspector]
    public int lose;
    [HideInInspector]
    public int kill;
    [HideInInspector]
    public int death;


    public void SetUserInfo(Dictionary<string, object> _userInfoMap)
    {
        deviceId = SystemInfo.deviceUniqueIdentifier;

        Dictionary<string, object> _accountInfoMap = _userInfoMap["account_info"]
            as Dictionary<string, object>;                                               // 계정 정보 딕셔너리를 추출함


        // 각 정보 값들을 추출함
        deviceId = _accountInfoMap["device_ID"].ToString();
        nickName = _accountInfoMap["nickname"].ToString();       // 유저 아이디
        userIcon = int.Parse(_accountInfoMap["icon"].ToString());
        gold = int.Parse(_accountInfoMap["gold"].ToString());
        cash = int.Parse(_accountInfoMap["cash"].ToString());
        win = int.Parse(_accountInfoMap["win"].ToString());
        lose = int.Parse(_accountInfoMap["lose"].ToString());
        kill = int.Parse(_accountInfoMap["kill"].ToString());
        death = int.Parse(_accountInfoMap["death"].ToString());
    }
}
