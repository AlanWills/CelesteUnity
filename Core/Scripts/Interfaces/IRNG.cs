namespace Celeste.Core.Interfaces
{
    public interface IRNG
    {
        int FromRangeInclusive(int inclusiveMin, int inclusiveMax);
    }
}