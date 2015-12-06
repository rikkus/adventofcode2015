input.Aggregate(
 	new {
		SantaPosition = Tuple.Create(0, 0),
		Visits = new Dictionary<Tuple<int, int>, int>
		{ { Tuple.Create(0, 0), 1 } },
		Index = 0
	},
	(state, direction) =>
	{
		int xDelta = 0;
		int yDelta = 0;

		switch (direction)
		{
			case '>': xDelta = 1; break;
			case '<': xDelta = -1; break;
			case '^': yDelta = 1; break;
			case 'v': yDelta = -1; break;
		}
		
		var newSantaPosition = state.SantaPosition;		
		var newRoboSantaPosition = state.RoboSantaPosition;
			
		var visitPosition = Tuple.Create(state.SantaPosition.Item1 + xDelta,state.SantaPosition.Item2 + yDelta);
		
		if (!state.Visits.ContainsKey(visitPosition))
			state.Visits[visitPosition] = 1;
		else
			state.Visits[visitPosition] = state.Visits[visitPosition] + 1;
			
		return new {
			SantaPosition = visitPosition,
			Visits = state.Visits,
			Index = state.Index + 1
		};
	}
).Visits.Count()