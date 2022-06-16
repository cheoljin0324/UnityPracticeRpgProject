using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Test : MonoBehaviour
{
    public enum PlayerState {None, Idle , Move, Attack, Dmage, Die}

    [SerializeField]
    public ShopCanvasManager Shop;
    ItemDataBase itemData;

    public int Hp = 3;
    public int attack = 30;
    public string name = "player";

    
    private Vector3 vecNowVelocity = Vector3.zero;

    //움직이는 위치
    private Vector3 vecMoveDirection = Vector3.zero;

    PlayerState playerState = PlayerState.None;
    Animator Getanim;

    //걷는 움직임 속도
    public float walkMoveSpd = 2.0f;

    //회전 속도
    public float rotateMoveSpd = 100.0f;

    //회전하는 하는 속도
    public float rotateBodySpd = 2.0f;

    //움직임이 바뀌는 속도
    public float moveChageSpd = 0.1f;

    //캐릭터 컨트롤러 컴포넌트
    private CharacterController controllerCharacter = null;

    //플래그
    private CollisionFlags collisionFlagsCharacter = CollisionFlags.None;

    //중력
    private float gravity = 9.8f;

    //현재 스피드
    private float verticalSpd = 0f;

    //움직임을 멈췄는가?
    private bool stopMove = false;
    //회전 하는가?
    public bool isQMove = false;

    public bool SetI = false;


    void Start()
    {
        itemData = GameObject.Find("ItemDataBase").GetComponent<ItemDataBase>();
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

        GUILayout.Label("공격력 : " + attack.ToString(), labelStyle);

        GUILayout.Label("이름 : " + name, labelStyle);

        GUILayout.Label("현재 갖고 있는 코인:" + GameManager.Instance.userData.coin, labelStyle);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.nowState == GameManager.GameState.Gaming)
        {
            if (Hp == 0)
            {
                gameObject.SetActive(false);
                playerState = PlayerState.Die;
            }
            Move();
            vecDirectionChangeBody();

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
        Vector3 forward = CameraTransform.TransformDirection(Vector3.forward);
        forward.y = 0.0f;

        Vector3 right = new Vector3(forward.z, 0.0f, -forward.x);


        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        Vector3 targetDirection = horizontal * right + vertical * forward;



        vecMoveDirection = Vector3.RotateTowards(vecMoveDirection, targetDirection, rotateMoveSpd * Mathf.Deg2Rad * Time.deltaTime, 1000.0f);
        vecMoveDirection = vecMoveDirection.normalized;

        float spd = walkMoveSpd;
        spd = walkMoveSpd;



        Vector3 _vecTemp = new Vector3(0f, verticalSpd, 0f);


        Vector3 moveAmount = (vecMoveDirection * spd * Time.deltaTime) + _vecTemp;

        collisionFlagsCharacter = controllerCharacter.Move(moveAmount);


    }

    float getNowVelocityVal()
    {

        if (controllerCharacter.velocity == Vector3.zero)
        {

            vecNowVelocity = Vector3.zero;
        }
        else
        {


            Vector3 retVelocity = controllerCharacter.velocity;
            retVelocity.y = 0.0f;

            vecNowVelocity = Vector3.Lerp(vecNowVelocity, retVelocity, moveChageSpd * Time.fixedDeltaTime);

        }

        return vecNowVelocity.magnitude;
    }

    void vecDirectionChangeBody()
    {

        if (getNowVelocityVal() > 0.0f)
        {

            Vector3 newForward = controllerCharacter.velocity;
            newForward.y = 0.0f;
 
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
