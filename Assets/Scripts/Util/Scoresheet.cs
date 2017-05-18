using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class Scoresheet
{
    [Serializable]
    public class Node : IComparable
    {
        public String name { get; set; } //TODO: fix getters
        public int score { get; set; }
        public String time { get; set; }    // TimeSpan cannot be serialized
        public DateTime date { get; set; }
        public ExitStatus exitStatus { get; set; }

        public Node(String name, int score, TimeSpan time, DateTime date, ExitStatus exitStatus)
        {
            this.name = name;
            this.score = score;
            this.time = time.ToString();
            this.date = date;
            this.exitStatus = exitStatus;
        }

        public Node() { }

        public int CompareTo(object obj)
        {
            if (obj.GetType().Equals(this.GetType()))
            {
                Node n = (Node)obj;
                return n.date.CompareTo(date);
            }
            else
            {
                throw new InvalidOperationException("uncomparable objects, check types");
            }
        }

        public override String ToString()
        {
            return name + " | " + score + " | " + time + " | " + date.ToString() + " | " + exitStatus.ToString();
        }
    }

    public ArrayList nodes { get; }

    public Scoresheet()
    {
        nodes = new ArrayList();
    }

    public void addGame(Node game)
    {
        nodes.Add(game);
        nodes.Sort();
    }
}
