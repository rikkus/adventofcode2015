input.Aggregate(
 	new {
		SantaPosition = Tuple.Create(0, 0),
		RoboSantaPosition = Tuple.Create(0, 0),
		Visits = new Dictionary<Tuple<int, int>, int>
		{ { Tuple.Create(0, 0), 2 } },
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
			
		Tuple<int,int> visitPosition;
		
		if (state.Index % 2 == 0)
		{
			newSantaPosition = Tuple.Create(state.SantaPosition.Item1 + xDelta,state.SantaPosition.Item2 + yDelta);
			visitPosition = newSantaPosition;
		}
		else
		{
			newRoboSantaPosition = Tuple.Create(state.RoboSantaPosition.Item1 + xDelta,state.RoboSantaPosition.Item2 + yDelta);
			visitPosition = newRoboSantaPosition;
		}
		
		if (!state.Visits.ContainsKey(visitPosition))
			state.Visits[visitPosition] = 1;
		else
			state.Visits[visitPosition] = state.Visits[visitPosition] + 1;
			
		return new {
			SantaPosition = newSantaPosition,
			RoboSantaPosition = newRoboSantaPosition,
			Visits = state.Visits,
			Index = state.Index + 1
		};
	}
).Visits.Count()