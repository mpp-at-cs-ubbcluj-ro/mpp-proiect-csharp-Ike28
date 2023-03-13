namespace Ubb.BikeContest.Model;

public class RaceEntry : Identifiable<long>
{
    private readonly Race _race;
    private readonly Participant _participant;

    public RaceEntry(long id, Participant participant, Race race)
    {
        Id = id;
        _participant = participant;
        _race = race;
    }

    public Participant Participant => _participant;

    public Race Race => _race;
}