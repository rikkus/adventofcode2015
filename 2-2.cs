input
	.Split(new [] { Environment.NewLine })
	.Aggregate(
		0,
		(feetOfRibbonRequired, dimensionsText) =>
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
			
			return feetOfRibbonRequired
				new [] { l+l+w+w, w+w+h+h, l+l+h+h }.Min() + l*w*h;
		}
	)