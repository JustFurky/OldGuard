using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PresidentScript : MonoBehaviour
{
    public Light SpotLight;
    int Life = 3;
    public Slider MySlider;
    private void Start()
    {
        MySlider.maxValue = 3;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ThrowObject")
        {
            other.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
            other.transform.GetComponent<MeshRenderer>().enabled = false;
            other.transform.GetComponent<Rigidbody>().detectCollisions = false;
            Destroy(other.gameObject, 1);
            Life--;
            MySlider.value = Life;
            if (Life == 2)
            {
                SpotLight.color = Color.yellow;
                MySlider.transform.GetChild(0).transform.GetComponent<Image>().color = new Color32(247, 127, 0, 255);
            }
            if (Life == 1)
            {
                SpotLight.color = Color.red;
                MySlider.transform.GetChild(0).transform.GetComponent<Image>().color = new Color32(214, 40, 40, 255);
            }
            if (Life <= 0)
            {
                StartCoroutine(Failed());
            }
        }
        if (other.tag == "Finish")
        {
            StartCoroutine(Complete());
        }
    }
    IEnumerator Complete()
    {
        GameManager.Instance.characterMove.enabled = false;
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    IEnumerator Failed()
    {
        GameManager.Instance.characterMove.GuardsBackİddle();
        transform.GetComponent<Animator>().SetLayerWeight(1, 0);
        transform.GetComponent<Animator>().SetTrigger("Failed");
        GameManager.Instance.characterMove.enabled = false;
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
