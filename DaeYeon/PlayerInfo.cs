using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MyTools = CommonFunction;
/// <summary>
/// 플레이어의 정보를 가지고 있는 클래스다.
/// 플레이어의 콜리전 처리도 담당한다.
/// </summary>
public class PlayerInfo : MonoBehaviour
{
    public enum ObjType
    {
        EMPTY,
        SHOP
    }
    ObjType _objType;

    public float _shieldDuration;                  // 실드 파티클 유지시간
    public float _healDuration;                    // 힐 파티클 유지시간

    public int playerHp { get; set; }              // 플레이어의 Hp
    public int statueTouch { get; set; }           // 석상을 때릴 때 쓰는 카운트

    public bool isIam { get; set; }                // 현재 오리지널 캐릭터인지 식별하는 변수
    public bool isStatue { get; set; }             // 플레이어가 석상이 되었는지 식별하는 변수
    public bool isShop { get; private set; }       // 상점 Ui를 열기위한 변수
    public bool isDeath { get; set; }              // 죽는지 알기 위한 변수

    public bool isCrouched { get; set; }           // 플레이어가 살금살금 움직일지 결정하는 변수
    public bool isStopMoving { get; set; }         // 플레이어가 움직일지, 말지 결정하는 변수

    public bool isDoingAction { get; set; }         // 플레이어가 애니메이션을 재생중인지 식별하는 변수
    public bool isAttacking { get; set; }           // 플레이어가 해머를 쓰는지 알기 위한 변수
    public bool isFlashlight { get; set; }          // 손전등 사용중인지 알기 위한 변수

    public bool isHealing { get; private set; }     // 메디킷 사용중인지 알기 위한 변수
    public bool isShielding { get; private set; }   // 실드 사용중인지 알기 위한 변수

    GameObject _playerParticle;
    ParticleSystem _playerBleeding;
    ParticleSystem _playerShield;
    ParticleSystem _playerHeal;

    public GameObject playerHand { get; private set; }                 // 플레이어의 손
    public GameObject flashLight { get; private set; }                // 플레이어의 손전등
    public GameObject sledgeHammer { get; private set; }               // 플레이어의 망치

    private void Awake()
    {
        _objType = ObjType.EMPTY;

        isIam = true;
        isStatue = false;
        isShop = false;
        isDeath = false;

        isCrouched = false;
        isStopMoving = false;

        isDoingAction = false;
        isAttacking = false;

        isHealing = false;
        isShielding = false;

        playerHp = 3;
        statueTouch = 3;
        _shieldDuration = 5.0f;
        _healDuration = 1.5f;

        // 플레이어가 사용할 파티클을 찾아온다.
        _playerParticle = transform.Find("PlayerParticle").gameObject;
        _playerBleeding = _playerParticle.transform.Find("Bleeding").GetComponent<ParticleSystem>();
        _playerShield = _playerParticle.transform.Find("Shield").GetComponent<ParticleSystem>();
        _playerHeal = _playerParticle.transform.Find("HealStream").GetComponent<ParticleSystem>();

        // 플레이어가 들고 있는 오브젝트를 찾아온다.
        playerHand = MyTools.SearchObjectWithTag(transform.gameObject, "PlayerHand");
        if (playerHand == null)
        {
            MyTools.LoadObjectMessage(false, "CharactorAnimationV2.cs -> Player's Hand Object");
        }
        else
        {
            sledgeHammer = playerHand.transform.Find("sledgeHammer").gameObject;
            flashLight = playerHand.transform.Find("FlashLight").gameObject;
        }
        if (sledgeHammer == null)
        {
            MyTools.LoadObjectMessage(false, "CharactorAnimationV2.cs -> SledgeHammer Object");
        }
        if (flashLight == null)
        {
            MyTools.LoadObjectMessage(false, "CharactorAnimationV2.cs -> FlashLight Object");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        SetupObjTypeFromCollider(other.tag);
    }

    /// <summary>
    /// Collider에 부딪힌 상대가 누구인지 판단해서 처리하는 함수
    /// </summary>
    /// <param name="colliderTag">충돌한 상대방의 태그</param>
    void SetupObjTypeFromCollider(string colliderTag)
    {
        if (string.Compare(colliderTag, "Shop") == 0)
        {
            //Debug.Log("Collider Shop");
            _objType = ObjType.SHOP;
            isShop = true;
        }
    }

    /// <summary>
    /// 상점 UI를 여는 함수
    /// </summary>
    public void OpenShopUi()
    {
        //Debug.Log("PlayerInfo.cs -> OpenShop");
        GameObject objShop = MyTools.LoadObject(MyTools.LoadType.SHOP_PANEL);
        objShop.GetComponent<Shop>().OpenShop();
    }

    public void HitPlayer()
    {
        // 플레이어가 쉴드를 사용중이라면 피가 닳지 않는다.
        if (isShielding) { }
        else
        {
            playerHp--;             // 플레이어 체력 깎기
            _playerBleeding.Play(); // 피흘리는거 실행
        }
    }
    public void HealPlayer()
    {
        playerHp++;
    }// 서영이가 고칠 곳?

    public void OnShield()
    {
        if (_playerShield == null)
        {
            MyTools.LoadObjectMessage(false, "PlayerInfo.cs -> _playerShield");
        }
        else
        {
            isShielding = true;     // 쉴드 작동중
            _playerShield.Play();   // 보호막 파티클 재생
            StartCoroutine(ShieldDuration());
        }
    }
    IEnumerator ShieldDuration()
    {
        yield return new WaitForSeconds(_shieldDuration);
        isShielding = false;
        _playerShield.Stop();
    }

    public void OnHealkit()
    {
        if (_playerHeal == null)
        {
            MyTools.LoadObjectMessage(false, "PlayerInfo.cs -> _playerHeal");
        }
        else
        {
            HealPlayer();
            isHealing = true;     // 힐 하는 중
            _playerHeal.Play();   // 힐 파티클 재생
            StartCoroutine(HealDuration());
        }
    }
    IEnumerator HealDuration()
    {
        yield return new WaitForSeconds(_healDuration);
        isHealing = false;
        _playerHeal.Stop();
    }

    // 석상 때리는 부분
    public void HitStatue()
    {
        if (statueTouch > 0)
        {
            statueTouch--;
        }
        else
        {
            statueTouch = 3;
            HealPlayer();
            OnShield();
        }
    }
}
