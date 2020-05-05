using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
/*
 변수 앞에 s는 scene의 약자임
 */


public class LodingBar : MonoBehaviourPunCallbacks
{


    public UISlider s_SliderLoding = null;
    public UILabel s_LabelLoding;
    void Start()
    {
      
        StartCoroutine(LoadScene());
    }
  
    IEnumerator LoadScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("cityJack");
        operation.allowSceneActivation = false;

        // slider의 valiue를 매 프레임 증가
        while (!operation.isDone)                                                                                    //isDone은 다운로드가 완료되었는지 확인으로 사용
        {
            //yield return  new WaitForSeconds(15);                                                                                                //update()가 실행될 때까지 기다림. (null)을 양보 반환했던 코루틴이 이어서 진행됨.
            yield return  null;                                                                                                //update()가 실행될 때까지 기다림. (null)을 양보 반환했던 코루틴이 이어서 진행됨.

            if (s_SliderLoding.value < 0.9f)
            {
                s_SliderLoding.value = Mathf.MoveTowards(s_SliderLoding.value, 0.9f, Time.deltaTime);
            }
            if (s_SliderLoding.value >= 0.9f)
            {
                s_SliderLoding.value = Mathf.MoveTowards(s_SliderLoding.value, 1f, Time.deltaTime);
            }
            if (s_SliderLoding.value >= 1f && operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
                SoundManager.instance.ChangeBGM("IngameBGM");
       

            }
        }

    }
}
/*
 비동기 로드는 Scene을 불러올 때 멈추지 않고 다른 작업을 할 수 있습니다.
    LoadScene()로 Scene을 불러오면 완료될 때까지 다른 작업을 수행하지 않습니다.
 */


/*
 유니티에서 화면의 변화를 일으키기 위해서는 update()함수 내에서 작업을 하게 된다. 
Update 함수는 매 프레임을 그릴때마다 호출되며 60fps의 경우라면 초당 60번의 update()함수에서 발생한다.
하지만 다수의 프레임을 오고가며 어떤 작업을 한다면 프레임 드랍이 발생할 수 있다. 그래서 코루틴을 사용함
  */
