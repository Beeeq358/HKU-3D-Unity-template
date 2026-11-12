using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bow : MonoBehaviour
{
    public Vector3 idleSpot = new(-0.3f, 0, 0);
    public Quaternion idleRotation = Quaternion.Euler(-45, 0, 0);

    public InputActionAsset InputActions;
    private InputAction playerAttackAction;
    public CinemachineOrbitalFollow orbitalFollow;
    public float chargeStrength;
    public float chargeSpeed = 15f;
    public Camera cam;

    public Vector3 defaultCameraOffset = new(0,2,0);

    public enum BowState
    {
        Idle,
        Aiming,
        Shooting
    }
    public BowState currentState = BowState.Idle;

    private void Awake()
    {
        playerAttackAction = InputSystem.actions.FindAction("Attack");
    }

    private void OnEnable()
    {
        InputActions.FindActionMap("Player").Enable();
    }

    private void OnDisable()
    {
        InputActions.FindActionMap("Player").Disable();
    }

    private void Update()
    {
        chargeStrength = Mathf.Clamp(chargeStrength, 0, 50);
        Camerawork();


        switch (currentState)
        {
            case BowState.Idle:
                if (playerAttackAction.IsPressed())
                {
                    currentState = BowState.Aiming;
                }
                break;
            case BowState.Aiming:
                chargeStrength += Time.deltaTime * chargeSpeed;
                if (!playerAttackAction.IsPressed())
                {
                    currentState = BowState.Shooting;
                }
                break;
            case BowState.Shooting:
                Fire();
                currentState = BowState.Idle;
                break;
        }
    }
    
    public void Camerawork()
    {
        orbitalFollow.TargetOffset = new Vector3(0, 2 - chargeStrength / (50 / defaultCameraOffset.y), chargeStrength / 10);
    }

    public void Fire()
    {
        chargeStrength = 0;
    }

}
