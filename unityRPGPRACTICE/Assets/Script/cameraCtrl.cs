using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraCtrl : MonoBehaviour
{
    [Header("카메라 기본 속성")]
    //카메라 위치 캐싱 준비
    private Transform cameraTransform;

    //target
    public GameObject objTarget;

    //player transform 캐싱
    private Transform objTargetTransform;

    float posY = 0;

  
    [Header("3인칭 카메라")]
    //떨어진 거리
    public float distance = 6.0f;

    public float mouseSet = 4.3f;

    //추가 높이
    public float height = 1.75f;

    public float addHeight = 0f;

    //smooth time
    public float heightDamp = 2.0f;
    public float rotationDamping = 3.0f;

    private void LateUpdate()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        { 
            if (addHeight < 10 || Input.GetAxis("Mouse ScrollWheel")<0)
            {
             
                    Debug.Log(Input.GetAxis("Mouse ScrollWheel"));
                    addHeight += Input.GetAxis("Mouse ScrollWheel")*mouseSet;

                if (addHeight < 0)
                {
                    addHeight = 0;
                }
               
            }
        }
      
      
      ThirdCamera();
    }

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = GetComponent<Transform>();

        if (objTarget != null)
        {
            objTargetTransform = objTarget.transform;
        }
    }

    /// <summary>
    /// 3인칭 카메라 기본 동작 함수
    /// </summary>
    void ThirdCamera()
    {
        //현재 타겟 Y축 각도 값
        float objTargetRotationAngle = objTargetTransform.eulerAngles.y;

        //현재 타겟 높이 + 카메라가 위치한 높이 추가 높이
        float objHeight = objTargetTransform.position.y + height;

        //현재 각도 높이
        float nowRotationAngle = cameraTransform.eulerAngles.y;
        float nowHeight = cameraTransform.position.y-addHeight;

        //현재 각도에 대한 DAMP
        nowRotationAngle = Mathf.LerpAngle(nowRotationAngle, objTargetRotationAngle, rotationDamping * Time.deltaTime);

        //현재 높이에 대한 DAMP
        nowHeight = Mathf.Lerp(nowHeight, objHeight, heightDamp * Time.deltaTime);



        //유니티 각도로 변경
        Quaternion nowRotation = Quaternion.Euler(0f, nowRotationAngle, 0f);
        if (objTarget.GetComponent<Test>().isQMove == true)
        {
            nowRotation = Quaternion.Euler(0f, nowRotationAngle, 0f);
            if (Input.GetAxis("Vertical") == 0)
            {
                nowRotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }

        else if (objTarget.GetComponent<Test>().isQMove == false)
        {
            nowRotation = Quaternion.Euler(0f, 0f, 0f);
        }

        if (addHeight > 0)
        {
            nowRotation = Quaternion.Euler(0f, 0f, 0f);
        }

        //카메라 위치 포지션 이동
        cameraTransform.position = objTargetTransform.position;
        cameraTransform.position -= nowRotation * Vector3.forward;

        //최종이동
        cameraTransform.position = new Vector3(cameraTransform.position.x, nowHeight+addHeight, cameraTransform.position.z-1);

        cameraTransform.LookAt(objTargetTransform);
    }
}