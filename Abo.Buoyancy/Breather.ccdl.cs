namespace Abo.Buoyancy
{
public partial class Breather
{
    private decimal _weightKg ;
    private decimal _volumeDm3 ;
    private string _name ;
    ///<summary>
    ///Poids du lest en Kg
    ///WeightKg must be >=1 and <=4.4
    ///</summary>
    public decimal WeightKg
    {
            get
            {
                    return _weightKg;
            }
            set
            {
                    if(value<1m||value>4.4m){throw new Exception("WeightKg must be >=1 and <=4.4");}
                            if(_weightKg!=value)
                            {
                                            _weightKg=value;
                            }
            }
    }
    ///<summary>
    ///Volume des palmes en dm3
    ///VolumeDm3 must be >=1 and <=4.4
    ///</summary>
    public decimal VolumeDm3
    {
            get
            {
                    return _volumeDm3;
            }
            set
            {
                    if(value<1m||value>4.4m){throw new Exception("VolumeDm3 must be >=1 and <=4.4");}
                            if(_volumeDm3!=value)
                            {
                                            _volumeDm3=value;
                            }
            }
    }
    ///<summary>
    ///Nom des palmes
    ///Name cannot be null
    ///Name length must be >=1 and <=250
    ///</summary>
    public string Name
    {
            get
            {
                    return _name;
            }
            set
            {
                    if(value==null){throw new Exception("Name cannot be null");}
                    if(value.Length<1||value.Length>250){throw new Exception("Name length must be >=1 and <=250");}
                            if(_name!=value)
                            {
                                            _name=value;
                            }
            }
    }
}
}

