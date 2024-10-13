using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerController : MonoBehaviour
{
    MainManager _mainManager;

    private void Start()
    {
        _mainManager = GameObject.Find("MainManager").GetComponent<MainManager>();
    }

    // �÷��̾�� �浹�� �ݶ��̴��� name������ �̵�
    // �浹���϶� �������� ä���, �������� �� ���� swithMap()�� ȣ���ϴ� ������� �����ؾ���
    private void OnTriggerEnter(Collider other)
    {
        string sceneName = other.gameObject.name;
        _mainManager.SwitchMap(sceneName);
    }
}
