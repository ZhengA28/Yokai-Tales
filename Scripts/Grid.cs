using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Godot;

public partial class Grid : Control
{
	[Signal] public delegate void UpdateScoreEventHandler(int points);
	[Signal] public delegate void UpdateTurnsEventHandler();
	private Orbs[,] grid;   //2D array containing each Orb
	private List<Orbs> matchedPieces;   //list to keep track of current matched pieces
	private PackedScene[] orbType = [
		GD.Load<PackedScene>("res://Scenes/Orbs/BlueOrb.tscn"),
		GD.Load<PackedScene>("res://Scenes/Orbs/RedOrb.tscn"),
		GD.Load<PackedScene>("res://Scenes/Orbs/GreenOrb.tscn"),
		GD.Load<PackedScene>("res://Scenes/Orbs/WhiteOrb.tscn"),
		GD.Load<PackedScene>("res://Scenes/Orbs/YellowOrb.tscn")
	];  //Array containing scene of each orb

	[Export] private Vector2 startPosition;  //Coordinate of the first block on grid
	[Export] private int offset;    //How many pixels to move by
	[Export] private int numRows;   //Number of rows in grid
	[Export] private int numColumns;    //Number of columns in grid


	private Vector2 initialMousePosition;   //Position of mouse on click
	private Vector2 finalMousePosition; //Position of mouse on release

	Vector2I initialGridPos;    //Initial mouse position relative to grid
	Vector2I finalGridPos;  //Final mouse pisition relative to grid
	private Timer fallTimer;
	private Timer refillTimer;
	private Timer destroyTimer;
	private bool orbSelected = false;   //if piece in grid is selected
	private enum STATE {Move, Wait};
	private STATE state;
	private bool swapped = false;
	private Random rand;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		fallTimer = GetNode<Timer>("FallTimer");
		refillTimer = GetNode<Timer>("RefillTimer");
		destroyTimer = GetNode<Timer>("DestroyTimer");

		startPosition.X = -112.0f;
		startPosition.Y = -16.0f;

