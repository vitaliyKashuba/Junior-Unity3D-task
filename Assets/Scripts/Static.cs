using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Policy;
using UnityEditorInternal;
using UnityEngine;

public static class Static
{
    private const String DEFAULT_NAME = "Player1";
    private static String playerName;// { get; set; } TODO: find out why error here
    private static int score = 0;
    private static Scoresheet scoresheet;

    public static void setName(String name)
    {
        playerName = name;
    }

    public static String getName()
    {
        return playerName ?? DEFAULT_NAME; // nlv(playerName, DEFAULT_NAME)
    }

    public static int getScore()
    {
        return score;
    }

    public static void increaseScore()
    {
        score++;
    }

    public static Scoresheet getScoresheet()
    {
        return scoresheet;
    }

    public static void setScoresheet(Scoresheet sheet)
    {
        scoresheet = sheet;
    }

}
