namespace Ubb.BikeContest.Model;

public class RaceEntry : Identifiable<long>
{
    private readonly Race _race;

    public RaceEntry(long id, Participant participant, Race race)
    {
        Id = id;
        Participant = participant;
        _race = race;
    }

    public Participant Participant { get; }

    public Race Race => _race;
}