using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BalloonMapManager : MonoBehaviour
{
    /*
    // [ ǳ�� ���� �� ��ġ ]
    public GameObject[] balloonPrefabs;  // 6�� ������ ǳ�� ������ �迭 (�� ���� �´� �±׸� ����)
    private int totalBalloons = 120;     // ��ü ǳ���� ����
    public int balloonsPerScreen = 30;   // �� ȭ��� ������ ǳ�� ��
    public Plane[] planes;               // 4���� ȭ���� ��Ÿ���� Plane ������Ʈ �迭

    // [ ���� �ð�, ���� Ŭ���� ���� ]
    public float timeLimit = 90f;        // ���� �ð� (1�� 30��)
    private float timeRemaining;         // ���� �ð�
    private bool gameEnded = false;      // ������ �������� ���� üũ
    public GameObject gameClearUI;       // ���� Ŭ���� �� ������ UI

    // [ ������ �ý��� ]
    public Slider progressBar;   // �������� ��Ÿ���� �����̴� UI
    // private int totalBalloons;   // ��ü ǳ���� ����
    private int poppedBalloons;  // ��Ʈ�� ǳ���� ����


    void Start()
    {
        // SpawnBalloons();

        timeRemaining = timeLimit;  // ���� �ð��� ���� �ð����� ����

        // totalBalloons = GameObject.FindGameObjectsWithTag("Balloon").Length; // ��ü ǳ�� ���� ����
        poppedBalloons = 0;   // ��Ʈ�� ǳ���� ���� �ʱ�ȭ
        UpdateProgressBar();  // �ʱ� ������ ���� ������Ʈ
    }


    // [ ���� �ð� üũ ]
    //  - �ð� ���� (�� ����)
    //  - ���� ���� ����: �ð� �ʰ� �� ���� ���� / ��� ǳ���� ��Ʈ���� ���� Ŭ����
    //
    void Update()
    {
        if (!gameEnded)
        {
            timeRemaining -= Time.deltaTime;

            if (timeRemaining <= 0)
            {
                EndGame(false); 
            }

            if (GameObject.FindGameObjectsWithTag("Balloon").Length == 0)
            {
                EndGame(true); 
            }
        }
    }


    // �� [ ǳ�� -> �� ȭ���� Plane ���� �����ϰ� ��ġ ] 
    // 
    
    void SpawnBalloons()
    {
        for (int i = 0; i < planes.Length; i++)
        {  // �� ȭ�� (Plane)���� �ݺ�
            for (int j = 0; j < balloonsPerScreen; j++)
            {  
                // �� ȭ��� 30���� ǳ�� ����
                // ������ ��ġ�� Plane�� ���� ������ ����
                Vector3 randomPosition = GetRandomPositionOnPlane(planes[i]);

                // ������ ǳ�� �������� �����Ͽ� �ش� ��ġ�� ����
                GameObject balloon = Instantiate(balloonPrefabs[Random.Range(0, balloonPrefabs.Length)], randomPosition, Quaternion.identity);
            }
        }
    }
    

    // Plane�� ���� ������ ������ ��ġ�� ��ȯ�ϴ� �Լ�
    
    Vector3 GetRandomPositionOnPlane(Plane plane)
    {
        // Plane�� ������ �������� X, Z ��ǥ�� �������� ����
        float randomX = Random.Range(plane.bounds.min.x, plane.bounds.max.x);
        float randomZ = Random.Range(plane.bounds.min.z, plane.bounds.max.z);

        // Plane�� ����(Y ��)�� �����ϹǷ�, �� ���� ������ ä�� ��ȯ
        return new Vector3(randomX, plane.transform.position.y, randomZ);
    }

    Vector3 GetObjectBounds(GameObject obj)
    {
        // ������Ʈ���� Collider ������Ʈ ��������
        Collider collider = obj.GetComponent<Collider>();

        if (collider != null)
        {
            // bounds�� Collider�� ��� ���ڸ� ����
            Bounds bounds = collider.bounds;

            // ��� ������ �ּ�, �ִ� ��ǥ ���
            Vector3 min = bounds.min;  // �ּ� ��ǥ
            Vector3 max = bounds.max;  // �ִ� ��ǥ
            Vector3 center = bounds.center;  // �߽� ��ǥ

            Debug.Log("Min Bounds: " + min);
            Debug.Log("Max Bounds: " + max);
            Debug.Log("Center Bounds: " + center);

            // ��� ������ �߽� ��ǥ�� ��ȯ
            return center;
        }
        else
        {
            Debug.LogError("Collider�� �����ϴ�.");
            return Vector3.zero;  // Collider�� ���� ��� �⺻�� ��ȯ
        }
    }

    void EndGame(bool cleared)
    {
        gameEnded = true;

        if (cleared)
        { 
            gameClearUI.SetActive(true); // Ŭ���� UI ǥ��
        }
        else
        {
            // ���� ���� ó��
        }
    }



    // �� [ ������ ������Ʈ ]
    void UpdateProgressBar()
    {
        progressBar.value = (float)poppedBalloons / totalBalloons; // ����! (0 ~ 1 ���� ��)
    }

    // �� [ ǳ�� ������ �� ȣ�� ]
    void OnBalloonPopped()
    {
        poppedBalloons++;
        UpdateProgressBar();
    }

    */
}
