using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfBuoyancy.ViewModels
{
    public class GraphAdjustement
    {
        public GraphAdjustement(decimal graphFactor,double offsetX,double offsetY)
        {
            GraphFactor = graphFactor;
            OffsetX = offsetX;
            OffsetY = offsetY;
        }

        public decimal GraphFactor { get; }
        public double OffsetX { get; }
        public double OffsetY { get; }
    }

    public class GraphAdjustements
    {
        private Dictionary<Arrow, GraphAdjustement> _adjustements = new();

        public void Add(Arrow arrow, decimal graphFactor, double offsetX, double offsetY)
        {
            GraphAdjustement adjustment = new GraphAdjustement(graphFactor, offsetX, offsetY);
            _adjustements[arrow] = adjustment;
        }

        public GraphAdjustement this[Arrow arrow]
        {
            get=>_adjustements[arrow];
        }
    }
}
