using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public enum State
    {
        Idle,
        Running,
        Crouching,
        Dashing,
        Sliding,
        Jumping,
        Talking
    }

    public State state;

    private void Start()
    {
        state = State.Idle;
    }

    private void Update()
    {
        //Debug.Log(state);
    }
}
