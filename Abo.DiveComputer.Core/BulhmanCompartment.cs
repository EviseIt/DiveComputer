using Meta.Numerics.Functions;
using RealWorldPlot.Interfaces;
using RealWorldPlot.Interfaces.GeometryHelpers;

namespace Abo.DiveComputer.Core
{
  

    public sealed class BulhmanCompartment
    {


        private double _latestElapsedTime;

        public double _halfLife { get; } // min (demi-vie du compartiment)

        // �tat interne
        public double PN2Tissue { get; private set; } // bar (tension N2 tissulaire)
        public double Depth { get; private set; }   // m (derni�re profondeur atteinte)

        private double K;   // ln2/halfperiodMin
        public MValues MValues { get; }

        public BulhmanCompartment(BulhmanCompartments owner,double halfTimeMin, double aBulhmanCoeff, double bBulhmanCoeff)
        {
            this.Compartments = owner;
            _halfLife = halfTimeMin;
            Depth = 0;
            PN2Tissue = PinspN2(Pamb(Depth)); // �tat initial � l��quilibre
            this.Name = $"{_halfLife} mn";
            ABulhmanCoeff = aBulhmanCoeff;
            BBulhmanCoeff = bBulhmanCoeff;
            K = BulhmanCompartments.ln2 / _halfLife;

            this.MValues = new MValues(ABulhmanCoeff, BBulhmanCoeff);
       
            SetComputationParameters(GradientFactorsSettings.Default, DiveProfile);

        }

        public BulhmanCompartments Compartments { get; }

        public GradientFactorLines GradientFactorLines { get; private set; }

        public void Reset()
        {
            N2TensionPoints.Clear();
            GradientFactorPoints.Clear();
            _latestElapsedTime = 0;
            PN2Tissue = PinspN2(Pamb(Depth)); // �tat initial � l��quilibre
                    }
        public double BBulhmanCoeff { get; }

        public double ABulhmanCoeff { get; }

        protected bool Equals(BulhmanCompartment other)
        {
            return _halfLife == other._halfLife;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((BulhmanCompartment)obj);
        }

        public override int GetHashCode()
        {
            return _halfLife.GetHashCode();
        }

