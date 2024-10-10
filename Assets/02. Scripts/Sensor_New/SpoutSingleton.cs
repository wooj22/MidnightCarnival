using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///SpoutSender �̱���
public class SpoutSingleton : MonoBehaviour
{
    static SpoutSingleton instance;

    private void Awake()
    {
        if (instance == null) 
        {
            instance = this; 
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            if (instance != this) 
                Destroy(this.gameObject);
        }
        DontDestroyOnLoad(gameObject); 
    }
}

