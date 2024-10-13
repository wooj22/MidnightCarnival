using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    BalloonMapManager balloonMapManager;

    public bool isEventBalloon = false;  // �̺�Ʈ ǳ�� ����


    void Start()
    {
        balloonMapManager = FindObjectOfType<BalloonMapManager>();
    }



    // -------------------------------------------------------------------------------------
    // �� [ �浹 ���� �޼ҵ� ] �� ----------------------------------------------------------


    // [ ǳ�� - �÷��̾� �ݶ��̴� �浹 ]
    //
    // �ִϸ��̼� ��� �ð� ���� �ߺ� �浹 ���� : �ݶ��̴� ��Ȱ��ȭ
    // 
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("�÷��̾�� �浹�߽��ϴ�.");

            GetComponent<Collider>().enabled = false;

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
