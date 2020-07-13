using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Utility
{
    public static ColorBlock ButtonColor(bool isPressed)
    {
        ColorBlock colorBlock = new ColorBlock();

        colorBlock.normalColor = new Color(1f, 1f, 1f);
        colorBlock.highlightedColor = new Color(1f, 1f, 1f);
        colorBlock.pressedColor = new Color(0.6f, 0.6f, 0.6f);
        if (!isPressed) // If true button will fade to black when pressed properly
            colorBlock.selectedColor = new Color(1f, 1f, 1f);

        colorBlock.colorMultiplier = 1f;
        colorBlock.fadeDuration = 0.35f;

        return colorBlock;
    }
}
