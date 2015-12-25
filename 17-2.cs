public class LambdaComparer<T> : IComparer<T>
{
	private readonly Func<T, T, int> func;

	public LambdaComparer(Func<T, T, int> func)
	{
		this.func = func;
	}

	public int Compare(T x, T y)
	{
		return func(x, y);
	}
}

public static class IEnumerableExtensions
{
	public static IEnumerable<TSource> MinBy<TSource, TValue>(this IEnumerable<TSource> self, Func<TSource, TValue> selector)
	{
		return MinBy(self, selector, null);
	}

	public static IEnumerable<TSource> MinBy<TSource, TValue>(this IEnumerable<TSource> self, Func<TSource, TValue> selector, IComparer<TValue> comparer)
	{
		comparer = comparer ?? Comparer<TValue>.Default;
		return MaxBy(self, selector, new LambdaComparer<TValue>((x, y) => comparer.Compare(y, x)));
	}
	
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
				max = new List<TSource>() { e };
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


void Main()
{
	var containers = @"11
30
47
31
32
36
3
1
5
3
32
36
15
11
46
26
28
1
19
3"
	.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
	.Select(s => int.Parse(s.Trim()))
	.ToList();

	Enumerable.Range(1, containers.Count).SelectMany
	(
		containerCount =>
		new Combinatorics.Collections.Combinations<int>
		(
			containers,
			containerCount,
			Combinatorics.Collections.GenerateOption.WithoutRepetition
		)
	)
	.Where(combination => combination.Sum() == 150)
	.MinBy(combination => combination.Count())
	.Count()
	.Dump();
}

