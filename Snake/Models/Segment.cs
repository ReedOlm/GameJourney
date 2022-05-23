using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snake.Models
{
    public enum Dir
    {
        Up,
        Right,
        Down,
        Left
    }
    public class Segment
    {
        public Segment next;
        public Rectangle position;
        public Color color;
        public Segment prev;

        public Segment(Segment next, Segment prev, Rectangle position)
        {
            this.next = next;
            this.prev = prev;
            this.position = position;
            color = Color.White;
        }
    }

    public class Snake
    {
        public Segment head;
        public Segment tail;
        public Dir headDir;
        public int segments;

        public Snake(Segment head, Segment tail, int segments, Dir hdir)
        {
            this.head = head;
            this.tail = tail;
            this.segments = segments;
            this.headDir = hdir;
        }

        public void AddNode(Segment newNode, Color color)
        {
            tail.next = newNode;
            newNode.prev = tail;
            newNode.next = null;
            tail = newNode;
            segments++;
            newNode.color = color;
        }

        public Rectangle ShiftSegments(Rectangle goalPos)
        {
            Segment temp = head;
            Rectangle prevPos;
            for(int i = 0; i < segments; i++)
            {
                temp = temp.next;
                prevPos = temp.position;
                temp.position = goalPos;
                goalPos = prevPos;
            }
            return goalPos;
        }

        public bool DetectCollisions()
        {
            Segment temp = head.next;
            for(int i = 1; i < segments; i++)
            {
                if (head.position.Intersects(temp.position))
                    return true;
                temp = temp.next;
            }
            if (head.position.Intersects(tail.position))
                return true;
            return false;
        }
    }
}
