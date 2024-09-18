using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemController : MonoBehaviour
{
    public HorrorGameManager _gameManager;
    //eatItemCount

    private void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<HorrorGameManager>();
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
        Debug.Log("������ ����");
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }
}
