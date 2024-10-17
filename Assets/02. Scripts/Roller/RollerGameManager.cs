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
