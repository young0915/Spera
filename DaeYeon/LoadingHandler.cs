using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class LoadingHandler : MonoBehaviour
{
    public UISlider slider;
    UISprite _frontSkull;
    float _speed;
    private void Awake()
    {
        // 로딩 스컬을 찾아온다
        string path = "Anchor/LoadingSkullFront";
        _frontSkull = transform.Find(path).GetComponent<UISprite>();

        _speed = 0.5f;
        StartCoroutine(StartLoading());
    }


    IEnumerator StartLoading()
    {
        //cityJack을 비동기 로드한다.
       AsyncOperation operation = SceneManager.LoadSceneAsync("LobbyScene");
        operation.allowSceneActivation = false;
        // 로딩 프로그레스를 이미지의 fillAmount와 맞춘다.
        Debug.Log("Loading: " + operation.progress);

        while (!operation.isDone)
        {
            //_frontSkull.fillAmount = operation.progress;
            yield return null;
            //if (_frontSkull.fillAmount >= 1.0f)
            //{
            //    System.Threading.Thread.Sleep(5000);
            //    operation.allowSceneActivation = true;

            //}

            if (slider.value < 0.9f)
            {
                Debug.Log("HiHi");
                slider.value = Mathf.MoveTowards(slider.value, 0.9f, Time.deltaTime);
            }
            if (slider.value >= 0.9f)
            {
                slider.value = Mathf.MoveTowards(slider.value, 1f, Time.deltaTime);
            }
            if (slider.value >= 1f && operation.progress >= 0.9f)
            {
                //yield return new WaitForSeconds(10.0f);
                //if(PhotonNetwork.IsMasterClient)
                //{
                //operation.allowSceneActivation = true;
                
                PhotonNetwork.LoadLevel("cityJack");
                //}
            }
            _frontSkull.fillAmount = slider.value;

            //operation.allowSceneActivation = true;
        }
        //StartCoroutine(StartLoading());
    }
}
