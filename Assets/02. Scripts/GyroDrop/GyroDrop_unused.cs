using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroDrop_unused : MonoBehaviour
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
    private bool isRising = false;           // ī�޶� ��� ������ ����
    private bool isPaused = false;           // ����� �Ͻ� ���� �������� ����
    private bool gameEnded = false;          // ������ ����Ǿ����� ����
    private bool hasPlayerEntered = false;   // �÷��̾ ���ۿ� ������ Ȯ��

    // [ �ӵ� ��� ]
    private float riseSpeed;  // ī�޶� ��� �ӵ�


    void Start()
    {
        Debug.Log("���� ����! ���� ���� �ö������.");

        // ��� �ӵ� ��� (��ǥ ��ġ���� ���� �ð��� �°�)
        riseSpeed = (TargetYPosition - InitialRiseAmount) / TotalRiseDuration;
        print("��� �ӵ� = " + riseSpeed);

        // 5�� �� ī�޶� ��� ����
        Invoke("StartRising", 5f); // 1

        // Ÿ�̸ӿ� ���� ���� ���� ��ƾ ����
        StartCoroutine(GameTimer()); // 2

        StartCoroutine(PlatformHoleRoutine()); // 3
    }

    void Update()
    {
        // ���� ȸ�� (������ ������ �ʰ� ��� ���� ��)
        if (isRising && !gameEnded)
        {
            disk.transform.Rotate(Vector3.up, RotationSpeed * Time.deltaTime);
        }
    }

    private void StartRising()
    {
        isRising = true; // ��� ����
        StartCoroutine(RiseCoroutine()); // ī�޶� ��� ��ƾ ����
    }

    private IEnumerator RiseCoroutine()
    {
        while (!gameEnded && cameraObject.transform.position.y < TargetYPosition)
        {
            if (!isPaused) // ������ ���� ��쿡�� ���
            {
                float currentY = cameraObject.transform.position.y;
                MoveCameraAndDisk(currentY + riseSpeed * Time.deltaTime);

                // 50 ���̿� �����ϸ� ��� ����
                if (currentY >= 50f && !isPaused)
                {
                    isPaused = true; // �Ͻ� ���� �÷���
                    Debug.Log("ī�޶� ����! 5�� ��� �� ����.");

                    yield return new WaitForSeconds(PauseDuration); // ���� �� ���
                    isPaused = false; // �ٽ� ��� ����
                }
            }
            yield return null;
        }

        // ��ǥ ���̿� �����ϸ� ���� Ŭ���� ó��
        if (!gameEnded) GameClear();
    }

    private void MoveCameraAndDisk(float newY)
    {
        // ī�޶�� ���� ��ġ ����
        newY = Mathf.Min(newY, TargetYPosition); // ��ǥ ���� �ʰ� ����
        cameraObject.transform.position = new Vector3(cameraObject.transform.position.x, newY, cameraObject.transform.position.z);
        disk.transform.position = new Vector3(disk.transform.position.x, newY - DiskCameraOffset, disk.transform.position.z);
    }

    private IEnumerator PlatformHoleRoutine()
    {
        while (!gameEnded)
        {
            GameObject selectedPiece = platformPieces[Random.Range(0, platformPieces.Length)];
            StartCoroutine(BlinkPlatform(selectedPiece));
            yield return new WaitForSeconds(10f); // ���� ���۱��� ���
        }
    }

    private IEnumerator BlinkPlatform(GameObject piece)
    {
        Renderer renderer = piece.GetComponent<Renderer>();
        Collider collider = piece.GetComponent<Collider>();

        // ���� ������ (5ȸ �ݺ�)
        for (int i = 0; i < 5; i++)
        {
            renderer.enabled = !renderer.enabled;
            yield return new WaitForSeconds(0.5f);
        }

        // ���� ����
        renderer.enabled = false;
        collider.enabled = false;

        // 5�� �� ����
        yield return new WaitForSeconds(5f);
        renderer.enabled = true;
        collider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasPlayerEntered)
        {
            hasPlayerEntered = true; // �� ���� ó��
            Debug.Log("���� ����! 5% �ϰ��մϴ�.");
            LowerHeight();
        }
    }

    private void LowerHeight()
    {
        float currentY = cameraObject.transform.position.y;
        float lowerY = currentY - (TargetYPosition * LowerPercentage);
        MoveCameraAndDisk(Mathf.Max(lowerY, 0)); // �ּ� ���� ����
    }

    private IEnumerator GameTimer()
    {
        yield return new WaitForSeconds(ClearTimeLimit);

        if (!gameEnded) GameOver(); // �ð� �ʰ� �� ���� ����
    }

    private void GameClear()
    {
        gameEnded = true; // ���� ����
        Debug.Log("���� Ŭ����! ������ �ϰ��մϴ�.");
        StartCoroutine(Drop(20)); // ������ �ϰ�
    }

    private void GameOver()
    {
        gameEnded = true; // ���� ����
        Debug.Log("���� ����! õõ�� �ϰ��մϴ�.");
        StartCoroutine(Drop(1)); // õõ�� �ϰ�
    }

    private IEnumerator Drop(float speedMultiplier)
    {
        while (cameraObject.transform.position.y > 0)
        {
            MoveCameraAndDisk(cameraObject.transform.position.y - riseSpeed * speedMultiplier * Time.deltaTime);
            yield return null;
        }
        Debug.Log("�ϰ� �Ϸ�.");
    }
}
