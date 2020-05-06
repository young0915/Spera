using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotalCollider : MonoBehaviour
{
    public bool isConnectPotal { get; set; }
    public Transform potalTrans;
    private CheaserHitBButton potalActive;

    private void Awake()
    {
        potalActive = GameObject.FindWithTag("CheaserAttackButton").GetComponent<CheaserHitBButton>();
    }

    private void Update()
    {
        this.transform.position = potalTrans.position;
        this.transform.rotation = potalTrans.rotation;

        if (potalActive.isPotal == false)
        {
            this.GetComponent<BoxCollider>().enabled = false;
        }
        else if (potalActive.isPotal == true)
        {
            this.GetComponent<BoxCollider>().enabled = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {    
        if(other.tag == "Cheaser")
        {
            Debug.Log("포탈과 충돌했다."); ;
            isConnectPotal = true;
        }
    }
}
