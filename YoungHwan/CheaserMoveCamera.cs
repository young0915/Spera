using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CheaserMoveCamera : MonoBehaviourPun
{
    public Camera Camera;
    private NGUIJoystick JoyStick;
    private FixedTouchField touchField;

    CameraManager cameraManager;
    Rigidbody CharactRigidbody;
    Quaternion rotation = Quaternion.identity;
    private Vector3 dir;
    public float speed = 0.5f;
    private float turn;
    private bool isOnceTurn;
    float tochAngle;
    private float vertical;
    private float CameraAngleY;
    private Rigidbody rigidbody;

    //  public bool isStopMove { get; set; }    // 플레이어를 강제로 멈출 변수다.

    // private float CameraAngleSpeed = 0.1f;
    // private float CameraPosY = 3f;
    //private float CameraPosSpeed = 0.02f;

    void Start()
    {
        JoyStick = GameObject.Find("Joystick").GetComponent<NGUIJoystick>();
        touchField = GameObject.Find("cameraBack").GetComponent<FixedTouchField>();
        rigidbody = GetComponent<Rigidbody>();
        SoundManager.instance.playerWalkSource = GetComponents<AudioSource>()[0]; //사운드매니저에 넣어주기 - 지민추가
        SoundManager.instance.playerEffectSource = GetComponents<AudioSource>()[1]; //사운드매니저에 넣어주기 - 지민추가
                                                                                    //cameraManager = GameObject.Find("CMcheaserFreeLook").GetComponent<CameraManager>();
                                                                                    //  isStopMove = false;
        SoundManager.instance.ChangeBGM("IngameBGM");
    }

    private void FixedUpdate()
    {
        //if (!photonView.IsMine & PhotonNetwork.IsConnected)
        //{
        //    return;
        //}

        if (touchField.Pressed == true)
        {
            turn = Camera.transform.rotation.eulerAngles.y;
            isOnceTurn = true;
        }

        if (touchField.Pressed == false)
        {
            CharacterRotate();
        }

        if (!JoyStick.MoveFlag)
        {
            SoundManager.instance.playerWalkSource.Stop();
            musicStart = false;
        }

        if (touchField.Pressed == true && JoyStick.MoveFlag == true)
        {
            turn = Camera.transform.rotation.eulerAngles.y;
            bothPressMove();
        }
    }

    bool musicStart = false;

    void CharacterRotate()
    {
        if (JoyStick.MoveFlag)
        {
            //플레이어 걷기 사운드
            if (!musicStart)
            {
                SoundManager.instance.playerWalkSource.Play();
                musicStart = true;
            }

            if (isOnceTurn)
            {
                transform.rotation = Quaternion.Euler(new Vector3(JoyStick.joyStickPosX, turn, 0));
                isOnceTurn = false;
            }

            //  Debug.Log(Camera.transform.rotation.eulerAngles.x);

            transform.Translate(Vector3.forward * speed * Time.deltaTime);

            tochAngle = Mathf.Atan2(JoyStick.joyStickPosX, JoyStick.joyStickPosY) * Mathf.Rad2Deg;
            tochAngle += turn;
            transform.rotation = Quaternion.Euler(new Vector3(JoyStick.joyStickPosX, tochAngle, 0));
            //Debug.Log(tochAngle);
        }
        else
        {
            SoundManager.instance.playerWalkSource.Stop();
            musicStart = false;
        }
    }

    void bothPressMove()
    {
        //플레이어 걷기 사운드
        if (!musicStart)
        {
            SoundManager.instance.playerWalkSource.Play();
            musicStart = true;
        }

        //transform.rotation = Quaternion.Euler(new Vector3(JoyStick.joyStickPosX, turn, 0));
        tochAngle = Mathf.Atan2(JoyStick.joyStickPosX, JoyStick.joyStickPosY) * Mathf.Rad2Deg;
        tochAngle += turn;
        transform.rotation = Quaternion.Euler(new Vector3(JoyStick.joyStickPosX, tochAngle, 0));
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
