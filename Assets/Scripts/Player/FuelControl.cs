
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Events;

public class FuelControl : MonoBehaviour
{
    public float maxFuel = 1000;
    public float fuel = 1000;
    public float fuelPerForce = 0.002f;
    public float fuelExpValue = 2.0f;

    public Text textObj = null;
    public UIBar bar = null;

    public UnityEvent onOutOfFuel;
    // Note: Only triggers if privously out of fuel
    public UnityEvent onRestoreFuel;

    void Update()
    {
        if (textObj != null) textObj.text = "Fuel: " + fuel.ToString("F2") + " / " + maxFuel.ToString();
        if (bar != null)
        {
            bar.SetValue(fuel);
            bar.SetMinValue(0);
            bar.SetMaxValue(maxFuel);
        }
    }

    public void RecordForce(Vector3 force) { // public facing function
        float fuelUsed = Mathf.Abs(force.x) + Mathf.Abs(force.y) + Mathf.Abs(force.z);
        fuelUsed = Mathf.Pow(fuelUsed, fuelExpValue);
        fuel -= fuelUsed * fuelPerForce;
        if (fuel < 0)
        {
            fuel = 0;
            Debug.Log("Out of fuel!");
            onOutOfFuel.Invoke();
        }
    }

    public void OnCheatRefuel(InputValue value) //InputSystem Callback
    {
        if (value.isPressed)
        {
            bool triggerCallback = fuel == 0;
            fuel = maxFuel;
            Debug.Log("Refueled!");
            if (triggerCallback) onRestoreFuel.Invoke();
        }
    }


}
