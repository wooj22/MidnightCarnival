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

    // [ ���� ���� ��, ǳ�� ������ ���� ] 
    private string BalloonTag = "Balloon";
    private string eventBalloonTag = "EventBalloon"; 
    public GameObject Barricade1;  
    public GameObject Barricade2;

    // [ ���� Ŭ���� ��, ���� ������ ���� ]
    public GameObject Doll;
    private List<GameObject> dollObjects = new List<GameObject>(); // �ڽ� ������Ʈ ����Ʈ

    // ------------------------------------------------------------------------------------------------------------


    void Start()
    {
        // ���� ���� �� ��ü ǳ�� ���� ���
        foreach (GameObject screen in balloonScreens)
        {
            totalBalloons += screen.GetComponentsInChildren<Balloon>().Length;
            
        }
        print("��ü ǳ���� ���� = " + totalBalloons);

        // �����̴� �ʱ�ȭ
        balloonSlider.maxValue = totalBalloons;
        balloonSlider.value = 0;

        // BGM ��� �� �ȳ� ���� ����
        _balloonSoundManager.PlayBGMWithGuide(StartGameAfterGuide); // �ȳ� ���� ��� �߰�

        // ���� Ŭ���� ��, ������ �Ҵ� 
        int childCount = Doll.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            GameObject child = Doll.transform.GetChild(i).gameObject;
            dollObjects.Add(child); // ����Ʈ�� �߰�
        }

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
        if (gameStarted && !gameEnded) // �� ������ ���۵ǰ� ������� ���� ���¿�����
        {
            UpdateGameTimer();
            CheckEventBalloonTimer();
        }

    }

    void UpdateGameTimer()
    {
        gameDuration -= Time.deltaTime;
        UpdateTimerUI();

        if (gameDuration <= 0)
        {
            gameDuration = 0; // Ÿ�̸Ӱ� ������ �������� �ʵ��� 0���� ����
            GameOver();
        }
    }

    void CheckEventBalloonTimer()
    {
        timer += Time.deltaTime;
        if (timer >= eventBalloonTime)
        {
            SelectRandomEventBalloon();
            timer = 0f;
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

        DropDall();

        Invoke("ReturnToMainScene", 5f); 
    }

    // �� ���� ���� ó�� (���ѽð� ���� �������� ��)
    void GameOver()
    {
        gameEnded = true;
        UpdateTimerUI();  // Ÿ�̸Ӹ� 00:00���� ����
        Debug.Log("���� ����! ���� �ð� ���� ��� ǳ���� �Ͷ߸��� ���߽��ϴ�.");

        DropBalloon();

        Invoke("ReturnToMainScene", 5f); 
    }

    // ���� ���� �� ���� ������ ���ư��� (�� �Ŵ������� ����)
    void ReturnToMainScene()
    {
        _balloonSceneManager.LoadMainMenuMap();
    }




    // -----------------------------------------------------------------------------------------------
    // �� �̺�Ʈ ǳ�� ���� �޼ҵ� --------------------------------------------------------------------


    // �� [ �̺�Ʈ ǳ�� ���� ���� ] �� : Ư�� ���� ǳ���� ���ٸ� �ٸ� ���� �õ��ϵ��� ������ 
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
        /*
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

            // Balloon eventBalloon = eventBalloonObject.GetComponent<Balloon>(); // �̺�Ʈ ǳ���� ����Ʈ ���� �� �ڵ� 
            Balloon eventBalloon = eventBalloonObject.transform.GetChild(0).GetComponent<Balloon>();
            eventBalloon.isEventBalloon = true;

            Debug.Log("�̺�Ʈ ǳ���� �����Ǿ����ϴ�.");
        }
        else
        {
            Debug.Log("�ش� ���� ǳ���� �����ϴ�.");
        }
        */

        List<Balloon> availableBalloons = new List<Balloon>();

        foreach (GameObject screen in balloonScreens)
        {
            availableBalloons.AddRange(screen.GetComponentsInChildren<Balloon>());
        }

        if (availableBalloons.Count > 0)
        {
            int randomIndex = Random.Range(0, availableBalloons.Count);
            Balloon selectedBalloon = availableBalloons[randomIndex];

            Vector3 balloonPosition = selectedBalloon.transform.position;
            Quaternion balloonRotation = selectedBalloon.transform.rotation;

            Destroy(selectedBalloon.gameObject);

            GameObject eventBalloonObject = Instantiate(eventBalloonPrefab, balloonPosition, balloonRotation);
            Balloon eventBalloon = eventBalloonObject.transform.GetChild(0).GetComponent<Balloon>();
            eventBalloon.isEventBalloon = true;

            Debug.Log("�̺�Ʈ ǳ���� �����Ǿ����ϴ�.");
        }
        else
        {
            Debug.Log("��� ���� ǳ���� �����ϴ�.");
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



    // [ ���� ���� �� : ǳ���� ������ ]
    //
    // 1. �̺�Ʈ ǳ���� ��� ���� 
    // 2. �߷� �Ҵ� : �ʱ� ȸ���°� ���� �� �߰� 
    // 3. �ݶ��̴� is trigger ���� 
    // 4. ǳ���� ȭ�� ��� ���� ����, �ٸ�����Ʈ Ȱ��ȭ 
    // 
    public void DropBalloon()
    {
        RemoveEventBalloons();

        GameObject[] objects = GameObject.FindGameObjectsWithTag(BalloonTag);
        foreach (GameObject obj in objects)
        {
            if (obj.GetComponent<Rigidbody>() == null)
            {
                Rigidbody rb = obj.AddComponent<Rigidbody>();
                rb.useGravity = true;
                rb.mass = 0.5f;
                rb.drag = 0.2f;
                rb.angularDrag = 0.1f;

                Vector3 randomTorque = new Vector3( Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f) );
                rb.AddTorque(randomTorque, ForceMode.Impulse);

                Vector3 randomForce = new Vector3( Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f) );
                rb.AddForce(randomForce, ForceMode.Impulse);

                // rb.constraints = RigidbodyConstraints.None; // �ʿ� �� ȸ�� ���� ����
            }

            Collider[] colliders = obj.GetComponents<Collider>();
            foreach (Collider col in colliders)
            {
                col.isTrigger = false;  // isTrigger ����
            }
        }

        if (Barricade1 != null) Barricade1.SetActive(true);
        if (Barricade2 != null) Barricade2.SetActive(true);
    }

    // 'EventBalloon' �±׸� ���� ��� ������Ʈ ����
    public void RemoveEventBalloons()
    {
        GameObject[] eventBalloons = GameObject.FindGameObjectsWithTag(eventBalloonTag);

        foreach (GameObject balloon in eventBalloons)
        {
            Destroy(balloon);  
        }
    }


    // [ ���� Ŭ���� �� : ������ ������ ]
    // 
    public void DropDall()
    {
        // Ȥ�� �𸣴� ��� ǳ���� ����...?

        // �߷� ��ȭ (����, ��� Rigidbody�� ����)
        // Physics.gravity = new Vector3(0, -20f, 0);  // �⺻ �߷��� -9.81f

        if (Barricade1 != null) Barricade1.SetActive(true);
        if (Barricade2 != null) Barricade2.SetActive(true);

        // ������Ʈ Ȱ��ȭ �� ������ ���� ȸ�� ����
        foreach (GameObject obj in dollObjects)
        {
            obj.SetActive(true);

            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false; // �߷� Ȱ��ȭ
                rb.useGravity = true;
                rb.mass = 0.2f;
                rb.drag = 0.05f;
                rb.angularDrag = 0.05f;

                Vector3 randomTorque = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
                rb.AddTorque(randomTorque, ForceMode.Impulse);

                Vector3 randomForce = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-6f, -10f), Random.Range(-0.5f, 0.5f));
                rb.AddForce(randomForce, ForceMode.Impulse);
            }
        }
    }
}
