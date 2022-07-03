
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(MovementControl2))]
public class FuelControl : MonoBehaviour
{
    public float maxFuel = 1000;
    public float fuel = 1000;
    public float fuelPerForce = 0.002f;
    public float fuelExpValue = 2.0f;
    public UnityEvent onOutOfFuel;
    // Note: Only triggers if privously out of fuel
    public UnityEvent onRestoreFuel;
    MovementControl2 control2;
    CanvasHandler canvas;
    Tooltip tooltip;
    UIBar bar;

    private void Start()
    {
        control2 = GetComponent<MovementControl2>();
        canvas = FindObjectOfType<CanvasHandler>();
        tooltip = canvas.cornerHud.fuelBar.GetComponent<Tooltip>();
        bar = canvas.cornerHud.fuelBar.GetComponent<UIBar>();
    }

    void Update()
    {
        if (tooltip != null) tooltip.text = "Fuel: " + fuel.ToString("F2") + " / " + maxFuel.ToString();
        if (bar != null)
        {
            bar.SetValue(fuel);
            bar.SetMinValue(0);
            bar.SetMaxValue(maxFuel);
        }
    }

    public bool HasFuel() {
        return fuel > 0;
    }

    public bool RecordUseFuel(float fuelUsed) {
        if (fuel == 0) return false;
        fuel -= fuelUsed;
        if (fuel < 0) { 
            fuel = 0;
            Debug.Log("Out Of fuel!");
            control2.DisableSpaceshipInput();
            canvas.OnFail();
            onOutOfFuel.Invoke();
            return false;
        }
        return true;
    }

    public void RecordForce(Vector3 force) { // public facing function
        float fuelUsed = Mathf.Abs(force.x) + Mathf.Abs(force.y) + Mathf.Abs(force.z);
        fuelUsed = Mathf.Pow(fuelUsed, fuelExpValue);
        fuel -= fuelUsed * fuelPerForce;
        if (fuel < 0)
        {
            fuel = 0;
            Debug.Log("Out of fuel!");
            control2.DisableSpaceshipInput();
            canvas.OnFail();
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
            control2.EnableSpaceshipInput();
            FindObjectOfType<CanvasHandler>().DebugUndoFail();
            if (triggerCallback) onRestoreFuel.Invoke();
        }
    }


}
