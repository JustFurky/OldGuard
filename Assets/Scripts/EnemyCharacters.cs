using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacters : MonoBehaviour
{
    GameObject President;
    public GameObject[] ThrowObject;
    int random;
    private void Start()
    {
        random = Random.Range(0, 3);
        President = GameObject.FindGameObjectWithTag("President");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="President")
        {
            StartCoroutine(ThrowTimer());
        }
    }
    IEnumerator ThrowTimer()
    {
        transform.LookAt(President.transform.position);
        transform.GetComponent<Animator>().SetTrigger("ThrowAnim");
        yield return new WaitForSeconds(0.8f);
        Instantiate(ThrowObject[random], transform.position, Quaternion.identity);
    }
}
