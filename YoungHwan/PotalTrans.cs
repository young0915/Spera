using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotalTrans : MonoBehaviour
{
    private CheaserHitBButton potalButton;
    private Transform cheaserTrans;
    private Transform originTrans;
    private Transform skill3Trans;

    private bool isTransChange = false;

    private void Awake()
    {
        cheaserTrans = GameObject.FindWithTag("Cheaser").GetComponent<Transform>();
        potalButton = GameObject.FindWithTag("CheaserAttackButton").GetComponent<CheaserHitBButton>();
    }

    private void FixedUpdate()
    {
        if(potalButton.isPotal == false)
        {
            originTrans.position = cheaserTrans.position;
        }

        if(potalButton.isPotal == true && isTransChange == false)
        {
            skill3Trans.position = originTrans.position;
            this.transform.position = skill3Trans.position;
            isTransChange = true;
        }

        
    }
}
