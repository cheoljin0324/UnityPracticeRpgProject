using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraCtrl : MonoBehaviour
{
    [Header("ī�޶� �⺻ �Ӽ�")]
    //ī�޶� ��ġ ĳ�� �غ�
    private Transform cameraTransform;

    //target
    public GameObject objTarget;

    //player transform ĳ��
    private Transform objTargetTransform;

  
    [Header("3��Ī ī�޶�")]
    //������ �Ÿ�
    public float distance = 6.0f;

    //�߰� ����
    public float height = 1.75f;

    //smooth time
    public float heightDamp = 2.0f;
    public float rotationDamping = 3.0f;

    private void LateUpdate()
    { 
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
        float nowHeight = cameraTransform.position.y;

        //���� ������ ���� DAMP
        nowRotationAngle = Mathf.LerpAngle(nowRotationAngle, objTargetRotationAngle, rotationDamping * Time.deltaTime);

        //���� ���̿� ���� DAMP
        nowHeight = Mathf.Lerp(nowHeight, objHeight, heightDamp * Time.deltaTime);



        //����Ƽ ������ ����
        Quaternion nowRotation = Quaternion.Euler(0f, nowRotationAngle, 0f);

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") < 0)
        {
            nowRotation = Quaternion.Euler(0f, 0, 0f);
        }

        //ī�޶� ��ġ ������ �̵�
        cameraTransform.position = objTargetTransform.position;
        cameraTransform.position -= nowRotation * Vector3.forward;

        //�����̵�
        cameraTransform.position = new Vector3(cameraTransform.position.x, nowHeight, cameraTransform.position.z-1);
        cameraTransform.LookAt(objTargetTransform);
    }
}