using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Test : MonoBehaviour
{

    public enum PlayerState {None, Idle , Move, Attack, Dmage}

    public int Hp = 3;

    public float speed;      // 캐릭터 움직임 스피드.
    public float jumpSpeed; // 캐릭터 점프 힘.
    public float gravity;    // 캐릭터에게 작용하는 중력.

    PlayerState playerState = PlayerState.None;

    private CharacterController controller; // 현재 캐릭터가 가지고있는 캐릭터 컨트롤러 콜라이더.
    private Vector3 MoveDir;                // 캐릭터의 움직이는 방향.
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
    /// 이동함수 입니다 캐릭터
    /// </summary>
    void Move()
    {

        // 현재 캐릭터가 땅에 있는가?
        if (controller.isGrounded)
        {
            // 위, 아래 움직임 셋팅. 
            MoveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            // 벡터를 로컬 좌표계 기준에서 월드 좌표계 기준으로 변환한다.
            MoveDir = transform.TransformDirection(MoveDir);

            // 스피드 증가.
            MoveDir *= speed;

            // 캐릭터 점프
            if (Input.GetButton("Jump"))
                MoveDir.y = jumpSpeed;

        }

        // 캐릭터에 중력 적용.
        MoveDir.y -= gravity * Time.deltaTime;

        // 캐릭터 움직임.
        controller.Move(MoveDir * Time.deltaTime);


        //캐릭터 몸통이 바라볼 전방은? (캐릭터 속도 방향)
        Vector3 newForward = controller.velocity;
        newForward.y = 0.0f;

        //캐릭터를 전방으로 방향을 설정.
        transform.forward = Vector3.Lerp(transform.forward, newForward, 0.6f * Time.deltaTime);

    }
}
