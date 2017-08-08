using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CPlayerBlock : MonoBehaviour {

    private PhotonView pv = null;
    private CCharacterState cState = null;
    private CAnimaitonControl aniCtrl = null;
    private Button blockButton;


    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        cState = GetComponent<CCharacterState>();
        aniCtrl = GetComponent<CAnimaitonControl>();
        blockButton = GameObject.Find("BlockButton").GetComponent<Button>();

        blockButton.onClick.AddListener(BlockRPC);
    }


    private void BlockRPC()
    {
        if (pv.isMine && cState.isDie == false)
        {
            cState.state = CCharacterState.State.Block;
            pv.RPC("Block", PhotonTargets.All);
        }
    }


    [PunRPC]
    private void Block()
    {
        aniCtrl.AnimationCtrl(4);
    }
}
