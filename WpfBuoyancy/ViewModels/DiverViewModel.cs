using System.Windows;
using System.Windows.Media;
using Abo.Buoyancy;

namespace WpfBuoyancy.ViewModels;

public partial class DiverViewModel : ViewModelBase
{
    private Jacket _jacket = Jacket.LoisirStandard;
    private Tank _tank = Tank.TwelveLiters;
    private Breather _breather = Breather.DetendeurSimple;
    private Fins _fins = Fins.MaresAvantiQuattroPlus;
    private Gender _gender = Gender.Femme;
    private int _sizeCm = 170;
    private int _weightKg = 70;
    private int _age = 30;
    private int _thicknessMm = 5;
    private int _tankPressure = 230;
    private int _depthM = 0;
    private int _leadWeightKg = 0;
    private Person _person;
    private Suit _suit;
    private Arrow[] _arrows;
    private readonly bool _locked;
    private LeadWeight _leadWeight = new LeadWeight();
    private int _liftVolumeDm3;
    private Water _water;
    private decimal _proportionalGraphFactor;
    private bool _useProportionalGraphFactor;

    private GraphAdjustements _graphAdjustements = new();

    private VerticalArrow GlobalArrow { get; set; }


    public DiverViewModel()
    {
        _locked = true;

        _setupArrows();

        GlobalArrow = new VerticalArrow();
        GlobalArrow.StartPoint = new Point(140, 100);
        GlobalArrow.EndPoint = new Point(140, 300);
        GlobalArrow.Stroke = System.Windows.Media.Brushes.CornflowerBlue;


        ThicknessMm = 5;

        Fins = Fins.Items.First();
        Tank = Tank.Items.First();

        WeightKg = 75;
        SizeCm = 175;
        Age = 25;
        LeadWeightKg = 0;
        DepthM = 0;
        Water = Water.StillWater;

        Gender = Gender.Femme;
        Breather = Breather.DetendeurSimple;
        Jacket = Jacket.LoisirStandard;

        ProportionalGraphFactor = 5.0m;

        _setupArrows();
        _setupArrowsAdjustement();

        _locked = false;

    }

    partial void _setupArrowsAdjustement();
    partial void _setupArrows();
    public bool UseProportionalGraphFactor
    {
        get => _useProportionalGraphFactor;
        set => SetProperty(ref _useProportionalGraphFactor, value);
    }
    public decimal ProportionalGraphFactor
    {
        get => _proportionalGraphFactor;
        set => SetProperty(ref _proportionalGraphFactor, value);
    }

    public int LeadWeightKg
    {
        get => _leadWeightKg;
        set => SetProperty(ref _leadWeightKg, value);
    }
    public int TankPressure
    {
        get => _tankPressure;
        set => SetProperty(ref _tankPressure, value);
    }
    public int DepthM
    {
        get => _depthM;
        set => SetProperty(ref _depthM, value);
    }
    public Gender Gender
    {
        get => _gender;
        set => SetProperty(ref _gender, value);
    }
    public int ThicknessMm
    {
        get => _thicknessMm;
        set => SetProperty(ref _thicknessMm, value);
    }

    public int SizeCm
    {
        get => _sizeCm;
        set => SetProperty(ref _sizeCm, value);
    }

    public int LiftVolumeDm3
    {
        get => _liftVolumeDm3;
        set => SetProperty(ref _liftVolumeDm3, value);
    }

    public int WeightKg
    {
        get => _weightKg;
        set => SetProperty(ref _weightKg, value);
    }

    public int Age
    {
        get => _age;
        set => SetProperty(ref _age, value);
    }

    public Person Person
    {
        get => _person;
        set => SetProperty(ref _person, value);
    }

    public Suit Suit
    {
        get => _suit;
        set => SetProperty(ref _suit, value);
    }

    public Jacket Jacket
    {
        get => _jacket;
        set => SetProperty(ref _jacket, value);
    }

    public Tank Tank
    {
        get => _tank;
        set => SetProperty(ref _tank, value);
    }

    public Breather Breather
    {
        get => _breather;
        set => SetProperty(ref _breather, value);
    }

    public Water Water
    {
        get => _water;
        set => SetProperty(ref _water, value);
    }

    public LeadWeight LeadWeight
    {
        get => _leadWeight;
        set => SetProperty(ref _leadWeight, value);
    }
    public Fins Fins
    {
        get => _fins;
        set => SetProperty(ref _fins, value);
    }

    private decimal _personWeightArrowLength;
    public decimal PersonWeightArrowLength
    {
        get => _personWeightArrowLength;
        set => SetProperty(ref _personWeightArrowLength, value);
    }

    protected override void innerOnPropertyChanged(string? propertyName = null)
    {
        if (propertyName != nameof(Arrows) && propertyName != nameof(Suit) && propertyName != nameof(Person) && propertyName != nameof(LeadWeight))
        {

          
            Person = new Person(Gender, SizeCm, WeightKg, Age);


            PersonWeightArrowLength = Person.WeightKg;


            LeadWeight = new LeadWeight() { WeightKg = LeadWeightKg };
            Diver diver = Diver.New(Person, ThicknessMm, Jacket.Voyage, Tank, TankPressure, Breather, Fins, LeadWeight);
            Suit = diver.Suit;
            diver.Process(DepthM, TankPressure, LiftVolumeDm3);
            Arrows = null;
            if (!_locked)
            {
                //Size renderSize = RequestDrawingSize();
                //_updateArrows(renderSize);

                decimal globalWeigth = Person.WeightKg + Suit.WeightKg + Jacket.WeightKg + Tank.WeightKg + Breather.WeightKg + Fins.WeightKg + LeadWeight.WeightKg;
                decimal globalLift = Person.VolumeDm3 + Suit.VolumeDm3 + Jacket.VolumeDm3 + Tank.VolumeDm3 + Breather.VolumeDm3 + Fins.VolumeDm3 + LeadWeight.VolumeDm3;

                
                //  _updateArrow(GlobalArrow, this.PersonWeightArrow, this.PersonLiftArrow);


            }

            _setArrows();
        }
    }

    private void _updateArrow(IImmersible iImmersible, WeightArrow weightArrow, LiftArrow liftArrow)
    {

        decimal weigthGraphFactor = this.ProportionalGraphFactor;
        decimal liftGraphFactor = this.ProportionalGraphFactor;
        GraphAdjustement graphAdjustement = _graphAdjustements[weightArrow];
        if (!UseProportionalGraphFactor)
        {

            weigthGraphFactor = graphAdjustement.GraphFactor;
            liftGraphFactor = graphAdjustement.GraphFactor;
        }
        weightArrow.OffsetX=graphAdjustement.OffsetX;
        weightArrow.OffsetY=graphAdjustement.OffsetY;
        liftArrow.OffsetX=graphAdjustement.OffsetX;
        liftArrow.OffsetY=graphAdjustement.OffsetY;
        // Update the arrow based on the person's properties
        weightArrow.SetLength(-iImmersible.WeightKg *  weigthGraphFactor); // Example calculation
        liftArrow.SetLength(iImmersible.VolumeDm3 * Water.Density * liftGraphFactor); // Example calculation
    }

    partial void _updateArrows(Size size);
    partial void _setArrows();
    public Arrow[] Arrows
    {
        get => _arrows;
        set => SetProperty(ref _arrows, value);
    }

    public Func<Size> RequestDrawingSize { get; set; }
}

