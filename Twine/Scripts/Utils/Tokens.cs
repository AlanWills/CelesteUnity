using System.Collections;
using System.Collections.Generic;

namespace Celeste.Twine
{
    public class Tokens : IEnumerable<string>
    {
        private string text;
        private char startDelimiter;
        private char endDelimiter;

        private Tokens(string text, char startDelimiter, char endDelimiter)
        {
            this.text = text;
            this.startDelimiter = startDelimiter;
            this.endDelimiter = endDelimiter;
        }

        public IEnumerator<string> GetEnumerator()
        {
            return new Iterators.TokenIterator(text, startDelimiter, endDelimiter);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Iterators.TokenIterator(text, startDelimiter, endDelimiter);
        }

        public static Tokens Get(string text, char startDelimiter, char endDelimiter)
        {
            return new Tokens(text, startDelimiter, endDelimiter);
        }
    }

    namespace Iterators
    {
        public class TokenIterator : IEnumerator<string>
        {
            #region Properties and Fields

            public string Current { get; private set; }

            object IEnumerator.Current { get { return Current; } }

            private readonly string text;
            private readonly char startDelimiter;
            private readonly char endDelimiter;

            private int currentStartDelimiterIndex = -1;
            private int currentEndDelimiterIndex = -1;

            #endregion

            public TokenIterator(string text, char startDelimiter, char endDelimiter)
            {
                this.text = text;
                this.startDelimiter = startDelimiter;
                this.endDelimiter = endDelimiter;
            }

            public void Dispose() { }

            public bool MoveNext()
            {
                currentStartDelimiterIndex = text.IndexOf(startDelimiter, currentEndDelimiterIndex + 1);

                if (currentStartDelimiterIndex != -1)
                {
                    currentEndDelimiterIndex = text.IndexOf(endDelimiter, currentStartDelimiterIndex + 1);
                    Current = text.Substring(currentStartDelimiterIndex + 1, currentEndDelimiterIndex - currentStartDelimiterIndex - 1);

                    return true;
                }
                else
                {
                    Current = "";
                    return false;
                }
            }

            public void Reset()
            {
                Current = "";
                currentStartDelimiterIndex = -1;
                currentEndDelimiterIndex = -1;
            }
        }
    }
}