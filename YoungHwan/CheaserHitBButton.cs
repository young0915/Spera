using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheaserHitBButton : MonoBehaviour
{
    public bool isAttack { get; set; }
    public bool isPotal { get; set; }

    private UILabel uiLabel;

    private void Awake()
    {
        uiLabel = GetComponentInChildren<UILabel>();
    }
    public void OnClickHit()
    {
        if(uiLabel.text == "Hit")
        {
            isAttack = true;
        }

        if(uiLabel.text == "Potal")
        {
            isPotal = true;
        }
    }
}

