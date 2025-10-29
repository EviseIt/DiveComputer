namespace Abo.Buoyancy;

public class Diver: IImmersible
{
    private Diver(Person person,Suit suit, Jacket jacket,Tank tank,Breather breather,Fins fins,LeadWeight leadWeight)
    {
        this.Person = person;
        this.Suit = suit;
        this.Jacket = jacket;
        this.Tank = tank;
        this.Breather = breather;
        this.Fins = fins;
        this.LeadWeight = leadWeight;

    }

    public LeadWeight LeadWeight { get;  }

    public Jacket Jacket { get; }

    public Fins Fins { get;  }

    public Breather Breather { get;  }

    public Tank Tank { get;  }

    public Suit Suit { get;  }

    public Person Person { get; }

    public static Diver New(Person person, int suitThicknessMm, Jacket jacket,Tank tank,int initialGasPressureInBar, Breather breather, Fins fins, LeadWeight leadWeight)
    {
        Suit suit = Suit.Build(person, suitThicknessMm);
        tank.Fill(initialGasPressureInBar);
        return new Diver(person,suit,jacket,tank,breather,fins,leadWeight);
    }

    public void Process(int depthMeter, int barLeft,decimal volumeVessie)
    {
        LeadWeight.Process(depthMeter, barLeft,volumeVessie);
        Fins.Process(depthMeter, barLeft, volumeVessie);
        Breather.Process(depthMeter, barLeft, volumeVessie);
        Jacket.Process(depthMeter, barLeft, volumeVessie);
        Tank.Process(depthMeter, barLeft, volumeVessie);
        Suit.Process(depthMeter, barLeft, volumeVessie);
        Person.Process(depthMeter, barLeft, volumeVessie);
    }

    public decimal WeightKg
    {
        get
        {
            return LeadWeight.WeightKg+
                   Fins.WeightKg+
                   Jacket.WeightKg+
                   Breather.WeightKg+
                   Tank.WeightKg+
                   Suit.WeightKg+
                   Person.WeightKg;
        }
    }

    public decimal VolumeDm3
    {
        get
        {
            return LeadWeight.VolumeDm3+
                   Fins.VolumeDm3+
                   Jacket.VolumeDm3+
                   Breather.VolumeDm3+
                   Tank.VolumeDm3+
                   Suit.VolumeDm3+
                   Person.VolumeDm3;
        }
    }
}