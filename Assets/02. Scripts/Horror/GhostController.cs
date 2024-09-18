using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GhostController : MonoBehaviour
{
    [SerializeField] Animator goshAnimator;

    private void Start()
    {
        goshAnimator = GetComponent<Animator>();
    }

    /// �÷��̾�� �浹
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            StartCoroutine(FoundPlayer());
        }
    }

    IEnumerator FoundPlayer()
    {
        Debug.Log("�ͽ� �߰�");
        goshAnimator.Play("Attack", 0, 0);
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }
}
