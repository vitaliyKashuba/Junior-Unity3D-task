using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEditorInternal;
using UnityEngine;

public static class Static
{
    private static String playerName;// { get; set; } TODO: find out why error here
    private static int score = 0;

    public static void setName(String name)
    {
        playerName = name;
    }

    public static String getName()
    {
        return playerName;
    }

    public static int getScore()
    {
        return score;
    }

    public static void increaseScore()
    {
        score++;
    }

}
