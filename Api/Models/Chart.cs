namespace DentResistanceOilCanningUpgrade.Models
{
  public class Chart
  {
    private double _deflection;
    private double _load;

    public Chart(double Deflection, double Load)
    {
      this._deflection = Deflection;
      this._load = Load;
    }

    public double Deflection
    {
      get => this._deflection;
      set => this._deflection = value;
    }

    public double Load
    {
      get => this._load;
      set => this._load = value;
    }
  }
}
