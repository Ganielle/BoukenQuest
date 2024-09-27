using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAttackController : MonoBehaviour
{
    [SerializeField] private float delay;

    public Action finalAction;

    private void OnEnable()
    {
        StartCoroutine(NextPhase());
    }

    IEnumerator NextPhase()
    {
        yield return new WaitForSeconds(delay);

        finalAction?.Invoke();

        Destroy(gameObject);
    }
}
