using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "camp-lilac/Dialogue", order = 0)]

[System.Serializable]

// dialogue class sets the parameters for how big the text box is
// in the game manager and how many words will fit on the page
public class Dialogue: ScriptableObject
{
    public string name;

    [TextArea(3, 10)]
    // list of dialogue elements that we will cycle through in the dialogue manager
    public string[] sentences;
}
