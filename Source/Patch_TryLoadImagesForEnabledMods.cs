using HarmonyLib;
using SecretHistories.Fucine;
using SecretHistories.Infrastructure.Modding;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[HarmonyPatch(typeof(ModManager))]
public class Patch_TryLoadImagesForEnabledMods
{
    [HarmonyPrefix]
    [HarmonyPatch("TryLoadImagesForEnabledMods", new[] { typeof(ContentImportLog) })]
    static bool TryLoadImagesForEnabledMods(ModManager __instance, Dictionary<string, Sprite> ____images, ContentImportLog log)
    {
        ____images.Clear();
        List<string> imagesFilesList = new List<string>() { ".dds", ".png" };
        //MethodInfo getFilesRecursiveMethod = AccessTools.Method(typeof(ModManager), "GetFilesRecursive", new[] { typeof(string), typeof(string) });
        MethodInfo loadImageMethod = AccessTools.Method(typeof(ModManager), "LoadImage", new[] { typeof(string), typeof(string), typeof(ContentImportLog) });
        foreach (Mod mod in __instance.GetEnabledModsInLoadOrder())
        {
            if (mod.HasValidImageFolder())
            {
                foreach (string text in GetFilesRecursive(mod.ImageFolder, imagesFilesList))
                {
                    //NoonUtility.Log($"DDS get Files: {text}");
                    loadImageMethod.Invoke(__instance, new object[] { mod.ModRootFolder, text, log });
                }
            }
        }
        return false;
    }
    private static List<string> GetFilesRecursive(string path, List<string> extensions)
    {
        List<string> list = new List<string>();
        if (Directory.Exists(path))
        {
            foreach (string subdirectory in Directory.GetDirectories(path))
            {
                list.AddRange(GetFilesRecursive(subdirectory, extensions));
            }

            foreach (string file in Directory.GetFiles(path))
            {
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);
                string ddsFile = Path.Combine(path, fileNameWithoutExtension + ".dds");
                string pngFile = Path.Combine(path, fileNameWithoutExtension + ".png");

                if (File.Exists(ddsFile) && File.Exists(pngFile))
                {
                    if (!list.Contains(ddsFile))
                        list.Add(ddsFile);
                }
                else if (extensions.Any(ext => file.EndsWith(ext)))
                {
                    if (!list.Contains(file))
                        list.Add(file);
                }
            }
        }
        //foreach (var item in list)
        //{
        //    NoonUtility.Log(item);
        //}
        return list;
    }



}
