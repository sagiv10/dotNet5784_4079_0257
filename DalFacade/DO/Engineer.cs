﻿namespace DO;
/// <summary>
/// the record of engineer containing the folloing paraneters:
/// </summary>
/// <param name="Id"> id of the engineer </param>
/// <param name="Cost"> salary for hour of the engineer</param>
/// <param name="Email"> Email of the engineer </param>
/// <param name="Name"> name of the engineer  </param>
/// <param name="Level"> skill level of the engineer  </param>
public record Engineer
(
    int Id,
    double Cost, 
    string Email = "",
    string Name="",
    DO.EngineerExperience Level=
)
{
    public Engineer() : this(0,0) { }
}
