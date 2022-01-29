using System.Collections.Generic;

namespace Celeste.DataStructures
{
    public class ShuffleBag<T>
    {
        #region Properties and Fields

        public int Capacity => data.Capacity;
        public int Count => data.Count;

        private List<T> data = default;
        private int currentPosition = -1;

        #endregion

        public ShuffleBag(int initCapacity)
        {
            data = new List<T>(initCapacity);
        }

        public void Add(T item, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                data.Add(item);
            }

            currentPosition = Count - 1;
        }

        public T Next()
        {
            if (currentPosition < 1)
            {
                currentPosition = Count - 1;
                return data[0];
            }

            var pos = UnityEngine.Random.Range(0, currentPosition + 1);

            T currentItem = data[pos];
            data[pos] = data[currentPosition];
            data[currentPosition] = currentItem;
            currentPosition--;

            return currentItem;
        }
    }
}