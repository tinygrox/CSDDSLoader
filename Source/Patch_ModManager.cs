using HarmonyLib;
using SecretHistories.Fucine;
using SecretHistories.Infrastructure.Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[HarmonyPatch(typeof(ModManager))]
public class Patch_ModManager
{
    [HarmonyPrefix]
    [HarmonyPatch("LoadSprite", new[] { typeof(string) })]
    static bool Patch_LoadSprite(string imagePath, ref Sprite __result)
    {
        __result = DDS_Utility.LoadSprite(imagePath);
        return false;
    }

}