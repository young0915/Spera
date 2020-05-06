﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MyTools = CommonFunction;
public class ItemButtonHandler : MonoBehaviour
{
    public enum ItemType
    {
        ITM_EMPTY = -1,
        ITM_SPEED_UP = 0,
        ITM_HEAL_KIT,
        ITM_FLASHLIGHT,
        ITM_SHIELD
    }

    public ItemType itemType { get; set; }
    public GameObject item;
    public GameObject speeditem;
    public GameObject HealKititem;
    public GameObject FlashLightitem;

    GameObject _globalItems;
    SpeedUp _speedUp;
    HealKit _healKit;
    FlashLight _flashLight;

    GameObject _myPlayer;               // 플레이어 오브젝트
    PlayerInfo _playerInfo;             // 플레이어의 정보
    PlayerRangeHandler _playerRange;    // 플레이어의 콜라이더
    Animator _playerAni;    // 플레이어의 애니메이터


    void Start()
    {
        _myPlayer = MyTools.LoadObject(MyTools.LoadType.PLAYER);
        _playerRange = _myPlayer.transform.Find("InteractCollider").
            GetComponent<PlayerRangeHandler>();
        _playerAni = _myPlayer.GetComponent<Animator>();

        _playerInfo = _myPlayer.GetComponent<PlayerInfo>();

        // Items
        itemType = ItemType.ITM_EMPTY;
    //    itemType = ItemType.ITM_SPEED_UP;
        _globalItems = MyTools.LoadObject(MyTools.LoadType.GLOBAL_ITEMS);
        _speedUp = _globalItems.transform.Find("ItemSpeedUp").GetComponent<SpeedUp>();
        _healKit = _globalItems.transform.Find("ItemHealKit").GetComponent<HealKit>();
        _flashLight = _globalItems.transform.Find("ItemFlashLight").GetComponent<FlashLight>();
    }

    /// <summary>
    /// 아이템 버튼을 클릭했을 때 실행하는 함수다.
    /// </summary>
    public void OnItemButtonClick()
    {
        // 직접 사용할지 말지 정한다.
        //  bool useSelf = _playerRange.isSelf;
        // 현재 아이템창에 어떤 아이템이 올라왔는지 체크하고, 아이템을 사용한다.
        switch ((ItemType)(itemType))
        {
            case ItemType.ITM_SPEED_UP:
             //   _speedUp.Use(_myPlayer);
                speeditem.SetActive(false);
                item.SetActive(true);
               itemType = ItemType.ITM_EMPTY;
                break;
            case ItemType.ITM_HEAL_KIT:
                if (_playerInfo.isHealing == false)
                {
                    if (_playerRange.isSelf)
                    {
                        _playerInfo.OnHealkit();
                    }
                    else
                    {
                        _playerRange.targetInfo.OnHealkit();
                    }
                    HealKititem.SetActive(false);
                    item.SetActive(true);
                    itemType = ItemType.ITM_EMPTY;
                }
                break;
            case ItemType.ITM_FLASHLIGHT:
                float flashDuration = 7.0f;
                if (_playerInfo.isFlashlight == false &&
                    _playerInfo.isDoingAction == false)
                {
                    _playerInfo.isFlashlight = true;
                    _playerInfo.isDoingAction = true;
                    _playerInfo.flashLight.SetActive(true);
                    _playerAni.SetBool("IsFlash", true);
                    // 일정 시간 후에 손전등을 끈다.
                    StartCoroutine(FlashDuration(flashDuration));

                    // 아이템 창에서 아이템 빼는 코드
                    FlashLightitem.SetActive(false);
                    item.SetActive(true);
                    itemType = ItemType.ITM_EMPTY;

                }
                break;
            case ItemType.ITM_SHIELD:
                if (_playerInfo.isShielding == false)
                {
                    _playerInfo.OnShield();
                }
                itemType = ItemType.ITM_EMPTY;
                //Debug.Log("Is Player Shield Now: " + _playerInfo.isShielding);
                break;
            default:
                break;
        }
    }

    IEnumerator FlashDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        _playerInfo.flashLight.SetActive(false);
        _playerInfo.isFlashlight = false;
        _playerInfo.isDoingAction = false;
        _playerAni.SetBool("IsFlash", false);
    }
}
