using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroDropGameManager : MonoBehaviour
{
    public GameObject disk;                // ����
    public GameObject cameraObject;        // ī�޶� 
    public GameObject[] platformPieces;    // ���� ������

    private float rotationSpeed = 10f;     // ȸ�� �ӵ�
    private float riseSpeed;               // ��� �ӵ�

    private float targetYPosition = 525f;  // ī�޶��� ��ǥ Y ��ġ
    private float diskCameraOffset = 53f;  // ����-ī�޶� Y�� ������

    private float initialRiseAmount = 20f; // �ʱ� ��� Y��
    private float pauseDuration = 5f;      // ��� ���ߴ� �ð�
    private float totalRiseDuration = 90f; // ���� ��� �ð�

    private float lowerPercentage = 0.05f; // �ϰ� ���� (5%)
    private float clearTimeLimit = 120f;   // ���� �ð� 2�� ����

    private bool isRising = false;         // ��� ���� �÷���
    private bool isPaused = false;         // ��� ���� ���� �÷���
    private bool gameEnded = false;        // ���� ���� ���� �÷���


    void Start()
    {
        Debug.Log("������ ���۵Ǵ� ���� ���� �ö���ּ���");
        Invoke("StartRising", 5f); 

        riseSpeed = (targetYPosition - initialRiseAmount) / totalRiseDuration; // ��� �ӵ� ���

        StartCoroutine(GameTimer());           // ���� Ÿ�̸� ����
        StartCoroutine(PlatformHoleRoutine()); // ���� ���� ��ƾ ����
    }


    // [ ��Ȳ�� ���� �÷��� ���� ]
    // 
    void Update()
    {
        if (isRising && !gameEnded)
        {
            disk.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

            if (!isPaused)
            {
                float currentY = cameraObject.transform.position.y;
                if (currentY < 50f)
                {
                    MoveCameraAndDisk(currentY + riseSpeed * Time.deltaTime);
                }
                else if (currentY >= 50f && !isPaused)
                {
                    isPaused = true;
                    Invoke("ResumeRising", pauseDuration);
                }
            }
        }
    }

    private void MoveCameraAndDisk(float newY)
    {
        newY = Mathf.Min(newY, targetYPosition);
        cameraObject.transform.position = new Vector3(cameraObject.transform.position.x, newY, cameraObject.transform.position.z);
        disk.transform.position = new Vector3(disk.transform.position.x, newY - diskCameraOffset, disk.transform.position.z);
    }

    private void StartRising() => isRising = true;

    private void ResumeRising()
    {
        isPaused = false;
        StartCoroutine(RiseCoroutine());
    }

    private IEnumerator RiseCoroutine()
    {
        while (cameraObject.transform.position.y < targetYPosition && !gameEnded)
        {
            MoveCameraAndDisk(cameraObject.transform.position.y + riseSpeed * Time.deltaTime);
            yield return null;
        }

        if (!gameEnded)
        {
            GameClear(); // ��ǥ ���̿� ���� �� Ŭ���� ó��
        }
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

        // ������
        for (int i = 0; i < 5; i++)
        {
            renderer.enabled = !renderer.enabled;
            yield return new WaitForSeconds(0.5f);
        }

        // ���� �ո�
        renderer.enabled = false;
        collider.enabled = false;

        yield return new WaitForSeconds(5f); // 5�� ����

        // ���� �޿�
        renderer.enabled = true;
        collider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("������ ��ҽ��ϴ�! 5% �ϰ��մϴ�.");
            LowerHeight();
        }
    }

    private void LowerHeight()
    {
        float currentY = cameraObject.transform.position.y;
        float lowerY = currentY - (targetYPosition * lowerPercentage);
        MoveCameraAndDisk(Mathf.Max(lowerY, 0)); // �ּ� ���� 0���� ����
    }

    private IEnumerator GameTimer()
    {
        yield return new WaitForSeconds(clearTimeLimit);

        if (!gameEnded)
        {
            GameOver(); // �ð� �ʰ� �� ���� ���� ó��
        }
    }

    private void GameClear()
    {
        gameEnded = true;
        Debug.Log("���� Ŭ����! ������ �ϰ��մϴ�.");
        StartCoroutine(FastDrop());
    }

    private IEnumerator FastDrop()
    {
        yield return new WaitForSeconds(5f); // �޽��� ��� �ð�

        while (cameraObject.transform.position.y > 0)
        {
            MoveCameraAndDisk(cameraObject.transform.position.y - riseSpeed * 5 * Time.deltaTime); // ������ �ϰ�
            yield return null;
        }

        Debug.Log("�ϰ� �Ϸ�! �����մϴ�.");
    }

    private void GameOver()
    {
        gameEnded = true;
        Debug.Log("���� ����! ���� ���̿��� õõ�� �ϰ��մϴ�.");
        StartCoroutine(SlowDrop());
    }

    private IEnumerator SlowDrop()
    {
        while (cameraObject.transform.position.y > 0)
        {
            MoveCameraAndDisk(cameraObject.transform.position.y - riseSpeed * Time.deltaTime); // õõ�� �ϰ�
            yield return null;
        }

        Debug.Log("�ϰ� �Ϸ�.");
    }

}
