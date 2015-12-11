input
    .Select(line => new {Chars = line.Length, UnescapedChars = Regex.Unescape(line.Substring(1, line.Length - 2)).Length})
    .Aggregate(0, (total, sums) => total + sums.Chars - sums.UnescapedChars)
