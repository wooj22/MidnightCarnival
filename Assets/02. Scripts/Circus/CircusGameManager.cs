using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircusGameManager : MonoBehaviour
{
    [Header("Levels")]
    [SerializeField] float gamePlayTime;           // ��ü ���� ���� �ð� 120��
    [SerializeField] float currentAttackCycle;     // ������ ���� ���� �ֱ�
    [SerializeField] float currentBallSpeed;       // ������ �� �ӵ�

    [Header("Managers")]
    [SerializeField] CircusUIManager _circusUIManager;
    [SerializeField] CircusSoundManager _circusSoundManager;
    [SerializeField] CircusSceneManager _circusSceneManager;

    private void Start()
    {
        _circusSoundManager.PlayBGM();
        _circusUIManager.StartCountDown(5);
        _circusUIManager.StartTimer(gamePlayTime);
    }
}
