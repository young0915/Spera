using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // AI, 내비게이션 시스템 관련 코드 가져오는거


// 까마귀 AI를 구현
public class CrowNavi : MonoBehaviour
{
    public enum CrowStatus
    {
        WAIT , MARKING 
    }
    // 스킬을 쓰는중이 아니면 그냥 추적자 옆에 붙어 다닌다.
    private CheaserAnimationPoton cheaserAniScript;
    private CheaserAttackMode cheaserAttackMode;
    // 추적자 옆에 있는 좌표가 origin 좌표가 될것
    private Transform originTrans;
    public Transform cheaserCrow2trans;

    Rigidbody myRigid;
    [SerializeField] private float moveSpeed = 15.0f;
    
    NavMeshAgent agent;

    [SerializeField] private Transform[] tf_Destination;
    private Vector3[] wayPoints;
    private Vector3 originPos;

    // crow의 상태값
    private CrowStatus crowStatus;
    public bool isFind { get; set; }


    private void Start()
    {
        myRigid = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();

        originTrans = GetComponent<Transform>();
        originTrans = cheaserCrow2trans;

        cheaserAniScript = GameObject.FindWithTag("Cheaser").GetComponent<CheaserAnimationPoton>();
        cheaserAttackMode = GameObject.FindWithTag("Cheaser").GetComponent<CheaserAttackMode>();
        agent.enabled = false;
        // +1 은 오리지널 포지션 값
        wayPoints = new Vector3[tf_Destination.Length + 1];

        for (int i = 0; i < tf_Destination.Length; i++)
            wayPoints[i] = tf_Destination[i].position;
        wayPoints[wayPoints.Length - 1] = transform.position;

        // 상태값 초기화
        crowStatus = CrowStatus.WAIT;
        isFind = false;
    }

    private void Update()
    {
        // 매번 업데이트 되는 cheaser옆의 crow2 좌표

        // 스킬3 버튼을 누르지 않으면 originTrans 좌표에 있다.
        if(cheaserAniScript.isCrowSpawn == false)
        {
            originTrans = cheaserCrow2trans;
            this.transform.position = originTrans.position;
            wayPoints[wayPoints.Length - 1] = transform.position;
        }

        // 스킬3 버튼을 누르면 네비메쉬 좌표로 이동해야 한다.
        if(cheaserAniScript.isCrowSpawn == true)
        {
            this.transform.position = new Vector3(this.transform.position.x, 1.447f, this.transform.position.z);
            PlayerCollider();
            Patrol();
        }
       agent.enabled = true;

        if (isFind)
        {
            StartCoroutine("slowMove");
        }

    }

    private void Patrol()
    {
        for(int  i  = 0; i < wayPoints.Length; i++)
        {
            if(Vector3.Distance(transform.position , wayPoints[i]) <= 0.5f)
            {
                if (i != wayPoints.Length - 1)
                {
                    agent.SetDestination(wayPoints[i + 1]);
                }
                else
                {
                    agent.SetDestination(wayPoints[0]);      
                }
            }
        }
    }

    // 충돌한 object들 중 Player 태그 감지
    private void PlayerCollider()
    {
        Collider[] col = Physics.OverlapSphere(transform.position, 8.6f);

        if(col.Length > 0)
        {
            for(int i = 0; i < col.Length; i++)
            {
                Transform tf_Target = col[i].transform;

               if(tf_Target.tag == "Player")
               {
                    transform.rotation = Quaternion.LookRotation(tf_Target.position - this.transform.position);
                    moveSpeed = 1.0f;
                    agent.speed = moveSpeed;
                    isFind = true;
                    Debug.Log("발견했습니다. Player를");
               }
            }
        }

    }

    IEnumerator slowMove()
    {
        yield return new WaitForSeconds(15f);
        moveSpeed = 3.5f;
        agent.speed = moveSpeed;
        isFind = false;
    }

}
