input
	.Split(new [] { Environment.NewLine })
	.Aggregate(
		0,
		(squareFeetToOrder, dimensionsText) =>
		{
			var match = Regex.Match(
				dimensionsText,
				@"(?<l>\d+)x(?<w>\d+)x(?<h>\d+)"
			);
			
			Func<Match, string, int> val = (m, name) =>
				int.Parse(m.Groups[name].Value);
			
			var l = val(match, "l");
			var w = val(match, "w");
			var h = val(match, "h");
			
			return squareFeetToOrder
				+ 2*l*w + 2*w*h + 2*h*l
				+ new [] { l*w, w*h, h*l }.Min();
		}
	)