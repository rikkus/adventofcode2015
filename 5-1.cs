input.Split(new[]{Environment.NewLine}, StringSplitOptions.None)
	.Count(s => {
		
	var vowelCount = Regex.Matches(s, "[aeiou]").Count;
	var hasRepeatedCharacter = Regex.IsMatch(s, @"(.)\1");
	var hasBadPair = Regex.IsMatch(s, @"(ab)|(cd)|(pq)|(xy)");
	
	return	(vowelCount >= 3)
		&&	hasRepeatedCharacter
		&&	!hasBadPair
		;
})