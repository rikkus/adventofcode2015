void Main()
{
	var doc = JObject.Parse(File.ReadAllText(@"c:\temp\aoc-12.txt"));
	doc.Dump();
	Parse(doc, 0).Dump();
}

int Parse(JToken token, int sum)
{
	switch (token.Type)
	{
		case JTokenType.Object:
			{
				var o = (JObject)token;
				if (o.Properties().Any(p => p.Value.Type == JTokenType.String && p.Value.Value<string>() == "red"))
					return sum;
				return o.Properties().Select(p => p.Value).Sum(p => Parse(p, sum));
			}
		case JTokenType.Property:
			return Parse(((JProperty)token).Value, sum);
		case JTokenType.Array:
			return token.Sum(v => Parse(v, sum));
		case JTokenType.Integer:
			return token.Value<int>() + sum;
		default:
			return sum;
	}
}
