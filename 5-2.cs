input.Split(new[]{Environment.NewLine}, StringSplitOptions.None)
	.Count(s => {
		
	var repeatedPairCount = Regex.Matches(s, @"(..).*(\1)").Count;
	var hasRepeatedCharacterAroundAnother = Regex.IsMatch(s, @"(.).(\1)");
​
	return	repeatedPairCount > 0
		&&	hasRepeatedCharacterAroundAnother
		;
})