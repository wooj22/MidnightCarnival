using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class BalloonMapManager : MonoBehaviour
{
    public GameObject[] balloonScreens;  // 5���� �� ������Ʈ �迭

    List<GameObject> availableScreens;   // �̺�Ʈ �߻����� ���� �� ����Ʈ


    // [ �̺�Ʈ ǳ�� ]
    string[] balloonTags = { "red", "yellow", "green", "blue", "pink", "purple" }; // ǳ�� ���� �ش��ϴ� �±� ����Ʈ
    Balloon eventBalloon = null;        // ���� �̺�Ʈ ǳ�� 
    float eventBalloonTime = 30f;       // 30�ʸ��� �̺�Ʈ �߻�
    float eventDuration = 5f;           // 5�� �ȿ� �̺�Ʈ ǳ���� ��Ʈ���� �� => ���� �ÿ� ���� ����Ǿ�����
    float timer = 0f;


    // [ ���� �����Ȳ ǥ�� ]
    public Slider balloonSlider;        // ǳ�� ���� ǥ���� �����̴�
    public Text timerText;              // ���� �ð��� ǥ���� �ؽ�Ʈ UI

    private int totalBalloons = 0;      // ��ü ǳ�� ����
    private int poppedBalloons = 0;     // ���� ǳ�� ����

    private float gameDuration = 90f;   // 1�� 30��(90��) Ÿ�̸�
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
        }

        // �����̴� �ʱ�ȭ
        balloonSlider.maxValue = totalBalloons;
        balloonSlider.value = 0;


        // ó���� ��� ȭ���� ��� �����ϵ��� ����
        availableScreens = new List<GameObject>(balloonScreens);


        // BGM ���
        // _balloonSoundManager.PlayBGM();

        // BGM ��� �� �ȳ� ���� ����
        _balloonSoundManager.PlayBGMWithGuide(); // �ȳ� ���� ��� �߰�

    }




    // �� --------------------------------------- ��
    //
    // 1. [ Ÿ�̸� ������Ʈ ]
    // 2. [ �̺�Ʈ ǳ�� ó�� ]
    // 2-1. 30�ʸ��� �̺�Ʈ ǳ�� ����
    //     - Ÿ�̸� ����
    // 2-2. �̺�Ʈ ǳ���� �ð� ���� Ȯ��
    //     - �ð� �ʰ� �� ���� ������� ���ư��� ��
    //
    void Update()
    {
        if (!gameEnded)
        {
            gameDuration -= Time.deltaTime;
            UpdateTimerUI();

            if (gameDuration <= 0)
            {
                gameDuration = 0; // Ÿ�̸Ӱ� ������ �������� �ʵ��� 0���� ����
                GameOver();
            }
            UpdateTimerUI(); // Ÿ�̸� UI�� ��� ������Ʈ�Ͽ� 00:00�� ������ ��Ȯ�� �ð� ǥ��
        }

        
        timer += Time.deltaTime;

        if (timer >= eventBalloonTime)
        {
            SelectRandomEventBalloon();
            timer = 0f; 
        }

        if (eventBalloon != null && eventBalloon.isEventBalloon)
        {
            eventBalloon.timer += Time.deltaTime;
            if (eventBalloon.timer >= eventDuration)
            {
                ResetBalloon(eventBalloon); 
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

    // �� ���� ���� �� ���� ������ ���ƿ��� �Լ� �߰�
    void ReturnToMainScene()
    {
        SceneManager.LoadScene("Main"); // ���� �� �̸��� ������ �������ּ���
    }

    // �� ���� ���� ó�� (1�� 30�� ���� �������� ��)
    void GameOver()
    {
        gameEnded = true;
        UpdateTimerUI();  // Ÿ�̸Ӹ� 00:00���� ����
        Debug.Log("���� ����! ���� �ð� ���� ��� ǳ���� �Ͷ߸��� ���߽��ϴ�.");
        Invoke("ReturnToMainScene", 3f); // 3�� �� ���� ������ �̵�
    }

    // �� ���� Ŭ���� ó�� (��� ǳ���� �Ͷ߷��� ��)
    void GameClear()
    {
        gameEnded = true;
        Debug.Log("���� Ŭ����! ��� ǳ���� �Ͷ߷Ƚ��ϴ�.");
        Invoke("ReturnToMainScene", 3f); // 3�� �� ���� ������ �̵�
    }



    // -----------------------------------------------------------------------------------------------
    // �� �̺�Ʈ ǳ�� ���� �޼ҵ� --------------------------------------------------------------------



    // �� [ �̺�Ʈ ǳ�� ���� ���� ] ��
    // 
    // 1. �������� ��� ������ �� ����
    //   - ���õ� �� (������Ʈ)�� �ڽ� ǳ���� ������ 
    // 2. ���� ǳ�� ����
    //   - ���õ� ǳ���� �̺�Ʈ ǳ������ ����
    //   - �̺�Ʈ �߻��� ���� ������Ʈ�� ����Ʈ���� ����
    //
    void SelectRandomEventBalloon()
    {
        if (availableScreens.Count == 0)
        {
            Debug.Log("�̺�Ʈ�� �߻���ų ȭ���� �� �̻� ����.");
            return; 
        }

        int randomScreenIndex = Random.Range(0, availableScreens.Count);
        GameObject selectedScreen = availableScreens[randomScreenIndex];

        Balloon[] balloons = selectedScreen.GetComponentsInChildren<Balloon>();

        if (balloons.Length > 0)
        {
            int randomBalloonIndex = Random.Range(0, balloons.Length);
            eventBalloon = balloons[randomBalloonIndex];

            eventBalloon.isEventBalloon = true;
            eventBalloon.timer = 0f;
            eventBalloon.ChangeToEventBalloonAppearance();

            availableScreens.Remove(selectedScreen);

            Debug.Log("�̺�Ʈ ǳ���� Ȱ��ȭ �Ǿ���.");
        }
        else
        {
            Debug.Log("�ش� ���� ǳ���� ����"); // ���� ǳ���� ���ٸ� => ���� �� ���� ���ٴ� �Ŵϱ� �̺�Ʈ ǳ�� �߻� X
        }
    }



    // �� [ �̺�Ʈ ǳ�� �ı� �� : ���� ������ ǳ�� �ı� ] ��
    // 
    // 1. ���� ���� �±� ����
    // 2. �̺�Ʈ ǳ���� ���� ������ �ش� �±��� ǳ������ ã�� �ı�
    // 3. �̺�Ʈ ǳ�� ���� ����
    //
    void DestroyRandomColorBalloons(Balloon eventBalloon)
    {
        string randomTag = balloonTags[Random.Range(0, balloonTags.Length)];

        foreach (Transform balloon in eventBalloon.transform.parent)
        {
            if (balloon.CompareTag(randomTag))
            {
                Destroy(balloon.gameObject); 
            }
        }

        Debug.Log("���� : �̺�Ʈ ǳ���� ȿ���� " + randomTag + "���� ǳ���� ������!!");

        ResetBalloon(eventBalloon);
    }


    // �� [ �̺�Ʈ ǳ���� ��ġ���� �� ó�� ]
    // 
    // 1. ǳ�� �ı� 
    //   - �̺�Ʈ ǳ���� ���� ���� ���� ǳ�� �ı�
    //   - �Ϲ� ǳ���� ���� �ܼ� �ı� -> ����� Balloon ��ũ��Ʈ���� ���ٰ��� 
    // 2. �����̴� ������Ʈ
    //   - ��� ǳ���� �Ͷ߷ȴ��� Ȯ��
    public void OnBalloonPopped(Balloon balloon)
    {
        if (balloon.isEventBalloon)
        {
            DestroyRandomColorBalloons(balloon);
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

        if (poppedBalloons >= totalBalloons)
        {
            GameClear();
        }
    }



    // �� [ �ð� �ʰ� �� �Ǵ� ǳ���� ��Ʈ�� �� : �̺�Ʈ ǳ���� ���� ���·� ���� ]
    // 
    // - ���� ������� �ǵ���
    // - �ش� ���� ������Ʈ�� �ٽ� ��� �����ϰ� ����Ʈ�� �߰� => ����� ��Ȱ��ȭ 
    // - �̺�Ʈ ǳ�� ����
    void ResetBalloon(Balloon balloon)
    {
        Debug.Log("�̺�Ʈ ǳ���� ���� ������� ���ƿ���");
        balloon.isEventBalloon = false;
        balloon.ResetAppearance(); 

        // availableScreens.Add(balloon.transform.parent.gameObject);

        eventBalloon = null;
    }
}
