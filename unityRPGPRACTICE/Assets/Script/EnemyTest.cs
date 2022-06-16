using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTest : MonoBehaviour
{
    public int ID;
    private GameManager gameManager;

    public GameObject Canvas;

    public GameObject DamageText;

    public GameObject InstOb;

    public int SetCoin = 50;

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
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        skullTransform = GetComponent<Transform>();
        Canvas = GameObject.Find("Canvas");
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

    public void Damage(int damage)
    {
        hp -= damage;
        if (hp < 0)
        {
            InstOb.SendMessage("SetRemoveList", gameObject);
            gameManager.SetRemoveEnemy(gameObject);
            Destroy(gameObject);
            GameManager.Instance.userData.coin += SetCoin;
            if (GameManager.Instance.EnemyObList.Count == 0)
            {
                GameManager.Instance.EndState();
            }
        }
        GameObject TextImpact = Instantiate(DamageText,Canvas.transform);
        TextImpact.GetComponent<Text>().text = "-" + damage.ToString();
        TextImpact.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        TextImpact.transform.DOMoveY(TextImpact.transform.position.y + 300,3f);
        TextImpact.GetComponent<Text>().DOFade(0f, 3f);
        StartCoroutine(DesTextDamage(TextImpact));
        Debug.Log("한대를 쳤다");
    }

    IEnumerator DesTextDamage(GameObject gameOb)
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameOb);
    }

    void SetIdle()
    {
        if (targetCharacter == null)
        {
            posTarget = new Vector3(skullTransform.position.x + Random.Range(-10f, 10f), skullTransform.position.y + 1000f, skullTransform.position.z + Random.Range(-10f, 10f));


            Ray ray = new Ray(posTarget, Vector3.down);
            RaycastHit infoRayCast = new RaycastHit();
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
        Vector3 distance = Vector3.zero;
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

        Vector3 direction = distance.normalized;
        direction = new Vector3(direction.x, 0f, direction.z);
        Vector3 amount = direction * spdMove * Time.deltaTime;


        skullTransform.Translate(amount, Space.World);
        skullTransform.LookAt(posLookAt);
    }



    /// <summary>
    /// 해골 모드가 관찰 상태일 때 함수
    /// </summary>
    IEnumerator SetWait()
    {
        enemyState = EnemyState.Wait;
        float timeWait = Random.RandomRange(1f, 3f);
        yield return new WaitForSeconds(timeWait);
        enemyState = EnemyState.Idle;
    }

    void SetAtk()
    {
        float distance = Vector3.Distance(targetTransform.position, skullTransform.position);

        if (distance > atkRange + 0.5f)
        {
            enemyState = EnemyState.GoTarget;
        }
    }

    void Update()
    {
        CeckState();
    }
}
