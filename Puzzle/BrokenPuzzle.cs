using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenPuzzle : MonoBehaviour
{
    GameObject[] players;                                                       //플레이어들을  알아야할 오브젝트

    public GameObject puzzle;                                               //퍼즐
    public GameObject bar;                                                    // 맞추어야 할 바                                    
    public GameObject checkBar;                                          //움직이는 체크 바
    public GameObject attack_panel;                                    //퍼즐 실패시 뜨는  panel
    public UILabel HitCount;
    UISlider puzzleslider;

    float _posX = 1.0f;
    float _posY = 0f;

    public int speed = 11;

    public bool isTouch = false;
    public bool okTime = false;
    public bool isfail =false;

    public float second = 2.0f;
    bool _turn = false;

    StatueOn statue;

    private void OnEnable()
    {
        _posX = -200.0f;
        speed = Random.Range(5, 10);
        bar.transform.localPosition = new Vector3(_posX, _posY, 0f);
        isTouch = false;
        isfail = false;
        okTime = false;
        _turn = false;


        // 빨간색 바 위치조정
        float random = Random.Range(0f, 200f);
        checkBar.transform.localPosition = new Vector3(random, _posY, 0f);
        //자리 지정 
    }
    void Update()
    {
        if (isTouch) puzzle.SetActive(false);
        HitCount.text = statue.Hitcount.ToString();
        //좌우 이동을 판단 
        if (bar.transform.localPosition.x <= -200) _turn = true;
            else if (bar.transform.localPosition.x >= 200) _turn = false;

            //좌우로 이동시켜줌
            if (_turn) _posX += speed;
            else _posX -= speed;
            bar.transform.localPosition = new Vector3(_posX, _posY, 0f);

            //충돌을 판정 okTime일때 터치해야됨
            if (checkBar.transform.localPosition.x - 90 < bar.transform.localPosition.x &&
                checkBar.transform.localPosition.x + 90 > bar.transform.localPosition.x)
            {
                okTime = true;
            }
            else
            {
                okTime = false;
            }
            //터치 했을 때 
            if (Input.GetMouseButtonDown(0))
            {
                isTouch = true;
            }
            if(isTouch)
           {
            PuzzlePlaying();
            isTouch = false;
        }
    }

    public void SetStatue(StatueOn _statue)
    {
        statue = _statue;
    }
    public void PuzzlePlaying()
    {
        if (okTime)
        {
            Debug.Log("HitCount : " + statue.Hitcount);
            
            isfail = true;
            statue.Hitcount -= 1;
            if(statue.Hitcount ==0)
            {
               statue.BombEffect();
            }
        }
        else
        {
            //못맞췄으니
            isfail = false;
            attack_panel.SetActive(true);
            PuzzleFail();
        }
        if (isfail)
        {
            puzzle.SetActive(false);
        }
    
    }
    //퍼즐 못 맞출 경우 
   public void PuzzleFail()
    {
        StartCoroutine(FailPanel());
    }
    private IEnumerator FailPanel()
    {
        yield return new WaitForSeconds(second);
        attack_panel.SetActive(false);
        puzzle.SetActive(false);
        Debug.Log("야호");
    }


}

/*
 플레이어가 석상에게 다가온다. 
 망치 버튼 나온다.
 ->플레이어가  석상에 닿았을 때 상호작용이 된다. 
 퍼즐 생성
 퍼즐을 뿌심 
 망치질 시작
     BrokenPuzzle borke_Gargoyle;

    void TriggerEnter(collision other)
    {
                if(other.tag  == "석상")
                {
                if(버튼을 누른다)
                {
                borke_Gargoyle.puzzleStart();
                }
                }
    }



 */

