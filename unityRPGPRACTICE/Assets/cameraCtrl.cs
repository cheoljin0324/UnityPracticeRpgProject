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

    public enum CameraTypeState { First, Second, Third }

    //ī�޶� �⺻ 3��Ī
    public CameraTypeState cameraState = CameraTypeState.Third;

    [Header("3��Ī ī�޶�")]
    //������ �Ÿ�
    public float distance = 6.0f;

    //�߰� ����
    public float height = 1.75f;

    //smooth time
    public float heightDamp = 2.0f;
    public float rotationDamping = 3.0f;

    [Header("2��Ī ī�޶�")]
    public float rotationSpd = 10.0f;

    [Header("1��Ī ī�޶�")]
    //���콺 ī�޶� ���� ������ ��ǥ
    public float detailX = 5f;
    public float detailY = 5f;

    //���콺 ȸ�� ��

    private float rotationX = 0f;
    private float rotationY = 0f;

    //ĳ��
    public Transform posFirstTarget = null;

    /// <summary>
    /// 1��Ī ī�޶� ����
    /// </summary>
    void FirstCamera()
    {
        //���콺 ��ǥ ���� ����
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotationX = cameraTransform.localEulerAngles.y + mouseX * detailX;
        rotationX = (rotationX > 180.0f) ? rotationX - 360.0f : rotationX;
        rotationY = cameraTransform.localEulerAngles.x + mouseY * detailY;
        rotationY = (rotationY > 180.0f) ? rotationY - 360.0f : rotationY;

        cameraTransform.localEulerAngles = new Vector3(-rotationY, rotationX, 0f);
        cameraTransform.position = posFirstTarget.position;
    }

    /// <summary>
    /// 2��Ī ī�޶� ����
    /// </summary>
    void SecondCamera()
    {
        cameraTransform.RotateAround(objTargetTransform.position, Vector3.up, rotationSpd * Time.deltaTime);
        cameraTransform.LookAt(objTargetTransform);
    }

    private void LateUpdate()
    {
        if (objTarget == null)
        {
            return;
        }

        if (objTargetTransform == null)
        {
            objTargetTransform = objTarget.transform;
        }

        switch (cameraState)
        {
            case CameraTypeState.Third:
                ThirdCamera();
                break;
            case CameraTypeState.Second:
                SecondCamera();
                break;
            case CameraTypeState.First:
                FirstCamera();
                break;
        }
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

    // Update is called once per frame
    void Update()
    {

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

        //ī�޶� ��ġ ������ �̵�
        cameraTransform.position = objTargetTransform.position;
        cameraTransform.position -= nowRotation * Vector3.forward;

        //�����̵�
        cameraTransform.position = new Vector3(cameraTransform.position.x, nowHeight, cameraTransform.position.z);
        cameraTransform.LookAt(objTargetTransform);
    }
}