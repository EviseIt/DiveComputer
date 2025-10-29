namespace Abo.Buoyancy
{
public partial class Jacket
{
    private decimal _emptyVolumeDm3 ;
    ///<summary>
    ///Volume … vide
    ///EmptyVolumeDm3 must be >0 and <=10
    ///</summary>
    public decimal EmptyVolumeDm3
    {
            get
            {
                    return _emptyVolumeDm3;
            }
            set
            {
                    if(value<=0m||value>10m){throw new Exception("EmptyVolumeDm3 must be >0 and <=10");}
                            if(_emptyVolumeDm3!=value)
                            {
                                            _emptyVolumeDm3=value;
                            }
            }
    }
}
}

