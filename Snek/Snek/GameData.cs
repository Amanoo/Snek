using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

public class GameData
{
    private static readonly Random getrandom = new Random();

    public IntPoint playingField;
    public IntPoint foodLocation;
    public int score = 0;
    bool isAlive=true;

    public struct IntPoint
    {
        public int X, Y;
    }
    enum Direction
    {
        Right,
        Down,
        Left,
        Up
    }

    public struct SnakeChunk
    {
        public int x;
        public int y;
        public int sprite;
    }

    Direction direction;
    Direction newDirection;
    public LinkedList<SnakeChunk> snake;

    public string getNewDirection()
    {
        string dir = "";
        if (newDirection == Direction.Right) dir= "right";
        if (newDirection == Direction.Left) dir= "left";
        if (newDirection == Direction.Up) dir= "up";
        if (newDirection == Direction.Down) dir= "down";
        return dir;
    }

    public GameData(int x, int y)
	{
        playingField = new IntPoint { X = x, Y = y };
        snake = new LinkedList<SnakeChunk> { };
        resetSnake();
    }

    private void resetSnake()
    {
        snake.Clear();
        snake.AddLast(new SnakeChunk { x = playingField.X / 2 +2, y = playingField.Y/2, sprite = 100 });
        snake.AddLast(new SnakeChunk { x = playingField.X / 2 +1, y = playingField.Y / 2, sprite = 300 });
        snake.AddLast(new SnakeChunk { x = playingField.X / 2 , y = playingField.Y / 2, sprite = 300 });
        snake.AddLast(new SnakeChunk { x = playingField.X / 2 -1, y = playingField.Y / 2, sprite = 300 });
        snake.AddLast(new SnakeChunk { x = playingField.X / 2 -2, y = playingField.Y / 2, sprite = 104 });
        direction = Direction.Right;
        newDirection = Direction.Right;
        spawnFood();
    }




    public void gameTick()
    {
        SnakeChunk currHead = snake.First.Value;
        SnakeChunk newHead = new SnakeChunk {};
        newHead.x = currHead.x;
        newHead.y = currHead.y;
        switch (newDirection)
        {
            case Direction.Up:
                newHead.y=(newHead.y-1+ playingField.Y) %playingField.Y;
                break;
            case Direction.Down:
                newHead.y = (newHead.y + 1) % playingField.Y;
                break;
            case Direction.Left:
                newHead.x = (newHead.x - 1+playingField.X) % playingField.X;
                break;
            case Direction.Right:
                newHead.x = (newHead.x + 1) % playingField.X;
                break;
        }

        newHead.sprite = getNewHeadSprite(newHead.x, newHead.y);
        currHead.sprite=replaceOldHeadSprite(currHead.sprite);
        snake.First.Value = currHead;
        snake.AddFirst(newHead);
        direction = newDirection;

        if (foodLocation.X == newHead.x && foodLocation.Y == newHead.y)
        {
            score++;
            spawnFood();
        }
        else
        {

            snake.RemoveLast();
            double[] lastcoords = { snake.Last.Value.x, snake.Last.Value.y };
            double[] penultimate = { snake.Last.Previous.Value.x, snake.Last.Previous.Value.y };
            SnakeChunk newTail = snake.Last.Value;
            if ((lastcoords[0] + 1) % playingField.X == penultimate[0]) { newTail.sprite = 104; }    //tail is moving right
            else if ((penultimate[0] + 1) % playingField.X == lastcoords[0]) { newTail.sprite = 106; } //tail is moving left
            else if ((lastcoords[1] + 1) % playingField.X == penultimate[1]) { newTail.sprite = 105; } //tail is moving down
            else if ((penultimate[1] + 1) % playingField.X == lastcoords[1]) { newTail.sprite = 107; } //tail is moving up
            snake.Last.Value = newTail;
        }


    }

