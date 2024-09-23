using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerController : MonoBehaviour
{
    MainSceneManager _mainSceneManager;

    private void Start()
    {
        _mainSceneManager = GameObject.Find("SceneManager").GetComponent<MainSceneManager>();
    }


    // �÷��̾�� �浹�� �ݶ��̴��� name������ �̵�
    private void OnTriggerEnter(Collider other)
    {
        string sceneName = other.gameObject.name;
        _mainSceneManager.OnLoadSceneByName(sceneName);
    }
}
