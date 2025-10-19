namespace RealWorldPlot.Interfaces.GeometryHelpers;

public class ProjectionResult
{
    public RealWorldPoint ProjectedPoint { get; set; }       // Le point projeté
    public bool IsOnSegment { get; set; }            // True si sur le segment P1–P2
    public double DistanceToLine { get; set; }        // Distance de A1 à la droite P1–P2
}