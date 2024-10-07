using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// �̱���
public class SensorActiveState : MonoBehaviour
{
    [Header ("SensorActiveState Singleton")]
    public static SensorActiveState instance;
    public bool[] SensorState;

    private void Awake()
    {
        if (instance == null) 
        {
            instance = this; 
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this) 
                Destroy(this.gameObject);
        }
    }

    void Start()
    {
        // 5size sensor state array �ʱ�ȭ
        // OSCManager���� ����
        // PlayerManager���� Ȱ��
        SensorState = new bool[System.Enum.GetValues(typeof(SensorEnum)).Length];
        for (int i = 0; i < SensorState.Length; i++)
            SensorState[i] = false;
    }
}
