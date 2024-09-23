using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonSoundManager : MonoBehaviour
{
    [SerializeField] AudioSource bgmSource;  // BGM ����� �ҽ�
    [SerializeField] AudioSource sfxSource;  // SFX ����� �ҽ�
    [SerializeField] AudioSource guideSource; // �ȳ� ���� ����� �ҽ� 

    [SerializeField] AudioClip bgmClip;      // BGM Ŭ��
    [SerializeField] AudioClip sfxClip;      // SFX Ŭ��
    [SerializeField] AudioClip guideClip;    // �ȳ� ���� Ŭ�� 

    private float originalBgmVolume;         // BGM ���� ���� ���� ����
    private System.Action onGuideComplete;   // �ȳ� ������ ���� �� ȣ��� �ݹ� �Լ�


    // BGM ��� �� �ȳ� ���� ����
    public void PlayBGMWithGuide(System.Action guideCompleteCallback)
    {
        originalBgmVolume = bgmSource.volume; // BGM�� ���� ���� ����
        PlayBGM();
        onGuideComplete = guideCompleteCallback; // �ȳ� ������ ���� �� ������ �ݹ� �Լ� ����
        Invoke("PlayGuide", 3f); // 3�� �� �ȳ� ���� ���
    }

    // BGM ���
    public void PlayBGM()
    {
        Debug.Log("BGM ����մϴ�.");
        bgmSource.clip = bgmClip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    // �ȳ� ���� ���
    public void PlayGuide()
    {
        Debug.Log("�ȳ� ���� ����.");
        bgmSource.volume = originalBgmVolume * 0.2f; // BGM ���� ���̱�
        guideSource.clip = guideClip;
        guideSource.Play();
        Invoke("RestoreBGMVolume", guideClip.length); // �ȳ� ������ ���� �� BGM ���� ����
    }

    // �ȳ� ������ ���� �� BGM ���� ����
    public void RestoreBGMVolume()
    {
        bgmSource.volume = originalBgmVolume;
        Debug.Log("�ȳ� ���� ����. BGM ���� ����.");

        // �ȳ� ������ ���� �� �ݹ� �Լ� ����
        if (onGuideComplete != null)
        {
            onGuideComplete();
        }
    }

    // SFX ���
    public void PlaySFX()
    {
        sfxSource.PlayOneShot(sfxClip);
    }

}
