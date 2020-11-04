using UnityEngine;
using System.Collections.Generic;

public class ButtonClick : MonoBehaviour
{
    Dictionary<string, string> URLs = new Dictionary<string, string>() {
        { "Twitter", "https://twitter.com/JustinMajetich" },
        { "LinkedIn", "https://www.linkedin.com/in/justin-majetich-43246b192/" },
        { "GitHub", "https://github.com/justinmajetich" },
        { "Email", "mailto:justinmajetich@gmail.com" }
    };

    public void Click(string buttonName) {
        Application.OpenURL(URLs[buttonName]);
    }
}
