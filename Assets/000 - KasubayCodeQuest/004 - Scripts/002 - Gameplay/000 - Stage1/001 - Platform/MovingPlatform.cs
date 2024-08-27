using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private bool onAwake;
    [SerializeField] private bool isMovingUp;
    [SerializeField] private LeanTweenType easeType;
    [SerializeField] private float speed;
    [SerializeField] private float destination;

    int animation;

    private void Awake()
    {
        if (onAwake)
        {
            if (isMovingUp)
                LeanTween.moveY(gameObject, destination, speed).setEase(easeType).setLoopPingPong();
            else
                LeanTween.moveX(gameObject, destination, speed).setEase(easeType).setLoopPingPong();
        }
    }

    public void MoveToDestination(float finalDestination, bool isLooping = false)
    {
        if (animation != 0)
            LeanTween.cancel(animation);

        if (isLooping)
        {
            if (isMovingUp)
                animation = LeanTween.moveY(gameObject, finalDestination, speed).setEase(easeType).setLoopPingPong().id;
            else
                animation = LeanTween.moveX(gameObject, finalDestination, speed).setEase(easeType).setLoopPingPong().id;
        }
        else
        {
            if (isMovingUp)
                animation = LeanTween.moveY(gameObject, finalDestination, speed).setEase(easeType).id;
            else
                animation = LeanTween.moveX(gameObject, finalDestination, speed).setEase(easeType).id;
        }
    }
}
