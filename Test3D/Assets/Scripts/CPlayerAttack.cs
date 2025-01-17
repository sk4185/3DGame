﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CPlayerAttack : MonoBehaviour {


    private PhotonView pv = null;
    private CCharacterState cState = null;
    private CAnimaitonControl aniCtrl = null;
    private Button attackButton;

    public int attackDamage = 1;
    public Transform attackPoint;


    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        cState = GetComponent<CCharacterState>();
        aniCtrl = GetComponent<CAnimaitonControl>();
        attackButton = GameObject.Find("AttackButton").GetComponent<Button>();

        attackButton.onClick.AddListener(AttackRPC);
    }


    private void AttackRPC()
    {
        if (pv.isMine && cState.isDie == false)
        {
            pv.RPC("Attack", PhotonTargets.All);
        }
    }


    [PunRPC]
    private void Attack()
    {
         aniCtrl.AnimationCtrl(20);
    }


    public void AttackAnimationEvent()
    {
        Collider[] _collider = Physics.OverlapSphere(attackPoint.position, 0.09f,
            1 << LayerMask.NameToLayer("Walker"));

        if (_collider.Length <= 0) return;
        
        if(_collider[_collider.Length-1].GetComponent<CCharacterState>().state != CCharacterState.State.Block)
            _collider[_collider.Length - 1].GetComponent<CPlayerDamage>().Damage(attackDamage);
    }


    public void OnAttackAnimaitonStart()
    {
        attackButton.interactable = false;
    }


    public void OnAttackAnimationEnd()
    {
        attackButton.interactable = true;
    }
}
