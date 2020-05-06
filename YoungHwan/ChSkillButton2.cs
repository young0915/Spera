using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChSkillButton2 : MonoBehaviour
{
    private ChSkillButton1 chButton1;
    private Skill3Button chButton3;

   public bool isHidePlayerShow { get; set; }
    public bool isEnableButton2 { get; set; }

    private void Awake()
    {
        chButton1 = GameObject.FindWithTag("CheaserSkillUiButton").GetComponent<ChSkillButton1>();
        chButton3 = GameObject.FindWithTag("CheaserSkillUiButton3").GetComponent<Skill3Button>();
    }

    public void Skill2Button()
    {
        if(chButton1.isFlashing == false && chButton3.isSummonCrow  ==  false )
        {
            if(chButton1.isEnableButton1 == true) // 스킬1 쿨타임 대기
            {
                if(isEnableButton2 == false)  // 쿨타임 BOOL 변수
                {
                        if(isHidePlayerShow == false)
                        {
                            isHidePlayerShow = true;
                        }
                }
            }
            else if(chButton1.isEnableButton1 == false && isEnableButton2 == false)
            {
                StartCoroutine("skillButton1wait");

            }
        }
    }

    IEnumerator skillButton1wait()
    {
        yield return new WaitForSeconds(0.3f);
        if (isHidePlayerShow == false)
        {
            isHidePlayerShow = true;
        }
    }
}
