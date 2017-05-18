using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class XMLUtil
{
    private const String FILENAME = "Highscores.xml";

    public static void writeData(Scoresheet scoresheet) //TODO: replace with serialization?
    {
        if (!File.Exists(FILENAME))
        {
            createFile();
        }

        XmlDocument doc = new XmlDocument();
        doc.Load(FILENAME);

        XmlElement gameNode = doc.CreateElement("game");
        doc.DocumentElement.AppendChild(gameNode);

        XmlElement name = doc.CreateElement("name");    // TODO: simplify if possible
        XmlElement score = doc.CreateElement("score");
        XmlElement time = doc.CreateElement("time");
        XmlElement date = doc.CreateElement("date");
        XmlElement status = doc.CreateElement("status");

        name.InnerText = scoresheet.getName();
        score.InnerText = scoresheet.getScore().ToString();
        time.InnerText = scoresheet.getTime().ToString();
        date.InnerText = scoresheet.getDate().ToString();
        status.InnerText = scoresheet.getExitStatus().ToString();

        gameNode.AppendChild(name);
        gameNode.AppendChild(score);
        gameNode.AppendChild(time);
        gameNode.AppendChild(date);
        gameNode.AppendChild(status);

        doc.Save(FILENAME);
    }

    public static void createFile()
    {
        XmlTextWriter textWritter = new XmlTextWriter(FILENAME, null);
        textWritter.WriteStartDocument();
        textWritter.WriteStartElement("root");
        textWritter.WriteEndElement();
        textWritter.Close();
    }
}
