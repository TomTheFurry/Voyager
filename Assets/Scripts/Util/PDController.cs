using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PDController
{
    double _error = 0;
    public double p;
    public double d;
    public PDController(double p = 0.02, double d = 0.02) {
        this.p = p;
        this.d = d;
    }

    public double tick(double newError)
    {
        double v = p * newError + d * (newError - _error);
        _error = newError;
        return v;
    }
    public void reset(double error = 0) {
        _error = error;
    }
}
