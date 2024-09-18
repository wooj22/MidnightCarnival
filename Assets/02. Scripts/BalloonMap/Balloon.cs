using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{

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

        yield return new WaitForSeconds(0.7f); 
        Destroy(gameObject);
    }
}
