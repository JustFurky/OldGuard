using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Throw : MonoBehaviour
{
    public Rigidbody RB;
    Transform Target;

    public float h = 25;
    public float Gravity = -18;




    void Start()
    {
        Target = GameObject.FindGameObjectWithTag("President").transform;
        transform.LookAt(Target.transform.position);
        RB.useGravity = false;
        Launch();
        transform.DORotate(new Vector3(Random.Range(90, 360), Random.Range(180, 360), Random.Range(60, 360)), 2);
    }

    void Launch()
    {
        Physics.gravity = Vector3.up * Gravity;
        RB.useGravity = true;
        RB.velocity = CalculateLaunchVelocity();
    }

    Vector3 CalculateLaunchVelocity()
    {
        float DisplacementY = Target.position.y - transform.position.y;
        Vector3 DisplacementXZ = new Vector3((Target.transform.position.x-4.5f) - transform.position.x, 0, Target.transform.position.z - transform.position.z);
        Vector3 VelocityY = Vector3.up * Mathf.Sqrt(-2 * Gravity * h);
        Vector3 VelocityXY = DisplacementXZ / (Mathf.Sqrt(-2 * h / Gravity) + Mathf.Sqrt(2 * (DisplacementY - h) / Gravity));
        return VelocityXY + VelocityY;
    }
}
