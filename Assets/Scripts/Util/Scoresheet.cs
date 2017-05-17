using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Scoresheet
{
    private String name;// { get; } //TODO: fix getters
    private int score;// { get; }
    private TimeSpan time;
    private DateTime date;
    private ExitStatus exitStatus;

    public Scoresheet(String name, int score, TimeSpan time, DateTime date, ExitStatus exitStatus)
    {
        this.name = name;
        this.score = score;
        this.time = time;
        this.date = date;
        this.exitStatus = exitStatus;
    }

    public String getName()
    {
        return name;
    }

    public int getScore()
    {
        return score;
    }

    public TimeSpan getTime()
    {
        return time;
    }

    public DateTime getDate()
    {
        return date;
    }

    public ExitStatus getExitStatus()
    {
        return exitStatus;
    }

}
