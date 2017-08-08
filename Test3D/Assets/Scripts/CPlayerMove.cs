using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CnControls;


public class CPlayerMove : MonoBehaviour {

    private Transform tr = null;
    private CAnimaitonControl aniCtrl = null;
    private Transform camPivot = null;
    private CCharacterState cState =null;
    private PhotonView pv = null;
    private CharacterController cCtrl = null;

    private Button jumpButton;
    private Button runButton;

    public float walkSpeed;
    public float moveSpeed;
    public float rotSpeed;

    private float verticalVelocity;
    private float jumpForce = 5.0f;
    private float gravity = 7.0f;

    private Vector3 currPos = Vector3.zero;
    private Quaternion currRot = Quaternion.identity;

    private bool isRunning = false;
    private bool isJumping = false;

    private void Awake()
    {
        tr = GetComponent<Transform>();
        aniCtrl = GetComponent<CAnimaitonControl>();
        camPivot = GameObject.Find("CamPivot").GetComponent<Transform>();
        cState = GetComponent<CCharacterState>();
        pv = GetComponent<PhotonView>();
        cCtrl = GetComponent<CharacterController>();

        jumpButton = GameObject.Find("JumpButton").GetComponent<Button>();
        runButton = GameObject.Find("RunButton").GetComponent<Button>();

        jumpButton.onClick.AddListener(Jump);
        runButton.onClick.AddListener(IsRunning);

        pv.synchronization = ViewSynchronization.UnreliableOnChange;
        pv.ObservedComponents[0] = this;

        currPos = tr.position;
        currRot = tr.rotation;
    }


    private void Start()
    {
        Camera.main.GetComponent<CFollowCamera>().target = camPivot;
    }


    // 동기화 메소드
    private void OnPhotonSerializeView(PhotonStream _stream, PhotonMessageInfo _info)
    {
        if (_stream.isWriting)
        {
            _stream.SendNext(tr.position);
            _stream.SendNext(tr.rotation);
        }
        else
        {
            currPos = (Vector3)_stream.ReceiveNext();
            currRot = (Quaternion)_stream.ReceiveNext();
        }
    }


    private void LateUpdate()
    {
        if (pv.isMine && cState.isDie == false)
        {
            Move();
        }
        else if (pv.isMine && cState.isDie == true) { }
        else
        {
            tr.position = Vector3.Lerp(tr.position, currPos, Time.deltaTime * 3.0f);
            tr.rotation = Quaternion.Slerp(tr.rotation, currRot, Time.deltaTime * 3.0f);
        }
    }


    private void Move()
    {
        if (cCtrl.isGrounded)
        {
            jumpButton.interactable = true;
            verticalVelocity = -gravity * Time.deltaTime;

            if (isJumping == false)
            {
                verticalVelocity = jumpForce;
            }
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
            isJumping = true;
            jumpButton.interactable = false;
        }

        Vector3 moveVector = Vector3.zero;
        moveVector = new Vector3(CnInputManager.GetAxis("Horizontal"), verticalVelocity,
        CnInputManager.GetAxis("Vertical"));

        if(moveVector.sqrMagnitude > 0.5f)
        {
            int index;

            index = isRunning == false ? 18 : 15;

            pv.RPC("SetAni", PhotonTargets.All, index);

            moveVector.z = isRunning == false ? moveVector.z * walkSpeed :
                moveVector.z * moveSpeed;

            tr.Rotate(Vector3.up * moveVector.x * rotSpeed * Time.deltaTime);
            moveVector = tr.TransformDirection(moveVector);

            cCtrl.Move(moveVector * Time.deltaTime);
            cState.state = CCharacterState.State.Move;
        }
        
        if(moveVector.sqrMagnitude < 0.5f)
        {
            pv.RPC("SetAni", PhotonTargets.All, 1);
        }
    }


    private void Jump()
    {
        isJumping = !isJumping;
        pv.RPC("SetAni", PhotonTargets.All, 9);
    }


    private void IsRunning()
    {
        isRunning = !isRunning;
    }


    [PunRPC]
    private void SetAni(int index)
    {
        aniCtrl.AnimationCtrl(index);
    }
}
