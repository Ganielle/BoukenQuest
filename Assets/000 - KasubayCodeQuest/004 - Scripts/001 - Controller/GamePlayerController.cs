using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GamePlayerController : MonoBehaviour
{
    private struct TouchData
    {
        public int touchId;
        public bool isOverUI;
    }


    [field: Header("DEBUGGER")]
    [field: SerializeField] public Vector2 MovementDirection { get; private set; }
    [field: SerializeField] public Vector2 LookDirection { get; private set; }
    [field: SerializeField] public bool Interact { get; private set; }
    [field: SerializeField] public bool IsJump { get; private set; }
    [field: SerializeField] public bool Pause { get; private set; }
    [field: SerializeField] public bool Disable { get; set; }

    //  ====================

    GameControls gameControls;

    //  ====================

    private void OnEnable()
    {
        gameControls = new GameControls();
        gameControls.Enable();

        gameControls.School.Move.performed += _ => MovementStart();
        gameControls.School.Move.canceled += _ => MovementStop();
        gameControls.School.Interact.started += _ => InteractStart();
        gameControls.School.Interact.canceled += _ => InteractStop();
        gameControls.School.Pause.started += _ => PauseStart();
        gameControls.School.Pause.canceled += _ => PauseStop();
        gameControls.School.Jump.started += _ => JumpStart();
        gameControls.School.Jump.canceled += _ => JumpStop();
        gameControls.School.Look.performed += _ => LookStart();
        gameControls.School.Look.canceled += _ => LookStop();
    }

    private void OnDisable()
    {
        gameControls.School.Move.performed -= _ => MovementStart();
        gameControls.School.Move.canceled -= _ => MovementStop();
        gameControls.School.Interact.started += _ => InteractStart();
        gameControls.School.Interact.canceled += _ => InteractStop();
        gameControls.School.Pause.started -= _ => PauseStart();
        gameControls.School.Pause.canceled -= _ => PauseStop();
        gameControls.School.Jump.started -= _ => JumpStart();
        gameControls.School.Jump.canceled -= _ => JumpStop();
        gameControls.School.Look.performed -= _ => LookStart();
        gameControls.School.Look.canceled -= _ => LookStop();

        gameControls.Disable();
    }

    private void LookStart()
    {
        if (Disable)
        {
            LookDirection = Vector2.zero;
            return;
        }

        LookDirection = gameControls.School.Look.ReadValue<Vector2>();
    }

    private void LookStop()
    {
        LookDirection = Vector2.zero;
    }

    private void MovementStart()
    {
        if (Disable)
        {
            MovementDirection = Vector2.zero;
            return;
        }

        MovementDirection = gameControls.School.Move.ReadValue<Vector2>();
    }

    private void MovementStop()
    {
        MovementDirection = Vector2.zero;
    }

    private void InteractStart()
    {
        if (Disable)
        {
            Interact = false;
            return;
        }

        Interact = true;
    }

    private void InteractStop()
    {
        Interact = false;
    }

    public void InteractTurnOff()
    {
        Interact = false;
    }

    private void PauseStop()
    {
        if (Disable)
        {
            Pause = false;
            return;
        }

        Pause = true;
    }

    private void PauseStart()
    {
        Pause = false;
    }

    public void PauseTurnOff()
    {
        Pause = false;
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
