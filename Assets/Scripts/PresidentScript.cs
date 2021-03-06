using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using TapticPlugin;
public class PresidentScript : MonoBehaviour
{
    public Light SpotLight;
    int Life = 3;
    public Slider MySlider;
    GameObject Canvas;
    public GameObject OtherCharacter;
    public GameObject AmmoObject;
    private void Start()
    {
        Canvas = GameObject.FindGameObjectWithTag("Canvas");
        MySlider.maxValue = 3;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Rocket")
        {
            if (PlayerPrefs.GetInt("onOrOffVibration") == 1)
                TapticManager.Impact(ImpactFeedback.Light);
            Life = 0;
            MySlider.value = Life;
            other.transform.GetChild(0).gameObject.SetActive(false);
            other.transform.GetChild(1).gameObject.SetActive(false);
            other.transform.GetChild(2).gameObject.SetActive(true);
            other.transform.GetChild(3).gameObject.SetActive(false);
            StartCoroutine(Failed());
        }
        if (other.tag == "ThrowObject")
        {
            if (PlayerPrefs.GetInt("onOrOffVibration") == 1)
                TapticManager.Impact(ImpactFeedback.Light);
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
                //StartCoroutine(FailedOtherCharacter());
            }
        }
        if (other.tag == "Finish")
        {
            if (PlayerPrefs.GetInt("onOrOffVibration") == 1)
                TapticManager.Impact(ImpactFeedback.Light);
            StartCoroutine(Complete());
        }
    }
    IEnumerator Complete()
    {
        GameManager.Instance.characterMove.GuardsBackİddle();
        transform.GetComponent<Animator>().SetLayerWeight(1, 0);
        transform.GetComponent<Animator>().SetTrigger("Win");
        GameManager.Instance.characterMove.enabled = false;
        yield return new WaitForSeconds(1.5f);
        Canvas.transform.GetComponent<UIScript>().WinOpener();
    }
    IEnumerator Failed()
    {
        GameManager.Instance.characterMove.enabled = false;
        GameManager.Instance.characterMove.GuardsBackİddle();
        transform.GetComponent<Animator>().SetLayerWeight(1, 0);
        transform.GetComponent<Animator>().SetTrigger("Failed");
        yield return new WaitForSeconds(1f);
        Canvas.transform.GetComponent<UIScript>().FailOpener();
    }
}
