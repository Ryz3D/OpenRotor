public class Powertrain {
    public DataCurve throttleCurrentCurve;
    public DataCurve currentThrustCurve;

    public Powertrain() {
        throttleCurrentCurve = new DataCurve();
        currentThrustCurve = new DataCurve();
    }
}