        public string Name { get; }
        /// <summary>
        /// Fait �voluer le compartiment pendant 'elapsedTimeInSeconds' s en allant
        /// lin�airement de la profondeur actuelle vers 'profondeur'. Renvoie la tension finale (bar).
        /// </summary>
        public BulhmanCompartmentValue MoveTo(Guid diveProfilePointId, double elapsedTimeInSeconds, double profondeur)
        {
            var start = DateTime.Now;
            double elapsedTimeInSecondsStep = elapsedTimeInSeconds - _latestElapsedTime;
            if (_halfLife == 635)
            {

            }
            if (elapsedTimeInSecondsStep <= 0)
            {
                Depth = profondeur;
                return new BulhmanCompartmentValue(PN2Tissue, double.PositiveInfinity, GradientFactorLines.AffineLine.GetY(BulhmanCompartments.PSurfaceBar).Y);
            }

            double deltaT = elapsedTimeInSecondsStep / 60.0;
            double pi0 = PinspN2(Pamb(Depth));       // d�but de segment
            double piEnd = PinspN2(Pamb(profondeur));  // fin de segment
            double R = (piEnd - pi0) / deltaT;           // bar/min (variation lin�aire de Pi)
            double p0 = PN2Tissue;

            // �quation de Schreiner (variation lin�aire)
            double kt = K * deltaT;


            double pShreiner = pi0 + R * (deltaT - 1.0 / K) - (pi0 - p0 - R / K) * Math.Exp(-kt);
            double pHaldane = pi0 + (p0 - pi0) * Math.Exp(-kt);
            //Si R=0, on retrouve l'�quation de Haldane
            //double p=Pi0-(Pi0-P0)exp(-kt):


            #region reverse test
            //inversion de l'�quation de Schreiner pour retrouver deltaT et le temps pass�
            double num = pShreiner - pi0;
            double den = p0 - pi0;


            var reverseT = -Math.Log(num / den) / K;
            double reverseTT = reverseT * 60 + _latestElapsedTime;

            //  double xx= InverseTime(P, Pi0, P0, R, _halfLife);
            double gf = GradientFactorLines.AffineLine.GetY(Pamb(Depth)).Y;
            double yy;
            try
            {
                double reverseTimeMn=InverseTime(pShreiner, pi0, p0, R, _halfLife);
                double delta=reverseTimeMn- deltaT;
                yy = InverseTime(gf, pi0, p0, R, _halfLife);
                yy = InverseTime(gf, pi0, pShreiner, R, _halfLife);
                if (delta > 0.5)
                {

                }

                if (yy == double.NaN)
                {
                    yy = 99;
                }
                else
                {
                    
                }
            }
            catch (Exception e)
            {
                yy = 99;
            }

            if (yy < 0)
            {
                yy = 0;
            }
            #endregion
            
            System.Diagnostics.Debug.WriteLine($"NDL {_halfLife}=> {Depth}m => {yy}");
            double ndl = yy;

            PN2Tissue = pShreiner;
            Depth = profondeur;
            _latestElapsedTime = elapsedTimeInSeconds;

            var elapsed = DateTime.Now - start;
            //==> System.Diagnostics.Debug.WriteLine($"MoveTo: {elapsed.TotalMilliseconds} for {_halfLife}");

            if (ndl > 0)
            {

            }
            N2TensionPoints.AddNewPoint(new RealWorldPoint(elapsedTimeInSeconds / 60, PN2Tissue), diveProfilePointId);
            GradientFactorPoints.AddNewPoint(new RealWorldPoint(elapsedTimeInSeconds / 60, GradientFactorLines.AffineLine.GetY(Pamb(Depth)).Y), diveProfilePointId);


            return new BulhmanCompartmentValue(PN2Tissue, ndl, GradientFactorLines.AffineLine.GetY(Pamb(Depth)).Y);
        }
        // k = ln(2)/tau ; inverse ferm�e (branche principale W0)
        // t = B/R + (1/k) * W( -(k*A/R) * exp(-k*B/R) )
        public static double InverseTime(
            double Pstar, double Pi0, double P0, double R, double tau)
        {
            //TODO
            return 0;
            double k = Math.Log(2.0) / tau;
            const double EPS = 1e-12;

            if (Math.Abs(R) < EPS) // cas R=0 (Haldane)
            {
                return -Math.Log((Pstar - Pi0) / (P0 - Pi0)) / k;
            }

            double A = P0 - Pi0 + R / k;
            double B = Pstar - Pi0 + R / k;

            double z = -(k * A / R) * Math.Exp(-k * B / R);

            // Domaine r�el de W0 : z >= -1/e
            if (z <= -1.0 / Math.E)
                throw new ArgumentOutOfRangeException(nameof(Pstar),
                    "z <= -1/e : hors domaine de la branche principale W0.");

            double w = AdvancedMath.LambertW(z); // W0
            return B / R + w / k;
        }

        public RealWorldPoints N2TensionPoints { get; } = new();
        public RealWorldPoints GradientFactorPoints { get; } = new();
        // Utilitaires physiques
        public static double Pamb(double depthM) => BulhmanCompartments.PSurfaceBar + depthM / 10.0;              // bar
        public double PinspN2(double pamb)
        {
            double retValue;
            if (BulhmanCompartments.DEBUG)
            {
                retValue = 0.8 * pamb;
            }
            else
            {
                retValue =((double) Compartments.GasSettings.N2Percentage)/100 * (pamb - BulhmanCompartments.PH2O);
            }

            return retValue;
            // bar
        }

