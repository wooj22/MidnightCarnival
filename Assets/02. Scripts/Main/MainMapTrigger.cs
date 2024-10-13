using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMapTrigger : MonoBehaviour
{
    [SerializeField] MainManager _mainManager;
    [SerializeField] Image guage;
    [SerializeField] float fillSpeed = 0.2f; 
    private Coroutine fillCoroutine;         // �ڷ�ƾ ����

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            fillCoroutine = StartCoroutine(FillGauge());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (fillCoroutine != null)
            {
                StopCoroutine(fillCoroutine);
                fillCoroutine = null;
            }

            guage.fillAmount = 0f;
        }
    }

    /// �� �̵� ������ ä���
    private IEnumerator FillGauge()
    {
        while (guage.fillAmount < 1f)
        {
            guage.fillAmount += fillSpeed * Time.deltaTime;
            yield return null;
        }

        MaxGuage();
    }

    /// �� �̵� ������ Max, �� ��ȯ ȣ���ϱ�
    private void MaxGuage()
    {
        _mainManager.SwitchMap(this.gameObject.name);  // �� ��ȯ
    }
}
