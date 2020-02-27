﻿namespace Assessments.Testlet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class TestletItemsRandomizer : ITestletItemsRandomizer
    {
        private const int NumberOfFirstPretestItems = 2;
        private readonly Random random;

        public TestletItemsRandomizer(Random? random = default)
        {
            this.random = random ?? new Random();
        }

        public IReadOnlyList<Item> Randomize(IReadOnlyList<Item> items)
        {
            _ = items ?? throw new ArgumentNullException(nameof(items));

            var pretestItemIndicesToRandomize = items
                .SelectIndicesWhere((item, _) => item.Type == ItemType.Pretest)
                .ToList();

            var pretestItemsRandomizedIndices = Enumerable
                .Range(0, NumberOfFirstPretestItems)
                .Select(i => this.TakeRandomIndex(pretestItemIndicesToRandomize))
                .ToArray();

            var otherItemIndicesToRandomize = items
                .SelectIndicesWhere((_, index) => !pretestItemsRandomizedIndices.Contains(index))
                .ToList();

            var otherItemsRandomizedIndices = Enumerable
                .Range(0, otherItemIndicesToRandomize.Count)
                .Select(i => this.TakeRandomIndex(otherItemIndicesToRandomize));

            var randomizedItemIndices = pretestItemsRandomizedIndices.Union(otherItemsRandomizedIndices).ToArray();
            var randomizedItems = randomizedItemIndices.Select(i => items[i]).ToArray();
            return randomizedItems;
        }

        private int TakeRandomIndex(IList<int> indicesToRandomize)
        {
            var randomIndex = this.random.Next(indicesToRandomize.Count);
            var index = indicesToRandomize[randomIndex];
            indicesToRandomize.RemoveAt(randomIndex);
            return index;
        }
    }
    
}
