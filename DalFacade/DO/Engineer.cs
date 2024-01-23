using System.Reflection.Emit;

namespace DO;
/// <summary>
/// the record of engineer containing the folloing paraneters:
/// </summary>
/// <param name="_id"> id of the engineer </param>
/// <param name="_cost"> salary for hour of the engineer</param>
/// <param name="_email"> Email of the engineer </param>
/// <param name="_name"> name of the engineer  </param>
/// <param name="_level"> skill level of the engineer  </param>
/// /// <param name="_isActive"> shows if the engineer is active or 'deleted'  </param>
public record Engineer
(
    int _id,
    double _cost, 
    string _email = "",
    string _name="",
    DO.ComplexityLvls? _level=null,
    bool _isActive = true
)
{
    public Engineer() : this(0,0) { }
   
}

