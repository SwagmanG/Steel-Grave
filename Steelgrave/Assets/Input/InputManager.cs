using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class InputManager : MonoBehaviour
{
    private Player player;

    private Controls controls;

    private bool isDisabled;

    [Header("Attacks")]
    public ProjectileType DefaultProjectile;
    public ProjectileType RocketProjectile;

    [Header("Rotation")]
    public float rotationSpeed = 150f;

    private bool isRotating = false;
    private float rotationInput;

    [Header("Timing")]
    // HOLD / TAP
    public float HoldThreshold = 0.2f; // how long before tap becomes hold
    public float HoldInterval = 0.1f; // frequency of hold coroutine

    private bool isPressed;
    private bool isHolding = false;
    private float timeHeld = 0f;

    // LAUNCH
    public float LaunchThreshold = 0.3f; // window for launch combo

    private bool downPressed = false;

    public void Init(Player _player)
    {
        player = _player;
    }

    private void Awake()
    {
        controls = new Controls();

        // rotation
        controls.TankControls.Rotate.performed += RotateKeypadPerformed;
        controls.TankControls.Rotate.canceled += RotateKeypadCanceled;

        // fire and cooldown
        controls.TankControls.Trigger.started += TriggerStarted;
        controls.TankControls.Trigger.canceled += TriggerCanceled;

        // up
        controls.TankControls.Up.performed += UpPerformed;

        // down
        controls.TankControls.Down.performed += DownPerformed;

       
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void Update()
    {

        if (isRotating)
        {
            player?.transform.Rotate(Vector3.forward * rotationInput * rotationSpeed * Time.deltaTime);
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
    public void SetDisabled(bool disabled)
    {
        isDisabled = disabled;
        if (isDisabled)
            controls.Disable();
        else
            controls.Enable();

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
    void TriggerStarted(InputAction.CallbackContext context)
    {
        isPressed = true;
    }

    void TriggerCanceled(InputAction.CallbackContext context)
    {
        isPressed = false; 
        if (!isHolding && timeHeld > 0f && timeHeld <= HoldThreshold)
        {
            // PUT FIRE HERE
            Debug.Log("Tap!");
            player?.Shoot(DefaultProjectile);
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
            GameManager.Instance.HeatCooldown();
            yield return new WaitForSeconds(HoldInterval);
        }
    }

    // LAUNCH COMBO
    void UpPerformed(InputAction.CallbackContext context)
    {
        if (downPressed)
        {
            Debug.Log("Down + Up");
            downPressed = false;
            player.CannonDown(false);
            // LAUNCH ROCKET
            player?.Shoot(RocketProjectile);
        }
        else
        {
            Debug.Log("Up");
            // FEEDBACK FOR EMPTY SHOT

        }
    }

    void DownPerformed(InputAction.CallbackContext context)
    {
        if (!downPressed)
        {
            downPressed = true;
            StartCoroutine(LaunchRoutine());

        }
    }

    IEnumerator LaunchRoutine()
    {

        float timer = 0f;

        while (timer < LaunchThreshold)
        {
            if (!downPressed) yield break; 

            timer += Time.deltaTime;
            player.CannonDown(true);
            yield return null;
        }

        player.CannonDown(false);
        downPressed = false;
    }
}
