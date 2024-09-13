using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header ("PlayerManager")]
    [SerializeField] SensorEnum sensorEnum;
    [SerializeField] SensorManager sensorManager;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] Transform playerParent;
    [SerializeField] private List<GameObject> playerList = new List<GameObject>();

    void Update()
    {
        // Player �߰�
        if (sensorManager.GetSensorVector().Count > playerList.Count)
        {
            for (int i = playerList.Count; i < sensorManager.GetSensorVector().Count; i++)
            {
                playerList.Add(Instantiate(playerPrefab, playerParent));
            }
        }
        // Player ��Ȱ��ȭ
        else if (sensorManager.GetSensorVector().Count < playerList.Count)
        {
            for (int i = sensorManager.GetSensorVector().Count; i < playerList.Count; i++)
            {
                playerList[i].SetActive(false);
            }
        }

        // Player ��ġ ������Ʈ
        for (int i = 0; i < sensorManager.GetSensorVector().Count; i++)
        {
            playerList[i].SetActive(true);
            playerList[i].transform.localPosition = sensorManager.GetSensorVector()[i];
        }
    }
}