using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraCtrl : MonoBehaviour
{
    [Header("CameraPos")]
    //CameraPos
    private Transform cameraTransform;

    //target
    public GameObject objTarget;

    //player transform 
    private Transform objTargetTransform;

    float posY = 0;

  
    [Header("distance")]
    //ObjectDistance
    public float distance = 6.0f;

    public float mouseSet = 4.3f;

    //ObjectHeight
    public float height = 1.75f;

    //mouse addHeight
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
    /// 3��Ī ī�޶� �⺻ ���� �Լ�
    /// </summary>
    void ThirdCamera()
    {
        //���� Ÿ�� Y�� ���� ��
        float objTargetRotationAngle = objTargetTransform.eulerAngles.y;

        //���� Ÿ�� ���� + ī�޶� ��ġ�� ���� �߰� ����
        float objHeight = objTargetTransform.position.y + height;

        //���� ���� ����
        float nowRotationAngle = cameraTransform.eulerAngles.y;
        float nowHeight = cameraTransform.position.y-addHeight;

        //���� ������ ���� DAMP
        nowRotationAngle = Mathf.LerpAngle(nowRotationAngle, objTargetRotationAngle, rotationDamping * Time.deltaTime);

        //���� ���̿� ���� DAMP
        nowHeight = Mathf.Lerp(nowHeight, objHeight, heightDamp * Time.deltaTime);



        //����Ƽ ������ ����
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

        //ī�޶� ��ġ ������ �̵�
        cameraTransform.position = objTargetTransform.position;
        cameraTransform.position -= nowRotation * Vector3.forward;

        //�����̵�
        cameraTransform.position = new Vector3(cameraTransform.position.x, nowHeight+addHeight, cameraTransform.position.z-1);

        cameraTransform.LookAt(objTargetTransform);
    }
}