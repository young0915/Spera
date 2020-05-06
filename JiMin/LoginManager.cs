﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoginManager : MonoBehaviour
{
    #region 싱글톤
    public static LoginManager instance
    {
        get
        {
            if (m_instance == null) // 싱글톤 변수에 오브젝트가 할당되지 않았다면
            {
                // 씬에서 게임매니저 오브젝트를 찾아서 할당
                m_instance = FindObjectOfType<LoginManager>();
            }

            return m_instance;
        }
    }

    private static LoginManager m_instance;

    #endregion

    //유저의 이름과 총게임을 몇판했는지 담을 변수
    public string userID;
    public int lastRound = 0;

    private bool isLogin;
    private LoginSystem loginSystem;

    private void Awake()
    {

        if (instance != this)
        {
            Destroy(gameObject);
            //게임이 끝나 다시 로비신으로 돌아왔을경우
            //isLogin은 true가 됨
            isLogin = true;
        }

        if (isLogin)
        {
            //isLogin이 true일때 로그인 부분을 스킵하고 바로 로비로 넘어가게함
            loginSystem = GameObject.Find("LoginPanel").GetComponent<LoginSystem>();
            loginSystem.StartCoroutine(loginSystem.GoCamera());
            loginSystem.MatchingBtn.SetActive(true);
            loginSystem.LoginPanel.SetActive(false);
        }
        //실행했는데 디렉토리에 json폴더가 없다면(최초실행시)
        if (!Directory.Exists(Application.persistentDataPath + "/Json/"))
        {
            //디렉토리에 폴더 생성해줌
            Directory.CreateDirectory(Application.persistentDataPath + "/Json/");
            Directory.CreateDirectory(Application.persistentDataPath + "/Json/Item/");
            Directory.CreateDirectory(Application.persistentDataPath + "/Json/InGameData/");
        }
        else
        {
            //json폴더가 있다면(재시작시) 승패기록을 초기화해줌
            if (isLogin) return;

            ClearJsonFolder("InGameData");
        }
        DontDestroyOnLoad(gameObject);
    }
    public static void ClearJsonFolder(string folderName)
    {
        ///Json/folderName 안의 모든 파일의 이름을 가져옴
        string[] allFile = Directory.GetFiles(Application.persistentDataPath + "/Json/" + folderName);

        for (int i = 0; i < allFile.Length; i++)
        {
            //다 지움
            File.Delete(allFile[i]);
        }
    }
}
