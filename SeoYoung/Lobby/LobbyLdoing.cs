﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyLdoing : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(LobbyLoding());  
    }


    IEnumerator LobbyLoding()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("LodingScene");
        SoundManager.instance.ChangeBGM("LodingBGM");
        operation.allowSceneActivation = false;
        yield return new WaitForSeconds(2.0f);
        operation.allowSceneActivation = true;

    }



}
