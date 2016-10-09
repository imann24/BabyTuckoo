/*
 * Author: Isaiah Mann
 * Description: Position data
 */

using UnityEngine;

[System.Serializable]
public struct Position {
	public int X;
	public int Y;

	public Position (int x, int y) {
		this.X = x;
		this.Y = y;
	}

	public Position (float x, float y){ 
		this.X = (int) x;
		this.Y = (int) y;
	}

	public override bool Equals (object obj) {
		if (obj is Position) {
			Position otherPosition = (Position) obj;
			return otherPosition.X == X && otherPosition.Y == Y;
		} else {
			return false;
		}
	}

	public override int GetHashCode () {
		return X.GetHashCode() + Y.GetHashCode();
	}

	public Position Translate (int deltaX, int deltaY) {
		return new Position(X + deltaX, Y + deltaY);
	}

	public override string ToString () {
		return string.Format ("({0}, {1})", X, Y);
	}

	public Position Difference (Position subtractor) {
		return new Position(this.X - subtractor.X, this.Y - subtractor.Y);
	}

	public Position Sum (Position additor) {
		return new Position(this.X - additor.X, this.Y - additor.Y);
	}

	public Position Scale (float scaleFactor) {
		return new Position(this.X * scaleFactor, this.Y * scaleFactor);
	}

	public int Distance (Position from) {
		return (int) Mathf.Sqrt((Mathf.Pow(this.X - from.X, 2) + (Mathf.Pow(this.Y - from.Y, 2))));
	}

	public Position CloserTo (Position position, int steps) {
		Position deltaPosition = Difference(position);
		deltaPosition.Scale((float) steps / (float) this.Distance(position));
		return this.Difference(deltaPosition);
	}

	public Position[] GetPlus () {
		Position[] plus = new Position[4];
		plus[0] = this.Translate(1, 0);
		plus[1] = this.Translate(0, 1);
		plus[2] = this.Translate(-1, 0);
		plus[3] = this.Translate(0, -1);
		return plus;
	}
}
