using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    [Header ("ItemData")]
    [SerializeField] float itemGenerateTime;
    [SerializeField] List<GameObject> itemList;
    [SerializeField] List<Transform> linePos;

    [Header("Pulling")]
    [SerializeField] int poolSize;
    private List<GameObject> pooledItems;
    private int currentIndex = 0;

    private void Start()
    {
        PullingItem();
        InvokeRepeating("CreateItem", 0, itemGenerateTime);
    }

    /// ������Ʈ Ǯ��
    private void PullingItem()
    {
        pooledItems = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            int randomItemIndex = Random.Range(0, itemList.Count);
            GameObject it = Instantiate(itemList[randomItemIndex], this.transform);
            it.SetActive(false);
            pooledItems.Add(it);
        }
    }

    /// ������ Ȱ��ȭ
    private void CreateItem()
    {
        GameObject item = GetPooledItem();  // ������ ��������

        if (item != null)
        {
            int posIndex = Random.Range(0, linePos.Count);        // ��ġ ����
            item.transform.position = linePos[posIndex].position;
            item.SetActive(true);
        }
    }

    /// Ǯ�� ��Ȱ�� ������ ����
    private GameObject GetPooledItem()
    {
        for (int i = 0; i < pooledItems.Count; i++)
        {
            currentIndex = (currentIndex + 1) % pooledItems.Count;
            if (!pooledItems[currentIndex].activeInHierarchy)
            {
                return pooledItems[currentIndex];
            }
        }

        return null;  // ���� ����� �� �ִ� ��ü�� ������ null ��ȯ
    }
}
