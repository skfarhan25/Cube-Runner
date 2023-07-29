using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    public float forwardforce = 8000f;
    public float sidewaysforce = 120f;
    private PlayerMovementInput playerMovementInput;

    void Awake()
    {
        playerMovementInput = new PlayerMovementInput();
        playerMovementInput.Player.Enable();
        playerMovementInput.LevelChange.Enable();
        // playerMovementInput.LevelChange.AreaSelect.performed += AreaChange;

    }

    // private void AreaChange(InputAction.CallbackContext context)
    // {
    //     Debug.Log(context.action.name + " " + context.action.ReadValue<float>());
    // }

    // FixedUpdate is called once per physics frame
    // it is used to move the player and apply physics to the player
    void Update()
    {
        Vector2 sidewaysDirection = playerMovementInput.Player.Move.ReadValue<Vector2>();
        // bool num1 = playerMovementInput.LevelChange.AreaSelect.ReadValue<bool>();

        rb.AddForce(0, 0, forwardforce * Time.deltaTime);

        if (sidewaysDirection.x == -1)
        {
            rb.AddForce(-sidewaysforce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }
        if (sidewaysDirection.x == 1)
        {
            rb.AddForce(sidewaysforce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }
        
        if (Keyboard.current.numpadPlusKey.wasPressedThisFrame)
        {
            GameManager.instance.LoadNextLevel();
        }
        if (Keyboard.current.numpadMinusKey.wasPressedThisFrame)
        {
            GameManager.instance.LoadPreviousLevel();
        }

        if (transform.position.y < -1f)
        {
            Debug.Log("We Fell Off!");
            GameManager.instance.EndGame();
            enabled = false;
        }
    }
}