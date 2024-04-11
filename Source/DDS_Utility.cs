using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public static class DDS_Utility
{
    public static Sprite LoadSprite(string imagePath, bool readable = false)
    {
        imagePath = Path.GetFullPath(imagePath);
        Texture2D texture2D = null;
        try
        {
            string ddsExtensionPath = Path.ChangeExtension(imagePath, ".dds");
            if (File.Exists(ddsExtensionPath))
            {
                //NoonUtility.LogWarning($"-------dds find{ddsExtensionPath}");
                texture2D = DDSLoader.LoadDDS(ddsExtensionPath);
                if (!string.IsNullOrEmpty(DDSLoader.error))
                    NoonUtility.LogWarning($"DDS ERROR at '{imagePath}': {DDSLoader.error}");

                if (texture2D == null)
                    NoonUtility.LogWarning($"Couldn't load .dds from {imagePath} loading as png instead.");
            }

            if (texture2D == null && File.Exists(imagePath))
            {
                byte[] data = File.ReadAllBytes(imagePath);
                texture2D = new Texture2D(2, 2, TextureFormat.Alpha8, true); // minimap can be a setting for users
                texture2D.LoadImage(data);
            }

            if (texture2D == null)
            {
                return null;
            }

            //texture2D.Compress(true);
            texture2D.name = Path.GetFileNameWithoutExtension(imagePath);
            texture2D.filterMode = FilterMode.Trilinear; // Filter Mode can be a setting for users
            texture2D.anisoLevel = 9; // anisolevel can be a setting for users
            texture2D.mipMapBias = -0.5f; // can be a setting for users
            texture2D.Apply();
        }
        catch (Exception ex)
        {
            NoonUtility.LogWarning($"DDSLoader: {(imagePath ?? "Missing file?")} - {ex}");
        }
        return Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
    }
}
