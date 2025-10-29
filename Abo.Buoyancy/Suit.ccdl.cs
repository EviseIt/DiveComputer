namespace Abo.Buoyancy
{
public partial class Suit
{
    private decimal _thickNessMm ;
    ///<summary>
    ///Epaisseur en mm
    ///ThickNessMm must be >=3 and <=12
    ///</summary>
    public decimal ThickNessMm
    {
            get
            {
                    return _thickNessMm;
            }
            set
            {
                    if(value<3m||value>12m){throw new Exception("ThickNessMm must be >=3 and <=12");}
                            if(_thickNessMm!=value)
                            {
                                            _thickNessMm=value;
                            }
            }
    }
}
}

