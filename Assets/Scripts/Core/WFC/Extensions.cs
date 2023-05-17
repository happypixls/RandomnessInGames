using System.Collections.Generic;
using UnityEngine;

namespace StoryOfRandom.Core.WFC
{
    public static class Extensions
    {
        public static T PickRandomElement<T>(this IList<T> collection)
        {
            return collection[Random.Range(0, collection.Count)]; 
        }
    }
}
