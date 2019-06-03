using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkList<T>
{
    public ListNode<T> Head { set; get; }

    public LinkList()
    {
        Head = null;
    }

    public void Append(T item)
    {
        ListNode<T> foot = new ListNode<T>(item);
        ListNode<T> A = new ListNode<T>();

        if(Head == null)
        {
            Head = foot;
            return;
        }
        A = Head;
        while(A.Next != null)
        {
            A = A.Next;
        }
        A.Next = foot;
    }

    public void Remove(int i)
    {
        ListNode<T> A = new ListNode<T>();
        if(i == 0)
        {
            A = Head;
            Head = Head.Next;
            return;
        }
        ListNode<T> B = new ListNode<T>();
        B = Head;
    }
}
