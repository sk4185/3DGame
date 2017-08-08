using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CnControls;


public class CTestMove : MonoBehaviour
{

    private Transform tr = null;
    private CAnimaitonControl aniCtrl = null;
    private Transform camPivot = null;
    private CCharacterState cState = null;
    private Rigidbody rigid = null;
    private PhotonView pv = null;

    private Button jumpButton;
    private Button runButton;

    public float walkSpeed;
    public float moveSpeed;
    public float jumpSpeed;
    public float rotSpeed;

    private Vector3 currPos = Vector3.zero;
    private Quaternion currRot = Quaternion.identity;
    private Vector3 down = Vector3.down;

    private bool isRunning = false;
    private int index;
    private float speed;


    private void Awake()
    {
        tr = GetComponent<Transform>();
        aniCtrl = GetComponent<CAnimaitonControl>();
        camPivot = GameObject.Find("CamPivot").GetComponent<Transform>();
        cState = GetComponent<CCharacterState>();
        rigid = GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();

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
        var inputVector = new Vector3(CnInputManager.GetAxis("Horizontal"), 0,
            CnInputManager.GetAxis("Vertical"));

        if (inputVector.sqrMagnitude > 0.01f) // 움직이면
        {
            tr.Rotate(Vector3.up * inputVector.x * rotSpeed * Time.deltaTime);

            index = isRunning == false ? 18 : 15;

            pv.RPC("SetAni", PhotonTargets.All, index);

            tr.Translate(Vector3.forward * inputVector.z * (speed = 
                isRunning == false ? walkSpeed : moveSpeed), Space.Self);
        }
        else
        {
            pv.RPC("SetAni", PhotonTargets.All, 1);
        }
    }


    private void Jump()
    {
        if(Physics.Raycast(tr.position, down, 0.5f))
        {
            pv.RPC("SetAni", PhotonTargets.All, 9);
            rigid.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
        }
    }


    private void IsRunning()
    {
        isRunning = isRunning == false ? true : false;
    }


    [PunRPC]
    private void SetAni(int index)
    {
        aniCtrl.AnimationCtrl(index);
    }
}
