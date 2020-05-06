using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionButton : MonoBehaviour
{
    CharactorAnimationV2 _aniScript;    // 플레이어 애니메이터
    PlayerInfo _playerInfo;             // 플레이어 정보
    private void Start()
    {
        _aniScript = GetComponentInParent<CharactorAnimationV2>();
        _playerInfo = GetComponentInParent<PlayerInfo>();
    }
    /// <summary>
    /// 상호작용 버튼과 연결된 함수
    /// </summary>
    public void OnInteraction()
    {
        if (_playerInfo.isShop)
        {
            _playerInfo.OpenShopUi();
        }// if: 현재 상점 근처라면 상점을 연다
    }
}
