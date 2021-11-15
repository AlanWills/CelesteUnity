using Celeste.Twine;
using UnityEngine;

namespace CelesteEditor.Twine.AnalysisSteps
{
    public class TwineNodeAnalyseContext
    {
        public bool StopAnalysing { get; set; }
        public TwineStoryAnalysis Analysis { get; set; }
        public TwineStoryImporterSettings ImporterSettings { get; set; }
        public TwineNode TwineNode { get; set; }
    }

    public abstract class TwineNodeAnalysisStep : ScriptableObject
    {
        public abstract bool CanAnalyse(TwineNodeAnalyseContext analyseContext);
        public abstract void Analyse(TwineNodeAnalyseContext analyseContext);
    }
}