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
    private bool isRising = false;
    private bool gameEnded = false;          // ������ ����Ǿ����� ����
    private bool hasPlayerEntered = false;   // �÷��̾ ���ۿ� ������ Ȯ��
    private bool pausedOnce = false; // 50���� �� ���� ���߱� ���� �÷���


    // [ �ӵ� ��� ]
    private float riseSpeed;  // ī�޶� ��� �ӵ�


    void Start()
    {
        Debug.Log("���� ����! ���� ���� �ö������.");

        // ��� �ӵ� ��� (��ǥ ��ġ���� ���� �ð��� �°�)
        riseSpeed = (TargetYPosition - InitialRiseAmount) / TotalRiseDuration;
        print("��� �ӵ� = " + riseSpeed);

        // 5�� �� ī�޶� ��� ����
        Invoke("StartRising", 5f); 

        // Ÿ�̸ӿ� ���� ���� ���� ��ƾ ����
        StartCoroutine(GameTimer()); 

        StartCoroutine(PlatformHoleRoutine()); 
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
            float currentY = cameraObject.transform.position.y;

            // Y ��ǥ�� 30�� �� ���߰� 5�� ���
            if (currentY>=30f && !pausedOnce)
            {
                Debug.Log("ī�޶� ����! 5�� ��� �� ����.");
                yield return new WaitForSeconds(PauseDuration);

                pausedOnce = true; // ���� �� �ٽ� ������ �ʵ��� ����
            }

            // ī�޶�� ������ ��� �̵�
            MoveCameraAndDisk(currentY + riseSpeed * Time.deltaTime);
            yield return null;
        }


        // ��ǥ ���̿� �����ϸ� ���� Ŭ���� ó��
        if (!gameEnded) StartCoroutine(GameClear()); Debug.Log("���� ���̿� ���� =" + cameraObject.transform.position.y);
    }


    // �� [ ī�޶� - ���� ��ġ ] 
    // 
    private void MoveCameraAndDisk(float newY)
    {
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

    // �� [ �ð� �ʰ� �� ���� ���� ] 
    private IEnumerator GameTimer()
    {
        yield return new WaitForSeconds(ClearTimeLimit);

        if (!gameEnded) StartCoroutine(GameOver());
    }


    // --------------------------------------------------------------------------------------------------------------
    // �� [ ���� Ŭ����/���� ] �� -----------------------------------------------------------------------------------

    IEnumerator GameClear() 
    {
        gameEnded = true;
        Debug.Log("���� Ŭ����! 5�� �� ������ �ϰ��մϴ�.");
        yield return new WaitForSeconds(5f);
        StartCoroutine(Drop(25)); 
    }

    IEnumerator GameOver()
    {
        gameEnded = true; 
        Debug.Log("���� ����! 5�� �� õõ�� �ϰ��մϴ�.");
        yield return new WaitForSeconds(5f);
        StartCoroutine(Drop(1)); 
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
