using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReSkinAnimation : MonoBehaviour
{
    public string spriteSheetName;

    private void LateUpdate()
    {
        var subSprites = Resources.LoadAll<Sprite>("Characters/" + spriteSheetName);

        foreach (var renderer in GetComponentsInChildren<SpriteRenderer>())
        {
            string spriteNAme = renderer.sprite.name;

            string npcName = spriteSheetName.Substring(0, 4);
            string lastNumebrs = spriteNAme.Substring(4, spriteNAme.Length -4);
            string rightName = spriteSheetName + lastNumebrs;
            var newSprite = Array.Find(subSprites, item => item.name == rightName);
            if (newSprite)
            {
                 renderer.sprite = newSprite;
            }
        }
    }
}
