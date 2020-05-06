using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheaserAttackMode : MonoBehaviour
{
    private CheaserHitBButton cheaserAttackButton;
    private GameObject chSkillButton1;
    private ChSkillButton2 chSkillButton2;
    private Skill3Button chSkillButton3;
    private TweenAlpha tweenAlpha;

    // 스킬중에 못움직이게 하기
    private NGUIJoystick joyStick;

    private CheaserMoveCamera characterSpeed;
    private CheaserAnimationPoton cheaserAni;
    private Outline playerOutLine;

    public AudioClip flasingAudioClip;
    public float flashingAddSpeed = 0.1f;
    public Transform cheaserTrans;

    // 포탈 파티클
    public ParticleSystem potalPaticle;

    // crow2가 player을 찾으면
    private CrowNavi crow2Navi;
    // Attack ui 라벨을 바꿔준다.
    private UILabel attackUiLabel;
    // potal의 위치를 잡아주는 변수
    private Vector3 oriVecPotal;
    private bool isStopPos = false;
    // potal의 충돌체 확인
    private PotalCollider potalCollider;

    // crow의 들의 위치 받아오기
    public Transform crowLeft;
    public Transform crowRight;
    private bool isPotalInto = false;

    [SerializeField]
    private float skillTime = 15f;
    private float skill2WaitTime = 6f;
    private float movingDistance = 0.01f;

    RaycastHit rayHit;
    Ray cheaserRay;     // Ray캐스트를 사용하여 앞에 장애물을 파악한다.
    private float distance = 1.8f;
    private bool isDontGo = false;

    // 스킬3 까마귀의 스폰 타임 지정
    public bool isSpawnCrow { get; set; }
    private float CrowSpawnTime = 100.0f;
    public GameObject tweenTexture;

    // 애니메이션 정보 가져오기
    private Animator animator;

    // 스킬 웨이트 타임
    public float skill1WaitTime = 60.0f;

    void Awake()
    {
        chSkillButton1 = GameObject.FindWithTag("CheaserSkillUiButton");
        chSkillButton2 = GameObject.FindWithTag("CheaserSkillUiButton2").GetComponent<ChSkillButton2>();
        chSkillButton3 = GameObject.FindWithTag("CheaserSkillUiButton3").GetComponent<Skill3Button>();

        joyStick = GameObject.Find("Joystick").GetComponent<NGUIJoystick>();

        characterSpeed = GameObject.Find("Cheaser").GetComponent<CheaserMoveCamera>();
        playerOutLine = GameObject.FindGameObjectWithTag("Player").GetComponent<Outline>();
        cheaserAttackButton = GameObject.FindWithTag("CheaserAttackButton").GetComponent<CheaserHitBButton>();
        potalCollider = GameObject.FindWithTag("PotalCollision").GetComponent<PotalCollider>();

        cheaserAni = this.GetComponent<CheaserAnimationPoton>();
        cheaserRay = new Ray();

        crow2Navi = GameObject.FindWithTag("Crow2").GetComponent<CrowNavi>();
        attackUiLabel = GameObject.FindWithTag("CheaserAttackButton").GetComponentInChildren<UILabel>();

        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        potalPaticle.Stop(true);
    }

    void FixedUpdate()
    {
        cheaserRay.origin = cheaserTrans.position;
        cheaserRay.direction = cheaserTrans.transform.forward;
        Debug.DrawRay(cheaserRay.origin, cheaserRay.direction * distance, Color.green);

        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, transform.right);


        for (int i = 0; i > hits.Length; i++)
        {
            rayHit = hits[i];
            Debug.Log("부딪혔다.");

            isDontGo = true;
        }
        Skill1();
        Skill2();
        Skill3();

        // 버튼 웨이트 타임
        if (chSkillButton1.GetComponent<ChSkillButton1>().isEnableButton1 == true)
        {
            StartCoroutine("Skill1WaitTime");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // Debug.Log("플레이어 때리고 있음");
        }
    }

    private void Skill1()
    {
        // skill1구현부분
        if (chSkillButton1.GetComponent<ChSkillButton1>().isFlashing == true)
        {
            if (isDontGo == false)
            {
                movingDistance += flashingAddSpeed;
                //characterSpeed.speed = flashingAddSpeed;
                transform.Translate(0, 0, movingDistance * Time.deltaTime);
                chSkillButton1.GetComponentInChildren<TestButton>().currentAmount = 0f;
            }
            joyStick.MoveFlag = false;
            StartCoroutine("SkillSpeedUp");
        }
        if (Physics.Raycast(cheaserRay.origin, cheaserRay.direction, out rayHit, distance))
        {
            if (rayHit.collider.gameObject.name != "Skele")
            {
                Debug.Log(rayHit.collider.name);
                isDontGo = true;
                cheaserAni.paticleSkill1.Stop(true);
            }
        }
        // skill1구현부분
    }

    private void Skill2()
    {
        // skill2구현부분
        if (cheaserAni.isRealShowLine == true)
        {
            playerOutLine.enabled = true;
        }
        if (playerOutLine.enabled == true)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("skill2"))
            {
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
                {
                    //joyStick.MoveFlag = true;
                    chSkillButton2.isHidePlayerShow = false;
                }
                else
                {
                    joyStick.MoveFlag = false;
                }
            }
            StartCoroutine("Skill2Invisibility");
        }
    }
    bool musicStart = false;
    private void Skill3()
    {

        if (crow2Navi.isFind == true)
        {
            attackUiLabel.text = "Potal";
            if (!musicStart)
            {
                SoundManager.instance.PlayerEffectSound("crow");
            }
            musicStart = true;
        }

        if (cheaserAttackButton.isPotal == true)
        {
            if (isStopPos == false)
            {
                joyStick.MoveFlag = false;
                oriVecPotal = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z + 5f);
                isStopPos = true;
            }
            potalPaticle.transform.position = oriVecPotal;
            StartCoroutine("PotalMove");
        }

        // 만약 포탈과 플레이어가 충돌한다면
        if (potalCollider.isConnectPotal == true)
        {
            // 추적자의 위치를 Crow2위치로 바꿔준다.
            if (isPotalInto == false)
            {
                TweenAlpha.Begin(tweenTexture, 2f, 1);
                Vector3 crowTrans = new Vector3(crowRight.transform.position.x, this.transform.position.y, crowRight.transform.position.z);
                this.transform.position = crowTrans;
                isPotalInto = true;
            }
            StartCoroutine("PotalConnect");
        }
    }

    IEnumerator SkillSpeedUp() // Flashing 스킬 
    {
        yield return new WaitForSeconds(skillTime);
        chSkillButton1.GetComponent<ChSkillButton1>().isFlashing = false;
        characterSpeed.speed = 5.0f;
        isDontGo = false;
        chSkillButton1.GetComponent<ChSkillButton1>().isEnableButton1 = true;
    }

    IEnumerator Skill2Invisibility() // 플레이어 투명화
    {
        yield return new WaitForSeconds(skill2WaitTime);
        playerOutLine.enabled = false;
        if (chSkillButton2.isHidePlayerShow == false)
        {
            Debug.Log("isHidePlayerShow == false");
        }
        cheaserAni.isRealShowLine = false;
    }

    IEnumerator PotalMove()
    {
        potalPaticle.Play(true);
        yield return new WaitForSeconds(15.0f);
        potalPaticle.Stop(true);
        isStopPos = false;
        cheaserAttackButton.isPotal = false;
    }

    IEnumerator PotalConnect()
    {
        yield return new WaitForSeconds(3f);
        TweenAlpha.Begin(tweenTexture, 2f, 0);
        isPotalInto = false;
    }

    // 스킬 웨이트 타임
    IEnumerator Skill1WaitTime()
    {
        chSkillButton1.GetComponentInChildren<TestButton>().speed = skill1WaitTime / 60f;
        chSkillButton1.GetComponentInChildren<TestButton>().isFill = true;
        yield return new WaitForSecondsRealtime(skill1WaitTime);
        chSkillButton1.GetComponent<ChSkillButton1>().isEnableButton1 = false;
        chSkillButton1.GetComponentInChildren<TestButton>().isFill = false;
    }

}
