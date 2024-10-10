using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircusGameManager : MonoBehaviour
{
    [Header("Levels")]
    [SerializeField] public int currentLevel;              // 현재 레벨
    [SerializeField] public float currentLaserCycle;       // 현재 레이저 발생 주기
    [SerializeField] public float currentLaserSpeed;       // 현재 레이저 속도

    [Header("MapData")]
    [SerializeField] float gamePlayTime;                   // 전체 게임 진행 시간 120초
    [SerializeField] int maxLaserCount;                    // 전체 레이저 개수 29개
    [SerializeField] Transform laserParent;                // 레이저 생성 위치
    [SerializeField] List<GameObject> laserPrefabList;     // 겹 레이저 프리팹 3개
    [SerializeField] List<float> levelUpTime;              // 레벨 분기 단위
    [SerializeField] List<float> laserCycleList;           // 3레벨 사이클주기 데이터
    [SerializeField] List<float> laserSpeedList;           // 3레벨 스피드 데이터

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
    private void CircusMapStartSetting()
    {
        // 레벨 초기화
        currentLevel = 1;
        currentLaserCycle = laserCycleList[currentLevel - 1];
        currentLaserSpeed = laserSpeedList[currentLevel - 1];

        // BGM 시작
        _circusSoundManager.PlayBGM();

        // UI 게이지 초기화
        _circusUIManager.GaugeSetting(maxLaserCount * 0.9f);
    }

    IEnumerator CircusGame()
    {
        // UI 5초 카운트다운 후 Timer 시작
        _circusUIManager.StartCountDown(5);
        yield return new WaitForSeconds(8f);
        _circusUIManager.StartTimer(gamePlayTime);

        // 레이저 생성 시작
        StartCoroutine(GenerateLasers());
    }

    IEnumerator GenerateLasers()
    {
        int laserCount = 0;
        while (true)
        {
            SpawnLaser();
            laserCount++;
            yield return new WaitForSeconds(currentLaserCycle);
        }
    }

    private void SpawnLaser()
    {
        GameObject laserPrefab = laserPrefabList[Random.Range(0, laserPrefabList.Count)];
        GameObject Laser = Instantiate(laserPrefab, laserParent.position, laserPrefab.transform.rotation, laserParent);
    }

    /*-------------------- Event ----------------------*/
    public void OnLaserHitPlayer()
    {
        _circusUIManager.GaugeDown();
    }

    public void OnLaserReachBorder()
    {
        _circusUIManager.GaugeUp();
    }
}
