using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    BalloonMapManager balloonMapManager;

    public bool isEventBalloon = false;  // �̺�Ʈ ǳ������ ����
    // public float timer = 0f;             // �̺�Ʈ ���� �ð� ����
    public Material eventMaterial;       // �̺�Ʈ ǳ������ �ٲ� ���͸���
    private Material originalMaterial;   // ������ ���͸���
    private Renderer balloonRenderer;    // ǳ���� Renderer ������Ʈ


    void Start()
    {
        balloonRenderer = GetComponent<Renderer>();
        originalMaterial = balloonRenderer.material; // ���� �� ���� ���͸��� ����
                                                     // BalloonMapManager�� ã�Ƽ� ����
        balloonMapManager = FindObjectOfType<BalloonMapManager>();
    }



    // -------------------------------------------------------------------------------------
    // �� [ �̺�Ʈ ǳ�� ���� �޼ҵ� ] �� ---------------------------------------------------


    // [ �̺�Ʈ ǳ������ ���� ]
    public void ChangeToEventBalloonAppearance()
    {
        // �̺�Ʈ ǳ������ ������ �����ϴ� ����
        // ��: ���� ����, ũ�� ���� ��
        if (balloonRenderer != null && eventMaterial != null)
        {
            balloonRenderer.material = eventMaterial; // �̺�Ʈ ǳ�� ���͸��� ����
        }
    }

    // [ ���� ������� ���� ]
    public void ResetAppearance()
    {
        // ���� ǳ���� �������� �����ϴ� ����

        if (balloonRenderer != null && originalMaterial != null)
        {
            balloonRenderer.material = originalMaterial; // ���� ���͸���� ����
        }
        
    }


    // -------------------------------------------------------------------------------------
    // �� [ �浹 ���� �޼ҵ� ] �� ----------------------------------------------------------


    // [ ǳ�� - �÷��̾� �ݶ��̴� �浹 ]
    //
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("�÷��̾�� �浹�߽��ϴ�.");

            StartCoroutine(PopBalloon());
        }
    }


    // [ ǳ�� �Ͷ߸��� �ִϸ��̼� ��� -> ���� �ð� �� ǳ�� ���� ]
    //
    //  Animator�� 'Pop' Ʈ���Ÿ� �ɾ� ǳ�� �Ͷ߸��� �ִϸ��̼� ���
    //  
    IEnumerator PopBalloon()
    {
        print("������ �ִϸ��̼��� ����˴ϴ�.");
        // GetComponent<Animator>().SetTrigger("Pop"); // �ִϸ��̼� ���� ��, �Ҵ� �ʿ�

        balloonMapManager.OnBalloonPopped(this);

        yield return new WaitForSeconds(0.7f); 
        Destroy(gameObject);
    }
}
