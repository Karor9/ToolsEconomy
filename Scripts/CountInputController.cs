using Godot;
using System;
using System.Linq;

public partial class CountInputController : LineEdit
{
    double value = 0f;
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
        else if(newValue.EndsWith(","))
            Text = newValue;
        else if(double.TryParse(newValue, out double f))
        {
            value = f;
            Text = value.ToString();
        }
        CaretColumn = Text.Length;
    }
}
