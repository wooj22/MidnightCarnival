using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircusGameManager : MonoBehaviour
{
    [SerializeField] CircusSoundManager _circusSoundManager;
    [SerializeField] CircusSceneManager _circusSceneManager;

    void Start()
    {
        // �ñ׶����� ���� ����ְ� ������
        StartCoroutine(CircusSample_sigraf());
    }


    /*-------------------------- Corutines -----------------------------*/
    IEnumerator CircusSample_sigraf()
    {
        _circusSoundManager.PlayBGM();
        yield return new WaitForSeconds(5f);
        _circusSoundManager.bgmSource.volume = 0.3f;
        // �ȳ����� ���
        _circusSoundManager.PlaySFX("SFX_Horror_announcement");
        yield return new WaitForSeconds(60f);
        _circusSoundManager.bgmSource.volume = 1f;
        yield return new WaitForSeconds(10f);
        _circusSceneManager.LoadMainMenuMap();
    }
}
