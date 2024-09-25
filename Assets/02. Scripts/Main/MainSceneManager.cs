using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneManager : MonoBehaviour
{
    [SerializeField] GameObject spoutCamera;
    [SerializeField] MainSoundManager _mainSoundManager;

    void Awake()
    {
        spoutCamera.gameObject.SetActive(true);
    }

    /// �� �̵�
    public void OnLoadSceneByName(string sceneName)
    {
        _mainSoundManager.PlaySFX("SFX_Main_openMap");
        StartCoroutine(SoundWaiting(sceneName));
    }

    IEnumerator SoundWaiting(string sceneName)
    {
        yield return new WaitForSeconds(3f);
        spoutCamera.gameObject.SetActive(false);
        SceneManager.LoadScene(sceneName);
    }
}
