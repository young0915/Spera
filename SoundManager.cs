using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //소스
    public AudioSource BGMSource;
    public AudioSource UISource;
    public AudioSource playerWalkSource;
    public AudioSource playerEffectSource;

    //클립
    public AudioClip[] BGMs;
    public AudioClip[] effects;

    //옵션 조절바
    public UISlider optionMusicSlider;

    #region 싱글톤
    // 싱글톤 접근용 프로퍼티
    public static SoundManager instance
    {
        get
        {
            if (m_instance == null) // 싱글톤 변수에 오브젝트가 할당되지 않았다면
            {
                // 씬에서 게임매니저 오브젝트를 찾아서 할당
                m_instance = FindObjectOfType<SoundManager>();
            }

            return m_instance;
        }
    }

    private static SoundManager m_instance;
    #endregion
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance != this)
        {
            Destroy(gameObject);
        }
        //오디오 소스 자동으로 가져오기
        BGMSource = transform.GetChild(0).GetComponent<AudioSource>();
        UISource = transform.GetChild(1).GetComponent<AudioSource>();

        optionMusicSlider = GameObject.Find("UI Root").transform.Find("Camera").transform.Find("OptionPanel").transform.Find("OptionMenuWindow")
            .transform.Find("SoundSlider").gameObject.GetComponent<UISlider>();

        EventDelegate a = new EventDelegate(this, "ControlAllMusic");
        optionMusicSlider.onChange.Add(a);
        ChangeBGM("LobbyBGM");


        //씬이 변경되어도 유지됨
    }

    public void AllStop()
    {
        BGMSource.Stop();
        UISource.Stop();
        playerWalkSource.Stop();
        playerEffectSource.Stop();
    }

    public void ControlAllMusic()
    {
        //볼륨조절하는 함수
        BGMSource.volume = optionMusicSlider.value;
       // UISource.volume = optionMusicSlider.value;
    }

    public void ChangeBGM(string BGMname)
    {
        BGMSource.Stop();
        //기존 BGM을 멈추고 키값으로 받은 클립을 재생
        for (int i = 0; i < BGMs.Length; i++)
        {
            if (BGMs[i].name == BGMname)
            {
                BGMSource.clip = BGMs[i];
                break;
            }
        }
        BGMSource.Play();
    }

    public void PlayerEffectSound(string effectName, float volume = 1)
    {
        //키값으로 받은 이름을 클립배열에서 찾아서 실행
        for (int i = 0; i < effects.Length; i++)
        {
            if (effects[i].name == effectName)
            {
                playerEffectSource.PlayOneShot(effects[i], volume);
                break;
            }
            if (effects[i].name == "walking")
            {
                //playerSource.pitch = 1.6f;
            }
        }
    }
    public void UIEffectSound(string effectName, float volume = 1)
    {
        //키값으로 받은 이름을 클립배열에서 찾아서 실행
        for (int i = 0; i < effects.Length; i++)
        {
            if (effects[i].name == effectName)
            {
                UISource.PlayOneShot(effects[i], volume);
                break;
            }
        }
    }
}