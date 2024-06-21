namespace Celeste.Narrative
{
    public interface ITextChoice : IChoice
    {
        string DisplayText { get; }
        DialogueType DialogueType { get; }
    }
}