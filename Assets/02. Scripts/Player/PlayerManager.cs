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
    [SerializeField] private List<GameObject> players = new List<GameObject>();

    void Update()
    {
        // Player �߰�
        if (sensorManager.getSensorVector().Count > players.Count)
        {
            for (int i = players.Count; i < sensorManager.getSensorVector().Count; i++)
            {
                players.Add(Instantiate(playerPrefab, playerParent));
            }
        }
        // Player ��Ȱ��ȭ
        else if (sensorManager.getSensorVector().Count < players.Count)
        {
            for (int i = sensorManager.getSensorVector().Count; i < players.Count; i++)
            {
                players[i].SetActive(false);
            }
        }

        // Player ��ġ ������Ʈ
        for (int i = 0; i < sensorManager.getSensorVector().Count; i++)
        {
            players[i].SetActive(true);
            players[i].transform.localPosition = sensorManager.getSensorVector()[i];
        }
    }
}