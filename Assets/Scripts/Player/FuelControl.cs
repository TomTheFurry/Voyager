
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[RequireComponent(typeof(MovementControl2))]
public class FuelControl : MonoBehaviour
{
    public float maxFuel = 1000;
    public float fuel = 1000;
    public float fuelPerForce = 0.002f;
    public float fuelExpValue = 2.0f;
    public float fuelDeathTimer = 3f;
    public UnityEvent onOutOfFuel;
    // Note: Only triggers if privously out of fuel
    public UnityEvent onRestoreFuel;
    MovementControl2 control2;
    CanvasHandler canvas;
    Tooltip tooltip;
    UIBar bar;
    private float _deathTime = 0;

    private void Start()
    {
        control2 = GetComponent<MovementControl2>();
        canvas = FindObjectOfType<CanvasHandler>();
        tooltip = canvas.cornerHud.fuelBar.GetComponent<Tooltip>();
        bar = canvas.cornerHud.fuelBar.GetComponent<UIBar>();
    }

    void Update()
    {
        string langString = LangSystem.GetLang("UI","HudFuelTip");
        if (tooltip != null) tooltip.text = langString + " " + fuel.ToString("F2") + " / " + maxFuel.ToString();
        if (bar != null)
        {
            bar.SetValue(fuel);
            bar.SetMinValue(0);
            bar.SetMaxValue(maxFuel);
        }

        if (_deathTime != 0 && Time.time - _deathTime >= fuelDeathTimer)
        {
            canvas.OnFail();
            _deathTime = 0;
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
            _deathTime = Time.time;
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
            _deathTime = Time.time;
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
            if (_deathTime == 0) FindObjectOfType<CanvasHandler>().DebugUndoFail();
            _deathTime = 0;
            if (triggerCallback) onRestoreFuel.Invoke();
        }
    }


}
