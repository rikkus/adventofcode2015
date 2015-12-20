public static class IEnumerableExtensions
{
	public static IEnumerable<TSource> MaxBy<TSource, TValue>(this IEnumerable<TSource> self, Func<TSource, TValue> selector)
	{
		return MaxBy(self, selector, null);
	}

	public static IEnumerable<TSource> MaxBy<TSource, TValue>(this IEnumerable<TSource> self, Func<TSource, TValue> selector, IComparer<TValue> comparer)
	{
		comparer = comparer ?? Comparer<TValue>.Default;

		var max = new List<TSource>();
		TValue maxValue = default(TValue);
		bool haveMax = false;

		foreach (var e in self)
		{
			var newValue = selector(e);
			var comparison = comparer.Compare(newValue, maxValue);
			
			if (!haveMax || comparison > 0)
			{
				max = new List<TSource>() {e};
				maxValue = newValue;
				haveMax = true;
			}
			else if (comparison == 0)
			{
				max.Add(e);
			}
		}

		if (!haveMax)
			throw new InvalidOperationException("Sequence contains no elements.");

		return max;
	}
}

class Reindeer
{
	public string Name { get; set; }
	public int Speed { get; set; }
	public int MaxFlySeconds { get; set; }
	public int MaxRestSeconds { get; set; }
	public int Distance { get; set; }
	public int Points { get; set; }

	public void Start()
	{
		Distance = 0;
		IsFlying = true;
		RemainingSecondsBeforeChange = MaxFlySeconds;
		SecondsCompleted = 0;
		Points = 0;
	}

	public void DoOneSecondOfActivity()
	{
		if (IsFlying)
		{
			Distance += Speed;
		}

		--RemainingSecondsBeforeChange;

		if (RemainingSecondsBeforeChange == 0)
		{
			RemainingSecondsBeforeChange = IsFlying ? MaxRestSeconds : MaxFlySeconds;
			IsFlying = !IsFlying;
		}
	}

	public bool IsFlying { get; private set; }
	public int RemainingSecondsBeforeChange { get; private set; }
	public int SecondsCompleted { get; private set; }
}

#if true
static IEnumerable<string> input = @"Rudolph can fly 22 km/s for 8 seconds, but then must rest for 165 seconds.
Cupid can fly 8 km/s for 17 seconds, but then must rest for 114 seconds.
Prancer can fly 18 km/s for 6 seconds, but then must rest for 103 seconds.
Donner can fly 25 km/s for 6 seconds, but then must rest for 145 seconds.
Dasher can fly 11 km/s for 12 seconds, but then must rest for 125 seconds.
Comet can fly 21 km/s for 6 seconds, but then must rest for 121 seconds.
Blitzen can fly 18 km/s for 3 seconds, but then must rest for 50 seconds.
Vixen can fly 20 km/s for 4 seconds, but then must rest for 75 seconds.
Dancer can fly 7 km/s for 20 seconds, but then must rest for 119 seconds."
		.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
		

const int Seconds = 2503;
#endif

#if false
static IEnumerable<string> input = 
@"Comet can fly 14 km/s for 10 seconds, but then must rest for 127 seconds.
Dancer can fly 16 km/s for 11 seconds, but then must rest for 162 seconds."
		.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
		

const int Seconds = 1000;
#endif

static Regex reindeerExpression = new Regex(@"(?<name>\w+) can fly (?<speed>\d+) km/s for (?<flySeconds>\d+) seconds, but then must rest for (?<restSeconds>\d+) seconds.");

static Reindeer ParseReindeerDescription(string description)
{
	var match = reindeerExpression.Match(description);

	return new Reindeer
	{
		Name = match.Groups["name"].Value,
		Speed = int.Parse(match.Groups["speed"].Value),
		MaxFlySeconds = int.Parse(match.Groups["flySeconds"].Value),
		MaxRestSeconds = int.Parse(match.Groups["restSeconds"].Value)
	};
}

void Main()
{
	var reindeer = input.Select(ParseReindeerDescription).ToArray();

	foreach (var r in reindeer)
	{
		r.Start();
	}

	for (int i = 0; i < Seconds; i++)
	{
		foreach (var r in reindeer)
		{
			r.DoOneSecondOfActivity();
		}

		foreach (var r in reindeer.MaxBy(r => r.Distance))
		{
			++r.Points;
		}
	}

	reindeer.MaxBy(r => r.Distance).Dump();
	reindeer.MaxBy(r => r.Points).Dump();
}
