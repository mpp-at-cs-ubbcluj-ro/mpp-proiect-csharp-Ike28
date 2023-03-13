namespace Ubb.BikeContest.Model;

public class Identifiable<TId>
{
    private TId? _id;

    public TId Id
    {
        get => _id ?? throw new ArgumentNullException();
        protected set => _id = value;
    }
}