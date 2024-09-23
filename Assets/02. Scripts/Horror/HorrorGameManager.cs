using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorrorGameManager : MonoBehaviour
{
    [Header("Current Round")]
    [SerializeField] int curRound;                           // �������
    [SerializeField] int curRoundItemMaxCount;               // ������忡�� �Ծ���� ������ ����

    [Header("Round Data")]
    [SerializeField] List<int> itemMaxCountByRoundList;      // �� ���庰 �Ծ���� ������ ����
    [SerializeField] List<GameObject> mapByRoundList;        // �� ���庰 �� ������
    [SerializeField] List<GameObject> ghostsByRoundList;     // �� ���庰 �ͽ� ������
    [SerializeField] List<GameObject> itemsByRoundList;      // �� ���庰 ������ ������

    [Header("Setting")]
    [SerializeField] Transform mapParent;
    [SerializeField] Transform ghostParent;
    [SerializeField] Transform itemParent;

    [Header("Managers")]
    [SerializeField] HorrorUIManager _horrorUIManager;
    [SerializeField] HorrorSoundManager _horrorSoundManager;


    private int eatItemCount;       // ���� ���忡�� �÷��̾ ���� ������ ����
    private GameObject curMap;      // ���� ���忡 ������ �� (������)
    private GameObject curGhosts;   // ���� ���忡 ������ �ͽŵ� (������)
    private GameObject curItems;    // ���� ���忡 ������ �����۵� (������)


    void Start()
    {
        StartCoroutine(HorrorHouseStart());
    }


    /*-------------------------- Corutines -----------------------------*/
    IEnumerator HorrorInstructionStart()
    {
        curMap = Instantiate(mapByRoundList[0], mapParent);
        _horrorSoundManager.PlayBGM(0);
        yield return new WaitForSeconds(5f);
        _horrorSoundManager.bgmSource.volume = 0.1f;
        _horrorSoundManager.PlaySFX("SFX_Horror_announcement1");
        yield return new WaitForSeconds(48f);
        _horrorSoundManager.bgmSource.volume = 1f;
        yield return new WaitForSeconds(10f);
        _horrorSoundManager.StopBGM();
        Destroy(curMap);
    }

    IEnumerator HorrorHouseStart()
    {
        // �ȳ�����
        StartCoroutine(HorrorInstructionStart());
        yield return new WaitForSeconds(64f);

        // 1����
        Debug.Log("1Round Start");
        curRound = 1;
        RoundDataSetting(curRound);
        RoundUISetting();
        RoundSoundSetting(curRound);
        yield return new WaitForSeconds(60f);
        RoundCleaning(curRound);

        // 1���� ����
        if (eatItemCount < curRoundItemMaxCount)
        {
            Debug.Log("1���忡�� �����߽��ϴ�. ���α������� �̵��մϴ�.");
            // ���� UI ���� �ڷ�ƾ ���߰� ���α������� �̵�
        }

        // �����̵�
        Debug.Log("1���� ����. �����̵�. 5�� �� 2���带 �����մϴ�.");
        yield return new WaitForSeconds(5f);

        // 2����
        Debug.Log("2Round Start");
        curRound = 2;
        RoundDataSetting(curRound);
        RoundUISetting();
        RoundSoundSetting(curRound);
        yield return new WaitForSeconds(60f);
        RoundCleaning(curRound);

        // 2���� ����
        if (eatItemCount < curRoundItemMaxCount)
        {
            Debug.Log("2���忡�� �����߽��ϴ�. ���� �������� �̵��մϴ�.");
            // ���� UI ���� �ڷ�ƾ ���߰� ����ȭ�� �̵�
        }

        // ���Ӽ��� UI ����(+���� ȿ��) �ڷ�ƾ ���߰� ����ȭ�� �̵�
        Debug.Log("2���忡�� �����߽��ϴ�. ���� �������� �̵��մϴ�"); 
    }



    /*-------------------------- Round Setting -----------------------------*/

    private void RoundDataSetting(int round)
    {
        curMap = Instantiate(mapByRoundList[round - 1], mapParent);
        curGhosts = Instantiate(ghostsByRoundList[round-1], ghostParent);
        curItems = Instantiate(itemsByRoundList[round-1], itemParent);

        curRoundItemMaxCount = itemMaxCountByRoundList[curRound - 1];
        eatItemCount = 0;
    }

    private void RoundUISetting()
    {
        StartCoroutine(_horrorUIManager.StartRoundTimer());
        _horrorUIManager.ItemGaugeActive(curRoundItemMaxCount);
    }
    private void RoundSoundSetting(int round)
    {
        _horrorSoundManager.PlayBGM(round - 1);
    }

    private void RoundCleaning(int round)
    {
        Destroy(curMap);
        Destroy(curGhosts);
        Destroy(curItems);
        _horrorUIManager.ItemGaugeInactive();
        _horrorSoundManager.StopBGM();
    }

    


    /*--------------------------- Event ----------------------------*/

    public void OnPlayerEatItem()
    {
        eatItemCount++;
        _horrorUIManager.ItemGaugeUp();
        _horrorSoundManager.PlaySFX("SFX_Horror_item");
    }

    public void OnPlayerFindGhost()
    {
        _horrorSoundManager.PlaySFX("SFX_Horror_ghost");
    }
}
