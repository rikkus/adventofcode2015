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
	).Where(combination => combination.Sum() == 150).Count().Dump();
}

