using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneManager : MonoBehaviour
{
    /// �� �̵�
    public void OnLoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
