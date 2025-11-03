using System.Windows;
using Abo.Buoyancy;

namespace WpfBuoyancy.ViewModels;

public class DiverViewModel : ViewModelBase
{
    private Jacket _jacket=Jacket.LoisirStandard;
    private Tank _tank=Tank.TwelveLiters;
    private Breather _breather=Breather.DetendeurSimple;
    private Fins _fins=Fins.MaresAvantiQuattroPlus;
    private Gender _gender=Gender.Femme;
    private int _sizeCm=170;
    private int _weightKg=70;
    private int _age=30;
    private int _thicknessMm=5;
    private int _tankPressure=230;
    private int _depthM=0;
    private int _leadWeightKg=0;
    private Person _person;
    private Suit _suit;
    private Arrow[] _arrows;
    private readonly bool _locked;

    public DiverViewModel()
    {
        _locked = true;

        PersonLiftArrow.Start = new Point(20, 200);
        PersonWeightArrow.Start = new Point(20, 200);

        PersonLiftArrow.End = new Point(20, 100);
        PersonWeightArrow.End = new Point(20, 300);

        ThicknessMm = 5;

        Fins = Fins.Items.First();
        Tank=Tank.Items.First();

        WeightKg = 75;
        SizeCm = 175;
        Age = 25;
        
        Gender = Gender.Femme;
        Breather=Breather.DetendeurSimple;
        Jacket=Jacket.LoisirStandard;

        _locked = false;

    }
    public int LeadWeightKg
    {
        get => _leadWeightKg;
        set => SetProperty(ref _leadWeightKg, value);
    }
    public int TankPressure
    {
        get=> _tankPressure;
        set => SetProperty(ref _tankPressure, value);
    }
    public int DepthM
    {
        get=> _depthM;
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
        get=> _sizeCm;
        set => SetProperty(ref _sizeCm, value);
    }

    public int WeightKg
    {
        get=> _weightKg;
        set => SetProperty(ref _weightKg, value);
    }

    public int Age
    {
        get=> _age;
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
        set =>SetProperty(ref _suit, value);
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

    public Fins Fins
    {
        get => _fins;
        set => SetProperty(ref _fins, value);
    }

    public WeightArrow PersonWeightArrow
    {
        get;
    } = new WeightArrow();

    public LiftArrow PersonLiftArrow
    {
        get;
    } = new LiftArrow();

   

    protected override void innerOnPropertyChanged(string? propertyName = null)
    {
        if (propertyName != nameof(Arrows) && propertyName!=nameof(Suit))
        {
            var person = new Person(Gender, SizeCm, WeightKg, Age);
            Diver diver = Diver.New(person, ThicknessMm, Jacket.Voyage, Tank, TankPressure, Breather, Fins, new LeadWeight() { WeightKg = LeadWeightKg });
            Suit = diver.Suit;
            diver.Process(DepthM, TankPressure, 1.75m);
            Arrows = null;
            if (!_locked)
            {
                var renderSize = RequestDrawingSize();
                PersonWeightArrow.SetY(renderSize.Height/2);
                PersonLiftArrow.SetY(renderSize.Height / 2);

                IImmersibleArrowModifier.Update(person, this.PersonWeightArrow, this.PersonLiftArrow);
            }

            Arrows=new Arrow[] { PersonWeightArrow, PersonLiftArrow };
        }
    }

    public Arrow[] Arrows
    {
        get=>_arrows;
        set=> SetProperty(ref _arrows, value);
    }

    public Func<Size> RequestDrawingSize { get; set; }
}

public class IImmersibleArrowModifier
{
    public static decimal GraphFactor = 5m;
    public static void Update(IImmersible iImmersible, WeightArrow weightArrow,LiftArrow liftArrow)
    {
       
        // Update the arrow based on the person's properties
        weightArrow.SetLength(iImmersible.WeightKg*10.0m/ GraphFactor); // Example calculation
        liftArrow.SetLength(-iImmersible.VolumeDm3*10.0m/ GraphFactor); // Example calculation
    }
}