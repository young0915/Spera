using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill3Fill : MonoBehaviour
{
    public Transform LodingBar;
    public float currentAmount { get; set; }
    public float speed { get; set; }
    public bool isFill { get; set; }

    private void Update()
    {
        if (isFill == true)
        {
            if (currentAmount < 100)
            {
                currentAmount += speed * Time.smoothDeltaTime;
            }
            else
            {
                currentAmount = 0;
            }
            LodingBar.GetComponent<UISprite>().fillAmount = currentAmount / 20;
        }
    }

}
