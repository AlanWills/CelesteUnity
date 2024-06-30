namespace Celeste.Assets
{
    public interface ICustomiseJsonSerialization
    {
        string Serialize();
        void Deserialize(string json);
    }
}