using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : MonoBehaviour
{
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
        //���� ��ǥ ����
        if (targetCharacter == null)
        {
            posTarget = new Vector3(skullTransform.position.x + Random.Range(-10f, 10f), skullTransform.position.y + 1000f, skullTransform.position.z + Random.Range(-10f, 10f));


            //y�� Raycast�� ���ϱ�
            Ray ray = new Ray(posTarget, Vector3.down);

            //�浹ü O
            RaycastHit infoRayCast = new RaycastHit();

            //�浹ü ���� Ȯ��
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
        //������� ������ �� ������ ���� 
        Vector3 distance = Vector3.zero;

        //��������� ���� �ִ°�?
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

        //������� �̵�
        //�̵�����
        Vector3 direction = distance.normalized;
        //����x,z,y�� ����ϸ� �������� �̵� �׷��� 0
        direction = new Vector3(direction.x, 0f, direction.z);
        //�̵����� ������ ���ϰ�
        Vector3 amount = direction * spdMove * Time.deltaTime;

        //�ذ� �̵� 
        skullTransform.Translate(amount, Space.World);
        //�ذ� ����
        skullTransform.LookAt(posLookAt);
    }



    /// <summary>
    /// �ذ� ��尡 ���� ������ �� �Լ�
    /// </summary>
    IEnumerator SetWait()
    {
        //�ذ� ���¸� ���� ���·� �ٲ���
        enemyState = EnemyState.Wait;
        //�����ϴ� ���ð� 
        float timeWait = Random.RandomRange(1f, 3f);
        //�����ð��� ����
        yield return new WaitForSeconds(timeWait);
        //������� ��� ����
        enemyState = EnemyState.Idle;
    }

    void SetAtk()
    {
        //To Be Next Tiem...

        //Ʈ���ſ� �浹�� �±װ�

        float distance = Vector3.Distance(targetTransform.position, skullTransform.position);

        //���ݰŸ����� ���� �Ÿ��� �־����ٸ�
        if (distance > atkRange + 0.5f)
        {
            //Ÿ�ٰ��� �Ÿ��� �־����ٸ� �ٽ� Ÿ������ �̵�
            enemyState = EnemyState.GoTarget;
        }
    }

    void Update()
    {
        CeckState();
    }
}
