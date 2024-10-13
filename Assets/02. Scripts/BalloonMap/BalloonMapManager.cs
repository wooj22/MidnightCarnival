using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class BalloonMapManager : MonoBehaviour
{
    public GameObject[] balloonScreens;  // �� ������Ʈ �迭
    public GameObject Player;            // �÷��̾� ������Ʈ
    public GameObject eventBalloonPrefab; // �̺�Ʈ ǳ�� ������Ʈ 

    [SerializeField] BalloonSceneManager _balloonSceneManager;      // ǳ���� �� �Ŵ��� 

    // [ �̺�Ʈ ǳ�� ]
    float eventBalloonTime = 12f;       // 12�ʸ��� �̺�Ʈ �߻�
    float timer = 0f;


    // [ ���� �����Ȳ ǥ�� ]
    public Slider balloonSlider;        // ǳ�� ���� ǥ���� �����̴�
    public Text timerText;              // ���� �ð��� ǥ���� �ؽ�Ʈ UI

    private int totalBalloons = 0;      // ��ü ǳ�� ����
    private int poppedBalloons = 0;     // ���� ǳ�� ����

    private float gameDuration = 30f;   // 30�� Ÿ�̸�
    private bool gameStarted = false;   // ������ ���۵Ǿ����� ����
    private bool gameEnded = false;     // ���� ���� ����

    // [ ���� ���� ]
    [SerializeField] BalloonSoundManager _balloonSoundManager;


    // ------------------------------------------------------------------------------------------------------------


    void Start()
    {
        // ���� ���� �� ��ü ǳ�� ���� ���
        foreach (GameObject screen in balloonScreens)
        {
            totalBalloons += screen.GetComponentsInChildren<Balloon>().Length;
            print("��ü ǳ���� ���� = " + totalBalloons);
        }

        // �����̴� �ʱ�ȭ
        balloonSlider.maxValue = totalBalloons;
        balloonSlider.value = 0;

        // BGM ��� �� �ȳ� ���� ����
        _balloonSoundManager.PlayBGMWithGuide(StartGameAfterGuide); // �ȳ� ���� ��� �߰�

    }


    // �� �ȳ� ������ ���� �� ȣ��� �Լ�: ������ ����
    void StartGameAfterGuide()
    {
        Debug.Log("�ȳ� ������ �������ϴ�. ������ �����մϴ�.");
        gameStarted = true; 

        Player.SetActive(true); Debug.Log("�÷��̾ Ȱ��ȭ �˴ϴ�.");
    }


    // �� --------------------------------------- ��
    //
    // 1. [ Ÿ�̸� ������Ʈ ] 
    // 2. [ �̺�Ʈ ǳ�� ó�� ]
    // 2-1. ���� �ð����� �̺�Ʈ ǳ�� ����
    //     - Ÿ�̸� ����
    //
    void Update()
    {
        if (gameStarted && !gameEnded) // �� ������ ���۵ǰ� ������� ���� ���¿����� ����
        {
            gameDuration -= Time.deltaTime;
            UpdateTimerUI();

            if (gameDuration <= 0)
            {
                gameDuration = 0; // Ÿ�̸Ӱ� ������ �������� �ʵ��� 0���� ����
                GameOver();
            }

            timer += Time.deltaTime;

            if (timer >= eventBalloonTime)
            {
                SelectRandomEventBalloon();
                timer = 0f;
            }

        }
    }



    // --------------------------------------------------------------------------------------------
    // �� ���� ���� ���� ����     -----------------------------------------------------------------


    // [ Ÿ�̸� UI ������Ʈ ]
    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(gameDuration / 60f);
        int seconds = Mathf.FloorToInt(gameDuration % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // �� ���� Ŭ���� ó�� (��� ǳ���� �Ͷ߷��� ��)
    void GameClear()
    {
        gameEnded = true;
        Debug.Log("���� Ŭ����! ��� ǳ���� �Ͷ߷Ƚ��ϴ�.");
        Invoke("ReturnToMainScene", 3f); 
    }

    // �� ���� ���� ó�� (���ѽð� ���� �������� ��)
    void GameOver()
    {
        gameEnded = true;
        UpdateTimerUI();  // Ÿ�̸Ӹ� 00:00���� ����
        Debug.Log("���� ����! ���� �ð� ���� ��� ǳ���� �Ͷ߸��� ���߽��ϴ�.");
        Invoke("ReturnToMainScene", 3f); 
    }

    // ���� ���� �� ���� ������ ���ư��� (�� �Ŵ������� ����)
    void ReturnToMainScene()
    {
        _balloonSceneManager.LoadMainMenuMap();
    }




    // -----------------------------------------------------------------------------------------------
    // �� �̺�Ʈ ǳ�� ���� �޼ҵ� --------------------------------------------------------------------


    // �� [ �̺�Ʈ ǳ�� ���� ���� ] ��
    // 
    // 1. �������� ��� ������ �� ����
    //   - ���õ� �� (������Ʈ)�� �ڽ� ǳ���� ������ 
    // 2. ���� ǳ�� ����
    // 2-1. ���õ� ǳ���� �̺�Ʈ ǳ������ ���� (������Ʈ ��ü�� �̺�Ʈ ǳ�� ���������� ��ü)
    //      - ���� ǳ�� ��ġ �� ȸ���� ����
    //      - ���� ǳ�� ����
    //      - �̺�Ʈ ǳ�� ������ ����
    //      - ���� ������ �̺�Ʈ ǳ���� �߰� �۾��� �ʿ��� ���, Balloon Ŭ������ ����
    //
    void SelectRandomEventBalloon()
    {
        int randomScreenIndex = Random.Range(0, balloonScreens.Length);
        Balloon[] balloons = balloonScreens[randomScreenIndex].GetComponentsInChildren<Balloon>();

        if (balloons.Length > 0)
        {
            int randomBalloonIndex = Random.Range(0, balloons.Length);
            Balloon selectedBalloon = balloons[randomBalloonIndex];

            Vector3 balloonPosition = selectedBalloon.transform.position;
            Quaternion balloonRotation = selectedBalloon.transform.rotation;

            Destroy(selectedBalloon.gameObject);

            GameObject eventBalloonObject = Instantiate(eventBalloonPrefab, balloonPosition, balloonRotation);

            Balloon eventBalloon = eventBalloonObject.GetComponent<Balloon>();
            eventBalloon.isEventBalloon = true;

            Debug.Log("�̺�Ʈ ǳ���� �����Ǿ����ϴ�.");
        }
        else
        {
            Debug.Log("�ش� ���� ǳ���� �����ϴ�.");
        }
    }



    // �� [ ǳ���� ��ġ���� �� ó�� ]
    // 
    // 1. ǳ�� �ı� 
    //   - �̺�Ʈ ǳ���� �� : �ð� 8�� �߰�
    //   - �Ϲ� ǳ���� �� : �ܼ� �ı� -> ����� Balloon ��ũ��Ʈ���� ���ٰ��� 
    // 2. �����̴� ������Ʈ
    //   - ��� ǳ���� �Ͷ߷ȴ��� Ȯ��
    public void OnBalloonPopped(Balloon balloon)
    {
        if (balloon.isEventBalloon)
        {
            AddTime();

            _balloonSoundManager.PlaySFX();

            // �̺�Ʈ ǳ�� SFX �ֱ� 
        }
        else
        {
            // Destroy(balloon.gameObject);
            _balloonSoundManager.PlaySFX();
        }


        poppedBalloons++;
        balloonSlider.value = poppedBalloons;

        print("���� ǳ�� ���� : " + (totalBalloons - poppedBalloons));

        if (poppedBalloons >= totalBalloons)
        {
            GameClear();
        }
    }


    // �� [ �̺�Ʈ ǳ�� �ı� �� : �ð� 6�� �߰� ] ��
    //
    void AddTime()
    {
        gameDuration += 6f;
        print("�ð��� 6�� �߰��Ǿ����ϴ�");
    }

}
