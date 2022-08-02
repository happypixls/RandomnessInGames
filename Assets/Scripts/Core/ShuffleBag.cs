using System;
using System.Collections.Generic;

namespace StoryOfRandom.Core
{
    //Based on https://gamedevelopment.tutsplus.com/tutorials/shuffle-bags-making-random-feel-more-random--gamedev-1249
    public class ShuffleBag<T>
    {
        private Random NumberGenerator { get; set; }
        private List<T> Data { get; set; }
        private T CurrentItem { get; set; }
        private int CurrentPosition = -1;

        /// <summary>
        /// Returns the total size of the array
        /// </summary>
        /// <value>Size</value>
        public int Size
        {
            get { return Data.Count; }
        }

        public ShuffleBag()
        {
            this.Data = new List<T>();
            this.NumberGenerator = new Random();
        }

        public ShuffleBag(int seed)
        {
            this.Data = new List<T>();
            this.NumberGenerator = new Random(seed);
        }

        public ShuffleBag(T[] data)
        {
            this.Data = new List<T>(data);
            this.NumberGenerator = new Random();
            this.CurrentPosition = this.Size - 1;
        }

        public ShuffleBag(T[] data, int seed)
        {
            this.Data = new List<T>(data);
            this.NumberGenerator = new Random(seed);
            this.CurrentPosition = this.Size - 1;
        }

        public void Add(T item, int amount)
        {
            for (int i = 0; i < amount; i++)
                this.Data.Add(item);

            this.CurrentPosition = this.Size - 1;
        }

        public T Next()
        {
            if (Data.Count == 0)
                return default(T);

            if (CurrentPosition < 0)
            {
                this.CurrentPosition = this.Size - 1;
                CurrentItem = this.Data[0];
                return CurrentItem;
            }

            var Position = this.NumberGenerator.Next(this.CurrentPosition);
            CurrentItem = this.Data[Position];
            this.Data[Position] = Data[CurrentPosition];
            this.Data[CurrentPosition] = CurrentItem;
            CurrentPosition--;

            return CurrentItem;
        }

        /// <summary>
        /// Next element in a list with Ring Buffer behaviour.
        /// </summary>
        /// <returns>Returns object of type <typeparamref name="T"/></returns>
        public T NextNonRandom()
        {
            if (Data.Count == 0)
                return default(T);

            if (CurrentPosition < 0)
            {
                this.CurrentPosition = this.Size - 1;
                CurrentItem = this.Data[0];
                return CurrentItem;
            }

            CurrentItem = this.Data[this.CurrentPosition];
            CurrentPosition--;

            return CurrentItem;
        }

        public void Clear()
        {
            Data.Clear();
            CurrentItem = default(T); 
            CurrentPosition = -1;
        }
    }
}
