using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMove1 : MonoBehaviour
{
    public float speed = 50f;          // �����ϴ� �ӵ�
    public float waveFrequency = 2f;  // �¿�� ���ұ����ϴ� ��
    public float waveAmplitude = 0.05f;  // �¿�� ��鸮�� ũ��

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // �����ϴ� �������� �̵� (Z�� ����)
        Vector3 forwardMovement = transform.forward * speed * Time.deltaTime;

        // ���ұ����ϰ� �̵��ϴ� X�� ���� (Sine �Լ��� ����Ͽ� �¿�� ��鸲)
        float wave = Mathf.Sin(Time.time * waveFrequency) * waveAmplitude;
        Vector3 wavyMovement = new Vector3(wave, 0, 0);

        // ���ұ����� �����Ӱ� ������ ���ļ� ���� �̵� ó��
        transform.position += forwardMovement + wavyMovement;
    }
}
