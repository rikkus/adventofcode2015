Enumerable.Range(1, 50).Aggregate("1113122113", (acc, i) => Regex.Replace(acc, @"(.)\1*", m => (m.Value.Length + m.Groups[1].Value))).Length
