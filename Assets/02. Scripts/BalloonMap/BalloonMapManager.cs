using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonMapManager : MonoBehaviour
{

    // [ �̺�Ʈ ǳ�� ���� ���� ]

    public GameObject[] balloonScreens;  // 5���� �� ������Ʈ �迭

    List<GameObject> availableScreens;   // �̺�Ʈ �߻����� ���� �� ����Ʈ

    string[] balloonTags = { "red", "yellow", "green", "blue", "pink", "purple" }; // ǳ�� ���� �ش��ϴ� �±� ����Ʈ

    Balloon eventBalloon = null;        // ���� �̺�Ʈ ǳ�� 
    float eventBalloonTime = 30f;       // 30�ʸ��� �̺�Ʈ �߻�
    float eventDuration = 20f;          // 5�� �ȿ� �̺�Ʈ ǳ���� ��Ʈ���� ��
    float timer = 0f;


    void Start()
    {
        // ó���� ��� ȭ���� ��� �����ϵ��� ����
        availableScreens = new List<GameObject>(balloonScreens);
    }

    void Update()
    {
        timer += Time.deltaTime;

        // 30�ʸ��� �̺�Ʈ ǳ���� ����
        if (timer >= eventBalloonTime)
        {
            SelectRandomEventBalloon();
            timer = 0f; // Ÿ�̸� ����
        }

        // �̺�Ʈ ǳ���� �ð� ���� Ȯ��
        if (eventBalloon != null && eventBalloon.isEventBalloon)
        {
            eventBalloon.timer += Time.deltaTime;
            if (eventBalloon.timer >= eventDuration)
            {
                ResetBalloon(eventBalloon); // �ð� �ʰ� �� ���� ������� ���ư��� ��
            }
        }
    }


    // �� [ �̺�Ʈ ǳ�� ���� ���� ] ��
    // 
    // 1. �������� ��� ������ �� ����
    //   - ���õ� �� (������Ʈ)�� �ڽ� ǳ���� ������ 
    // 2. ���� ǳ�� ����
    //   - ���õ� ǳ���� �̺�Ʈ ǳ������ ����
    //   - �̺�Ʈ �߻��� ���� ������Ʈ�� ����Ʈ���� ����
    //
    void SelectRandomEventBalloon()
    {
        if (availableScreens.Count == 0)
        {
            Debug.Log("�̺�Ʈ�� �߻���ų ȭ���� �� �̻� ����.");
            return; 
        }

        print("�̺�Ʈ ǳ���� ���� �մϴ�.");

        int randomScreenIndex = Random.Range(0, availableScreens.Count);
        GameObject selectedScreen = availableScreens[randomScreenIndex];

        Balloon[] balloons = selectedScreen.GetComponentsInChildren<Balloon>();

        if (balloons.Length > 0)
        {
            int randomBalloonIndex = Random.Range(0, balloons.Length);
            eventBalloon = balloons[randomBalloonIndex];

            eventBalloon.isEventBalloon = true;
            eventBalloon.timer = 0f;
            eventBalloon.ChangeToEventBalloonAppearance();

            availableScreens.Remove(selectedScreen);

            Debug.Log("�̺�Ʈ ǳ���� Ȱ��ȭ �Ǿ����ϴ�.");
        }
        else
        {
            Debug.Log("�ش� ���� ǳ���� �����ϴ�."); // ���� ǳ���� ���ٸ� => ���� �� ���� ���ٴ� �Ŵϱ� �̺�Ʈ ǳ�� �߻� X
        }
    }



    // �� [ �̺�Ʈ ǳ�� �ı� �� : ���� ������ ǳ�� �ı� ] ��
    // 
    // 1. ���� ���� �±� ����
    // 2. �̺�Ʈ ǳ���� ���� ������ �ش� �±��� ǳ������ ã�� �ı�
    //
    void DestroyRandomColorBalloons(Balloon eventBalloon)
    {
        string randomTag = balloonTags[Random.Range(0, balloonTags.Length)];

        foreach (Transform balloon in eventBalloon.transform.parent)
        {
            if (balloon.CompareTag(randomTag))
            {
                Destroy(balloon.gameObject); 
                Debug.Log("�̺�Ʈ ǳ���� ȿ���� "+ randomTag + "���� ǳ���� ������!!");
            }
        }

        // �̺�Ʈ ǳ�� ����
        ResetBalloon(eventBalloon);
    }


    // �� [ �̺�Ʈ ǳ���� ��ġ���� �� ó�� ]
    // 
    public void OnBalloonPopped(Balloon balloon)
    {
        if (balloon.isEventBalloon)
        {
            // �̺�Ʈ ǳ���� ���� ���� ���� ǳ�� �ı�
            DestroyRandomColorBalloons(balloon);
        }
        else
        {
            // �Ϲ� ǳ���� ���� �ܼ� �ı� -> Balloon ��ũ��Ʈ���� ���ٰ��� 
            // Destroy(balloon.gameObject);
        }
    }


    // �� [ �ð� �ʰ� �� �Ǵ� ǳ���� ��Ʈ�� �� : �̺�Ʈ ǳ���� ���� ���·� ���� ]
    // 
    void ResetBalloon(Balloon balloon)
    {
        balloon.isEventBalloon = false;
        balloon.ResetAppearance(); // ���� ������� �ǵ���

        // �ش� ���� ������Ʈ�� �ٽ� ��� �����ϰ� ����Ʈ�� �߰� => ?? �� �߰���?
        // availableScreens.Add(balloon.transform.parent.gameObject);

        eventBalloon = null; // �̺�Ʈ ǳ�� ����
    }
}
