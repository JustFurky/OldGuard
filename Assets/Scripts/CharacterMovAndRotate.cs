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
    public List<GameObject> CollectedGuards = new List<GameObject>();
    [HideInInspector]
    public bool[] MainGuards;
    public Transform[] GuardsPositions;
    int siraIndex = 0;
    public GameObject Guards;
    public GameObject World;
    bool Move = false;
    Transform Target;
    GameObject currentMove;
    public bool AllDeath = false;

    //Input
    Vector3 translation2;
    public float Xspeed = 75;
    bool isMovementReleased;
    Vector2 firstPressPos;
    Touch touch;

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
            currentMove.transform.position = Vector3.MoveTowards(currentMove.transform.position, Target.position, 0.15f);
            if (Vector3.Distance(currentMove.transform.position, Target.position) < 0.0001f)
            {
                currentMove.transform.GetComponent<GuardScript>().MaterialChangeNormal();
                currentMove.GetComponent<Rigidbody>().detectCollisions = true;
                currentMove.transform.localRotation = Quaternion.identity;
                Move = false;
            }
        }
#if UNITY_EDITOR

        if (Input.GetMouseButton(0))
        {
                translation2 = new Vector3(0, Input.GetAxis("Mouse X") * Time.deltaTime * Xspeed, 0);

                Guards.transform.Rotate(translation2);
                //Guards.transform.eulerAngles = new Vector3(0, Mathf.Clamp(Guards.transform.eulerAngles.y, 20, 320), 0);
        }

#elif UNITY_IOS || UNITY_ANDROID

        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                isMovementReleased = false;
                Guards.transform.localEulerAngles = new Vector3(0, Guards.transform.localEulerAngles.y + touch.deltaPosition.x * Time.deltaTime *Xspeed, 0);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                isMovementReleased = true;
            }
            else if (touch.phase == TouchPhase.Began)
            {
                //save began touch 2d point
                firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            }
        }

#endif
        //if (MyJoystick.Horizontal < -0.15f)
        //{
        //    if (Guards.gameObject.transform.rotation.y > -1f)
        //    {
        //        Guards.transform.Rotate(0, -5, 0);
        //    }
        //}
        //if (MyJoystick.Horizontal > 0.15f)
        //{
        //    if (Guards.gameObject.transform.rotation.y <= 0.90f)
        //    {
        //        Guards.transform.Rotate(0, 5, 0);
        //    }
        //}
        transform.Translate(-Vector3.right * Time.deltaTime * 4, Space.World);
        Guards.transform.Translate(-Vector3.right * Time.deltaTime * 4, Space.World);
    }
    public void CollectGuard(GameObject NewGuard)
    {
        if (siraIndex < 6)
        {
            NewGuard.GetComponent<Animator>().SetTrigger("GuardWalk");
            NewGuard.transform.parent = null;
            NewGuard.transform.GetComponent<Rigidbody>().detectCollisions = false;
            CollectedGuards.Add(NewGuard);//alttaki hız*2 olacak
            NewGuard.transform.parent = GuardsYedekPos[siraIndex].transform;
            NewGuard.transform.DOMove(new Vector3(GuardsYedekPos[siraIndex].transform.position.x - 4, GuardsYedekPos[siraIndex].transform.position.y, GuardsYedekPos[siraIndex].transform.position.z), 1f).OnComplete(() => TweenCompeted(NewGuard));
            NewGuard.transform.DOLookAt(GuardsYedekPos[siraIndex].transform.position, 0.2f);
            siraIndex++;
        }
    }
    void TweenCompeted(GameObject go)
    {
        go.transform.localRotation = Quaternion.identity;
    }
    public void MoveCollecterdGuards(int Index, GameObject Go)
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
            CollectedGuards[i].transform.DOMove(GuardsYedekPos[i].transform.position + new Vector3(-4f, 0, 0), 1);
            CollectedGuards[i].transform.parent = GuardsYedekPos[i].transform;
        }
    }
    public void GuardsBackİddle()
    {
        for (int i = 0; i < GuardsPositions.Length; i++)
        {
            if (GuardsPositions[i].transform.childCount > 0)
            {
                GuardsPositions[i].transform.GetChild(0).GetComponent<Animator>().SetTrigger("BreathIddle");
            }
        }
        for (int i = 0; i < GuardsYedekPos.Count; i++)
        {
            if (GuardsYedekPos[i].transform.childCount > 0)
            {
                GuardsYedekPos[i].transform.GetChild(0).GetComponent<Animator>().SetTrigger("BreathIddle");
            }
        }
    }
    public int IsAllFull()
    {
        for (int i = 0; i < MainGuards.Length; i++)
        {
            if (MainGuards[i] == false)
            {
                return (i);
            }
        }
        return (5);
    }
}
