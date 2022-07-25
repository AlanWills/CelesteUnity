using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Localisation
{
    [CreateAssetMenu(fileName = nameof(EnglishNumberToLocalisedTextConverter), menuName = "Celeste/Localisation/Number To Text/English")]
    public class EnglishNumberToLocalisedTextConverter : NumberToLocalisedTextConverter
    {
        #region Properties and Fields

        public IReadOnlyDictionary<int, LocalisationKey> SpecialCases => specialCases;

        [SerializeField] private LocalisationKey and;
        [SerializeField] private LocalisationKey negative;
        [SerializeField] private List<LocalisationKey> zeroToNine = new List<LocalisationKey>();
        [SerializeField] private List<LocalisationKey> tenToNinety = new List<LocalisationKey>();
        [SerializeField] private List<LocalisationKey> powersOfTen = new List<LocalisationKey>();
        [SerializeField] private Dictionary<int, LocalisationKey> specialCases = new Dictionary<int, LocalisationKey>();

        private const int TEN = 10;
        private const int ONE_HUNDRED = 100;
        private const int ONE_THOUSAND = 1000;
        private const int ONE_MILLION = ONE_THOUSAND * ONE_THOUSAND;
        private const int ONE_BILLION = ONE_MILLION * ONE_THOUSAND;

        #endregion

        public override string Localise(int number, Language targetLanguage)
        {
            int absValue = Math.Abs(number);
            int sign = Math.Sign(number);

            if (specialCases.TryGetValue(absValue, out LocalisationKey localisationKey))
            {
                string specialCaseString = targetLanguage.Localise(localisationKey);
                return SignedNumberString(sign, specialCaseString, targetLanguage);
            }
            else if (absValue < TEN)
            {
                string lessThanTenString = targetLanguage.Localise(zeroToNine[number]);
                return SignedNumberString(sign, lessThanTenString, targetLanguage);
            }
            else if (absValue < ONE_HUNDRED)
            {
                int modTen = absValue % TEN;
                int multipleOfTenIndex = ((absValue - modTen) / TEN) - 1;   // Have to subtract 1 because 10 will be at index zero and so on

                string multipleOfTenString = targetLanguage.Localise(tenToNinety[multipleOfTenIndex]);
                string wholeNumberString = SignedNumberString(sign, multipleOfTenString, targetLanguage);

                if (modTen != 0)
                {
                    string lessThanTenString = Localise(modTen, targetLanguage);
                    wholeNumberString = $"{wholeNumberString} {lessThanTenString}";
                }

                return wholeNumberString;
            }
            else if (absValue < ONE_THOUSAND)
            {
                int modHundred = absValue % ONE_HUNDRED;
                int multipleOfHundred = (absValue - modHundred) / ONE_HUNDRED;
                UnityEngine.Debug.Assert(multipleOfHundred > 0, $"Something went wrong - this value should be at least >= {ONE_HUNDRED}.");

                string multipleOfHundredString = $"{Localise(multipleOfHundred, targetLanguage)} {targetLanguage.Localise(powersOfTen[0])}";
                string wholeNumberString = SignedNumberString(sign, multipleOfHundredString, targetLanguage);

                if (modHundred != 0)
                {
                    string lessThanHundredString = Localise(modHundred, targetLanguage);
                    wholeNumberString = $"{wholeNumberString} {lessThanHundredString}";
                }

                return wholeNumberString;
            }
            else if (absValue < ONE_MILLION)
            {
                return CreateNumberString(absValue, sign, ONE_THOUSAND, powersOfTen[1], targetLanguage);
            }
            else if (absValue < ONE_BILLION)
            {
                return CreateNumberString(absValue, sign, ONE_MILLION, powersOfTen[2], targetLanguage);
            }

            UnityEngine.Debug.LogAssertion($"Failed to localise {number} to language {targetLanguage.LanguageNameKey}.");
            return number.ToString();
        }

        public void SetSpecialCase(int number, LocalisationKey localisationKey)
        {
            UnityEngine.Debug.Assert(localisationKey, $"Inputting null localisation key for number {number}.");
            if (!specialCases.ContainsKey(number))
            {
                specialCases.Add(number, localisationKey);
            }
            else
            {
                specialCases[number] = localisationKey;
            }

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        private string CreateNumberString(
            int absValue,
            int sign,
            int multiplier,
            LocalisationKey multiplierLocalisationKey,
            Language targetLanguage)
        {
            int modMultiplier = absValue % multiplier;
            int multipleOfMultiplier = (absValue - modMultiplier) / multiplier;
            UnityEngine.Debug.Assert(multipleOfMultiplier > 0, $"Something went wrong - this value should be at least >= {multiplier}.");

            string multipleOfMultiplierString = $"{Localise(multipleOfMultiplier, targetLanguage)} {targetLanguage.Localise(multiplierLocalisationKey)}";
            string wholeNumberString = SignedNumberString(sign, multipleOfMultiplierString, targetLanguage);

            if (modMultiplier != 0)
            {
                string lessThanMultiplierString = Localise(modMultiplier, targetLanguage);
                wholeNumberString = $"{wholeNumberString} {lessThanMultiplierString}";
            }

            return wholeNumberString;
        }

        private string SignedNumberString(int sign, string numberString, Language targetLanguage)
        {
            return sign >= 0 ? numberString : $"{targetLanguage.Localise(negative)} {numberString}";
        }
    }
}
