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
    public class Node
    {
        public Node next;
        public Rectangle position;
        public Color color;
        public Node prev;

        public Node(Node next, Node prev, Rectangle position)
        {
            this.next = next;
            this.prev = prev;
            this.position = position;
            color = Color.White;
        }
    }

    public class LinkedList
    {
        public Node head;
        public Node tail;
        public Dir headDir;
        public int segments;

        public LinkedList(Node head, Node tail, int segments, Dir hdir)
        {
            this.head = head;
            this.tail = tail;
            this.segments = segments;
            this.headDir = hdir;
        }

        public void AddNode(Node newNode, Color color)
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
            Node temp = head;
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
    }
}
