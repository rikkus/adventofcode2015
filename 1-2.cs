input.Aggregate
(
	new { Floor = 0, BasementPosition = 0, Position = 1 },
	(state, brace) => {	
		var newFloor = state.Floor + (brace == '(' ? 1 : -1);
		return new {
			Floor = newFloor,
			BasementPosition = (newFloor == -1 && state.BasementPosition == 0)
				? state.Position
				: state.BasementPosition,
			Position = state.Position + 1
		};
	}
)