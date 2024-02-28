﻿using Celeste.FSM.Nodes.Events;
using Celeste.Sound;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Narrative.TwineImporter.ParserSteps
{
    #region SFX Key Struct

    [Serializable]
    public struct SFXKey : IKey
    {
        string IKey.Key => key;

        public string key;
        public AudioClip audioClip;

        public SFXKey(string key, AudioClip audioClip)
        {
            this.key = key;
            this.audioClip = audioClip;
        }
    }

    #endregion

    [CreateAssetMenu(fileName = nameof(TryCreatePlaySFXNode), order = CelesteMenuItemConstants.TWINE_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.TWINE_MENU_ITEM + "Parser Steps/Try Create Play SFX Node")]
    public class TryCreatePlaySFXNode : TwineNodeParserStep, IUsesKeys
    {
        #region Properties and Fields

        public IEnumerable<string> Keys
        {
            get
            {
                foreach (var sfxKey in sfxKeys)
                {
                    yield return sfxKey.key;
                }
            }
        }

        [SerializeField] private string instruction = "PlaySFX";
        [SerializeField] private AudioClipEvent playSFXEvent;
        [SerializeField] private List<SFXKey> sfxKeys = new List<SFXKey>();

        #endregion

        public void AddKeyForUse(IKey key)
        {
            sfxKeys.Add((SFXKey)key);
        }

        public bool CouldUseKey(IKey key)
        {
            return key is SFXKey;
        }

        public bool UsesKey(IKey key)
        {
            return HasSFX(key.Key);
        }

        #region Analyse

        public override bool CanAnalyse(TwineNodeAnalyseContext parseContext)
        {
            return !string.IsNullOrWhiteSpace(parseContext.StrippedLinksText);
        }

        public override void Analyse(TwineNodeAnalyseContext parseContext)
        {
            FindSFXs(parseContext.SplitStrippedLinksText, parseContext.Analysis);
        }

        #endregion

        #region Parse

        public override bool CanParse(TwineNodeParseContext parseContext)
        {
            if (parseContext.FSMNode != null)
            {
                return false;
            }

            string[] splitText = parseContext.SplitStrippedLinksText;

            if (splitText == null || splitText.Length < 2)
            {
                return false;
            }

            if (!IsInstruction(splitText[0]))
            {
                return false;
            }

            bool hasSFX = HasSFX(splitText[1]);
            UnityEngine.Debug.Assert(hasSFX, $"Want to play SFX, but could not find SFX with key {splitText[1]}.");

            return hasSFX;
        }

        public override void Parse(TwineNodeParseContext parseContext)
        {
            string[] splitText = parseContext.SplitStrippedLinksText;

            AudioClip audioClip = FindSFX(splitText[1]);
            AudioClipEventRaiserNode audioClipEventRaiserNode = parseContext.Graph.AddNode<AudioClipEventRaiserNode>();
            audioClipEventRaiserNode.argument.Value = audioClip;
            audioClipEventRaiserNode.toRaise = playSFXEvent;

            parseContext.FSMNode = audioClipEventRaiserNode;
        }

        #endregion

        private bool IsInstruction(string str)
        {
            return string.CompareOrdinal(instruction, str) == 0;
        }

        private bool HasSFX(string key)
        {
            return sfxKeys.Exists(x => string.CompareOrdinal(x.key, key) == 0);
        }

        private AudioClip FindSFX(string key)
        {
            return sfxKeys.Find(x => string.CompareOrdinal(x.key, key) == 0).audioClip;
        }

        private void FindSFXs(string[] splitStrippedLinkText, TwineStoryAnalysis analysis)
        {
            if (splitStrippedLinkText != null &&
                splitStrippedLinkText.Length >= 2 &&
                IsInstruction(splitStrippedLinkText[0]))
            {
                string sfxName = splitStrippedLinkText[1];

                if (HasSFX(sfxName))
                {
                    analysis.AddFoundSFXs(sfxName);
                }
                else
                {
                    analysis.AddUnrecognizedKey(sfxName);
                }
            }
        }
    }
}