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
            StartCoroutine(PopBalloon());
        }
    }


    // [ ǳ�� �Ͷ߸��� �ִϸ��̼� ��� -> ���� �ð� �� ǳ�� ���� ]
    //
    //  Animator�� 'Pop' Ʈ���Ÿ� �ɾ� ǳ�� �Ͷ߸��� �ִϸ��̼� ���
    //  
    IEnumerator PopBalloon()
    {
        GetComponent<Animator>().SetTrigger("Pop");
        print("������ �ִϸ��̼��� ����˴ϴ�.");

        yield return new WaitForSeconds(0.5f); 
        Destroy(gameObject);
    }
}
