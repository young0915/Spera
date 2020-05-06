using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueOn : MonoBehaviour
{
    public GameObject Puzzle;
    public GameObject Statue;
    public ParticleSystem Bombeffect;
    public GameObject Rock;
 
    public  int Hitcount;                                                                       //플레이어가 망치에 맞은 수 8번 정도 맞아야지 뿌셔짐

    private void Start()
    {
        Puzzle = GameObject.Find("NGUI").transform.Find("Camera").transform.Find("PuzzlePanel").gameObject;
        Hitcount = 8;                                                                 // 8번 정도 맞아야지 뿌셔짐
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Puzzle.SetActive(true);
            Puzzle.GetComponent<BrokenPuzzle>().SetStatue(this);
        }
    }
   
    public void BombEffect()
    {
        Debug.Log("폭파");
        Puzzle.SetActive(false);
        Bombeffect.Play(true);
        
        StartCoroutine(DestoryStatue());
    }

    IEnumerator DestoryStatue()
    {
        Rock.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        Statue.SetActive(false);
    }
}