    private int replaceOldHeadSprite(int oldsprite)
    {
        bool hasEaten = (oldsprite >= 200 && oldsprite <= 203);
        int sprite = 0;
        switch (direction)
        {
            case Direction.Right:
                switch (newDirection)
                {
                    case Direction.Right:
                        if (!hasEaten) { sprite = 300; }
                        else { sprite = 304; }
                        break;
                    case Direction.Down:
                        if (!hasEaten) { sprite = 401; }
                        else { sprite =  402; }
                        break;
                    case Direction.Up:
                        if (!hasEaten) { sprite = 403; }
                        else { sprite = 404; }
                        break;
                }
                break;
            case Direction.Down:
                switch (newDirection)
                {
                    case Direction.Right:
                        if (!hasEaten) { sprite = 510; }
                        else { sprite = 511; }
                        break;
                    case Direction.Down:
                        if (!hasEaten) { sprite = 301; }
                        else { sprite = 305; }
                        break;
                    case Direction.Left:
                        if (!hasEaten) { sprite = 512; }
                        else { sprite = 513; }
                        break;
                }
                break;
            case Direction.Left:
                switch (newDirection)
                {
                    case Direction.Down:
                        if (!hasEaten) { sprite = 621; }
                        else { sprite = 622; }
                        break;
                    case Direction.Left:
                        if (!hasEaten) { sprite = 302; }
                        else { sprite = 306; }
                        break;
                    case Direction.Up:
                        if (!hasEaten) { sprite = 623; }
                        else { sprite = 624; }
                        break;
                }
                break;
            case Direction.Up:
                switch (newDirection)
                {
                    case Direction.Right:
                        if (!hasEaten) { sprite = 730; }
                        else { sprite = 731; }
                        break;
                    case Direction.Left:
                        if (!hasEaten) { sprite = 732; }
                        else { sprite = 733; }
                        break;
                    case Direction.Up:
                        if (!hasEaten) { sprite = 303; }
                        else { sprite = 307; }
                        break;
                }
                break;
        }
        return sprite;
    }

    private int getNewHeadSprite(double x, double y)
    {
        int sprite = 0;
        switch (newDirection)
        {
            case Direction.Right:
                sprite = 100;
                if (objectAt( x , y)) { sprite = 200; }
                else if (objectAt((x + 1) % playingField.X, y)) { sprite = 204; }
                break;
            case Direction.Down:
                sprite = 101;
                if (objectAt(x, y)) { sprite = 201; }
                else if (objectAt(x , (y + 1 ) % playingField.Y)) { sprite = 205; }
                break;
            case Direction.Left:
                sprite = 102;
                if (objectAt(x, y)) { sprite = 202; }
                else if (objectAt(( x - 1+ playingField.X)% playingField.X , y)) { sprite = 206; }
                break;
            case Direction.Up:
                sprite = 103;
                if (objectAt(x, y)) { sprite = 203; }
                else if (objectAt( x ,  (y - 1 + playingField.Y) % playingField.Y)) { sprite = 207; }
                break;
        }
        return sprite;
    }

    private bool objectAt(double x, double y)
    {
        bool objectFound = (foodLocation.X == x && foodLocation.Y == y);
        if (!objectFound)
        {
            LinkedListNode<GameData.SnakeChunk> currChunk = snake.First.Next.Next.Next;
            do
            {
                if (currChunk.Value.x == x && currChunk.Value.y == y) objectFound=true;
            } while ((currChunk = currChunk.Next) != null) ;
        }
        return objectFound;
    }

    public bool snakeAlive()
    {
        int x = snake.First.Value.x;
        int y = snake.First.Value.y;
        isAlive = true;
        LinkedListNode<GameData.SnakeChunk> currChunk = snake.First.Next.Next.Next;
        do
        {
            if (currChunk.Value.x == x && currChunk.Value.y == y) isAlive = false;
        } while ((currChunk = currChunk.Next) != null);
        return isAlive;
    }

    private void spawnFood()
    {
        ArrayList validPoints = new ArrayList();
        for (int wid = 0; wid < playingField.X; wid++)
        {
            for (int hei = 0; hei < playingField.Y; hei++)
            {
                IntPoint pos = new IntPoint { X=wid, Y=hei };
                if (notInSnake(pos))
                {
                    validPoints.Add(pos);
                }
            }
        }

        int chosen = getrandom.Next(0, validPoints.Count - 1);
        foodLocation = (IntPoint)validPoints[chosen];
    }

    private bool notInSnake(IntPoint pos)
    {
        bool temp = true;
        foreach (SnakeChunk sc in snake.ToArray<SnakeChunk>())
        {
            if (sc.x == pos.X && sc.y == pos.Y) temp = false;
        }
        return temp;
    }

    internal void setDirection(SwipeDirection dir)
    {
        switch (dir)
        {
            case SwipeDirection.Left:
                if (direction != Direction.Right) newDirection = Direction.Left;
                break;
            case SwipeDirection.Right:
                if (direction != Direction.Left) newDirection = Direction.Right;
                break;
            case SwipeDirection.Up:
                if (direction != Direction.Down) newDirection = Direction.Up;
                break;
            case SwipeDirection.Down:
                if (direction != Direction.Up) newDirection = Direction.Down;
                break;
        }
    }
}
