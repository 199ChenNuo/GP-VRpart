using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListNode<T>
{
    public T Data { set; get; }
    public ListNode<T> Next { set; get; }

    public ListNode(T item)
    {
        this.Data = item;
        this.Next = null;
    }

    public ListNode()
    {
        this.Data = default(T);
        this.Next = null;
    }
}
