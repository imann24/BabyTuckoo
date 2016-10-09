/*
 * Author: Isaiah Mann
 * Description: Position data
 */

[System.Serializable]
public struct Position {
	public int X;
	public int Y;

	public Position (int x, int y) {
		this.X = x;
		this.Y = y;
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
}
