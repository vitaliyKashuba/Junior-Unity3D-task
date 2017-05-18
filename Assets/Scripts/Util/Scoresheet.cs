using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class Scoresheet
{
    [Serializable]
    public class Node
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
    }

    public ArrayList nodes;

    public Scoresheet()
    {
        nodes = new ArrayList();
    }

    public void addGame(Node game)
    {
        nodes.Add(game);
    }
}
