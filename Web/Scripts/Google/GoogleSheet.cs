﻿using Celeste.DataStructures;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Web
{
    public class GoogleSheet
    {
        public class Column
        {
            public string Name { get; }
            public List<string> Values { get; } = new List<string>();

            public Column(string name)
            {
                Name = name;
            }
        }

        #region Properties and Fields

        public int NumColumns => columns.Count;
        public int NumRows { get; private set; }

        private List<Column> columns = new List<Column>();

        #endregion

        #region Factory Methods

        public static GoogleSheet FromCSV(string csv)
        {
            GoogleSheet googleSheet = new GoogleSheet();

            int namedColumnsEnd = csv.IndexOf('\r');
            string separatedColumnNames = namedColumnsEnd >= 0 ? csv.Substring(0, namedColumnsEnd) : csv;
            string[] columnNames = separatedColumnNames.Split(',');

            for (int i = 0; i < columnNames.Length; ++i)
            {
                googleSheet.AddColumn(columnNames[i]);
            }

            if (namedColumnsEnd >= 0)
            {
                int currentColumn = 0;

                for (int i = namedColumnsEnd + 2, n = csv.Length; i < n;)
                {
                    if (csv[i] == '\n')
                    {
                        currentColumn = 0;
                    }
                    else
                    {
                        char delimiterChar = currentColumn == googleSheet.NumColumns - 1 ? '\r' : ',';
                        int nextDelimiter = csv.IndexOf(delimiterChar, i);
                        string value = nextDelimiter >= 0 ? csv.Substring(i, nextDelimiter - i) : csv.Substring(i);

                        googleSheet.GetColumn(currentColumn).Values.Add(value);
                        i = nextDelimiter >= 0 ? nextDelimiter : n;
                        ++currentColumn;
                    }

                    ++i;
                }
            }

            int numRows = googleSheet.GetColumn(0).Values.Count;
            googleSheet.NumRows = numRows;

            for (int i = 1; i < googleSheet.NumColumns; ++i)
            {
                Debug.Assert(googleSheet.GetColumn(i).Values.Count == numRows, $"Inconsistent column values - looks like there's a bug in parsing.");
            }

            return googleSheet;
        }

        #endregion

        private void AddColumn(string name)
        {
            columns.Add(new Column(name));
        }

        public Column GetColumn(int index)
        {
            return columns.Get(index);
        }

        public bool HasColumn(string columnName)
        {
            return columns.Exists(x => string.CompareOrdinal(columnName, x.Name) == 0);
        }
    }
}