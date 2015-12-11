input
  .Select(line => new
    {
        Chars = line.Length,
        EscapedChars = (
            '"'
            + string.Join("", line.Select(c =>
            {
                switch (c.ToString())
                {
                    case @"\":
                        return @"\\";
                    case @"""":
                        return @"\""";
                    default:
                        return c.ToString();
                }
            }))
            + '"').Length
    }
    )
    .Aggregate(0, (total, sums) => total + sums.EscapedChars - sums.Chars
    )
