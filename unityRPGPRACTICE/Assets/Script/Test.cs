using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Test : MonoBehaviour
{

    public enum PlayerState {None, Idle , Move, Attack, Dmage}

    [SerializeField]
    public ShopCanvasManager Shop;
    ItemDataBase itemData;

    public int Hp = 3;
    public int attack = 30;
    public string name = "player";

    //현재 캐릭터 이동 백터 값 
    private Vector3 vecNowVelocity = Vector3.zero;

    //현재 캐릭터 이동 방향 벡터 
    private Vector3 vecMoveDirection = Vector3.zero;

    PlayerState playerState = PlayerState.None;
    Animator Getanim;

    //캐릭터 직선 이동 속도 (걷기)
    public float walkMoveSpd = 2.0f;

    //캐릭터 회전 이동 속도 
    public float rotateMoveSpd = 100.0f;

    //캐릭터 회전 방향으로 몸을 돌리는 속도
    public float rotateBodySpd = 2.0f;

    //캐릭터 이동 속도 증가 값
    public float moveChageSpd = 0.1f;

    //CharacterController 캐싱 준비
    private CharacterController controllerCharacter = null;

    //캐릭터 CollisionFlags 초기값 설정
    private CollisionFlags collisionFlagsCharacter = CollisionFlags.None;

    //캐릭터 중력값
    private float gravity = 9.8f;

    //캐릭터 중력 속도 값
    private float verticalSpd = 0f;

    //캐릭터 멈춤 변수 플래그
    private bool stopMove = false;

    public bool isQMove = false;

    public bool SetI = false;


    void Start()
    {
        itemData = GameObject.Find("ItemDataBase").GetComponent<ItemDataBase>();
        //CharacterController 캐싱
        controllerCharacter = GetComponent<CharacterController>();
        Getanim = GetComponent<Animator>();
        
        ItemSetting();
        
    }

    public void ItemSetting()
    {
        for(int i = 0; i<itemData.setItem.Length; i++)
        {
            itemData.setItem[i].Item.SetActive(false);
            if (GameManager.Instance.userData.isUse[i] == true)
            {
                itemData.setItem[i].Item.SetActive(true);
                SetI = true;
            }
        }
        if (SetI == false)
        {
            GameManager.Instance.userData.isUse[0] = true;
            itemData.setItem[0].isUse = true;
            itemData.setItem[0].Item.SetActive(true);

        }
        for (int i = 0; i < itemData.setItem.Length; i++)
        {
            if (GameManager.Instance.userData.isUse[i] == true)
            {
                attack = itemData.setItem[i].ItemAddDamage;
            }
        }
        GameManager.Instance.StartState();
    }

    private void OnGUI()
    {
        var labelStyle = new GUIStyle();
        labelStyle.fontSize = 50;
        labelStyle.normal.textColor = Color.black;

        GUILayout.Label("체력 : " + Hp.ToString(),labelStyle);

        //현재 캐릭터 방향 + 크기
        GUILayout.Label("공격력 : " + attack.ToString(), labelStyle);

        //현재  재백터 크기 속도
        GUILayout.Label("플레이어 이름 : " + name, labelStyle);

        GUILayout.Label("보유 코인:" + GameManager.Instance.userData.coin, labelStyle);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.nowState == GameManager.GameState.Gaming)
        {
            if (Hp == 0)
            {
                gameObject.SetActive(false);
            }
            //캐릭터 이동 
            Move();
            // Debug.Log(getNowVelocityVal());
            //캐릭터 방향 변경 
            vecDirectionChangeBody();

            //중력 적용
            setGravity();

            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                playerState = PlayerState.Move;
                Getanim.SetBool("isMove", true);
                isQMove = true;
                if (Input.GetAxis("Vertical") < 0)
                {
                    isQMove = false;
                }
            }
            else if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
            {
                playerState = PlayerState.Idle;
                Getanim.SetBool("isMove", false);
                isQMove = false;
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
    }

    void Move()
    {
        if (stopMove == true)
        {
            return;
        }
        Transform CameraTransform = Camera.main.transform;
        //메인 카메라가 바라보는 방향이 월드상에 어떤 방향인가.
        Vector3 forward = CameraTransform.TransformDirection(Vector3.forward);
        forward.y = 0.0f;

        //forward.z, forward.x
        Vector3 right = new Vector3(forward.z, 0.0f, -forward.x);

        //키입력 
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        //케릭터가 이동하고자 하는 방향 
        Vector3 targetDirection = horizontal * right + vertical * forward;

        //현재 이동하는 방향에서 원하는 방향으로 회전 

        vecMoveDirection = Vector3.RotateTowards(vecMoveDirection, targetDirection, rotateMoveSpd * Mathf.Deg2Rad * Time.deltaTime, 1000.0f);
        vecMoveDirection = vecMoveDirection.normalized;
        //캐릭터 이동 속도
        float spd = walkMoveSpd;
        spd = walkMoveSpd;


        //중력이동 
        Vector3 _vecTemp = new Vector3(0f, verticalSpd, 0f);

        // 프레임 이동 양
        Vector3 moveAmount = (vecMoveDirection * spd * Time.deltaTime) + _vecTemp;

        collisionFlagsCharacter = controllerCharacter.Move(moveAmount);


    }

    float getNowVelocityVal()
    {
        //현재 캐릭터가 멈춰 있다면 
        if (controllerCharacter.velocity == Vector3.zero)
        {
            //반환 속도 값은 0
            vecNowVelocity = Vector3.zero;
        }
        else
        {

            //반환 속도 값은 현재 /
            Vector3 retVelocity = controllerCharacter.velocity;
            retVelocity.y = 0.0f;

            vecNowVelocity = Vector3.Lerp(vecNowVelocity, retVelocity, moveChageSpd * Time.fixedDeltaTime);

        }
        //거리 크기
        return vecNowVelocity.magnitude;
    }

    void vecDirectionChangeBody()
    {
        //캐릭터 이동 시
        if (getNowVelocityVal() > 0.0f)
        {
            //내 몸통  바라봐야 하는 곳은 어디?
            Vector3 newForward = controllerCharacter.velocity;
            newForward.y = 0.0f;

            //내 캐릭터 전면 설정 
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
