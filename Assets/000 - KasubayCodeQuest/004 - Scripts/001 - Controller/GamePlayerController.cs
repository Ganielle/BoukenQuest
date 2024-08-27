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
    [field: SerializeField] public bool Pause { get; private set; }
    [field: SerializeField] public bool Disable { get; set; }

    [SerializeField] private TouchData[] activeTouches = new TouchData[10];  // Predefined array for a maximum of 10 touches
    [SerializeField] private int activeTouchCount = 0;

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

#if !UNITY_ANDROID
        gameControls.School.Look.performed += _ => LookStart();
        gameControls.School.Look.canceled += _ => LookStop();
#endif
    }

    private void OnDisable()
    {
        gameControls.School.Move.performed -= _ => MovementStart();
        gameControls.School.Move.canceled -= _ => MovementStop();
        gameControls.School.Interact.started += _ => InteractStart();
        gameControls.School.Interact.canceled += _ => InteractStop();
        gameControls.School.Pause.started -= _ => PauseStart();
        gameControls.School.Pause.canceled -= _ => PauseStop();


#if !UNITY_ANDROID
        gameControls.School.Look.performed -= _ => LookStart();
        gameControls.School.Look.canceled -= _ => LookStop();
#endif

        gameControls.Disable();
    }

    private void Update()
    {
#if UNITY_ANDROID
        MobileLookStart();
#endif
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

    private void MobileLookStart()
    {
        if (Disable)
        {
            LookDirection = Vector2.zero;
            activeTouchCount = 0;
            return;
        }

        int touchCount = Touchscreen.current.touches.Count;

        for (int i = 0; i < touchCount; i++)
        {
            var touch = Touchscreen.current.touches[i];
            int touchId = touch.touchId.ReadValue();
            UnityEngine.InputSystem.TouchPhase touchPhase = touch.phase.ReadValue();

            switch (touchPhase)
            {
                case UnityEngine.InputSystem.TouchPhase.Began:
                    {
                        // Check if the touch is already in the activeTouches array
                        bool isAlreadyActive = false;
                        for (int j = 0; j < activeTouchCount; j++)
                        {
                            if (activeTouches[j].touchId == touchId)
                            {
                                isAlreadyActive = true;
                                break;
                            }
                        }

                        // If the touch is new, add it to the activeTouches array
                        if (!isAlreadyActive)
                        {
                            bool isOverUI = EventSystem.current.IsPointerOverGameObject(touchId);

                            activeTouches[activeTouchCount] = new TouchData { touchId = touchId, isOverUI = isOverUI };
                            activeTouchCount++;
                        }
                        break;
                    }

                case UnityEngine.InputSystem.TouchPhase.Ended:
                case UnityEngine.InputSystem.TouchPhase.Canceled:
                    {
                        // Remove the touch by shifting the array
                        for (int j = 0; j < activeTouchCount; j++)
                        {
                            if (activeTouches[j].touchId == touchId)
                            {
                                // Shift all elements after the removed one to the left
                                for (int k = j; k < activeTouchCount - 1; k++)
                                {
                                    activeTouches[k] = activeTouches[k + 1];
                                }
                                activeTouchCount--;
                                break;
                            }
                        }

                        // Reset LookDirection if no active touches are left
                        if (activeTouchCount == 0)
                        {
                            LookDirection = Vector2.zero;
                        }
                        break;
                    }

                case UnityEngine.InputSystem.TouchPhase.Moved:
                case UnityEngine.InputSystem.TouchPhase.Stationary:
                    {
                        // Process active touches
                        for (int j = 0; j < activeTouchCount; j++)
                        {
                            if (activeTouches[j].touchId == touchId && !activeTouches[j].isOverUI)
                            {
                                LookDirection += touch.delta.ReadValue();
                                break;
                            }
                        }
                        break;
                    }
            }
        }
    }
}
