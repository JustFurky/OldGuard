using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

public class CharacterMovAndRotate : MonoBehaviour
{
    public FloatingJoystick MyJoystick;
    public GameObject Character;
    public List<GameObject> GuardsYedekPos = new List<GameObject>();
    public List<GameObject> CollectedGuards=new List<GameObject>();
    [HideInInspector]
    public bool[] MainGuards;
    public Transform[] GuardsPositions;
    int siraIndex = 0;
    public GameObject Guards;
    public GameObject World;
    bool Move=false;
    Transform Target;
    GameObject currentMove;
    private void Start()
    {
        MainGuards = new bool[3];
        for (int i = 0; i < MainGuards.Length; i++)
        {
            MainGuards[i] = true;
        }
    }
    void FixedUpdate()
    {
        if (Move)
        {
            currentMove.transform.LookAt(Target.position);
            currentMove.transform.position = Vector3.MoveTowards(currentMove.transform.position, Target.position, 0.050f);
            if (Vector3.Distance(currentMove.transform.position, Target.position) < 0.001f)
            {
                currentMove.GetComponent<Rigidbody>().detectCollisions = true;
                Move = false;
                currentMove.transform.localRotation = Quaternion.identity;
            }
        }
        Guards.transform.Rotate(0, MyJoystick.Horizontal*3f,0);
        transform.Translate(-Vector3.right*Time.deltaTime*4,Space.World);
    }
    public void CollectGuard(GameObject NewGuard)
    {
        if (siraIndex<6)
        {
            NewGuard.GetComponent<Animator>().SetTrigger("GuardWalk");
            NewGuard.transform.parent =null;
            NewGuard.transform.GetComponent<Rigidbody>().detectCollisions = false;
            CollectedGuards.Add(NewGuard);//alttaki hız*2 olacak
            NewGuard.transform.parent = GuardsYedekPos[siraIndex].transform;


            // // Şöyle bir şey kullanabilirsin
            // TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = DOTween.To(() => GuardsYedekPos[siraIndex].transform.position, delegate (Vector3 x)
            // {
            //     NewGuard.transform.position += x; // Her framede eklemeleri olarak pozisyonu ekler, yani rotasyonda (dotween rotate de bunun gibi bi şey yaptırtabilirsin yani)
            // }, GuardsYedekPos[siraIndex].transform.position, 2);
            // tweenerCore.SetOptions(false).SetTarget(NewGuard);


            NewGuard.transform.DOMove(new Vector3(GuardsYedekPos[siraIndex].transform.position.x-4, GuardsYedekPos[siraIndex].transform.position.y, GuardsYedekPos[siraIndex].transform.position.z), 1f).OnComplete(()=>TweenCompeted(NewGuard));
            NewGuard.transform.DOLookAt(GuardsYedekPos[siraIndex].transform.position, 0.2f);
            siraIndex++; 
        }
    }
    void TweenCompeted(GameObject go)
    {
        go.transform.localRotation=Quaternion.identity;
    }
    public void MoveCollecterdGuards(int Index,GameObject Go)
    {
        Go.GetComponent<Animator>().SetTrigger("GuardWalk");
        Move = true;
        Target = GuardsPositions[Index];
        currentMove = Go;
        Go.GetComponent<GuardScript>().PositionIndex = Index;
        Go.transform.parent = GuardsPositions[Index].transform;
        Go.transform.localRotation = Quaternion.identity;
    }
    public void MoveBackupGuards(int Index)
    {
        Move = true;
        CollectedGuards[0].tag = "Guard";
        Target = GuardsPositions[Index - 1];
        currentMove = CollectedGuards[0];
        CollectedGuards[0].GetComponent<GuardScript>().PositionIndex = Index;
        CollectedGuards[0].transform.parent = GuardsPositions[Index - 1].transform;
        CollectedGuards.Remove(CollectedGuards[0]);
        ////
        siraIndex--;
        for (int i = 0; i < CollectedGuards.Count; i++)
        {
            CollectedGuards[i].transform.DOMove(GuardsYedekPos[i].transform.position+new Vector3(-4f,0,0), 1);
        }
    }
    public void GuardsBackİddle()
    {
        for (int i = 0; i < GuardsPositions.Length; i++)
        {
            Debug.Log(GuardsPositions[i].name);
            GuardsPositions[i].transform.GetChild(0).GetComponent<Animator>().SetTrigger("BreathIddle");
        }
    }
    public int IsAllFull()
    {
        for (int i = 0; i < MainGuards.Length; i++)
        {
            if (MainGuards[i]==false)
            {
                return (i);
            }
        }
        return (5);
    }
}