        /// <summary>
        /// NDL d'un compartiment (min) depuis une phase de fond � profondeur constante,
        /// d�part tissu � l'�quilibre surface, remont�e lin�aire en pression.
        /// Contrainte � l'arriv�e surface : P_t(T) <= P_s + GF*(a + b*P_s - P_s).
        /// </summary>
        private double _computeNdlSingleCompartment(
           double depthMeters,
           double fInert,

           double mValueAtSurface,
           double ascentRate_m_per_min = 10.0,

           double pSurfaceBar = 1.013,

           double? tissueStartAtSurfaceOverride = null // si non null, valeur P0 (bar)
       )
        {
            if (depthMeters <= 0)
                return 0.0;

            // Pressions
            double Pamb0 = pSurfaceBar + depthMeters / 10.0; // ~1 bar / 10 m
            double Pi0 = fInert * (Pamb0 - BulhmanCompartments.PH2O);       // inspir�e au fond
            double P0 = tissueStartAtSurfaceOverride ??
                           fInert * (pSurfaceBar - BulhmanCompartments.PH2O); // tissu au d�part (�quilibre surface)

            // Cin�tique
            double lambda = Math.Log(2.0) / _halfLife; // min^-1
            double tau = 1.0 / lambda;

            // Remont�e
            double v_bar_per_min = -ascentRate_m_per_min / 10.0; // dPamb/dt < 0 (bar/min)
            if (v_bar_per_min >= 0)
                throw new ArgumentException("La vitesse de remont�e doit �tre > 0 m/min.");
            double R = fInert * v_bar_per_min;                   // dPi/dt (bar/min)
            double T = (Pamb0 - pSurfaceBar) / Math.Abs(v_bar_per_min); // dur�e remont�e (min)

            // M-value (avec GF �ventuel) � la surface
            // M pur (B�hlmann)
            double M_GF = pSurfaceBar + (mValueAtSurface - pSurfaceBar);       // GF appliqu�

            // --- �tape 1 : pression tissu requise � la fin du fond (Pb) ---
            // Forme de Schreiner pendant la remont�e (t in [0,T]) :
            // Pt(t) = Pi0 + R*(t - tau) + (Pb - Pi0 + R*tau) * exp(-t/tau)
            // Condition no-stop � l'arriv�e surface : Pt(T) = M_GF
            // => Pb = Pi0 - R*tau + exp(T/tau) * (M_GF - Pi0 - R*(T - tau))
            double Pb = Pi0 - R * tau + Math.Exp(T / tau) * (M_GF - Pi0 - R * (T - tau));

            // --- �tape 2 : fermeture sur le temps de fond tb ---
            // Fond (R=0) : Pb = Pi0 + (P0 - Pi0) * exp(-lambda * tb)
            // => tb = -(1/lambda) * ln( (Pb - Pi0) / (P0 - Pi0) )
            double denom = (P0 - Pi0);
            double numer = (Pb - Pi0);

            if (denom == 0)
                return double.NaN;          // cas d�g�n�r�
            double ratio = numer / denom;
            if (ratio <= 0)
                return double.PositiveInfinity; // illimit� (pas de contrainte)
            if (ratio >= 1)
                return 0.0;                     // d�j� limitant

            double tb = -Math.Log(ratio) / lambda;          // minutes
            double retValue = tb < 0 ? 0.0 : tb;

            System.Diagnostics.Debug.WriteLine("----------------------" + retValue);

            return retValue;
        }
        
        public void SetComputationParameters(GradientFactorsSettings gradientFactorsSettings, DiveProfile diveProfile)
        {
            this.DiveProfile = diveProfile;
            this.GradientFactorLines = new GradientFactorLines(MValues.AffineLine, N2AmbiantPressure.GetInstance().AffineLine, gradientFactorsSettings.High, gradientFactorsSettings.Low);
            GradientFactorLines.SolveForX(1, 7);
        }

        public DiveProfile DiveProfile { get; private set; }
    }

    public class GasSettings
    {
        private int _o2Percentage;
        private static readonly GasSettings _air;

        public int O2Percentage
        {
            get => _o2Percentage;
            set
            {
                if (value < 0 || value > 100)
                    throw new ArgumentOutOfRangeException(nameof(O2Percentage), value, "O2Percentage must be between 0 and 100 inclusive.");
                _o2Percentage = value;
            }
        }

        public int N2Percentage
        {
            get => 100 - O2Percentage;
            
        }

        static GasSettings()
        {
            _air = new GasSettings() { O2Percentage = 21 };
        }
        public static GasSettings Air
        {
            get
            {
                return _air;
            }
        }
    }
    public class GradientFactorsSettings
    {
        private int _low;
        private int _high;
        private static readonly GradientFactorsSettings _default;

        public int Low
        {
            get => _low;
            set
            {
                if (value < 0 || value > 100)
                    throw new ArgumentOutOfRangeException(nameof(Low), value, "Low must be between 0 and 100 inclusive.");
                _low = value;
            }
        }

        public int High
        {
            get => _high;
            set
            {
                if (value < 0 || value > 100)
                    throw new ArgumentOutOfRangeException(nameof(High), value, "High must be between 0 and 100 inclusive.");
                _high = value;
            }
        }

        static GradientFactorsSettings()
        {
            _default = new GradientFactorsSettings() { High = 85, Low = 85 };
        }
        public static GradientFactorsSettings Default
        {
            get
            {
                return _default;
            }
        }
    }
}