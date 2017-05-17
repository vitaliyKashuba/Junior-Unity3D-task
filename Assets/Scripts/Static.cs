using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Static
{
    static String name;// { get; set; }

    public static void setName(String name)
    {
        Static.name = name;
    }

    public static String getName()
    {
        return name;
    }

}
