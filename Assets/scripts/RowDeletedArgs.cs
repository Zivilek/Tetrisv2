using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class RowDeletedArgs: EventArgs
{
    public int Count { get; set; }
    public RowDeletedArgs(int count)
    {
        Count = count;
    }
}
