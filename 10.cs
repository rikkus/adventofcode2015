Regex.Replace(input, @"(.)\1*", m => (m.Value.Length + m.Groups[1].Value))
