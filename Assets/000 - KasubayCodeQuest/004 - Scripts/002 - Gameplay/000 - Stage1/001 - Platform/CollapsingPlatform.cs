using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapsingPlatform : MonoBehaviour
{
    [SerializeField] private Rigidbody floorRB;
    [SerializeField] private MeshRenderer floorMR;
    [SerializeField] private Collider floorCollider;
    [SerializeField] private float waitTimeCollapse;
    [SerializeField] private float waitTimeToSpawn;

    [Header("DEBUGGER")]
    [SerializeField] private Vector3 defaultPosition;
    [SerializeField] private bool cannotStop;

    //  =====================

    Coroutine triggerToCollapse;
    int collapseAnimation;

    //  =====================

    private void Awake()
    {
        floorRB.velocity = Vector2.zero;
        floorRB.useGravity = false;
        floorMR.enabled = true;
        floorCollider.enabled = true;
        cannotStop = false;
        floorRB.isKinematic = true;

        defaultPosition = transform.position;
    }

    public void StartToCollapse()
    {
        triggerToCollapse = StartCoroutine(TriggerToCollapse());
    }

    public void StopCollapseOnExit()
    {
        if (!cannotStop)
        {
            if (triggerToCollapse != null)
            {
                StopCoroutine(triggerToCollapse);
                triggerToCollapse = null;
            }
        }
    }

    IEnumerator TriggerToCollapse()
    {
        float currentTime = 0f;

        while (currentTime < waitTimeCollapse)
        {
            currentTime += Time.deltaTime;
            yield return null;
        }
        cannotStop = true;
        collapseAnimation = LeanTween.moveX(gameObject, transform.position.x + 1.5f, 0.05f).setLoopPingPong().id;
        StartCoroutine(CheckToDestroy());
    }

    IEnumerator CheckToDestroy()
    {
        float time = 0f;

        while (time < waitTimeCollapse)
        {
            time += Time.deltaTime;
            yield return null;
        }

        if (collapseAnimation != 0) LeanTween.cancel(collapseAnimation);

        floorRB.isKinematic = false;
        transform.position = defaultPosition;
        floorRB.useGravity = true;

        yield return new WaitForSeconds(waitTimeToSpawn);

        floorMR.enabled = false;
        floorCollider.enabled = false;

        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        float currentTime = 0f;

        while (currentTime < waitTimeToSpawn)
        {
            currentTime += Time.deltaTime;
            yield return null;
        }

        floorRB.velocity = Vector2.zero;
        floorRB.useGravity = false;
        transform.position = defaultPosition;
        floorMR.enabled = true;
        floorCollider.enabled = true;
        cannotStop = false;
        floorRB.isKinematic = true;
    }
}
