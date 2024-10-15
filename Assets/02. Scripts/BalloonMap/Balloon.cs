using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    BalloonMapManager balloonMapManager;
    public bool isEventBalloon = false;  // �̺�Ʈ ǳ�� ����
    private Animator animator;

    void Start()
    {
        balloonMapManager = FindObjectOfType<BalloonMapManager>();
        animator = GetComponent<Animator>();  
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
            GetComponent<Collider>().enabled = false;

            StartCoroutine(PopBalloon());
        }
    }


    // [ ǳ�� �Ͷ߸��� �ִϸ��̼� ��� -> ���� �ð� �� ǳ�� ���� ]
    //
    //  Animator�� Ʈ���Ÿ� �ɾ� ǳ�� ���� �ִϸ��̼� ���
    //  
    IEnumerator PopBalloon()
    {
        // �̺�Ʈ ǳ�� ���ŵǴ� �ִϸ��̼� ����
        if (isEventBalloon)
        {
            Debug.Log("�̺�Ʈ ǳ���� �浹�߽��ϴ�.");
            animator.SetTrigger("Destroy"); 
        }

        balloonMapManager.OnBalloonPopped(this);

        yield return new WaitForSeconds(1f); 
        Destroy(gameObject);
    }
}
