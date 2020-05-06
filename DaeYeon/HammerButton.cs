using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MyTools = CommonFunction;
public class HammerButton : MonoBehaviour
{
    GameObject _myPlayer;
    PlayerInfo _playerInfo;
    Animator _playerAni;
    PlayerRangeHandler _playerCollision;

    float _aniDuration;

    private void Awake()
    {
        _aniDuration = 3.8f;
    }
    private void Start()
    {
        _myPlayer = MyTools.LoadObject(MyTools.LoadType.PLAYER);
        if (_myPlayer == null)
        {
            MyTools.LoadObjectMessage(false, "HammerButton.cs -> myPlayer");
        }
        _playerInfo = _myPlayer.GetComponent<PlayerInfo>();
        _playerAni = _myPlayer.GetComponent<Animator>();

        _playerCollision = _myPlayer.GetComponent<PlayerRangeHandler>();
    }

    /// <summary>
    /// 망치 버튼을 누르면 실행하는 함수
    /// </summary>
    public void OnHammerButton()
    {
        // 아무 액션도 재생하고 있지 않을 때 해머를 휘두른다.
        if(_playerInfo.isAttacking == false && 
            _playerInfo.isDoingAction == false)
        {
            _playerInfo.sledgeHammer.SetActive(true);
            _playerInfo.isStopMoving = true;
            _playerInfo.isAttacking = true;
            _playerInfo.isDoingAction = true;
            _playerAni.SetBool("IsHammer", true);

            StartCoroutine(HammerDuration());

            // 해머를 휘두를 때 target이 석상이라면 석상 상태를 풀어준다.
            if(_playerCollision.target != null)
            {
                ReviveStatuePlayer();
            }
        }
    }

    IEnumerator HammerDuration()
    {
        yield return new WaitForSeconds(_aniDuration);
        _playerInfo.sledgeHammer.SetActive(false);
        _playerInfo.isStopMoving = false;
        _playerInfo.isAttacking = false;
        _playerInfo.isDoingAction = false;
        _playerAni.SetBool("IsHammer", false);
    }

    void ReviveStatuePlayer()
    {
        if(_playerCollision.targetInfo.isStatue)
        {
            _playerCollision.targetInfo.HitStatue();
        }
    }
}
