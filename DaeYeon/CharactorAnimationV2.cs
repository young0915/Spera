using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

using MyTools = CommonFunction;
public class CharactorAnimationV2 : MonoBehaviourPun
{
    Animator animator;
    NGUIJoystick JoyStick;

    PlayerInfo _playerInfo;                 // 플레이어의 상태 정보를 가지고 있는 클래스
    float _moveSpeed;                       // 플레이어가 받아오는 스피드 값이다.
    public bool isInteracting { get; set; } // 플레이어가 현재 상호작용중인지 정의하는 변수다.

    Rigidbody _playerRigid;
    charactorMovingCamera _playerMove;      // 플레이어의 움직임을 결정하는 클래스

    //public 
    // CheaserHitBButton hitButton;

    void Start()
    {
        animator = GetComponent<Animator>();
        JoyStick = GameObject.Find("Joystick").GetComponent<NGUIJoystick>(); // 다른클래스를 호출한다.

        _playerInfo = GetComponent<PlayerInfo>();
        _moveSpeed = 0;
        isInteracting = false;
        _playerRigid = GetComponent<Rigidbody>();
        _playerMove = GetComponent<charactorMovingCamera>();

        // hitButton = GameObject.Find("AttckButton").GetComponent<CheaserHitBButton>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if (!photonView.IsMine && PhotonNetwork.IsConnected)
        //{
        //    return;
        //}

        CheckPlayerState();
    }

    /// <summary>
    /// 플레이어의 애니메이션 조건을 검사하는 함수
    /// </summary>
    void CheckPlayerState()
    {
        int playerHp = _playerInfo.playerHp;

        if (_playerInfo.isStopMoving) { }
        else
        {
            // 캐릭터의 움직임은 idle, run이 블렌딩 되어 있다.
            // MoveSpeed 값으로 어느 모션을 렌더할지 결정한다.

            _moveSpeed = MyTools.ComputeBigValue(Mathf.Abs(JoyStick.joyStickPosX),
                Mathf.Abs(JoyStick.joyStickPosY));
            animator.SetFloat("MoveSpeed", _moveSpeed);
        }

        // 살금살금 걷는 애니메이션
        if(_playerInfo.playerHp < 3)
        {
            _playerInfo.isCrouched = false;
        }

        // 다치는 애니메이션
        animator.SetInteger("PlayerHp", playerHp);

        // 죽는 애니메이션
        if (playerHp > 0)
        {
            //Debug.Log("No Death");
            _playerInfo.isDeath = false;
        }
        else
        {
            //Debug.Log("Death");
            _playerInfo.isDeath = true;
        }
        animator.SetBool("IsDeath", _playerInfo.isDeath);
    }
}
