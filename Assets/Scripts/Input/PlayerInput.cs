using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInput : GameInput
{
	CaptureableObjectBehaviour capture;

    protected override void SetReferences()
    {
		base.SetReferences();
        node = GetComponent<Node>();
		capture = GetComponent<CaptureableObjectBehaviour>();
    }
		
	protected override void FetchReferences () {
		base.FetchReferences ();
		setupCapture(Player.Instance);
	}

	void setupCapture (Agent agent) {
		float captureTime;
		if (node.IsOwned && node.Owner != agent) {
			captureTime = captureEnemyNodeTime;
		} else if (!node.IsOwned) {
			captureTime = captureEmptyNodeTime;
		} else {
			captureTime = 0;
		}
		capture.SetCaptureTime(captureTime, CaptureTimeout);
		capture.SetCaptureColour(agent.Colour);
	}

    // Variables
    private Node node;
    private float timer;
    private int seconds;
    public int captureEnemyNodeTime = 5;
    public int captureEmptyNodeTime = 2;
	public int MaxCaptureDistance = 3;
	public float CaptureTimeout = 2;
    private Node previousCapturedNode;
    private List<Node> captureChain = new List<Node>();
    private Node[] extremes = new Node[4];
    private List<Node> unclaimedNodes;
	void OnMouseEnter () {
		if (!InputEnabled) {
			return;
		}

		node.StartCapturing(Player.Instance);
	}

	bool canCapture (Player player) {
		return DistanceBetwenTwoPositions(player.LastCapturedNode.Position.X, node.Position.X) <= MaxCaptureDistance 
			&& DistanceBetwenTwoPositions(player.LastCapturedNode.Position.Y, node.Position.Y) <= MaxCaptureDistance;
	}

    // A function for tracking mouse position over node objects
    void OnMouseOver()
    {
		if (!InputEnabled) {
			return;
		}

        // A timer to track how long the player is hovering over a node
        timer += Time.deltaTime;
		Player player = Player.Instance;
        // Start to capture node - check node owner
		if (node.IsOwned && node.Owner != player)
        {
            // Node is captured and does not belong to the player
            if (timer > captureEnemyNodeTime)
            {
				if (canCapture(Player.Instance))
				{
					CaptureNodes(Player.Instance.LastCapturedNode, node);
					Player.Instance.LastCapturedNode = node;
				}

			} 
			else if (canCapture(Player.Instance)) 
			{
				node.UpdateCaptureProgress(player, timer / captureEmptyNodeTime);
			}
        }
        else if (!node.IsOwned)
        {
            // Node is not captured
            if(timer > captureEmptyNodeTime)
            {
				//node.Owner = Player.Instance;
				if (canCapture(Player.Instance))
				{
					CaptureNodes(Player.Instance.LastCapturedNode, node);
					Player.Instance.LastCapturedNode = node;
				}			
			}
			else if (canCapture(Player.Instance)) 
			{
				node.UpdateCaptureProgress(player, timer / captureEmptyNodeTime);
			}

		} 
		else if (node.Owner == player) {
			node.Link(player);
		}
    }
           

    // A function for tracking mouse position leaving node objects
    void OnMouseExit()
    {
		if (!InputEnabled) {
			return;
		}

        // Reset the hover over timer
        timer = 0;
		node.EndCapturing();
    }
    // A function to return the distance between two nodes
    int DistanceBetwenTwoPositions(int value1, int value2)
    {
        return Mathf.Abs(value1 - value2);
    }
    // A function to capture a chain of nodes
    void CaptureNodes(Node start, Node end)
    {
        int moveValueX = Mathf.Abs(start.Position.X - end.Position.X);
        int moveValueY = Mathf.Abs(start.Position.Y - end.Position.Y);
        Node nodeToCapture;

        if (start.Position.X == end.Position.X)
        {
            if (start.Position.Y < end.Position.Y)
            {
                // Loop towards end position upwards
                for (int i = 0; i <= moveValueY; i++)
                {
                    nodeToCapture = Game.Instance.GetNode(new Position(start.Position.X, start.Position.Y + i));
                    nodeToCapture.Owner = Player.Instance;
                    previousCapturedNode = Game.Instance.GetNode(new Position(start.Position.X, start.Position.Y + i - 1));
                    if (nodeToCapture != null && !captureChain.Contains(nodeToCapture)) {
                        captureChain.Add(nodeToCapture);
                    }
                    // Check to see if link looped around
                    if (captureChain.Count > 0 && nodeToCapture != null && previousCapturedNode != null)
                    {
                        CheckForCompletedLoop(captureChain, previousCapturedNode, nodeToCapture);
                    }
                }
            }
            else if (start.Position.Y > end.Position.Y)
            {
                // Loop towards end position downwards
                for (int i = 0; i <= moveValueY; i++)
                {
                    nodeToCapture = Game.Instance.GetNode(new Position(start.Position.X, start.Position.Y - i));
                    nodeToCapture.Owner = Player.Instance;
                    previousCapturedNode = Game.Instance.GetNode(new Position(start.Position.X, start.Position.Y - i + 1));
                    if (nodeToCapture != null && !captureChain.Contains(nodeToCapture))
                    {
                        captureChain.Add(nodeToCapture);
                    }
                    // Check to see if link looped around
                    if (captureChain.Count > 0 && nodeToCapture != null && previousCapturedNode != null)
                    {
                        CheckForCompletedLoop(captureChain, previousCapturedNode, nodeToCapture);
                    }
                }
            }
        }
        else if (start.Position.Y == end.Position.Y)
        {
            if (start.Position.X < end.Position.X)
            {
                // Loop towards end position upwards
                for (int i = 0; i <= moveValueX; i++)
                {
                    nodeToCapture = Game.Instance.GetNode(new Position(start.Position.X + i, start.Position.Y));
                    Game.Instance.GetNode(new Position(start.Position.X + i, start.Position.Y)).Owner = Player.Instance;
                    previousCapturedNode = Game.Instance.GetNode(new Position(start.Position.X + i - 1, start.Position.Y));
                    if (Game.Instance.GetNode(new Position(start.Position.X + i, start.Position.Y)) != null && !captureChain.Contains(nodeToCapture))
                    {
                        captureChain.Add(Game.Instance.GetNode(new Position(start.Position.X + i, start.Position.Y)));
                    }
                    // Check to see if link looped around
                    if (captureChain.Count > 0 && nodeToCapture != null && Game.Instance.GetNode(new Position(start.Position.X + i - 1, start.Position.Y)) != null)
                    {
                        CheckForCompletedLoop(captureChain, Game.Instance.GetNode(new Position(start.Position.X + i - 1, start.Position.Y)), Game.Instance.GetNode(new Position(start.Position.X + i, start.Position.Y)));
                    }
                }
            }
            else if (start.Position.X > end.Position.X)
            {
                // Loop towards end position downwards
                for (int i = 0; i <= moveValueX; i++)
                {
                    nodeToCapture = Game.Instance.GetNode(new Position(start.Position.X - i, start.Position.Y));
                    nodeToCapture.Owner = Player.Instance;
                    previousCapturedNode = Game.Instance.GetNode(new Position(start.Position.X - i + 1, start.Position.Y));
                    if (nodeToCapture != null && !captureChain.Contains(nodeToCapture))
                    {
                        captureChain.Add(nodeToCapture);
                    }
                    // Check to see if link looped around
                    if (captureChain.Count > 0 && nodeToCapture != null && previousCapturedNode != null)
                    {
                        CheckForCompletedLoop(captureChain, previousCapturedNode, nodeToCapture);
                    }
                }
            }
        }
        else if ((start.Position.X < end.Position.X && start.Position.Y < end.Position.Y) && (DistanceBetwenTwoPositions(start.Position.X, end.Position.X) == DistanceBetwenTwoPositions(start.Position.Y, end.Position.Y)))
        {
            // Top right
            for (int i = 0; i <= moveValueX; i++)
            {
                nodeToCapture = Game.Instance.GetNode(new Position(start.Position.X + i, start.Position.Y + i));
                nodeToCapture.Owner = Player.Instance;
                previousCapturedNode = Game.Instance.GetNode(new Position(start.Position.X + i - 1, start.Position.Y + i - 1));
                if (nodeToCapture != null && !captureChain.Contains(nodeToCapture))
                {
                    captureChain.Add(nodeToCapture);
                }
                // Check to see if link looped around
                if (captureChain.Count > 0 && nodeToCapture != null && previousCapturedNode != null)
                {
                    CheckForCompletedLoop(captureChain, previousCapturedNode, nodeToCapture);
                }
            }
        }
        else if ((start.Position.X < end.Position.X && start.Position.Y > end.Position.Y) && (DistanceBetwenTwoPositions(start.Position.X, end.Position.X) == DistanceBetwenTwoPositions(start.Position.Y, end.Position.Y)))
        {
            // Bottom right
            for (int i = 0; i <= moveValueX; i++)
            {
                nodeToCapture = Game.Instance.GetNode(new Position(start.Position.X + i, start.Position.Y - i));
                nodeToCapture.Owner = Player.Instance;
                previousCapturedNode = Game.Instance.GetNode(new Position(start.Position.X + i - 1, start.Position.Y - i + 1));
                if (nodeToCapture != null && !captureChain.Contains(nodeToCapture))
                {
                    captureChain.Add(nodeToCapture);
                }
                // Check to see if link looped around
                if (captureChain.Count > 0 && nodeToCapture != null && previousCapturedNode != null)
                {
                    CheckForCompletedLoop(captureChain, previousCapturedNode, nodeToCapture);
                }
            }
        }
        else if ((start.Position.X > end.Position.X && start.Position.Y < end.Position.Y) && (DistanceBetwenTwoPositions(start.Position.X, end.Position.X) == DistanceBetwenTwoPositions(start.Position.Y, end.Position.Y)))
        {
            // Top left
            for (int i = 0; i <= moveValueX; i++)
            {
                nodeToCapture = Game.Instance.GetNode(new Position(start.Position.X - i, start.Position.Y + i));
                nodeToCapture.Owner = Player.Instance;
                previousCapturedNode = Game.Instance.GetNode(new Position(start.Position.X - i + 1, start.Position.Y + i - 1));
                if (nodeToCapture != null && !captureChain.Contains(nodeToCapture))
                {
                    captureChain.Add(nodeToCapture);
                }
                // Check to see if link looped around
                if (captureChain.Count > 0 && nodeToCapture != null && previousCapturedNode != null)
                {
                    CheckForCompletedLoop(captureChain, previousCapturedNode, nodeToCapture);
                }
            }
        }
        else if ((start.Position.X > end.Position.X && start.Position.Y > end.Position.Y) && (DistanceBetwenTwoPositions(start.Position.X, end.Position.X) == DistanceBetwenTwoPositions(start.Position.Y, end.Position.Y)))
        {
            // Bottom left
            for (int i = 0; i <= moveValueX; i++)
            {
                nodeToCapture = Game.Instance.GetNode(new Position(start.Position.X - i, start.Position.Y - i));
                nodeToCapture.Owner = Player.Instance;
                previousCapturedNode = Game.Instance.GetNode(new Position(start.Position.X - i + 1, start.Position.Y - i + 1));
                if (nodeToCapture != null && !captureChain.Contains(nodeToCapture))
                {
                    captureChain.Add(nodeToCapture);
                }
                // Check to see if link looped around
                if (captureChain.Count > 0 && nodeToCapture != null && previousCapturedNode != null)
                {
                    CheckForCompletedLoop(captureChain, previousCapturedNode, nodeToCapture);
                }
            }
        }
    }
    // A function to check if a circuit of nodes has been complete
    void CheckForCompletedLoop(List<Node> nodes, Node previous, Node current)
    {
        int[] xDirections = { 1, 1, 1, 0, 0, 0, -1, -1, -1 };
        int[] yDirections = { -1, 0, 1, -1, 0, 1, -1, 0, 1 };
        for (int i = 0; i <= 8; i++)
        {
            if (Game.Instance.GetNodeFromOffset(previous, xDirections[i], yDirections[i]) != null) {
                if (Game.Instance.GetNodeFromOffset(previous, xDirections[i], yDirections[i]).Owner == Player.Instance
                    && (Game.Instance.GetNodeFromOffset(previous, xDirections[i], yDirections[i]).Position.X != previous.Position.X
                    || Game.Instance.GetNodeFromOffset(previous, xDirections[i], yDirections[i]).Position.Y != previous.Position.Y))
                {
                    Debug.Log("Scooby doo: " );
                    // Found a node that belongs to the player that isn't the previous node
                    // Do something magical
                    // Get the extremes of the provides list i.e. most north,south,east and west nodes and check every node inside that shape
                    Node currentWest = current, currentEast = current, currentNorth = current, currentSouth = current;
                    foreach (Node a in nodes)
                    {
                        if (a.Position.X < currentWest.Position.X)
                        {
                            currentWest = a;
                        }
                        if (a.Position.Y < currentSouth.Position.Y)
                        {
                            currentSouth = a;
                        }
                        if (a.Position.Y > currentNorth.Position.Y)
                        {
                            currentNorth = a;
                        }
                        if (a.Position.X > currentEast.Position.X)
                        {
                            currentEast = a;
                        }
                    }
                    // we have the extremes now, fill in the middle
                    unclaimedNodes = Game.Instance.GetUnclaimedNodes();
                    foreach (Node n in unclaimedNodes)
                    {
                        if (n.Position.X < currentEast.Position.X
                            && n.Position.X > currentWest.Position.X
                            && n.Position.Y < currentNorth.Position.Y
                            && n.Position.Y > currentSouth.Position.Y)
                        {
                            // Node is inside the bounds, change its owner to Player
                            n.Owner = Player.Instance;
                        }
                    }
                }
            }
        }
    }
}
