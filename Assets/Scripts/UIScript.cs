using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TapticPlugin;

public class UIScript : MonoBehaviour
{
    public GameObject GamePanel;
    public GameObject FailPanel;
    public GameObject CompletePanel;
    public Slider Barslider;
    public GameObject Character;
    public GameObject Finish;
    public Text LevelText;
    float StartDistance;
    public GameObject MainGameObject;
    public GameObject TutorialObject;
    public GameObject[] RocketMans;

    bool FirstTab = true;

    void Start()
    {
        LevelText.text = "Level "+PlayerPrefs.GetInt("LevelIndex").ToString();
        Barslider.maxValue = Vector3.Distance(Character.transform.position, Finish.transform.position);
        StartDistance = Vector3.Distance(Character.transform.position, Finish.transform.position);
        MainGameObject.transform.GetComponent<CharacterMovAndRotate>().enabled = false;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (FirstTab==true)
            {
                if (PlayerPrefs.GetInt("onOrOffVibration") == 1)
                    TapticManager.Impact(ImpactFeedback.Light);
                FirstTab = false;
                TutorialObject.SetActive(false);
                Character.transform.GetComponent<Animator>().SetTrigger("Start");
                MainGameObject.transform.GetComponent<CharacterMovAndRotate>().enabled = true;
                MainGameObject.transform.GetComponent<CharacterMovAndRotate>().GuardStartAnim();
                for (int i = 0; i < RocketMans.Length; i++)
                {
                    RocketMans[i].transform.GetComponent<RocketmanScript>().enabled = true;
                } 
            }
        }
        Barslider.value = StartDistance-Vector3.Distance(Character.transform.position, Finish.transform.position);
    }
    public void WinOpener()
    {
        GamePanel.SetActive(false);
        FailPanel.SetActive(false);
        CompletePanel.SetActive(true);
    }
    public void FailOpener()
    {
        GamePanel.SetActive(false);
        FailPanel.SetActive(true);
        CompletePanel.SetActive(false);
    }
    public void NextButton()
    {
        if (PlayerPrefs.GetInt("onOrOffVibration") == 1)
            TapticManager.Impact(ImpactFeedback.Light);
        PlayerPrefs.SetInt("LevelIndex", PlayerPrefs.GetInt("LevelIndex") + 1);
        if (PlayerPrefs.GetInt("LevelIndex") > 5)
        {
            SceneManager.LoadScene("Level" + Random.Range(2, 5));
        }
        else
        {
            SceneManager.LoadScene("Level" + PlayerPrefs.GetInt("LevelIndex"));
        }
    }
    public void RestartButton()
    {
        if (PlayerPrefs.GetInt("onOrOffVibration") == 1)
            TapticManager.Impact(ImpactFeedback.Light);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
