using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Localisation
{
    [CreateAssetMenu(fileName = nameof(DutchNumberToLocalisedTextConverter), menuName = CelesteMenuItemConstants.LOCALISATION_MENU_ITEM + "Number To Text/Dutch", order = CelesteMenuItemConstants.LOCALISATION_MENU_ITEM_PRIORITY)]
    public class DutchNumberToLocalisedTextConverter : NumberToLocalisedTextConverter
    {
        #region Properties and Fields

        [SerializeField] private LocalisationKey and;
        [SerializeField] private LocalisationKey negative;
        [SerializeField] private List<LocalisationKey> zeroToNine = new List<LocalisationKey>();
        [SerializeField] private List<LocalisationKey> tenToNinety = new List<LocalisationKey>();
        [SerializeField] private List<LocalisationKey> powersOfTen = new List<LocalisationKey>();
        [SerializeField] private List<SpecialCase> specialCases = new List<SpecialCase>();

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

            SpecialCase specialCase = specialCases.Find(x => x.number == absValue);
            if (specialCase.IsValid)
            {
                string specialCaseString = targetLanguage.Localise(specialCase.localisationKey);
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
                int multipleOfTenIndex = ((absValue - modTen) / TEN) - 1;   // Have to subtract one here because 10 will be at index 0 and so on

                string wholeNumberString = targetLanguage.Localise(tenToNinety[multipleOfTenIndex]);

                if (modTen != 0)
                {
                    // We have a single digit value, so we have to concatenate with 'and'
                    string lessThanTenString = Localise(modTen, targetLanguage);
                    wholeNumberString = $"{lessThanTenString}{targetLanguage.Localise(and)}{wholeNumberString}";
                }
                
                return SignedNumberString(sign, wholeNumberString, targetLanguage);
            }
            else if (absValue < ONE_THOUSAND)
            {
                return CreateNumberString(absValue, sign, ONE_HUNDRED, powersOfTen[0], targetLanguage);
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
            if (!specialCases.Exists(x => x.number == number))
            {
                specialCases.Add(new SpecialCase(number, localisationKey));
            }
            else
            {
                specialCases[number] = new SpecialCase(number, localisationKey);
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

            string wholeNumberString = $"{Localise(multipleOfMultiplier, targetLanguage)}{targetLanguage.Localise(multiplierLocalisationKey)}";

            if (modMultiplier != 0)
            {
                string lessThanMultiplierString = Localise(modMultiplier, targetLanguage);
                wholeNumberString = $"{wholeNumberString}{lessThanMultiplierString}";
            }

            return SignedNumberString(sign, wholeNumberString, targetLanguage);
        }

        private string SignedNumberString(int sign, string numberString, Language targetLanguage)
        {
            return sign >= 0 ? numberString : $"{targetLanguage.Localise(negative)} {numberString}";
        }
    }
}
