using System.Collections.Generic;
using UnityEngine;

public class OSCManager : MonoBehaviour
{
    [Header ("OSC")]
    [SerializeField] public OSC sensorOSC;
    public SensorDataFormat[] sensorData;

    private void Start()
    {
        SetOSC_Event();
        sensorData = new SensorDataFormat[System.Enum.GetValues(typeof(SensorEnum)).Length];

        //5 size �迭
        for (int i = 0; i < sensorData.Length; i++)
            sensorData[i] = new SensorDataFormat();
    }

    /// ���� �ڵ鷯 - Start, Update, Stop, Quit
    #region Front Sensor Handler
    public void GetFrontStartMessage(OscMessage message)
    {
        sensorData[((int)SensorEnum.Front)].rectSize = new Vector2(message.GetFloat(0), message.GetFloat(1));
        sensorData[((int)SensorEnum.Front)].positionList.Clear();
        Debug.Log("Front ���� ����");
    }

    public void GetFrontSensorMessage(OscMessage message)
    {
        sensorData[((int)SensorEnum.Front)].positionList.Add(new Vector3(message.GetFloat(0), message.GetFloat(1), 0));
    }

    public void GetFrontStopMessage(OscMessage message)
    {
        Debug.Log("Front ���� ����");
    }

    public void FrontSensorQuit(OscMessage message)
    {
        Debug.Log("Front ���� ����");
    }
    #endregion

    #region Right Sensor Handler
    public void GetRightStartMessage(OscMessage message)
    {
        sensorData[((int)SensorEnum.Right)].rectSize = new Vector2(message.GetFloat(0), message.GetFloat(1));
        sensorData[((int)SensorEnum.Right)].positionList.Clear();
        Debug.Log("Right ���� ����");
    }

    public void GetRightSensorMessage(OscMessage message)
    {
        sensorData[((int)SensorEnum.Right)].positionList.Add(new Vector3(message.GetFloat(0), message.GetFloat(1), 0));
    }

    public void GetRightStopMessage(OscMessage message)
    {
        Debug.Log("Right ���� ����");
    }

    public void RightSensorQuit(OscMessage message)
    {
        Debug.Log("Right ���� ����");
    }
    #endregion

    #region Back Sensor Handler
    public void GetBackStartMessage(OscMessage message)
    {
        sensorData[((int)SensorEnum.Back)].rectSize = new Vector2(message.GetFloat(0), message.GetFloat(1));
        sensorData[((int)SensorEnum.Back)].positionList.Clear();
        Debug.Log("Back ���� ����");
    }

    public void GetBackSensorMessage(OscMessage message)
    {
        sensorData[((int)SensorEnum.Back)].positionList.Add(new Vector3(message.GetFloat(0), message.GetFloat(1), 0));
    }

    public void GetBackStopMessage(OscMessage message)
    {
        Debug.Log("Back ���� ����");
    }

    public void BackSensorQuit(OscMessage message)
    {
        Debug.Log("Back ���� ����");
    }
    #endregion

    #region Left Sensor Handler
    public void GetLeftStartMessage(OscMessage message)
    {
        sensorData[((int)SensorEnum.Left)].rectSize = new Vector2(message.GetFloat(0), message.GetFloat(1));
        sensorData[((int)SensorEnum.Left)].positionList.Clear();
        Debug.Log("Left ���� ����");
    }

    public void GetLeftSensorMessage(OscMessage message)
    {
        sensorData[((int)SensorEnum.Left)].positionList.Add(new Vector3(message.GetFloat(0), message.GetFloat(1), 0));
    }

    public void GetLeftStopMessage(OscMessage message)
    {
        Debug.Log("Left ���� ����");
    }

    public void LeftSensorQuit(OscMessage message)
    {
        Debug.Log("Left ���� ����");
    }
    #endregion

    #region Down Sensor Handler
    public void GetDownStartMessage(OscMessage message)
    {
        sensorData[((int)SensorEnum.Down)].rectSize = new Vector2(message.GetFloat(0), message.GetFloat(1));
        sensorData[((int)SensorEnum.Down)].positionList.Clear();
        Debug.Log("Down ���� ����");
    }
    public void GetDownSensorMessage(OscMessage message)
    {
        sensorData[((int)SensorEnum.Down)].positionList.Add(new Vector3(message.GetFloat(0), message.GetFloat(1), 0));
    }
    public void GetDownStopMessage(OscMessage message)
    {
        Debug.Log("Down ���� ����");
    }

    public void DownSensorQuit(OscMessage message)
    {
        Debug.Log("Down ���� ����");
    }
    #endregion

    
    
    /// �ڵ鷯 ���

    void SetOSC_Event()
    {
        // Front
        sensorOSC.SetAddressHandler("/Front/Start", GetFrontStartMessage);
        sensorOSC.SetAddressHandler("/Front/Data", GetFrontSensorMessage);
        sensorOSC.SetAddressHandler("/Front/End", GetFrontStopMessage);
        sensorOSC.SetAddressHandler("/Front/Quit", FrontSensorQuit);

        // Right
        sensorOSC.SetAddressHandler("/Right/Start", GetRightStartMessage);
        sensorOSC.SetAddressHandler("/Right/Data", GetRightSensorMessage);
        sensorOSC.SetAddressHandler("/Right/End", GetRightStopMessage);
        sensorOSC.SetAddressHandler("/Right/Quit", RightSensorQuit);

        // Back
        sensorOSC.SetAddressHandler("/Back/Start", GetBackStartMessage);
        sensorOSC.SetAddressHandler("/Back/Data", GetBackSensorMessage);
        sensorOSC.SetAddressHandler("/Back/End", GetBackStopMessage);
        sensorOSC.SetAddressHandler("/Back/Quit", BackSensorQuit);


        // Left
        sensorOSC.SetAddressHandler("/Left/Start", GetLeftStartMessage);
        sensorOSC.SetAddressHandler("/Left/Data", GetLeftSensorMessage);
        sensorOSC.SetAddressHandler("/Left/End", GetLeftStopMessage);
        sensorOSC.SetAddressHandler("/Left/Quit", LeftSensorQuit);

        // Down
        sensorOSC.SetAddressHandler("/Down/Start", GetDownStartMessage);
        sensorOSC.SetAddressHandler("/Down/Data", GetDownSensorMessage);
        sensorOSC.SetAddressHandler("/Down/End", GetDownStopMessage);
        sensorOSC.SetAddressHandler("/Down/Quit", DownSensorQuit);
    }
}
