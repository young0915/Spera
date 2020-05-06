using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueSpwan : MonoBehaviour
{
    private Transform[] wayPoints;
    private void Start()
    {
        wayPoints = transform.Find("StatueWayPoints").GetComponentsInChildren<Transform>();
        int random = Random.Range(0,wayPoints.Length-1);

        transform.position = wayPoints[random].position;
    }

}
