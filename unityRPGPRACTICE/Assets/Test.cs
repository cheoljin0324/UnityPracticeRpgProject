using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Test : MonoBehaviour
{

    public enum PlayerState {None, Idle , Move, Attack, Dmage}

    public int Hp = 3;
    public int attack = 30;
    public string name = "player";

    //���� ĳ���� �̵� ���� �� 
    private Vector3 vecNowVelocity = Vector3.zero;

    //���� ĳ���� �̵� ���� ���� 
    private Vector3 vecMoveDirection = Vector3.zero;

    PlayerState playerState = PlayerState.None;
    Animator Getanim;

    //ĳ���� ���� �̵� �ӵ� (�ȱ�)
    public float walkMoveSpd = 2.0f;

    //ĳ���� ȸ�� �̵� �ӵ� 
    public float rotateMoveSpd = 100.0f;

    //ĳ���� ȸ�� �������� ���� ������ �ӵ�
    public float rotateBodySpd = 2.0f;

    //ĳ���� �̵� �ӵ� ���� ��
    public float moveChageSpd = 0.1f;

    //CharacterController ĳ�� �غ�
    private CharacterController controllerCharacter = null;

    //ĳ���� CollisionFlags �ʱⰪ ����
    private CollisionFlags collisionFlagsCharacter = CollisionFlags.None;

    //ĳ���� �߷°�
    private float gravity = 9.8f;

    //ĳ���� �߷� �ӵ� ��
    private float verticalSpd = 0f;

    //ĳ���� ���� ���� �÷���
    private bool stopMove = false;

    void Start()
    {
        //CharacterController ĳ��
        controllerCharacter = GetComponent<CharacterController>();
        Getanim = GetComponent<Animator>();
    }

    private void OnGUI()
    {
        var labelStyle = new GUIStyle();
        labelStyle.fontSize = 50;
        labelStyle.normal.textColor = Color.black;

        GUILayout.Label("ü�� : " + Hp.ToString(),labelStyle);

        //���� ĳ���� ���� + ũ��
        GUILayout.Label("���ݷ� : " + attack.ToString(), labelStyle);

        //����  ����� ũ�� �ӵ�
        GUILayout.Label("�÷��̾� �̸� : " + name, labelStyle);
    }

    // Update is called once per frame
    void Update()
    {

        if (Hp == 0)
        {
            gameObject.SetActive(false);
        }
        //ĳ���� �̵� 
        Move();
        // Debug.Log(getNowVelocityVal());
        //ĳ���� ���� ���� 
        vecDirectionChangeBody();

        //�߷� ����
        setGravity();

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.D))
        {
            playerState = PlayerState.Move;
            Getanim.SetBool("isMove", true);
        }
        else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.D))
        {
            playerState = PlayerState.Idle;
            Getanim.SetBool("isMove", false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            playerState = PlayerState.Attack;
            Getanim.SetBool("isAttack", true);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            playerState = PlayerState.Idle;
            Getanim.SetBool("isAttack", false);
        }
    }

    void Move()
    {
        if (stopMove == true)
        {
            return;
        }
        Transform CameraTransform = Camera.main.transform;
        //���� ī�޶� �ٶ󺸴� ������ ����� � �����ΰ�.
        Vector3 forward = CameraTransform.TransformDirection(Vector3.forward);
        forward.y = 0.0f;

        //forward.z, forward.x
        Vector3 right = new Vector3(forward.z, 0.0f, -forward.x);

        //Ű�Է� 
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        //�ɸ��Ͱ� �̵��ϰ��� �ϴ� ���� 
        Vector3 targetDirection = horizontal * right + vertical * forward;

        //���� �̵��ϴ� ���⿡�� ���ϴ� �������� ȸ�� 

        vecMoveDirection = Vector3.RotateTowards(vecMoveDirection, targetDirection, rotateMoveSpd * Mathf.Deg2Rad * Time.deltaTime, 1000.0f);
        vecMoveDirection = vecMoveDirection.normalized;
        //ĳ���� �̵� �ӵ�
        float spd = walkMoveSpd;
        spd = walkMoveSpd;


        //�߷��̵� 
        Vector3 _vecTemp = new Vector3(0f, verticalSpd, 0f);

        // ������ �̵� ��
        Vector3 moveAmount = (vecMoveDirection * spd * Time.deltaTime) + _vecTemp;

        collisionFlagsCharacter = controllerCharacter.Move(moveAmount);


    }

    float getNowVelocityVal()
    {
        //���� ĳ���Ͱ� ���� �ִٸ� 
        if (controllerCharacter.velocity == Vector3.zero)
        {
            //��ȯ �ӵ� ���� 0
            vecNowVelocity = Vector3.zero;
        }
        else
        {

            //��ȯ �ӵ� ���� ���� /
            Vector3 retVelocity = controllerCharacter.velocity;
            retVelocity.y = 0.0f;

            vecNowVelocity = Vector3.Lerp(vecNowVelocity, retVelocity, moveChageSpd * Time.fixedDeltaTime);

        }
        //�Ÿ� ũ��
        return vecNowVelocity.magnitude;
    }

    void vecDirectionChangeBody()
    {
        //ĳ���� �̵� ��
        if (getNowVelocityVal() > 0.0f)
        {
            //�� ����  �ٶ���� �ϴ� ���� ���?
            Vector3 newForward = controllerCharacter.velocity;
            newForward.y = 0.0f;

            //�� ĳ���� ���� ���� 
            transform.forward = Vector3.Lerp(transform.forward, newForward, rotateBodySpd * Time.deltaTime);

        }
    }

    void setGravity()
    {
        if ((collisionFlagsCharacter & CollisionFlags.CollidedBelow) != 0)
        {
            verticalSpd = 0f;
        }
        else
        {
            verticalSpd -= gravity * Time.deltaTime;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(Damage());
        }
        else if (collision.gameObject.CompareTag("obstacle"))
        {
            Hp = 0;
            Debug.Log("You Die by my Trap");
        }
    }

    IEnumerator Damage()
    {
        if(playerState != PlayerState.Dmage&&Hp>0)
        {
            playerState = PlayerState.Dmage;
            Debug.Log(playerState);
            Hp--;
            if (Hp == 0)
            {
                Debug.Log("You DIe");
            }
            StartCoroutine(DamageImpact());
            yield return new WaitForSeconds(2.5f);
            playerState = PlayerState.Idle;
        }
    }

    IEnumerator DamageImpact()
    {
        Color First = Color.red;
        Color Second = Color.white;
        for (int i = 0; i < 5; i++)
        {
            transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().materials[0].DOColor(First, 0.25f);
            transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().materials[1].DOColor(First, 0.25f);
            yield return new WaitForSeconds(0.25f);
            transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().materials[0].DOColor(Second, 0.25f);
            transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().materials[1].DOColor(Second, 0.25f);
            yield return new WaitForSeconds(0.25f);
        }

    }
}
