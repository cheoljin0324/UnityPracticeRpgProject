using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Test : MonoBehaviour
{

    public enum PlayerState {None, Idle , Move, Attack, Dmage}

    public int Hp = 3;

    public float speed;      // ĳ���� ������ ���ǵ�.
    public float jumpSpeed; // ĳ���� ���� ��.
    public float gravity;    // ĳ���Ϳ��� �ۿ��ϴ� �߷�.

    PlayerState playerState = PlayerState.None;

    private CharacterController controller; // ���� ĳ���Ͱ� �������ִ� ĳ���� ��Ʈ�ѷ� �ݶ��̴�.
    private Vector3 MoveDir;                // ĳ������ �����̴� ����.
    Animator Getanim;

    
    // Start is called before the first frame update
    void Start()
    {
        speed = 6.0f;
        jumpSpeed = 8.0f;
        gravity = 20.0f;

        playerState = PlayerState.Idle;

        MoveDir = Vector3.zero;
        controller = GetComponent<CharacterController>();
        Getanim = GetComponent<Animator>();
    }



    // Update is called once per frame
    void Update()
    {
        //Debug.Log(playerState);
        Move();
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

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(Damage());
        }
    }

    IEnumerator Damage()
    {

        playerState = PlayerState.Dmage;
        Debug.Log(playerState);

        Hp--;
        StartCoroutine(DamageImpact());
        yield return new WaitForSeconds(2.5f);
        playerState = PlayerState.Idle;
    }

    IEnumerator DamageImpact()
    {
        Color First = Color.red;
        Color Second = Color.white;
        for(int i = 0; i<5; i++)
        {
            transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().materials[0].DOColor(First, 0.25f);
            transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().materials[1].DOColor(First, 0.25f);
            yield return new WaitForSeconds(0.25f);
            transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().materials[0].DOColor(Second, 0.25f);
            transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().materials[1].DOColor(Second, 0.25f);
            yield return new WaitForSeconds(0.25f);
        }
       
    }

    /// <summary>
    /// �̵��Լ� �Դϴ� ĳ����
    /// </summary>
    void Move()
    {

        // ���� ĳ���Ͱ� ���� �ִ°�?
        if (controller.isGrounded)
        {
            // ��, �Ʒ� ������ ����. 
            MoveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            // ���͸� ���� ��ǥ�� ���ؿ��� ���� ��ǥ�� �������� ��ȯ�Ѵ�.
            MoveDir = transform.TransformDirection(MoveDir);

            // ���ǵ� ����.
            MoveDir *= speed;

            // ĳ���� ����
            if (Input.GetButton("Jump"))
                MoveDir.y = jumpSpeed;

        }

        // ĳ���Ϳ� �߷� ����.
        MoveDir.y -= gravity * Time.deltaTime;

        // ĳ���� ������.
        controller.Move(MoveDir * Time.deltaTime);


        //ĳ���� ������ �ٶ� ������? (ĳ���� �ӵ� ����)
        Vector3 newForward = controller.velocity;
        newForward.y = 0.0f;

        //ĳ���͸� �������� ������ ����.
        transform.forward = Vector3.Lerp(transform.forward, newForward, 0.6f * Time.deltaTime);

    }
}
