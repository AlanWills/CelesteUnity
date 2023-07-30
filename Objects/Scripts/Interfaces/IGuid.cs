namespace Celeste.Objects
{
    public interface IGuid
    {
        string name { get; }
        string Guid { get; set; }
    }

    public interface IIntGuid
    {
        string name { get; }
        int Guid { get; set; }
    }
}
