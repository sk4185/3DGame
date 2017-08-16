using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGameManager : MonoBehaviour {

    private PhotonView pv = null;
    private Transform[] spawnPoints;


    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        spawnPoints = GameObject.Find("SpawnPoints").GetComponentsInChildren<Transform>();

        CreatePhotonGameObject("Prefabs/Bear");    // 플레이어 생성

        PhotonNetwork.isMessageQueueRunning = true;
    }


    private void Start()
    {
        if(PhotonNetwork.player.ID == PhotonNetwork.room.MasterClientId)    // 방장이면
        {
            for (int i = 30; i > 0; i--)
            {
                CreatePhotonGameObject("Prefabs/AI");
            }
        }
    }


    // 포톤 객체 생성 메소드
    private void CreatePhotonGameObject(string _name)
    {
        int rand = Random.Range(0, spawnPoints.Length);

        // 포톤 네트워크용 게임 오브젝트를 생성함
        // 이 게임 오브젝트는 포톤용 게임오브젝트이므로 반드시 PhotonView 컴포넌트를 가져야 함
        PhotonNetwork.Instantiate(_name,
            new Vector3(spawnPoints[rand].position.x, 1.0f, spawnPoints[rand].position.z), Quaternion.identity, 0);
    }
}
