using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] AudioSource hornSFX;

    [Header("Input Actions")]
    [SerializeField] bool isPlayer = false;
    public InputActionReference move;
    public InputActionReference horn;
    public InputActionReference brake;

    
    CarPhysics carPhysics;

    // Start is called before the first frame update
    void Start()
    {
        carPhysics = GetComponent<CarPhysics>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayer)
        {
            AccelerationInput();
            TurnInput();
            HonkInput();
            BrakeInput();
        }
    }

#region Player Controls

    void AccelerationInput()
    {
        float acceleration = move.action.ReadValue<Vector3>().y;
        carPhysics.SetAccelerationInput(acceleration);
    }

    void TurnInput()
    {
        float turning = move.action.ReadValue<Vector3>().x;
        carPhysics.SetTurnInput(turning);
    }

    void BrakeInput()
    {
        float braking = brake.action.ReadValue<float>();
        carPhysics.SetBrakeInput(braking);
    }

    void HonkInput()
    {
        //FIXME: Change horn sound to a consistent sound

        float honkInput = horn.action.ReadValue<float>();
        if (honkInput > 0 && !hornSFX.isPlaying)
            {
                hornSFX.Stop();
                hornSFX.Play();
            }
    }
#endregion

}
