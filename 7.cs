using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace aoc
{
    class EdgeBuilder
    {
        public Regex Expression { get; set; }
        public Func<Match, Func<ushort[], ushort>> NodeBuilder { get; set; }

        public EdgeBuilder(string expression, Func<Match, Func<ushort[], ushort>> nodeBuilder)
        {
            Expression = new Regex(expression);
            NodeBuilder = nodeBuilder;
        }
    }

    internal class Program
    {
        private readonly Dictionary<string, ushort> evaluationCache = new Dictionary<string, ushort>();

        private readonly EdgeBuilder[] edgeBuilders =
        {
            new EdgeBuilder(@"^(?<n1>\w+) -> (?<name>\w+)$",
                match => inputs => inputs[0]),

            new EdgeBuilder(@"^(?<n1>\w+) AND (?<n2>\w+) -> (?<name>\w+)$",
                match => inputs => (ushort) (inputs[0] & inputs[1])),

            new EdgeBuilder(@"^(?<n1>\w+) OR (?<n2>\w+) -> (?<name>\w+)$",
                match => inputs => (ushort) (inputs[0] | inputs[1])),

            new EdgeBuilder(@"^(?<n1>\w+) LSHIFT (?<n2>\w+) -> (?<name>\w+)$",
                match => inputs => (ushort) (inputs[0] << inputs[1])),

            new EdgeBuilder(@"^(?<n1>\w+) RSHIFT (?<n2>\w+) -> (?<name>\w+)$",
                match => inputs => (ushort) (inputs[0] >> inputs[1])),

            new EdgeBuilder(@"^NOT (?<n1>\w+) -> (?<name>\w+)$",
                match => inputs => (ushort) (~inputs[0]))
        };

        private static void Main()
        {
            var input = @"af AND ah -> ai ... [elided]".Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            
            var program = new Program(input);

            foreach (var s in new[] {"a"})
                Console.WriteLine(s + ":  "+ program.Evaluate(s));

            Console.ReadKey();
        }

        private readonly Dictionary<string, Tuple<string[], Func<ushort[], ushort>>> edges;

        Program(IEnumerable<string> input)
        {
            edges = input.Select(Edge).ToDictionary(tuple => tuple.Item1, tuple => Tuple.Create(tuple.Item2, tuple.Item3));
        }

        public ushort Evaluate(string edgeName)
        {
            ushort constantValue;

            if (ushort.TryParse(edgeName, out constantValue))
            {
                return constantValue;
            }

            ushort cachedEvaluation;

            if (evaluationCache.TryGetValue(edgeName, out cachedEvaluation))
            {
                return cachedEvaluation;
            }

            var edge = edges[edgeName];

            var paramValues = edge.Item1.Select(Evaluate).ToArray();

            var value = edge.Item2.Invoke(paramValues);
            evaluationCache[edgeName] = value;
            return value;
        }

        private static List<string> EdgeReferences(Match match)
        {
            var edgeReferences = new List<string>();

            if (match.Groups["n1"].Success)
                edgeReferences.Add(match.Groups["n1"].Value);

            if (match.Groups["n2"].Success)
                edgeReferences.Add(match.Groups["n2"].Value);

            return edgeReferences;
        }

        private Tuple<string, string[], Func<ushort[], ushort>> Edge(string inputLine)
        {
            return edgeBuilders
                .Select(
                    builder =>
                        new
                        {
                            Match = builder.Expression.Match(inputLine),
                            Builder = builder.NodeBuilder
                        })
                .Where(x => x.Match.Success)
                .Select(x => Tuple.Create(
                    x.Match.Groups["name"].Value,
                    EdgeReferences(x.Match).ToArray(),
                    x.Builder.Invoke(x.Match)
                    )
                )
                .First();
        }
    }
}
