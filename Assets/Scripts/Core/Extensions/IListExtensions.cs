using System.Collections.Generic;
using UnityEngine;

namespace StoryOfRandom.Core.Extensions
{
    public static class IListExtensions
    {
        public static T PickRandom<T>(this IList<T> collection)
        {
            return collection[Random.Range(0, collection.Count)]; 
        }
    }
}
