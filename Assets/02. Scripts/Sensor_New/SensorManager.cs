using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorManager : MonoBehaviour
{
    [Header ("SensorManager")]
    [SerializeField] OSCManager _oscManager;
    [SerializeField] SensorEnum sensorEnum;                              // ���� Enum
    [SerializeField] RectTransform sensorGround;                         // ���� �׶���

    private List<Vector3> sensorPositionList = new List<Vector3>();      // ������ ����Ʈ
    private SensorDataFormat SensorData;                                 // ���� ������ ����


    void Start()
    {
        _oscManager = GameObject.Find("OSCManager").GetComponent<OSCManager>();
        StartCoroutine(GetSendsorData());
    }

    /// ������ �νĵ� ��ġ�� ������Ʈ(�ݺ�)
    IEnumerator GetSendsorData()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            SensorData = _oscManager.sensorData[((int)sensorEnum)];
            sensorPositionList.Clear();

            // ���� �νĵǾ��ִ� �������� �����ǰ� ������Ʈ
            for (int i = 0; i < SensorData.positionList.Count; i++)
            {
                sensorPositionList.Add(new Vector3(Scale(-SensorData.rectSize.x / 2, SensorData.rectSize.x / 2, sensorGround.position.x - sensorGround.rect.width / 2, sensorGround.position.x + sensorGround.rect.width / 2, SensorData.positionList[i].x),
                                        Scale(-SensorData.rectSize.y / 2, SensorData.rectSize.y / 2, sensorGround.position.y - sensorGround.rect.height / 2, sensorGround.position.y + sensorGround.rect.height / 2, SensorData.positionList[i].y),
                                        0));
            }
        }
    }

    /// ������ ����Ʈ getter ����
    public List<Vector3> GetSensorVector()
    {
        return sensorPositionList;
    }


    /// ���� �׶��� ����
    private float Scale(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
    {
        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        return (NewValue);
    }
}
