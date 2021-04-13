using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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


    void Start()
    {
        LevelText.text = "Level "+PlayerPrefs.GetInt("LevelIndex").ToString();
        Barslider.maxValue = Vector3.Distance(Character.transform.position, Finish.transform.position);
        StartDistance = Vector3.Distance(Character.transform.position, Finish.transform.position);
    }
    void Update()
    {
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
        PlayerPrefs.SetInt("LevelIndex", PlayerPrefs.GetInt("LevelIndex") + 1);
        if (PlayerPrefs.GetInt("LevelIndex") > 3)
        {
            SceneManager.LoadScene("Level" + Random.Range(1, 3));
        }
        else
        {
            SceneManager.LoadScene("Level" + PlayerPrefs.GetInt("LevelIndex"));
        }
    }
    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
