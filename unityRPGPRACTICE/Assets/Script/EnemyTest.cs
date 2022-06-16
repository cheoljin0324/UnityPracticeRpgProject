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

    //�ذ� ���
    public enum EnemyState { None, Idle, Move, Wait, GoTarget, Atk, Damage, Die }
    //�ذ� �⺻ �Ӽ�
    [Header("�⺻�Ӽ�")]
    //�ذ� �ʱ� ��� ��:
    public EnemyState enemyState = EnemyState.None;

    //�̵� �ӵ�
    public float spdMove = 1f;

    //�ذ��� �� Ÿ��
    public GameObject targetCharacter = null;



    //�ذ��� �� Ÿ�ٿ� ��ġ����
    public Transform targetTransform = null;

    //�ذ��� �� Ÿ�ٿ� ��ġ������ ��ǥ
    public Vector3 posTarget = Vector3.zero;

    //Ʈ������ ������Ʈ ĳ�� �غ�
    private Transform skullTransform = null;

    [Header("���� �Ӽ�")]
    //�ذ� ü��
    public int hp = 100;
    //�ذ� ���� �Ÿ� �� Ž�� �Ÿ�
    public float atkRange = 1.5f;

    void Start()
    {
        //ù ���
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
        Debug.Log("�Ѵ븦 �ƴ�");
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
    /// �ذ� ��尡 �̵� �����϶� �Լ�
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
    /// �ذ� ��尡 ���� ������ �� �Լ�
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
