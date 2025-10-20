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