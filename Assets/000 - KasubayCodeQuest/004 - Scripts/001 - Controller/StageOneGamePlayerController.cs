using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageOneGamePlayerController : MonoBehaviour
{
    [field: Header("DEBUGGER")]
    [field: SerializeField] public bool IsLeft { get; private set; }
    [field: SerializeField] public bool IsRight { get; private set; }
    [field: SerializeField] public bool IsJump { get; private set; }

    //  ==========================

    GameControls gameControls;

    //  ==========================

    private void OnEnable()
    {
        gameControls = new GameControls();
        gameControls.Enable();

        gameControls.StageOne.Left.started += _ => LeftStart();
        gameControls.StageOne.Left.canceled += _ => LeftStop();
        gameControls.StageOne.Right.started += _ => RightStart();
        gameControls.StageOne.Right.canceled += _ => RightStop();
        gameControls.StageOne.Jump.started += _ => JumpStart();
        gameControls.StageOne.Jump.canceled += _ => JumpStop();
    }

    private void OnDisable()
    {
        gameControls.StageOne.Left.started -= _ => LeftStart();
        gameControls.StageOne.Left.canceled -= _ => LeftStop();
        gameControls.StageOne.Right.started -= _ => RightStart();
        gameControls.StageOne.Right.canceled -= _ => RightStop();
        gameControls.StageOne.Jump.started -= _ => JumpStart();
        gameControls.StageOne.Jump.canceled -= _ => JumpStop();
    }

    private void LeftStart()
    {
        IsLeft = true;
    }

    private void LeftStop()
    {
        IsLeft = false;
    }

    private void RightStart()
    {
        IsRight = true;
    }

    private void RightStop()
    {
        IsRight = false;
    }

    private void JumpStart()
    {
        IsJump = true;
    }

    private void JumpStop()
    {
        IsJump = false;
    }

    public void JumpForceStop()
    {
        IsJump = false;
    }
}
