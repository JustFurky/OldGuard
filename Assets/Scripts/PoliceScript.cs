using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceScript : MonoBehaviour
{
    float randomFrame;
    Animator MyAnimator;
    int randomLuck;
    void Start()
    {
        randomLuck = Random.Range(0, 5);
        MyAnimator = transform.GetComponent<Animator>();
        StartCoroutine(AnimTimer());
    }
    IEnumerator AnimTimer()
    {
        MyAnimator.Play("Restrain", -1, Random.Range(0f,1));
        yield return new WaitForSeconds(Random.Range(3.10f,6.20f));
        if (randomLuck>=4)
        {

            transform.GetComponent<Animator>().SetTrigger("Restrain");
            yield return new WaitForSeconds(3.10f);
            transform.GetComponent<Animator>().SetTrigger("FinalTrigger");
        }
    }
}
