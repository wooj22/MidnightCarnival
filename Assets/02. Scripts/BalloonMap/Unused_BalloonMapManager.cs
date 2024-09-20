using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unused_BalloonMapManager : MonoBehaviour
{

    // �� [ ǳ�� ���� �� ������ġ ]
    // 
    public GameObject[] spawnPrefabs;   // ������ 6���� ǳ�� ������
    public GameObject[] centerObjects;  // �߽��� ������Ʈ �迭 (right, left, back, front, down)

    private float spawnRadiusY = 70f;   // Y�� ���� �Ÿ� ����
    private float spawnRadiusZ = 350f;  // Z�� ���� �Ÿ� ���� (right, left, down)
    private float spawnRadiusX = 250f;  // X�� ���� �Ÿ� ���� (back, front, down)

    #region down �߽����� �Ÿ� ���� ���� (Spout ī�޶� Ȯ�� �ʿ�)
    // private float downSpawnRadiusZ = 350f;  // down �߽����� Z�� ���� �Ÿ� ����
    // private float downSpawnRadiusX = 250f;  // down �߽����� X�� ���� �Ÿ� ����
    #endregion

    private int objectsPerCenter = 40;  // �� �߽������� ������ ������Ʈ ����
    private float minDistance = 20f;    // ������Ʈ �� �ּ� �Ÿ�


    // ------------------------------------------------------------------------------------------------


    void Start()
    {
        SpawnObjects();
    }


    // �� [ �� �߽����� �°� ������Ʈ ���� ] ��
    // 
    // - spawnedPositions : ������ ������Ʈ���� ��ġ ����Ʈ
    // - �� �߽������� objectsPerCenter �� ��ŭ ������Ʈ ����
    // 
    void SpawnObjects()
    {
        foreach (GameObject centerObject in centerObjects)
        {
            List<Vector3> spawnedPositions = new List<Vector3>(); 

            for (int i = 0; i < objectsPerCenter; i++)
            {
                SpawnAtCenter(centerObject, spawnedPositions);
            }
        }
    }



    // �� [ �־��� �߽������� �ϳ��� ������Ʈ ���� ]
    // 
    // �ִ� 100�� �õ� => ( ��ȿ�� ��ġ�� ������Ʈ ���� , �θ�� �߽��� ���� )
    // - selectedPrefab : �����ϰ� ������ ������ ����
    // - validPosition : ���� ��ġ�� ��ȿ���� ����
    // - attempts : ��ġ ���� �õ� Ƚ��
    // 
    void SpawnAtCenter(GameObject centerObject, List<Vector3> spawnedPositions)
    {
        GameObject selectedPrefab = spawnPrefabs[Random.Range(0, spawnPrefabs.Length)];
        Vector3 spawnPosition;
        bool validPosition = false;
        int attempts = 0;

        while (!validPosition && attempts < 100)
        {
            attempts++;

            spawnPosition = GetRandomPosition(centerObject);
            validPosition = IsPositionValid(spawnPosition, spawnedPositions);

            if (validPosition)
            {
                GameObject newObject = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);
                newObject.transform.parent = centerObject.transform;

                spawnedPositions.Add(spawnPosition);
            }
        }
    }


    // �� [ �߽����� ���� ���� ��ǥ ��ȯ ]
    // 
    // - randomY : ��� �߽������� ���
    // 
    // - �⺻������ �߽����� ��ġ�� ��ȯ (���� ������)
    // 
    Vector3 GetRandomPosition(GameObject centerObject)
    {
        float randomY = Random.Range(-spawnRadiusY, spawnRadiusY);

        float randomX = 0f, randomZ = 0f;

        if (centerObject.name == "right" || centerObject.name == "left")      // X�� ����, Y�� Z�� ����
        {
            randomZ = Random.Range(-spawnRadiusZ, spawnRadiusZ);
            return new Vector3(centerObject.transform.position.x,
                               centerObject.transform.position.y + randomY,
                               centerObject.transform.position.z + randomZ);
        }
        else if (centerObject.name == "back" || centerObject.name == "front")  // Z�� ����, X�� Y�� ����
        {
            randomX = Random.Range(-spawnRadiusX, spawnRadiusX);
            return new Vector3(centerObject.transform.position.x + randomX,
                               centerObject.transform.position.y + randomY,
                               centerObject.transform.position.z);
        }
        else if (centerObject.name == "down")                                  // Y�� ����, Z�� X�� ����
        {
            randomZ = Random.Range(-spawnRadiusZ, spawnRadiusZ);
            randomX = Random.Range(-spawnRadiusX, spawnRadiusX);
            return new Vector3(centerObject.transform.position.x + randomX,
                               centerObject.transform.position.y,
                               centerObject.transform.position.z + randomZ);
        }

        return centerObject.transform.position; 
    }


    // �� [ �ּ� �Ÿ� �����ϴ��� Ȯ�� ] 
    // 
    bool IsPositionValid(Vector3 position, List<Vector3> spawnedPositions)
    {
        foreach (Vector3 pos in spawnedPositions)
        {
            if (Vector3.Distance(position, pos) < minDistance)
            {
                return false;
            }
        }
        return true; 
    }

}
