using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollerGameManager : MonoBehaviour
{
    [Header("Levels")]
    [Header("MapData")]
    [Header("Managers")]
    [SerializeField] RollerUIManager _rollerUIManager;
    [SerializeField] RollerSoundManager _rollerSoundManager;
    [SerializeField] RollerSceneManager _rollerSceneManager;

    private void Start()
    {
        RollerMapStartSetting();

        // �� ���� �� ��ü�� ���� �׽�Ʈ
        StartCoroutine(test());
    }

    IEnumerator test()
    {
        yield return new WaitForSeconds(5f);
        _rollerSoundManager.StopBGM();
        _rollerUIManager.FadeOutImage();

        yield return new WaitForSeconds(8f);
        _rollerSceneManager.LoadMainMenuMap();
    }

    /*-------------- Game -------------------*/
    /// �� �ʱ� ����
    private void RollerMapStartSetting()
    {
        // BGM
        _rollerSoundManager.PlayBGM();

        // ���̵���
        _rollerUIManager.FadeInImage();
    }
}
