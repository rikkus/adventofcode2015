class Computer
{
	public ulong A;
	public ulong B;
	public int IP;
}

Action<Computer> Parse(string s)
{
	var tokens = s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

	if (tokens.Length == 3)
	{
		tokens[1] = tokens[1][0].ToString();
		tokens[2] = tokens[2].Replace("+", "");
	}

	int ipOffset = 0;
	
	switch (tokens[0])
	{
		case "jmp":
			ipOffset = int.Parse(tokens[1]);
        	break;

		case "jie":
			ipOffset = int.Parse(tokens[2]);
			break;

		case "jio":
			ipOffset = int.Parse(tokens[2]);
			break;
	}

	switch (tokens[0])
	{
		case "hlf":
			if (tokens[1] == "a")
				return (computer) => { computer.A /= 2; computer.IP++; };
			else
				return (computer) => { computer.B /= 2; computer.IP++; };
		case "tpl":
			if (tokens[1] == "a")
				return (computer) => { computer.A *= 3; computer.IP++; };
			else
				return (computer) => { computer.B *= 3; computer.IP++; };
		case "inc":
			if (tokens[1] == "a")
				return (computer) => { computer.A++; computer.IP++; };
			else
				return (computer) => { computer.B++; computer.IP++; };
		case "jmp":
			return (computer) => { computer.IP += ipOffset; };
		case "jie":
			if (tokens[1] == "a")
				return (computer) => { if (computer.A % 2 == 0) computer.IP += ipOffset; else computer.IP++; };
			else
				return (computer) => { if (computer.B % 2 == 0) computer.IP += ipOffset; else computer.IP++; };
		case "jio":
			if (tokens[1] == "a")
				return (computer) => { if (computer.A == 1) computer.IP += ipOffset; else computer.IP++; };
			else
				return (computer) => { if (computer.B == 1) computer.IP += ipOffset; else computer.IP++; };
				
		default:
			throw new Exception("Unknown instruction: '" + s + "'");
	}
}

void Main()
{

	var program = @"jio a, +18
inc a
tpl a
inc a
tpl a
tpl a
tpl a
inc a
tpl a
inc a
tpl a
inc a
inc a
tpl a
tpl a
tpl a
inc a
jmp +22
tpl a
inc a
tpl a
inc a
inc a
tpl a
inc a
tpl a
inc a
inc a
tpl a
tpl a
inc a
inc a
tpl a
inc a
inc a
tpl a
inc a
inc a
tpl a
jio a, +8
inc b
jie a, +4
tpl a
inc a
jmp +2
hlf a
jmp -7".Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

	var instructions = program.Select(line => Parse(line)).ToArray();

	var computer = new Computer(); // for part 2: { A = 1 };

	computer.Dump();

	while (true)
	{
		if (computer.IP >= instructions.Length)
		{
			computer.B.Dump();
			break;
		}
		
		instructions[computer.IP](computer);

		//computer.Dump();
	}

}
