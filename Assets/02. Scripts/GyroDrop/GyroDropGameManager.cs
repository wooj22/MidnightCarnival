using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroDropGameManager : MonoBehaviour
{
    // [ ���� ������Ʈ ���� ]
    public GameObject disk;                // ����
    public GameObject cameraObject;        // ī�޶� 
    public GameObject[] platformPieces;    // ���� ������

    // [ ���͸��� ���� ]
    public Material material_gray;                 
    public Material material_green;                 
    public Material material_blue;                 
    public Material material_orange;
    public Material material_red;

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

        riseSpeed = (TargetYPosition - InitialRiseAmount) / TotalRiseDuration; // ��� �ӵ� ��� (��ǥ ��ġ���� ���� �ð��� �°�)
        print("��� �ӵ� = " + riseSpeed);

        Invoke("StartRising", 5f);    // 5�� �� ī�޶� 1�� ��� ����
        
        StartCoroutine(GameTimer());  // Ÿ�̸ӿ� ���� ���� ���� ��ƾ ����

        StartCoroutine(PlatformHoleRoutine()); 
    }

    void Update()
    {
        // ���� ȸ�� (������ ������ �ʰ� ��� ���� ��)
        if (isRising && !gameEnded)
        {
            disk.transform.Rotate(Vector3.up, RotationSpeed * Time.deltaTime);
        }

        UpdateDiskMaterial();  // ���� ���͸��� ����
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
            if (cameraObject.transform.position.y >= 40f)  // Y ��ǥ�� 40 �̻��� ���� ���� ����
            {
                GameObject selectedPiece = platformPieces[Random.Range(0, platformPieces.Length)];
                StartCoroutine(BlinkPlatform(selectedPiece));
            }
            yield return new WaitForSeconds(10f);
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
        // collider.enabled = false;

        // 5�� �� ����
        yield return new WaitForSeconds(5f);
        renderer.enabled = true;
        // collider.enabled = true;
    }

    private void UpdateDiskMaterial()
    {
        float heightPercentage = cameraObject.transform.position.y / TargetYPosition;

        Material newMaterial = null;
        if (heightPercentage >= 0.8f)      newMaterial = material_red;
        else if (heightPercentage >= 0.6f) newMaterial = material_orange;
        else if (heightPercentage >= 0.4f) newMaterial = material_blue;
        else if (heightPercentage >= 0.2f) newMaterial = material_green;
        else                               newMaterial = material_gray;

        if (newMaterial != null)
        {
            foreach (GameObject piece in platformPieces)
            {
                piece.GetComponent<Renderer>().material = newMaterial;
            }
        }
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

        RestoreAllPlatformPieces(); // ��� ������ ������ �޿�� (����)

        yield return new WaitForSeconds(5f);
        StartCoroutine(Drop(25)); 
    }

    IEnumerator GameOver()
    {
        gameEnded = true; 
        Debug.Log("���� ����! 5�� �� õõ�� �ϰ��մϴ�.");

        RestoreAllPlatformPieces(); // ��� ������ ������ �޿�� (����)

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


    // ��� ������ ���󺹱��ϴ� �޼���
    private void RestoreAllPlatformPieces()
    {
        foreach (GameObject piece in platformPieces)
        {
            Renderer renderer = piece.GetComponent<Renderer>();
            Collider collider = piece.GetComponent<Collider>();

            renderer.enabled = true;  // ������ ���̰� ��
            collider.enabled = true;  // ��ȣ�ۿ� �����ϰ� ��
        }
    }
}
