using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RocketmanScript : MonoBehaviour
{
    public float MinWaittime;
    public float MaxWaittime;

    public GameObject AmmoObject;
    public GameObject Character;
    void Start()
    {
        StartCoroutine(RocketLaunch());
    }

    IEnumerator RocketLaunch()
    {
        yield return new WaitForSeconds(Random.Range(MinWaittime, MaxWaittime));
        gameObject.transform.GetComponent<Animator>().SetTrigger("Failed");
        gameObject.transform.DOMoveZ(Character.transform.position.z, 2);
        yield return new WaitForSeconds(2);
        transform.LookAt(Character.transform.position);
        transform.GetComponent<Animator>().SetTrigger("FailedTwo");
        AmmoObject.transform.GetChild(3).gameObject.SetActive(true);
        AmmoObject.transform.DOMove(new Vector3(Character.transform.position.x, Character.transform.position.y + 1, Character.transform.position.z), 2.2f);
        yield return new WaitForSeconds(2f);
        transform.GetComponent<Animator>().SetTrigger("GoBack");
        transform.rotation=new Quaternion(0,180,0,0);
        transform.DOMoveZ(transform.position.z - 20, 2);
    }
}
