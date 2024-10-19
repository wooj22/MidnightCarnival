using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GyroDropGameManager : MonoBehaviour
{
    // [ ���� ������Ʈ ���� ]
    public GameObject disk;                // ����
    public GameObject cameraObject;        // ī�޶� 
    public GameObject XRoom;
    public GameObject[] platformPieces;    // ���� ������

    // [ UI ��� ]
    public Text TimerText;                 // Ÿ�̸� �ؽ�Ʈ
    public Slider HeightSlider;             // ���� �����̴�

    // Ÿ�̸� ����
    private float remainingTime;

    // [ ���͸��� ���� ]
    public Material material_gray;                 
    public Material material_green;                 
    public Material material_blue;                 
    public Material material_orange;
    public Material material_red;

    // [ ���� ��� ]
    private const float TargetYPosition = 525f;  // ī�޶� ��ǥ Y ��ġ
    private const float DiskCameraOffset = 53f;  // ���ǰ� ī�޶� �� ������
    private const float PauseDuration = 5f;      // ��� ���� �ð�
    private const float TotalRiseDuration = 90f; // ��ü ��¿� �ɸ��� �ð�

    private const float LowerPercentage = 0.05f; // �ϰ� ���� (5%)
    private const float ClearTimeLimit = 120f;   // ���� ���� �ð� (2��)

    // [ ���� �÷��� ]
    private bool isRising = false;
    private bool gameEnded = false;          // ������ ����Ǿ����� ����
    // private bool hasPlayerEntered = false;   // �÷��̾ ���ۿ� ������ Ȯ��
    private bool pausedOnce = false;         // 50���� �� ���� ���߱� ���� �÷���
    bool IsLower = false;            // �ϰ� ���� �÷���

    // [ ȸ�� �� �ӵ� ]
    private float RotationSpeed = 10f;     // ���� ȸ�� �ӵ�
    private int RotationDirection = 1;  // 1: �ð� ����, -1: �ݽð� ����
    private float riseSpeed;  // ī�޶� ��� �ӵ�



    void Start()
    {
        Debug.Log("���� ����! ���� ���� �ö������.");

        riseSpeed = (TargetYPosition - 10f) / TotalRiseDuration; // ��� �ӵ� ��� (��ǥ ��ġ���� ���� �ð��� �°�)
        print("��� �ӵ� = " + riseSpeed);

        remainingTime = ClearTimeLimit; // ���� �ð� �ʱ�ȭ

        Invoke("StartRising", 5f);    // 5�� �� ī�޶� 1�� ��� ����
        
        StartCoroutine(GameTimer());  

        StartCoroutine(PlatformHoleRoutine());

        // Y ��ǥ�� 50 �̻��� ������ ȸ�� ������ �����ϴ� ��ƾ ����
        StartCoroutine(ChangeRotationDirectionRoutine());

    }

    void Update()
    {
        // ���� ���� �� ȸ�� ó��
        if (isRising && !gameEnded)
        {
            disk.transform.Rotate(Vector3.up, RotationSpeed * RotationDirection * Time.deltaTime);

            UpdateDiskMaterial(); // ���̿� ���� ���͸���� �ӵ� ����

            // �����̴� ������Ʈ (0���� 1�� ���� ���)
            HeightSlider.value = (cameraObject.transform.position.y / TargetYPosition);

            // Ÿ�̸� �ؽ�Ʈ ������Ʈ
            UpdateTimerText();
        }

        // �ְ� ���̿� �������� �� : ���� �޿�� 
        if (cameraObject.transform.position.y >= TargetYPosition) 
        {
            RestoreAllPlatformPieces(); 
        }
    }


    private void UpdateTimerText()
    {
        if (pausedOnce)
        {
            remainingTime -= Time.deltaTime;

            if (remainingTime <= 0)
            {
                remainingTime = 0;  // Ÿ�̸Ӱ� ������ �������� �ʵ��� 0���� ����
            }
            else
            {
                int minutes = Mathf.FloorToInt(remainingTime / 60);
                int seconds = Mathf.FloorToInt(remainingTime % 60);
                TimerText.text = $"{minutes:00}:{seconds:00}"; // "MM:SS" �������� �ؽ�Ʈ ������Ʈ
            }
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

                pausedOnce = true; 
            }


            // ī�޶�� ������ ��� �̵�
            MoveCameraAndDisk(currentY + riseSpeed * Time.deltaTime);
            yield return null;
        }

        // ��ǥ ���̿� �����ϸ� ���� Ŭ���� ó��
        if (!gameEnded) StartCoroutine(GameClear()); Debug.Log("�ְ� ���̿� ���� =" + cameraObject.transform.position.y);
    }


    // �� [ ī�޶� - ���� ��ġ ] 
    // 
    private void MoveCameraAndDisk(float newY)
    {
        newY = Mathf.Min(newY, TargetYPosition); // ��ǥ ���� �ʰ� ����
        cameraObject.transform.position = new Vector3(cameraObject.transform.position.x, newY, cameraObject.transform.position.z);
        disk.transform.position = new Vector3(disk.transform.position.x, newY - DiskCameraOffset, disk.transform.position.z);
        XRoom.transform.position = new Vector3(XRoom.transform.position.x, newY, XRoom.transform.position.z); 
    }

    

    private IEnumerator PlatformHoleRoutine()
    {
        while (!gameEnded)
        {
            if (cameraObject.transform.position.y >= 40f)  // Y ��ǥ�� 40 �̻��� ���� ���� ����
            {
                int numHoles = GetNumberOfHolesBasedOnHeight(); // ���̿� ���� ���� ���� ����

                List<GameObject> selectedPieces = new List<GameObject>();
                for (int i = 0; i < numHoles; i++)
                {
                    GameObject piece;
                    do
                    {
                        piece = platformPieces[Random.Range(0, platformPieces.Length)];
                    } while (selectedPieces.Contains(piece)); // �ߺ� ����

                    selectedPieces.Add(piece);
                }

                foreach (GameObject piece in selectedPieces)
                {
                    StartCoroutine(BlinkPlatform(piece));
                }
            }
            yield return new WaitForSeconds(10f);
        }
    }

    // �� ���̿� ���� ���� ���� ����
    private int GetNumberOfHolesBasedOnHeight()
    {
        float height = cameraObject.transform.position.y;

        if (height >= 300f)
        {
            return Random.Range(2, 4); // 2�� �Ǵ� 3��
        }
        else if (height >= 100f)
        {
            return Random.Range(1, 3); // 1�� �Ǵ� 2��
        }
        else
        {
            return 1; // �⺻ 1��
        }
    }


    private IEnumerator BlinkPlatform(GameObject piece)
    {
        PlatformPiece platformPiece = piece.GetComponent<PlatformPiece>();
        if (platformPiece != null)
        {
            platformPiece.StartBlinking(0.5f, 5); // ������ �Լ� ȣ��
        }

        // ������ ������ �� �ϰ� ����
        // CanLower = true;

        yield return new WaitForSeconds(5f); // 5�� �� ���� 

        // �ٽ� ������ ���̰� �ϰ� �浹 �����ϰ� ��
        piece.GetComponent<Renderer>().enabled = true;
        piece.GetComponent<Collider>().enabled = false; 

        // CanLower = false;
    }


    private void UpdateDiskMaterial()
    {
        float heightPercentage = cameraObject.transform.position.y / TargetYPosition;

        Material newMaterial = null;
        if (heightPercentage >= 0.8f) { newMaterial = material_red; RotationSpeed = 10f * 2.5f; } // �ӵ� 2.5��
        else if (heightPercentage >= 0.6f) { newMaterial = material_orange; RotationSpeed = 10f * 2f; } // �ӵ� 2��
        else if (heightPercentage >= 0.4f) { newMaterial = material_blue; RotationSpeed = 10f * 1.7f; } // �ӵ� 1.7��
        else if (heightPercentage >= 0.2f) { newMaterial = material_green; RotationSpeed = 10f * 1.3f; } // �ӵ� 1.3��
        else { newMaterial = material_gray; }

        if (newMaterial != null)
        {
            foreach (GameObject piece in platformPieces)
            {
                piece.GetComponent<Renderer>().material = newMaterial;
            }
        }
    }

    // �� ȸ�� ������ 8~12�� ���� ���� �������� �����ϴ� ��ƾ
    private IEnumerator ChangeRotationDirectionRoutine()
    {
        while (!gameEnded)
        {
            if (cameraObject.transform.position.y >= 50f)
            {
                RotationDirection *= -1;  // ȸ�� ���� ����
                Debug.Log("ȸ�� ���� ����! ���� ����: " + (RotationDirection == 1 ? "�ð�" : "�ݽð�"));
            }

            float randomWaitTime = Random.Range(8f, 12f);  // 8~12�� ���� ���� ���
            yield return new WaitForSeconds(randomWaitTime);
        }
    }

    
    // �� [ �ð� �ʰ� �� ���� ���� ] 
    private IEnumerator GameTimer()
    {
        yield return new WaitForSeconds(ClearTimeLimit);

        if (!gameEnded) StartCoroutine(GameOver());
    }


    // --------------------------------------------------------------------------------------------------------------
    // �� [ �浹 ���� ] �� -----------------------------------------------------------------------------------

    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && CanLower && !hasPlayerEntered)
        {
            hasPlayerEntered = true; // �� ���� ó��
            Debug.Log("���� ����! 5% �ϰ��մϴ�.");
            LowerHeight();
        }
        Debug.Log("�浹�� ��.");
    }
    */

    public void LowerHeight()
    {
        float currentY = cameraObject.transform.position.y;
        float lowerY = currentY - (TargetYPosition * LowerPercentage);
        MoveCameraAndDisk(Mathf.Max(lowerY, 0)); // �ּ� ���� ����
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


    // ��� ������ ���󺹱��ϴ� �޼���
    private void RestoreAllPlatformPieces()
    {
        foreach (GameObject piece in platformPieces)
        {
            piece.GetComponent<Renderer>().enabled = true; // ������ ���̰� �� 
            // piece.GetComponent<Collider>().enabled = true; // ��ȣ�ۿ� �����ϰ� �� 
        }
    }
}
