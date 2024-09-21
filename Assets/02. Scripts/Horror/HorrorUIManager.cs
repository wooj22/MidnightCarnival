using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HorrorUIManager : MonoBehaviour
{
    [Header("UI plug")]
    [SerializeField] Text timerText;
    [SerializeField] Slider itemGauge;
    [SerializeField] int roundDuration;
    
    /// Ÿ�̸�
    public IEnumerator StartRoundTimer()
    {
        int timeRemaining = roundDuration;
        timerText.gameObject.SetActive(true);

        while (timeRemaining > 0)
        {
            timerText.text = timeRemaining.ToString();
            yield return new WaitForSeconds(1f);
            timeRemaining--;
        }

        timerText.text = "0";
        timerText.gameObject.SetActive(false);
    }

    /// ������ ������
    public void ItemGaugeUp()
    {
        itemGauge.value++;
    }

    public void ItemGaugeActive(int itemMaxCount)
    {
        itemGauge.maxValue = itemMaxCount;
        itemGauge.value = 0;
        itemGauge.gameObject.SetActive(true);
    }

    public void ItemGaugeInactive()
    {
        itemGauge.gameObject.SetActive(false);
    }
}
