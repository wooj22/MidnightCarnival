using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorrorGameManager : MonoBehaviour
{
    [Header("Round Setting")]
    [SerializeField] int curRound;                           // �������
    [SerializeField] int curRoundItemMaxCount;               // ������忡�� �Ծ���� ������ ����
    [SerializeField] List<int> itemMaxCountByRoundList;      // �� ���庰 �Ծ���� ������ ����
    [SerializeField] List<GameObject> ghostsByRoundList;     // �� ���庰 �ͽŸ���Ʈ
    [SerializeField] List<GameObject> itemsByRoundList;      // �� ���庰 �����۸���Ʈ

    [Header("Else Setting")]
    [SerializeField] Transform ghostParent;
    [SerializeField] Transform itemParent;

    [Header("Sharing")]
    public int eatItemCount;        // ���� ���忡�� �÷��̾ ���� ������ ����(������,UI)

    private GameObject curGhosts;   // ���� ���忡 ������ �ͽŵ� (������)
    private GameObject curItems;    // ���� ���忡 ������ �����۵� (������)


    void Start()
    {
        StartCoroutine(HorrorHouseStart());
    }


    /// �ͽ��� �� ���� ���� �ڷ�ƾ
    IEnumerator HorrorHouseStart()
    {
        // 1����
        Debug.Log("1Round Start");
        curRound = 1;
        RoundSetting(curRound);
        yield return new WaitForSeconds(60f);
        Roundup(curRound);

        // 1���� ����
        if (eatItemCount < curRoundItemMaxCount)
        {
            Debug.Log("1���忡�� �����߽��ϴ�. ���α������� �̵��մϴ�.");
            // ���� UI ���� �ڷ�ƾ ���߰� ���α������� �̵�
        }

        // 1���� ����, �����̵�
        Debug.Log("1���� ����. ���� �̵��� �����մϴ�.");
        yield return new WaitForSeconds(5f);
        Debug.Log("5�ʵ� 2���带 �����մϴ�.");
        yield return new WaitForSeconds(5f);

        // 2����
        Debug.Log("2Round Start");
        curRound = 2;
        RoundSetting(curRound);
        yield return new WaitForSeconds(60f);
        Roundup(curRound);

        // 2���� ����
        if (eatItemCount < curRoundItemMaxCount)
        {
            Debug.Log("2���忡�� �����߽��ϴ�. ���� �������� �̵��մϴ�.");
            // ���� UI ���� �ڷ�ƾ ���߰� ����ȭ�� �̵�
        }

        // ���� ����
        Debug.Log("2���忡�� �����߽��ϴ�. ���� �������� �̵��մϴ�");
        // ���� ���� UI ����(+���� ȿ��) �ڷ�ƾ ���߰� ����ȭ�� �̵�
    }

    /*----------------------------------------------------------------*/
    /// ���� ����
    private void RoundSetting(int round)
    {
        // ������Ʈ ����
        curGhosts = Instantiate(ghostsByRoundList[round-1], ghostParent);
        curItems = Instantiate(itemsByRoundList[round-1], itemParent);

        // �Ծ���� ������ ���� ����
        curRoundItemMaxCount = itemMaxCountByRoundList[curRound - 1];
    }

    /// ���� ����
    private void Roundup(int round)
    {
        // ������Ʈ ����
        Destroy(curGhosts);
        Destroy(curItems);
    }
}
