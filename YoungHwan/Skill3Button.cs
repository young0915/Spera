using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill3Button : MonoBehaviour
{
    private ChSkillButton1 chButton1;
    private ChSkillButton2 chButton2;

    public bool isSummonCrow { get; set; }
    public bool isEnable { get; set; }

    private void Awake()
    {
        chButton1 = GameObject.FindWithTag("CheaserSkillUiButton").GetComponent<ChSkillButton1>();
        chButton2 = GameObject.FindWithTag("CheaserSkillUiButton2").GetComponent<ChSkillButton2>();
    }

    public void SummonCrow()
    {
    
        if(isEnable == false)
        {
            if (isSummonCrow == false)
            {
                isSummonCrow = true;
            }
        }
    }
}
