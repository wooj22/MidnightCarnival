using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircusGameManager : MonoBehaviour
{
    [Header("Levels")]
    [SerializeField] public int currentLevel;              // ���� ����
    [SerializeField] public float currentLaserCycle;       // ���� ������ �߻� �ֱ�
    [SerializeField] public float currentLaserSpeed;       // ���� ������ �ӵ�

    [Header("MapData")]
    [SerializeField] float gamePlayTime;                   // ��ü ���� ���� �ð� 120��
    [SerializeField] int maxLaserCount;                    // ��ü ������ ���� 29��
    [SerializeField] Transform laserParent;                // ������ ���� ��ġ
    [SerializeField] List<GameObject> laserPrefabList;     // �� ������ ������ 3��
    [SerializeField] List<float> levelUpTime;              // ���� �б� ����
    [SerializeField] List<float> laserCycleList;           // 3���� ����Ŭ�ֱ� ������
    [SerializeField] List<float> laserSpeedList;           // 3���� ���ǵ� ������

    [Header("Managers")]
    [SerializeField] CircusUIManager _circusUIManager;
    [SerializeField] CircusSoundManager _circusSoundManager;
    [SerializeField] CircusSceneManager _circusSceneManager;


    private void Start()
    {
        CircusMapStartSetting();
        StartCoroutine(CircusGame());
    }

    /*-------------------- Gaming ----------------------*/
    /// �� �ʱ� ����
    private void CircusMapStartSetting()
    {
        // ���� �ʱ�ȭ
        currentLevel = 1;
        currentLaserCycle = laserCycleList[currentLevel - 1];
        currentLaserSpeed = laserSpeedList[currentLevel - 1];

        // BGM ����
        _circusSoundManager.PlayBGM();

        // ���̵���
        _circusUIManager.FadeInImage();

        // UI ������ �ʱ�ȭ
        _circusUIManager.GaugeSetting(maxLaserCount * 0.9f);
    }

    /// ���� ����
    IEnumerator CircusGame()
    {
        // �� ���� ���
        yield return new WaitForSeconds(5f);

        // UI 5�� ī��Ʈ�ٿ� �� Timer ����
        _circusUIManager.StartCountDown(5);
        yield return new WaitForSeconds(8f);
        _circusUIManager.StartTimer(gamePlayTime);
        StartCoroutine(EndCountDownSound(gamePlayTime));

        // ���� ����
        Coroutine Level = StartCoroutine(LevelControl());
        Coroutine Laser = StartCoroutine(GenerateLasers());
        yield return new WaitForSeconds(gamePlayTime + 2f);

        // ���� ����
        StopCoroutine(Level);
        StopCoroutine(Laser);

        // ���� ��� Ȯ��
        CheckGameResult();
        yield return new WaitForSeconds(5f);

        // ���θ� ����
        StartCoroutine(ReturnMainMap());
    }

    /// ���� 10�� �� ī��Ʈ�ٿ�
    IEnumerator EndCountDownSound(float playTime)
    {
        yield return new WaitForSeconds(playTime - 8f);
        _circusSoundManager.PlaySFX("SFX_10Count");
        yield return new WaitForSeconds(10f);
        _circusSoundManager.StopSFX();
    }

    /// ���� ��Ʈ��
    IEnumerator LevelControl()
    {
        // 30��
        yield return new WaitForSeconds(levelUpTime[0]);
        currentLevel++;
        currentLaserCycle = laserCycleList[currentLevel - 1];
        currentLaserSpeed = laserSpeedList[currentLevel - 1];

        // 50��
        yield return new WaitForSeconds(levelUpTime[1]-levelUpTime[0]);
        currentLevel++;
        currentLaserCycle = laserCycleList[currentLevel - 1];
        currentLaserSpeed = laserSpeedList[currentLevel - 1];

        // 40��
        yield return new WaitForSeconds(levelUpTime[2] - levelUpTime[1]);
        currentLevel++;
        currentLaserCycle = laserCycleList[currentLevel - 1];
        currentLaserSpeed = laserSpeedList[currentLevel - 1];
    }

    /// ������ ����
    IEnumerator GenerateLasers()
    {
        while (true)
        {
            SpawnLaser();
            yield return new WaitForSeconds(currentLaserCycle);
        }
    }

    private void SpawnLaser()
    {
        if(currentLevel == 1)
        {
            // 1 level
            GameObject Laser = Instantiate(laserPrefabList[0], laserParent.position, laserPrefabList[0].transform.rotation, laserParent);
        }
        else
        {
            // 2, 3 level
            GameObject laserPrefab = laserPrefabList[Random.Range(0, laserPrefabList.Count)];
            GameObject Laser = Instantiate(laserPrefab, laserParent.position, laserPrefab.transform.rotation, laserParent);
        }
    }

    /// ���� ��� Ȯ��
    private void CheckGameResult()
    {
        if (_circusUIManager.GaugeValueCheck())
        {
            // ���Ӽ���
            this.GetComponent<CircusDirector>().PlayFirecracker();
            _circusUIManager.GameSuccessUI();
            _circusSoundManager.PlaySFX("SFX_Circus_GameClear");
            PlayCheerAnimation();
        }
        else
        {
            // ���ӽ���
            _circusUIManager.GameOverUI();
            _circusSoundManager.PlaySFX("SFX_Circus_GameOver");
        }
    }

    /// ���� ȯȣ �ִϸ��̼�
    private void PlayCheerAnimation()
    {
        GameObject[] audiences = GameObject.FindGameObjectsWithTag("Audience");
        foreach (GameObject audience in audiences)
        {
            AudienceController audienceController = audience.GetComponent<AudienceController>();
            audienceController.PlayCheerAnimation();
        }
    }


    /// ���� �� ����
    IEnumerator ReturnMainMap()
    {
        _circusUIManager.FadeOutImage();
        _circusSoundManager.StopBGM();

        yield return new WaitForSeconds(5f);
        _circusSceneManager.LoadMainMenuMap();
    }

    /*-------------------- Event ----------------------*/
    public void OnLaserHitPlayer()
    {
        _circusUIManager.GaugeDown();
        _circusSoundManager.PlaySFX("SFX_Circus_laserOver");
    }

    public void OnLaserReachBorder()
    {
        _circusUIManager.GaugeUp();
    }
}
