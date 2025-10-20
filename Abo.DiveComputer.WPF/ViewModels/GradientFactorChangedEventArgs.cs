namespace Abo.DiveComputer.WPF.ViewModels;

public class GradientFactorChangedEventArgs : EventArgs
{
    public int Low { get; }
    public int High { get; }
    public GradientFactorChangedEventArgs(int low, int high)
    {
        Low = low;
        High = high;
    }
}

public class GasSettingsChangedEventArgs : EventArgs
{
    public int O2Percentage { get; }
    public GasSettingsChangedEventArgs(int o2Percentage)
    {
        O2Percentage = o2Percentage;
    }
}