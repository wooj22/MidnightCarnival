using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroDropGameManager : MonoBehaviour
{
    public GameObject disk; // ���� ������Ʈ
    public GameObject cameraObject; // �ܼ��� ī�޶� ������Ʈ
    private float rotationSpeed = 10f; // ȸ�� �ӵ�
    private float riseSpeed; // ��� �ӵ�
    private float targetYPosition = 525f; // ī�޶��� ��ǥ Y ��ġ
    private float diskCameraOffset = 53f; // ���ǰ� ī�޶��� Y�� ������
    private float initialRiseAmount = 20f; // �ʱ� ��� Y��
    private float pauseDuration = 5f; // ��� ���ߴ� �ð�
    private float totalRiseDuration = 40f; // ���� ��� �ð�

    private bool isRising = false; // ��� ���� �÷���
    private bool isPaused = false; // ��� ���� ���� �÷���

    void Start()
    {
        Debug.Log("������ ���۵Ǵ� ���� ���� �ö���ּ���");
        Invoke("StartRising", 5f); // 5�� �� ��� ����
        riseSpeed = (targetYPosition - initialRiseAmount) / totalRiseDuration; // ��� �ӵ� ���
    }

    void Update()
    {
        if (isRising)
        {
            // ���� ȸ��
            disk.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

            // ī�޶�� ������ Y ��ġ ������Ʈ
            if (!isPaused)
            {
                float currentY = cameraObject.transform.position.y;

                // ī�޶�� ������ ����ϴ� ����
                if (currentY < 50f)
                {
                    // ó�� ���
                    float newY = currentY + riseSpeed * Time.deltaTime;
                    cameraObject.transform.position = new Vector3(cameraObject.transform.position.x, newY, cameraObject.transform.position.z);
                    disk.transform.position = new Vector3(disk.transform.position.x, newY - diskCameraOffset, disk.transform.position.z);
                }
                else if (currentY >= 50f && !isPaused)
                {
                    // Y�� 50�� �������� �� ��� ����
                    isPaused = true;
                    Invoke("ResumeRising", pauseDuration); // 5�� �� �ٽ� ��� ����
                }
            }
        }
    }

    void StartRising()
    {
        isRising = true; // ��� ����
    }

    void ResumeRising()
    {
        isPaused = false; // ��� �簳
        StartCoroutine(RiseCoroutine());
    }

    private System.Collections.IEnumerator RiseCoroutine()
    {
        while (cameraObject.transform.position.y < targetYPosition)
        {
            // ī�޶�� ���� Y ��ġ ������Ʈ
            float currentY = cameraObject.transform.position.y;
            float newY = Mathf.Min(currentY + riseSpeed * Time.deltaTime, targetYPosition);
            cameraObject.transform.position = new Vector3(cameraObject.transform.position.x, newY, cameraObject.transform.position.z);
            disk.transform.position = new Vector3(disk.transform.position.x, newY - diskCameraOffset, disk.transform.position.z);
            yield return null;
        }

        Debug.Log("�ְ� ���̿� ������.");
        isRising = false; // ��� ���� ����
    }
}
