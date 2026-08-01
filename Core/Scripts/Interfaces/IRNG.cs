namespace Celeste.Core.Interfaces
{
    public interface IRNG
    {
        int FromRangeInclusive(int inclusiveMin, int inclusiveMax);
        float FromRange01();
    }
}