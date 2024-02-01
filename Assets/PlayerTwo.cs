using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTwo : MonoBehaviour
{
    PlayerTwoInput input;
    InputAction action;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        input = new PlayerTwoInput();
        action = input.FindAction("Move");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerTwoMove()
    {
        Vector2 movement = action.ReadValue<Vector2>();
        transform.position += new Vector3(movement.x, 0f, movement.y) * Time.deltaTime * speed;
    }
}
