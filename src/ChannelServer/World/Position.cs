﻿// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see license file in the main folder

using System;

namespace Aura.Channel.World
{
	/// <summary>
	/// Describes the current position of an entity.
	/// </summary>
	public struct Position
	{
		public readonly int X;
		public readonly int Y;

		/// <summary>
		/// Returns position with X and Y being 0.
		/// </summary>
		public static Position Zero { get { return new Position(0, 0); } }

		public Position(int x, int y)
		{
			this.X = x;
			this.Y = y;
		}

		public Position(Position pos)
		{
			this.X = pos.X;
			this.Y = pos.Y;
		}

		/// <summary>
		/// Returns distance between this and another position.
		/// </summary>
		/// <param name="otherPos"></param>
		/// <returns></returns>
		public int GetDistance(Position otherPos)
		{
			return (int)Math.Sqrt(Math.Pow(X - otherPos.X, 2) + Math.Pow(Y - otherPos.Y, 2));
		}

		/// <summary>
		/// Returns true if the other position is within range.
		/// </summary>
		/// <param name="otherPos"></param>
		/// <param name="range"></param>
		/// <returns></returns>
		public bool InRange(Position otherPos, int range)
		{
			return (Math.Pow(X - otherPos.X, 2) + Math.Pow(Y - otherPos.Y, 2) <= Math.Pow(range, 2));
		}

		/// <summary>
		/// Returns random position around this position,
		/// not nearer than min, and not further than max.
		/// </summary>
		/// <param name="radius"></param>
		/// <param name="rnd"></param>
		/// <returns></returns>
		public Position GetRandomInRange(int distanceMin, int distanceMax, Random rnd)
		{
			return this.GetRandom(rnd.Next(distanceMin, distanceMax + 1), rnd);
		}

		/// <summary>
		/// Returns random position in radius around this position.
		/// </summary>
		/// <param name="radius"></param>
		/// <param name="rnd"></param>
		/// <returns></returns>
		public Position GetRandomInRange(int radius, Random rnd)
		{
			return this.GetRandom(rnd.Next(radius + 1), rnd);
		}

		/// <summary>
		/// Returns random position in radius around this position.
		/// </summary>
		/// <param name="radius"></param>
		/// <param name="rnd"></param>
		/// <returns></returns>
		private Position GetRandom(int distance, Random rnd)
		{
			var angle = rnd.NextDouble() * Math.PI * 2;
			var x = this.X + distance * Math.Cos(angle);
			var y = this.Y + distance * Math.Sin(angle);

			return new Position((int)x, (int)y);
		}

		/// <summary>
		/// Returns position on the line between position and other.
		/// </summary>
		/// <remarks>
		/// When you knock someone back, he gets pushed in the opposite
		/// direction. The other position would be the enemy, the distance
		/// the amount how far to push him away. A negative distance will
		/// return a position between you two.
		/// </remarks>
		public Position GetRelative(Position other, int distance)
		{
			if (this == other)
				return this;

			var deltaX = (double)other.X - this.X;
			var deltaY = (double)other.Y - this.Y;

			var deltaXY = Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2));

			var newX = other.X + (distance / deltaXY) * (deltaX);
			var newY = other.Y + (distance / deltaXY) * (deltaY);

			return new Position((int)newX, (int)newY);
		}

		public static bool operator ==(Position pos1, Position pos2)
		{
			return (pos1.X == pos2.X && pos1.Y == pos2.Y);
		}

		public static bool operator !=(Position pos1, Position pos2)
		{
			return !(pos1 == pos2);
		}

		public override int GetHashCode()
		{
			return this.X.GetHashCode() ^ this.Y.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return obj is Position && this == (Position)obj;
		}

		public override string ToString()
		{
			return "(Position: " + this.X + ", " + this.Y + ")";
		}
	}
}
