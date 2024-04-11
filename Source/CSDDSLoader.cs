using System;
using HarmonyLib;
public class CSDDSLoader
{
    public static void Initialise()
    {
        Harmony ddsLoader = new("tinygrox.ddsloader");
        ddsLoader.PatchAll();
        NoonUtility.LogWarning("DDSLoader: Hewaaa!");
    }
}