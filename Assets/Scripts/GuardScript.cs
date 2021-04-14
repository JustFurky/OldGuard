using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TapticPlugin;
public class GuardScript : MonoBehaviour
{
    public Material Orange;
    public Material Red;
    public GameObject Shield;
    public bool startGuard = false;
    public int PositionIndex;
    public int ThrowHit = 3;
    GameObject World;
    public List<GameObject> Chields = new List<GameObject>();
    public Material[] HoloMat;
    public Material[] SkinMat;
    public Material[] ShieldMats;
    bool go = false;
    public Transform[] Positions;
    public SkinnedMeshRenderer CharacterSkin;

    private void Start()
    {
        World = GameObject.FindGameObjectWithTag("World");
    }
    private void Update()
    {
        if (go==true)
        {
            transform.DOLocalMove(transform.parent.transform.position, 1);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Rocket")
        {
            if (PlayerPrefs.GetInt("onOrOffVibration") == 1)
                TapticManager.Impact(ImpactFeedback.Light);
            GameManager.Instance.characterMove.AllDeath = true;
            other.gameObject.transform.GetComponent<BoxCollider>().enabled = false;
            other.transform.GetChild(0).gameObject.SetActive(false);
            other.transform.GetChild(1).gameObject.SetActive(false);
            other.transform.GetChild(2).gameObject.SetActive(true);
            other.transform.GetChild(3).gameObject.SetActive(false);
            other.transform.GetChild(2).gameObject.transform.parent=null;
            Shield.transform.GetComponent<MeshRenderer>().material = Red;
            if (GameManager.Instance.characterMove.CollectedGuards.Count > 0)
            {
                GameManager.Instance.characterMove.MoveBackupGuards(PositionIndex);
                transform.parent = null;
                GetComponent<Animator>().SetTrigger("Die");
                transform.GetComponent<BoxCollider>().enabled = false;
                gameObject.tag = "Untagged";
            }
            else
            {
                GameManager.Instance.characterMove.MainGuards[PositionIndex - 1] = false;
                transform.parent = null;
                GetComponent<Animator>().SetTrigger("Die");
                transform.GetComponent<BoxCollider>().enabled = false;
                gameObject.tag = "Untagged";
            }
        }
        if (other.gameObject.tag == "GuardYedek")
        {
            if (GameManager.Instance.characterMove.IsAllFull() < 3)
            {
                GameManager.Instance.characterMove.MoveCollecterdGuards(GameManager.Instance.characterMove.IsAllFull(), other.gameObject);
            }
            else
            {
                other.transform.GetComponent<GuardScript>().MaterialChangeHolo();
                GameManager.Instance.characterMove.CollectGuard(other.gameObject);
            }
        }
        if (other.tag == "ThrowObject")
        {
            if (PlayerPrefs.GetInt("onOrOffVibration") == 1)
                TapticManager.Impact(ImpactFeedback.Light);
            transform.GetComponent<Animator>().SetTrigger("Hit");
            other.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
            other.transform.GetComponent<MeshRenderer>().enabled = false;
            other.transform.GetComponent<Rigidbody>().detectCollisions = false;
            Destroy(other.gameObject, 1);
            ThrowHit--;
            if (ThrowHit == 2)
            {
                Shield.transform.GetComponent<MeshRenderer>().material = Orange;
            }
            if (ThrowHit == 1)
            {
                Shield.transform.GetComponent<MeshRenderer>().material = Red;
            }
            if (ThrowHit == 0)
            {
                if (GameManager.Instance.characterMove.CollectedGuards.Count > 0)
                {
                    GameManager.Instance.characterMove.MoveBackupGuards(PositionIndex);
                    transform.parent = null;
                    GetComponent<Animator>().SetTrigger("Die");
                }
                else
                {
                    GameManager.Instance.characterMove.MainGuards[PositionIndex-1] = false;
                    transform.parent = null;
                    GetComponent<Animator>().SetTrigger("Die");
                }
            }
        }
    }
    public void MaterialChangeHolo()
    {
        Shield.transform.GetComponent<MeshRenderer>().materials = HoloMat;
        CharacterSkin.materials = HoloMat;
        for (int i = 0; i < Chields.Count; i++)
        {
            Chields[i].SetActive(false);
        }
    }
    public void MaterialChangeNormal()
    {
        Shield.transform.GetComponent<MeshRenderer>().materials = ShieldMats;
        CharacterSkin.materials = SkinMat;
        for (int i = 0; i < Chields.Count; i++)
        {
            Chields[i].SetActive(true);
        }
    }
}
