Enumerable.Range(1, 10000000)
	.Select
	(
		doorNumber =>
		{
			if (doorNumber % 100000 == 0)
				doorNumber.Dump();
				
			return new
			{
				DoorNumber = doorNumber,
				Score = Enumerable.Range(1, doorNumber / 2 + 1)
					.Concat(new[] { doorNumber })
					.Sum(n => (doorNumber % n == 0) ? n * 10 : 0)
			};
        }
	)
	.First(state => state.Score >= 34000000)
	.DoorNumber
