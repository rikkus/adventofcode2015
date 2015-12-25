	var sues = input.Select(i =>

		{
			var tokens = i.Split(new[] { ':' }, 2);

			var dict = Regex.Matches
			(
				tokens[1],
				@"(?<name>\w+):\s(?<n>\d+)",
				RegexOptions.ExplicitCapture
			)
			.Cast<Match>()
			.Select
			(
				m =>
				new
				{
					Name = m.Groups["name"].Value,
					Value = int.Parse(m.Groups["n"].Value)
				}
			)
			.ToDictionary(o => o.Name, o => o.Value);

			dict["i"] = int.Parse(tokens[0].Split(new[] { ' ' })[1]);

			return dict;
		}
	);

	var known = new Dictionary<string, int>()
	{
		{ "children", 3 },
		{ "cats", 7 },
		{ "samoyeds", 2 },
		{ "pomeranians", 3 },
		{ "akitas", 0 },
		{ "vizslas", 0 },
		{ "goldfish", 5 },
		{ "trees", 3 },
		{ "cars", 2 },
		{ "perfumes", 1 }
	};

	sues.Where(sue => known.Keys.All(key => !sue.ContainsKey(key) || sue[key] == known[key])).Dump();
