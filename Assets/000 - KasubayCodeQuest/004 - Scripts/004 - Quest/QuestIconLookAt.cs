using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestIconLookAt : MonoBehaviour
{
    public Transform target; // The target object to look at

    void Update()
    {
        transform.LookAt(transform.position + target.transform.rotation * Vector3.left);
    }
}
