using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMove2 : MonoBehaviour
{
    public float speed = 50f;          // �����ϴ� �ӵ�


    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // �����ϴ� �������� �̵� (Z�� ����)
        Vector3 forwardMovement = transform.forward * speed * Time.deltaTime;
        transform.position += forwardMovement;
    }
}
