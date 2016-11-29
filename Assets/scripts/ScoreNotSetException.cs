using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    public class ScoreNotSetException: Exception
    {
        public ScoreNotSetException()
        {
        }

        public ScoreNotSetException(string message)
        : base(message)
        {
        }

        public ScoreNotSetException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
