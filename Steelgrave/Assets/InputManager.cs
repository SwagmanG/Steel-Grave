using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class InputManager : MonoBehaviour
{
    private Controls controls;

    [Header("Rotation")]
    public float rotationSpeed = 100f;

    private bool isRotating = false;
    private float rotationInput;

    [Header("Hold")]
    public float HoldThreshold = 0.2f; // how long before tap becomes hold
    public float HoldInterval = 0.1f; // frequency of hold coroutine

    private bool isPressed;
    private bool isHolding = false;
    private float timeHeld = 0f;


    private void Awake()
    {
        controls = new Controls();

        // rotation
        controls.TankControls.Rotate.performed += RotateKeypadPerformed;
        controls.TankControls.Rotate.canceled += RotateKeypadCanceled;

        // fire and cooldown
        controls.TankControls.Primary.started += PrimaryStarted;
        controls.TankControls.Primary.canceled += PrimaryCanceled;

        // secondary fire
        controls.TankControls.Secondary.performed += SecondaryPerformed;

        // tertiary  fire
        controls.TankControls.Tertiary.performed += TertiaryPerformed;
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void Update()
    {
        if (isRotating)
        {
            transform.Rotate(Vector3.up * rotationInput * rotationSpeed * Time.deltaTime);
        }

        if (isPressed)
        {
            timeHeld += Time.deltaTime;

            if (timeHeld > HoldThreshold && !isHolding)
            {
                isHolding = true;
                Debug.Log("Hold!");
                StartCoroutine(HoldRoutine());
            }

        }

    }

    //  ROTATION
    private void RotateKeypadPerformed(InputAction.CallbackContext context)
    {

        isRotating = true;

        rotationInput = context.ReadValue<float>();
    }

    private void RotateKeypadCanceled(InputAction.CallbackContext context)
    {
        isRotating = false;
    }

    // PRIMARY FIRE (fire/cooldown)
    void PrimaryStarted(InputAction.CallbackContext context)
    {
        isPressed = true;
    }

    void PrimaryCanceled(InputAction.CallbackContext context)
    {
        isPressed = false; 
        if (!isHolding && timeHeld > 0f && timeHeld <= HoldThreshold)
        {
            // PUT FIRE HERE
            Debug.Log("Tap!");
        }

        timeHeld = 0f;
        StopCoroutine(HoldRoutine());
        isPressed = false;
        isHolding = false;
    }

    IEnumerator HoldRoutine()
    {
        while (isHolding)
        {
            Debug.Log("Holding...");
            // PUT COOLDOWN HERE
            yield return new WaitForSeconds(HoldInterval);
        }
    }

    // SECONDARY FIRE 
    void SecondaryPerformed(InputAction.CallbackContext context)
    {
       // PUT SECONDARY HERE
    }

    // TERTIARY FIRE FIRE
    void TertiaryPerformed(InputAction.CallbackContext context)
    {
        // PUT TERTIARY HERE
    }
}
