﻿namespace Ubb.BikeContest.Model;

[Serializable]
public class Participant : Identifiable<long>
{
    public Participant(string firstName, string lastName, int engineCapacity)
    {
        FirstName = firstName;
        LastName = lastName;
        EngineCapacity = engineCapacity;
    }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public int EngineCapacity { get; set; }

    public long? TeamId { get; set; }

    public override string ToString()
    {
        return FirstName + " " + LastName;
    }
}
    