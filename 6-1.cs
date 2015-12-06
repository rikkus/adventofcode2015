var lights = Enumerable.Range(0, 1000)
	.Select(n => new BitArray(1000))
	.ToArray();
​
var commandRegex = new Regex(@"(?<op>toggle|turn on|turn off) (?<x0>\d+),(?<y0>\d+) through (?<x1>\d+),(?<y1>\d+)", RegexOptions.Compiled);
​
foreach (var instruction in instructions)
{
	var match = commandRegex.Match(instruction);
	
	if (!match.Success)
		throw new Exception("Couldn't match '" + instruction + "'");
	
	Func<bool, bool> op = null;
	
	switch (match.Groups["op"].Value)
	{
		case "toggle":
			op = (l) => !l;
			break;
		case "turn on":
			op = (l) => true;
			break;
		case "turn off":
			op = (l) => false;
			break;
	}
	
	var x0 = int.Parse(match.Groups["x0"].Value);
	var y0 = int.Parse(match.Groups["y0"].Value);
	var x1 = int.Parse(match.Groups["x1"].Value);
	var y1 = int.Parse(match.Groups["y1"].Value);
	
	for (var y = y0; y <= y1; y++)
	{
		for (var x = x0; x <= x1; x++)
		{
			lights[y].Set(x, op(lights[y][x]));
		}
	}
}
​
lights
	.Sum(row =>
		row.OfType<bool>().Count(light => light) 
	)
	.Dump();