using Celeste.FSM;
using Celeste.Loading;
using Celeste.Twine;
using System;
using System.Collections;
using UnityEngine;

namespace Celeste.Narrative.TwineImporter
{
    public class ImportTwineStoryLoadJob : LoadJob
    {
        #region Builder

        public class Builder
        {
            private TwineStory twineStory;
            private TwineStoryImporterSettings twineStoryImporterSettings;
            private FSMGraph fsmGraph;

            public Builder WithTwineStory(TwineStory twineStory)
            {
                this.twineStory = twineStory;
                return this;
            }

            public Builder WithImporterSettings(TwineStoryImporterSettings twineStoryImporterSettings)
            {
                this.twineStoryImporterSettings = twineStoryImporterSettings;
                return this;
            }

            public Builder WithFSMGraph(FSMGraph fsmGraph)
            {
                this.fsmGraph = fsmGraph;
                return this;
            }

            public ImportTwineStoryLoadJob Build()
            {
                ImportTwineStoryLoadJob importTwineStoryLoadJob = CreateInstance<ImportTwineStoryLoadJob>();
                importTwineStoryLoadJob.name = nameof(ImportTwineStoryLoadJob);
                importTwineStoryLoadJob.twineStory = twineStory;
                importTwineStoryLoadJob.twineStoryImporterSettings = twineStoryImporterSettings;
                importTwineStoryLoadJob.fsmGraph = fsmGraph;

                return importTwineStoryLoadJob;
            }
        }

        #endregion

        #region Properties and Fields

        private TwineStory twineStory;
        private TwineStoryImporterSettings twineStoryImporterSettings;
        private FSMGraph fsmGraph;

        #endregion

        public override IEnumerator Execute(Action<float> setProgress, Action<string> setOutput)
        {
            TwineStoryImporter.Import(twineStory, twineStoryImporterSettings, fsmGraph, false);

            yield break;
        }
    }
}