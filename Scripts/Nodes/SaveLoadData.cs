using Godot;
using Godot.Collections;
using System;
using System.IO;
using System.Reflection.Metadata;
using System.Runtime.Serialization;

public partial class SaveLoadData : CanvasLayer
{
    [Export] Node ElementParent;
    [Export] Node GeneratorParent;
    [Export] Node CraftingParent;
    static string newPartString ="%%%NEW SECTION%%%";
    static string path = ProjectSettings.GlobalizePath("user://") + "save.sav";

    void Load()
    {
        string[] saveDataArray = File.ReadAllLines(path);
        int sectionId = -1;
        foreach (string item in saveDataArray)
        {
            if(item.Length < 2)
                continue;
            if(item == newPartString)
                sectionId += 1;
            GD.Print(item);
            string[] line = item.Split(";");
            if(line.Length <= 1)
                continue;
            switch(sectionId)
            {
                case 0:
                    Goods good;
                    ElementController ec = (ElementController)Globals.Instance.Good.Instantiate();
                    ec.Name = line[0];
                    string[] pos = line[1].Replace("(", "").Replace(")", "").Split(",");
                    ec.Position = new Vector2(float.Parse(pos[0].Replace(".", ",")), float.Parse(pos[1].Replace(".", ",")));
                    Globals.Instance.GoodParent.AddChild(ec);
                    good = new Goods(line[3], double.Parse(line[2]), ec);
                    Globals.Instance.Goods.Add(int.Parse(line[0]), good);
                    ec.UpdateText();
                    break;
            }
        }
    }

    void Save()
    {
        string output = ""; 
        output += newPartString + "\n";
        output += SaveElements();
        // TestParent(ElementParent);
        // TestParent(GeneratorParent);
        // TestParent(CraftingParent);
        GD.Print(path);
        File.WriteAllText(path, output);
    }

    // void TestParent(Node node)
    // {
    //     foreach (Node item in node.GetChildren())
    //     {
    //         if(item == GeneratorParent || item == CraftingParent || item == ElementParent)
    //             continue;
    //     };
    // }

    // string SaveGoods()
    // {
    //     string output = "";

    //     foreach (int item in Globals.Instance.Goods.Keys)
    //     {
    //         Goods g = Globals.Instance.Goods[item];

    //         output += item + ";" + g.Name + ";" + g.Count;
    //         output += "\n";
    //     }

    //     output += newPartString + "\n";
    //     return output;
    // }

    string SaveElements()
    {
        string result = "";
        foreach (Node item in ElementParent.GetChildren())
        {
            if(item == GeneratorParent || item == CraftingParent)
                continue;
            ElementController ec = (ElementController)item;
            Goods g = Globals.Instance.Goods[int.Parse(ec.Name)];
            result += ec.Name + ";" + ec.Position + ";" + g.Count + ";" + g.Name + "\n";

            foreach (Arrow arrow in ec.InLine)
            {
                result += arrow.Name + ";";
            }
            result += "\n";
            foreach (Arrow arrow in ec.OutLine)
            {
                result += arrow.Name + ";";
            }
        }
        result += newPartString + "\n";
        return result;
    }
}
