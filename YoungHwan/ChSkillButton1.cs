using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChSkillButton1 : MonoBehaviour
{
    private ChSkillButton2 chButton2;
    private Skill3Button chButton3;

    public bool isFlashing { get; set; }
    public bool isEnableButton1 { get; set; }

    private void Awake()
    {
        chButton2 = GameObject.FindWithTag("CheaserSkillUiButton2").GetComponent<ChSkillButton2>();
        chButton3 = GameObject.FindWithTag("CheaserSkillUiButton3").GetComponent<Skill3Button>();
    }

    public void OnClickFlashingButton()
    {
        if(chButton2.isHidePlayerShow != true && chButton3.isSummonCrow != true)
        {
             if(isEnableButton1 == false)
             {
                 if (isFlashing == false)
                 {
                     isFlashing = true;
                 }
             }
        }
    }
}
