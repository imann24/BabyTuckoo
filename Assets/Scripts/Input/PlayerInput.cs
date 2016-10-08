using UnityEngine;
using System.Collections;

public class PlayerInput : MannBehaviour
{
    // Abstract classes got from MannBehaviour
    protected override void CleanupReferences()
    {
        // Nothing
    }

    protected override void FetchReferences()
    {
        // Nothing
    }

    protected override void HandleNamedEvent(string eventName)
    {
        // Nothing
    }
    protected override void SetReferences()
    {
        node = this.gameObject.GetComponent<Node>();
    }
    // Variables
    private Node node;
    private float timer;
    private int seconds;
    public int captureEnemyNodeTime = 5;
    public int captureEmptyNodeTime = 2;

    // A function for tracking mouse position over node objects
    void OnMouseOver()
    {
        // A timer to track how long the player is hovering over a node
        timer += Time.deltaTime;

        // Start to capture node - check node owner
        if (node.IsOwned && node.Owner != Player.Instance)
        {
            // Node is captured and does not belong to the player
            if (timer > captureEnemyNodeTime)
            {
                //node.Owner = Player.Instance;
                if (DistanceBetwenTwoPositions(Player.Instance.LastCapturedNode.Position.X, node.Position.X) <= 3 && DistanceBetwenTwoPositions(Player.Instance.LastCapturedNode.Position.Y, node.Position.Y) <= 3)
                {
                    CaptureNodes(Player.Instance.LastCapturedNode, node);
                    Player.Instance.LastCapturedNode = node;
                }
            }
        }
        else if (!node.IsOwned)
        {
            // Node is not captured
            if(timer > captureEmptyNodeTime)
            {
                //node.Owner = Player.Instance;
                if (DistanceBetwenTwoPositions(Player.Instance.LastCapturedNode.Position.X, node.Position.X) <= 3 && DistanceBetwenTwoPositions(Player.Instance.LastCapturedNode.Position.Y, node.Position.Y) <= 3) {
                    CaptureNodes(Player.Instance.LastCapturedNode, node);
                    Player.Instance.LastCapturedNode = node;
                }
            }

        }
    }
    // A function for tracking mouse position leaving node objects
    void OnMouseExit()
    {
        // Reset the hover over timer
        timer = 0;
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
        if (start.Position.X == end.Position.X)
        {
            if (start.Position.Y < end.Position.Y)
            {
                // Loop towards end position upwards
                for (int i = 0; i <= moveValueY; i++)
                {
                    Game.Instance.GetNode(new Position(start.Position.X, start.Position.Y + i)).Owner = Player.Instance;
                }
            }
            else if (start.Position.Y > end.Position.Y)
            {
                // Loop towards end position downwards
                for (int i = 0; i <= moveValueY; i++)
                {
                    Game.Instance.GetNode(new Position(start.Position.X, start.Position.Y - i)).Owner = Player.Instance;
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
                    Game.Instance.GetNode(new Position(start.Position.X + i, start.Position.Y)).Owner = Player.Instance;
                }
            }
            else if (start.Position.X > end.Position.X)
            {
                // Loop towards end position downwards
                for (int i = 0; i <= moveValueX; i++)
                {
                    Game.Instance.GetNode(new Position(start.Position.X - i, start.Position.Y)).Owner = Player.Instance;
                }
            }
        }
    }
}
