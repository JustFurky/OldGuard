using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class GuardScript : MonoBehaviour
{
    public Material Orange;
    public Material Red;
    public GameObject Shield;
    public bool startGuard=false;
    public int PositionIndex;
    int ThrowHit = 3;
    GameObject World;
   
    private void Start()
    {
        World = GameObject.FindGameObjectWithTag("World");
        if (startGuard)
        {
            transform.GetComponent<Animator>().SetTrigger("GuardWalk");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="GuardYedek")
        {
            if (GameManager.Instance.characterMove.IsAllFull()<3)
            {
                GameManager.Instance.characterMove.MoveCollecterdGuards(GameManager.Instance.characterMove.IsAllFull(),other.gameObject);
            }
            else
            {
                GameManager.Instance.characterMove.CollectGuard(other.gameObject);
            }
        }
        if (other.tag == "ThrowObject")
        {
            transform.GetComponent<Animator>().SetTrigger("Hit");
            other.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
            other.transform.GetComponent<MeshRenderer>().enabled = false;
            other.transform.GetComponent<Rigidbody>().detectCollisions = false;
            Destroy(other.gameObject, 1);
            ThrowHit--;
            if (ThrowHit == 2)
            {
                Shield.transform.GetComponent<MeshRenderer>().material= Orange;
            }
            if (ThrowHit==1)
            {
                Shield.transform.GetComponent<MeshRenderer>().material=Red;
            }
            if (ThrowHit == 0)
            {
               // Shield.transform.parent = null;//tercihe göre kapatılacak
                if (GameManager.Instance.characterMove.CollectedGuards.Count > 0)
                {
                    GameManager.Instance.characterMove.MoveBackupGuards(PositionIndex);
                    transform.parent = null;
                    GetComponent<Animator>().SetTrigger("Die");
                }
                else
                {
                    GameManager.Instance.characterMove.MainGuards[PositionIndex - 1] = false;
                    transform.parent = World.transform;
                    GetComponent<Animator>().SetTrigger("Die");
                }
            }
        }
    }
}
