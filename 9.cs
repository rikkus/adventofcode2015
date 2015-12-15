using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;


namespace aoc
{
    public static class IEnumerableExtensions
    {
        public static T MinBy<T>(this IEnumerable<T> self, Func<T, int> comparer)
        {
            var minValue = int.MaxValue;
            var min = default(T);
            bool first = true;

            var iterator = self.GetEnumerator();

            while (iterator.MoveNext())
            {
                if (first)
                {
                    first = false;
                    min = iterator.Current;
                }

                var newValue = comparer(iterator.Current);
                if (newValue.CompareTo(minValue) < 0)
                {
                    minValue = newValue;
                    min = iterator.Current;
                }
            }

            return min;
        }


        public static IEnumerable<IEnumerable<T>> Permutations<T>(this IEnumerable<T> source)
        {
            var query =
                from item in source
                from others in source.SkipOnce(item).Permutations()
                select new[] { item }.Concat(others);
            return query.DefaultIfEmpty(Enumerable.Empty<T>());
        }

        public static IEnumerable<T> SkipOnce<T>(this IEnumerable<T> source, T itemToSkip)
        {
            bool skipped = false;
            var comparer = EqualityComparer<T>.Default;
            foreach (var item in source)
            {
                if (!skipped && comparer.Equals(item, itemToSkip))
                    skipped = true;
                else
                    yield return item;
            }
        }
    }

    internal class Program
    {
        class Edge
        {
            public string Source { get; set; }
            public string Target { get; set; }
            public int Distance { get; set; }

            public Edge(string source, string target, int distance)
            {
                Source = source;
                Target = target;
                Distance = distance;
            }
        }

        class Graph
        {
            public HashSet<string> Nodes { get; }
            public HashSet<Edge> Edges { get; }

            public int Distance(string source, string target)
            {
                var edge = Edges.SingleOrDefault(e => (e.Source == source && e.Target == target) || (e.Source == target && e.Target == source));

                if (edge == null)
                    return 0;

                return edge.Distance;
            }

            public Graph()
            {
                Nodes = new HashSet<string>();
                Edges = new HashSet<Edge>();
            }
        }

        private static void Main()
        {
            var lines = @"Tristram to AlphaCentauri = 34
Tristram to Snowdin = 100
Tristram to Tambi = 63
Tristram to Faerun = 108
Tristram to Norrath = 111
Tristram to Straylight = 89
Tristram to Arbre = 132
AlphaCentauri to Snowdin = 4
AlphaCentauri to Tambi = 79
AlphaCentauri to Faerun = 44
AlphaCentauri to Norrath = 147
AlphaCentauri to Straylight = 133
AlphaCentauri to Arbre = 74
Snowdin to Tambi = 105
Snowdin to Faerun = 95
Snowdin to Norrath = 48
Snowdin to Straylight = 88
Snowdin to Arbre = 7
Tambi to Faerun = 68
Tambi to Norrath = 134
Tambi to Straylight = 107
Tambi to Arbre = 40
Faerun to Norrath = 11
Faerun to Straylight = 66
Faerun to Arbre = 144
Norrath to Straylight = 115
Norrath to Arbre = 135
Straylight to Arbre = 127".Split(new[] { Environment.NewLine }, StringSplitOptions.None); // File.ReadLines(@"c:\temp\aoc-9.txt");


            var graph = new Graph();

            foreach (var line in lines)
            {
                var match = Regex.Match(line, @"(?<v1>\w+) to (?<v2>\w+) = (?<n>\d+)");

                graph.Nodes.Add(match.Groups["v1"].Value);
                graph.Nodes.Add(match.Groups["v2"].Value);

                graph.Edges.Add
                    (
                        new Edge
                            (
                            match.Groups["v1"].Value,
                            match.Groups["v2"].Value,
                            Convert.ToInt32(match.Groups["n"].Value)
                            )
                    );
            }

            int min = int.MinValue;

            IEnumerable<string> bestPath = null;

            foreach (var path in graph.Nodes.Permutations())
            {
                //Console.WriteLine(string.Join("->", path));

                var x = path.Aggregate
                    (
                        new { Sum = 0, Previous = (string)null },
                        (acc, c) =>
                            new
                            {
                                Sum = acc.Sum + (acc.Previous != null ? graph.Distance(c, acc.Previous) : 0),
                                Previous = c
                            }
                    );

                if (x.Sum > min)
                {
                    min = x.Sum;
                    bestPath = path;
                }
            }


            Console.WriteLine(min);
            Console.WriteLine(string.Join("->", bestPath));


            Console.ReadKey();
        }

        private void PrintPath(IEnumerable<string> path)
        {
            Console.WriteLine(string.Join("->", path));
        }
    }
}



