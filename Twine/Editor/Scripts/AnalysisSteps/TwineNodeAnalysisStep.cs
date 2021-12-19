using Celeste.Twine;
using UnityEngine;

namespace CelesteEditor.Twine.AnalysisSteps
{
    public class TwineNodeAnalyseContext
    {
        private TwineNode twineNode = default;
        public TwineNode TwineNode
        {
            get { return twineNode; }
            set
            {
                twineNode = value;
                StrippedLinksText = twineNode != null ? ImporterSettings.StripLinksFromText(twineNode.Text) : string.Empty;
            }
        }

        public bool StopAnalysing { get; set; }
        public TwineStoryAnalysis Analysis { get; set; }
        public TwineStoryImporterSettings ImporterSettings { get; set; }
        public string StrippedLinksText { get; private set; }
    }

    public abstract class TwineNodeAnalysisStep : ScriptableObject
    {
        public abstract bool CanAnalyse(TwineNodeAnalyseContext analyseContext);
        public abstract void Analyse(TwineNodeAnalyseContext analyseContext);
    }
}