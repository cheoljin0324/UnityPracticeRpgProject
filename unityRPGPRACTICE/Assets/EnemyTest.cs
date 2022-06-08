using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    //해골 모드
    public enum EnemyState { None, Idle, Move, Wait, GoTarget, Atk, Damage, Die }
    //해골 기본 속성
    [Header("기본속성")]
    //해골 초기 모드 값:
    public EnemyState enemyState = EnemyState.None;

    //이동 속도
    public float spdMove = 1f;

    //해골이 본 타겟
    public GameObject targetCharacter = null;

    //해골이 본 타겟에 위치정보
    public Transform targetTransform = null;

    //해골이 본 타겟에 위치정보에 좌표
    public Vector3 posTarget = Vector3.zero;

    //트랜스폼 컴포넌트 캐싱 준비
    private Transform skullTransform = null;

    [Header("전투 속성")]
    //해골 체력
    public int hp = 100;
    //해골 공격 거리 및 탐지 거리
    public float atkRange = 1.5f;

    void Start()
    {
        //첫 모드
        enemyState = EnemyState.Idle;
        skullTransform = GetComponent<Transform>();
    }

    void CeckState()
    {
        switch (enemyState)
        {
            case EnemyState.Idle:
                SetIdle();
                break;
            case EnemyState.GoTarget:
            case EnemyState.Move:
                SetMove();
                break;
            case EnemyState.Atk:
                SetAtk();
                break;
            default:
                break;
        }
    }

    void SetIdle()
    {
        //임의 좌표 설정
        if (targetCharacter == null)
        {
            posTarget = new Vector3(skullTransform.position.x + Random.Range(-10f, 10f), skullTransform.position.y + 1000f, skullTransform.position.z + Random.Range(-10f, 10f));


            //y값 Raycast로 구하기
            Ray ray = new Ray(posTarget, Vector3.down);

            //충돌체 O
            RaycastHit infoRayCast = new RaycastHit();

            //충돌체 여부 확인
            if (Physics.Raycast(ray, out infoRayCast, Mathf.Infinity) == true)
            {
                posTarget.y = infoRayCast.point.y;
            }
            enemyState = EnemyState.Move;
        }
        else
        {
            enemyState = EnemyState.GoTarget;
        }
    }

    /// <summary>
    /// 해골 모드가 이동 상태일때 함수
    /// </summary>
    void SetMove()
    {
        //출발점과 도착점 두 벡터의 차이 
        Vector3 distance = Vector3.zero;

        //어느방향을 보고 있는가?
        Vector3 posLookAt = Vector3.zero;

        switch (enemyState)
        {
            case EnemyState.Move:
                if (posTarget != Vector3.zero)
                {
                    distance = posTarget - skullTransform.position;
                    if (distance.magnitude < atkRange)
                    {
                        StartCoroutine(SetWait());
                        return;
                    }
                    posLookAt = new Vector3(posTarget.x, skullTransform.position.y, posTarget.z);
                }
                break;
            case EnemyState.GoTarget:
                if (targetCharacter != null)
                {
                    distance = targetCharacter.transform.position - skullTransform.position;
                    if (distance.magnitude < atkRange)
                    {
                        enemyState = EnemyState.Atk;
                        return;
                    }
                    posLookAt = new Vector3(targetCharacter.transform.position.x, skullTransform.position.y, targetCharacter.transform.position.z);
                }
                break;
            default:
                break;
        }

        //공동요소 이동
        //이동방향
        Vector3 direction = distance.normalized;
        //방향x,z,y를 사용하면 땅속으로 이동 그래서 0
        direction = new Vector3(direction.x, 0f, direction.z);
        //이동량과 방향을 구하고
        Vector3 amount = direction * spdMove * Time.deltaTime;

        //해골 이동 
        skullTransform.Translate(amount, Space.World);
        //해골 방향
        skullTransform.LookAt(posLookAt);
    }



    /// <summary>
    /// 해골 모드가 관찰 상태일 때 함수
    /// </summary>
    IEnumerator SetWait()
    {
        //해골 상태를 관찰 상태로 바꿔줌
        enemyState = EnemyState.Wait;
        //관찰하는 대기시간 
        float timeWait = Random.RandomRange(1f, 3f);
        //관찰시간을 적용
        yield return new WaitForSeconds(timeWait);
        //원래대로 모드 변경
        enemyState = EnemyState.Idle;
    }

    void SetAtk()
    {
        //To Be Next Tiem...

        //트리거에 충돌한 태그가

        float distance = Vector3.Distance(targetTransform.position, skullTransform.position);

        //공격거리보다 둘의 거리가 멀어졌다면
        if (distance > atkRange + 0.5f)
        {
            //타겟과의 거리가 멀어졌다면 다시 타겟으로 이동
            enemyState = EnemyState.GoTarget;
        }
    }

    void Update()
    {
        CeckState();
    }
}
