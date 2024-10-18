using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroDropGameManager : MonoBehaviour
{
    // [ ���� ������Ʈ ���� ]
    public GameObject disk;                // ����
    public GameObject cameraObject;        // ī�޶� 
    public GameObject[] platformPieces;    // ���� ������

    // [ ���� ��� ]
    private const float RotationSpeed = 10f;     // ���� ȸ�� �ӵ�
    private const float TargetYPosition = 525f;  // ī�޶� ��ǥ Y ��ġ
    private const float DiskCameraOffset = 53f;  // ���ǰ� ī�޶� �� ������
    private const float InitialRiseAmount = 10f; // �ʱ� ī�޶� ��� Y��
    private const float PauseDuration = 5f;      // ��� ���� �ð�
    private const float TotalRiseDuration = 90f; // ��ü ��¿� �ɸ��� �ð�
    private const float LowerPercentage = 0.05f; // �ϰ� ���� (5%)
    private const float ClearTimeLimit = 120f;   // ���� ���� �ð� (2��)

    // [ ���� �÷��� ]
    private bool isRising_1 = false;         // ù��° ��� ������ ����
    private bool isRising_2 = false;
    private bool isPaused = false;           // ����� �Ͻ� ���� �������� ����
    private bool gameEnded = false;          // ������ ����Ǿ����� ����
    private bool hasPlayerEntered = false;   // �÷��̾ ���ۿ� ������ Ȯ��

    // [ �ӵ� ��� ]
    private float riseSpeed;  // ī�޶� ��� �ӵ�


    // 1. ��� �ӵ� ��� (��ǥ ��ġ���� ���� �ð��� �°�)
    void Start()
    {
        riseSpeed = (TargetYPosition - InitialRiseAmount) / TotalRiseDuration;
        print("��� �ӵ� = " + riseSpeed);

        StartCoroutine(GameStart_1());
    }

    void Update()
    {
        
    }

    IEnumerator GameStart_1()
    {
        Debug.Log("���� ����! ���� ���� �ö������.");

        yield return new WaitForSeconds(5f);
        isRising_1 = true; 
    }

}
