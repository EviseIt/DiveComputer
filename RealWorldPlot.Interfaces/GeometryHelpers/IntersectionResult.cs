namespace RealWorldPlot.Interfaces.GeometryHelpers;

public class IntersectionResult
{
    public bool Intersects { get; set; }              // Les droites se croisent
    public bool OnBothSegments { get; set; }          // Le point est sur les deux segments
    public RealWorldPoint? Point { get; set; }                // Coordonnées du point d'intersection (null si aucune)
}