		grid = new Orbs[numRows, numColumns];
		matchedPieces = [];
		rand = new Random();
		state = STATE.Move;
		CreateGrid();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (state == STATE.Move)
		{
			GD.Print("Move");
			TouchInput();
		}
	}

	private void CreateGrid()
	{
		for (int i = 0; i < numRows; i++)
		{
			for (int j = 0; j < numColumns; j++)
			{
				//Instantiate Orbs and add to scene tree
				Orbs orb = (Orbs)orbType[rand.Next(5)].Instantiate();

				while (MatchAt(i, j, orb.Color))
				{
					orb = (Orbs)orbType[rand.Next(5)].Instantiate();
				}
				AddChild(orb);
				orb.Position = GridToPixel(0, j);

				orb.SpawnIn(GridToPixel(i, j));   //Set orbs grid coordinates
				grid[i, j] = orb;
				//GD.Print("X:" + grid[i, j].Position.X, "Y:" + grid[i, j].Position.Y);
				//GD.Print("Color: " + grid[i, j].Color);
			}
		}
	}

	private bool MatchAt(int row, int column, Orbs.TYPE type)
	{
		if (row > 1)
		{
			if (grid[row - 1, column] != null && grid[row - 2, column] != null)
			{
				if (grid[row - 1, column].Color == type && grid[row - 2, column].Color == type)
					return true;
			}
		}

		if (column > 1)
		{
			if (grid[row, column - 1] != null && grid[row, column - 2] != null)
			{
				if (grid[row, column - 1].Color == type && grid[row, column - 2].Color == type)
					return true;
			}
		}
		return false;
	}


	//Translate grid coordinates to pixel coordinates on screen
	private Vector2 GridToPixel(int row, int column)
	{
		var newX = startPosition.X + offset * column;
		var newY = startPosition.Y + offset * row;
		//GD.Print("X: " + newX + ", Y: " + newY);
		return new Vector2(newX, newY);
	}


	//Translate the pixel screen coordinates to grid coordinates
	private Vector2I PixelToGrid(float PixX, float PixY)
	{
		var newX = (int)Math.Round((PixX / Scale.X - startPosition.X) / offset);
		var newY = (int)Math.Round((PixY / Scale.X - startPosition.Y) / offset);
		//GD.Print("X: " + newX + ", Y: " + newY);
		return new Vector2I(newX, newY);
	}


	private void TouchInput()
	{

		if (Input.IsActionJustPressed("Click"))
		{
			initialMousePosition = GetGlobalMousePosition();
			initialGridPos = PixelToGrid(initialMousePosition.X - Position.X, initialMousePosition.Y - Position.Y);

			//Determine if initial mouse click is valid position on grid
			if (CheckInGrid(initialGridPos))
			{
				orbSelected = true;
			}

		}

		if (Input.IsActionJustReleased("Click"))
		{
			finalMousePosition = GetGlobalMousePosition();
			finalGridPos = PixelToGrid(finalMousePosition.X - Position.X, finalMousePosition.Y - Position.Y);

			//Determine if final mouse position is valid to swap orbs
			if (CheckInGrid(finalGridPos) && orbSelected)
			{
				state = STATE.Wait;	//Prevent user from swapping until matches are checked
				TouchDirection(initialGridPos, finalGridPos);   //Direction to swap orbs
				orbSelected = false;
			}
		}
	}


	private bool CheckInGrid(Vector2I position)
	{
		//GD.Print(position.X + ", " + position.Y);
		if (position.X >= 0 && position.X < numColumns)
		{
			if (position.Y >= 0 && position.Y < numRows)
			{
				return true;
			}
		}

		return false;
	}


	///	<summary>
	///	Determine which direction to swap the pieces
	///	</summary>
	private void TouchDirection(Vector2I iGrid, Vector2I fGrid)
	{
		int XDistance = Math.Abs(Math.Abs(fGrid.X) - Math.Abs(iGrid.X));
		int YDistance = Math.Abs(Math.Abs(fGrid.Y) - Math.Abs(iGrid.Y));


		if (XDistance > YDistance)
		{
			if (fGrid.X > iGrid.X)
			{
				SwapOrbs(iGrid.Y, iGrid.X, new Vector2I(1, 0)); //Swap orb right
			}
			else
			{
				SwapOrbs(iGrid.Y, iGrid.X, new Vector2I(-1, 0));    //Swap orb left
			}
		}
		else if (XDistance < YDistance)
		{
			if (fGrid.Y > iGrid.Y)
			{
				SwapOrbs(iGrid.Y, iGrid.X, new Vector2I(0, 1)); //Swap orb down
			}
			else
			{
				SwapOrbs(iGrid.Y, iGrid.X, new Vector2I(0, -1));    //swap orb up

			}
		}
	}


	private void SwapOrbs(int row, int column, Vector2I direction)
	{
		Orbs firstOrb = grid[row, column];
		Orbs secondOrb = grid[row + direction.Y, column + direction.X];

		grid[row, column] = secondOrb;
		grid[row + direction.Y, column + direction.X] = firstOrb;

		//GD.Print("X1: " + firstOrb.Position.X, ", Y1: " + firstOrb.Position.Y);
		//GD.Print("X2: " + secondOrb.Position.X + ", Y2: " + secondOrb.Position.Y);


		if (CheckMatches()) //Decrement turns if match found
		{
			firstOrb.Swap(GridToPixel(row + direction.Y, column + direction.X));
			secondOrb.Swap(GridToPixel(row, column));
			EmitSignal(SignalName.UpdateTurns);
		}
		else if (!swapped) //Swap orbs back if no match
		{
			GD.Print("No Match");
			swapped = true;
			SwapOrbs(row, column, direction);
			firstOrb.SwapBack(GridToPixel(row, column), GridToPixel(row + direction.Y, column + direction.X));
			secondOrb.SwapBack(GridToPixel(row + direction.Y, column + direction.X), GridToPixel(row, column));
		}
		else    //orbs have been swapped back after no match
		{
			swapped = false;
		}

	}


	private bool CheckMatches()
	{
		GD.Print(state);
		int points = FindMatches();
		GD.Print(points);

		if (points != 0)
		{
			destroyTimer.Start();
			EmitSignal(SignalName.UpdateScore, points);
			return true;
		}
		else
		{
			GD.Print("Move again");
			state = STATE.Move; //Allow player to move pieces again if no more match
		}

		return false;
	}

	private int FindMatches()
	{
		int horizontal = 0;
		int vertical = 0;

		for (int i = 0; i < numRows; i++)
		{
			for (int j = 0; j < numColumns; j++)
			{
				//GD.Print("row :" + i, "column: " + j);
				horizontal += HorizontalMatch(i, j, grid[i, j].Color);
				vertical += VerticalMatch(i, j, grid[i, j].Color);

			}
		}
		return horizontal + vertical;
	}


	private int HorizontalMatch(int row, int column, Orbs.TYPE type)
	{
		int matches = 0;
		for (int i = column; i < numColumns; i++)
		{
			if (grid[row, i].Color == type)
			{
				matches++;
			}
			else
			{
				break;
			}
		}

		if (matches >= 3)
		{
			for (int i = 0; i < matches; i++)
			{
				grid[row, column + i].SetMatched();
				matchedPieces.Add(grid[row, column + i]);
			}
		}
		else
		{
			matches = 0;
		}

		return matches;
	}


	private int VerticalMatch(int row, int column, Orbs.TYPE type)
	{
		int matches = 0;
		for (int i = row; i < numRows; i++)
		{
			if (grid[i, column].Color == type)
			{
				matches++;
			}
			else
			{
				break;
			}
		}

		if (matches >= 3)
		{
			for (int i = 0; i < matches; i++)
			{
				grid[row + i, column].SetMatched();
				matchedPieces.Add(grid[row + i, column]);
			}
		}
		else
		{
			matches = 0;
		}

		return matches;
	}

	private int CalculateScore(int matches)
	{
		int score = 0;
		switch (matches)
		{
			case 3:
				score = 300;
				break;
			case 4:
				score = 400;
				break;
			case 5:
				score = 500;
				break;
			default:
				score = 0;
				break;
		}

		return score;
	}


	private void RemoveMatched()
	{
		for (int i = 0; i < numRows; i++)
		{
			for (int j = 0; j < numColumns; j++)
			{
				if (grid[i, j].IsMatched())
				{
					//GD.Print("row: " + i + "column: " + j);
					grid[i, j].PopSFX();
					grid[i, j] = null;
				}
			}
		}
		fallTimer.Start();
	}


	private void CollapseColumn()
	{
		for (int i = numRows - 1; i > 0; i--)
		{
			for (int j = numColumns - 1; j >= 0; j--)
			{
				if (grid[i, j] == null) //Check if coordinate on grid is empty
				{
					for (int k = i - 1; k >= 0; k--) //Traverse up the column to find a piece
					{
						if (grid[k, j] != null)
						{
							grid[k, j].Swap(GridToPixel(i, j));
							grid[i, j] = grid[k, j];
							grid[k, j] = null;
							break;
						}

					}
				}
			}
		}

		refillTimer.Start();
	}

	private void RefillPiece()
	{
		for (int i = numRows - 1; i >= 0; i--)
		{
			for (int j = numColumns - 1; j >= 0; j--)
			{
				if (grid[i, j] == null)
				{
					Orbs orb = (Orbs)orbType[rand.Next(5)].Instantiate();
					AddChild(orb);
					orb.Position = GridToPixel(0, j);

					orb.Swap(GridToPixel(i, j));   //Set orbs grid coordinates
					grid[i, j] = orb;
				}
			}
		}

		CheckMatches();
	}


	//Move pieces on grid down when timer signal is emitted
	private void _Fall_Timer()
	{
		//GD.Print("Collapsing");
		CollapseColumn();
	}

	private void _Refill_Timer()
	{
		//GD.Print("Refilling Pieces");
		RefillPiece();
	}

	private void _Destroy_Piece()
	{
		RemoveMatched();
	}
}
