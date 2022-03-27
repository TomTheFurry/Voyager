
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class FuelControl : MonoBehaviour
{
    public float maxFuel = 1000;
    public float fuel = 1000;
    public float fuelPerForce = 0.002f;
    public float fuelExpValue = 2.0f;

    public Text textObj;

    void Update()
    {
        textObj.text = "Fuel: " + fuel + " / " + maxFuel;
    }

    public void OnForce(Vector3 force) {
        float fuelUsed = Mathf.Abs(force.x) + Mathf.Abs(force.y) + Mathf.Abs(force.z);
        fuelUsed = Mathf.Pow(fuelUsed, fuelExpValue);
        fuel -= fuelUsed * fuelPerForce;
        if (fuel < 0)
        {
            fuel = 0;
            SendMessage("OnPausePlayerInput");
        }
    }

    public void OnCheatRefuel(InputValue value)
    {
        if (value.isPressed)
        {
            fuel = maxFuel;
            SendMessage("OnResumePlayerInput");
        }
    }


}
