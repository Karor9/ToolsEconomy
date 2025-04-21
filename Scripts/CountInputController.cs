using Godot;
using System;
using System.Linq;
using System.Text.RegularExpressions;

public partial class CountInputController : InputController
{
    double value = 0f;

    [Export] RichTextLabel richTextLabel;

    [Signal]
    public delegate void OnCountChangeEventHandler(LineEdit le);

    void OnTextChanged(string newValue)
    {
        if(newValue.Contains("."))
            newValue = newValue.Replace(".", ",");
        if(newValue == "")
        {
            value = 0;
            Text = value.ToString();
        }
        else if(newValue.EndsWith(",") && newValue.Count(",") <= 1)
            Text = newValue;
        else if(double.TryParse(newValue, out double f))
        {
            value = f;
            Text = value.ToString();
        } else if(newValue.Count(",") > 1)
        {
            Text = CommaRemover(newValue);
        } 
        else 
        {
            Text = Regex.Replace(newValue, @"[^0-9,]", "");
        }
        
        CaretColumn = Text.Length;
    }

    public override void SaveEdits(ElementController ec)
    {
        base.SaveEdits(ec);
        if(newValue.EndsWith(","))
            newValue += 0;
        if(double.TryParse(newValue, out double parsed))
            g.Count = parsed;
        ((RichTextLabel)ec.GetChild(2)).Text = Utils.FormatNumbers(g.Count);

    }

    public override void SaveEdits(GeneratorController ec)
    {
        base.SaveEdits(ec);
        if(newValue.EndsWith(","))
            newValue += 0;
        if(double.TryParse(newValue, out double parsed))
            ((RichTextLabel)ec.GetChild(2)).Text = Utils.FormatNumbers(parsed);
        ((Control)ec.GetChild(4)).Visible = false;
        ((Control)ec.GetChild(5)).Visible = false;
    }

    string CommaRemover(string newValue)
    {
        string cleaned = newValue;
        int firstComma = cleaned.IndexOf(',');
        if (firstComma != -1)
        {
            // usuń wszystkie kolejne przecinki
            string before = cleaned.Substring(0, firstComma + 1);
            string after = cleaned.Substring(firstComma + 1).Replace(",", "");
            cleaned = before + after;
        }
        return cleaned;
    }

    public void OnLeaveFocusText()
    {
        richTextLabel.Text = Text;
        Visible = false;

        Node node = GetParent()?.GetParent(); // Zapewniamy, że nie jest null

        if (node is GeneratorController gc)
        {
            SaveEdits(gc);
        }
        else if (node is ElementController ec)
        {
            SaveEdits(ec);
        }
        else
        {
            GD.PrintErr($"OnLeaveFocusText: Parent is neither GeneratorController nor ElementController. Got: {node?.GetType().Name}");
        }
    }


    public void OnEnterFocus()
    {
        Text = richTextLabel.Text;
        Visible = true;
    }
}
