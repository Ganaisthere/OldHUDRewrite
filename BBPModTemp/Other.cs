using BepInEx;
using HarmonyLib;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace OldHUDRewrite
{
    public static class Other
    {
        public static string GetModFolder(BaseUnityPlugin plugin)
        {
            return Path.Combine(Application.streamingAssetsPath, "Modded", plugin.Info.Metadata.GUID);
        }

        public static Texture2D Texture2DFromFile(string fileFullPath)
        {
            if (File.Exists(fileFullPath))
            {
                byte[] data = File.ReadAllBytes(fileFullPath);
                Texture2D texture2D = new Texture2D(2, 2);
                texture2D.LoadImage(data);
                texture2D.filterMode = FilterMode.Point;
                texture2D.name = Path.GetFileNameWithoutExtension(fileFullPath);
                return texture2D;
            }
            BasePlugin.Log($"File not found: {fileFullPath}", 2);
            return null;
        }

        public static Sprite SpriteFromTexture2D(Texture2D texture, Vector2 center, float pix = 50f)
        {
            return Sprite.Create(
                texture,
                new Rect(0f, 0f, texture.width, texture.height),
                center,
                pix
                );
        }

        public static Sprite SpriteFromTexture2D(Texture2D texture, float pix = 50f)
        {
            return SpriteFromTexture2D(texture, new Vector2(0.5f, 0.5f), pix);
        }

        public static T FindObjectOfTypeWithName<T>(string name) where T : Object
        {
            return Resources.FindObjectsOfTypeAll<T>().FirstOrDefault(x => x.name == name);
        }

        public static T AccessToolsFieldGetValue<T>(this object obj, string name)
        {
            return (T)AccessTools.Field(obj.GetType(), name).GetValue(obj);
        }

        public static void AccessToolsFieldSetValue(this object obj, string name, object value)
        {
            AccessTools.Field(obj.GetType(), name).SetValue(obj, value);
        }

        public static AudioClip AudioClipFromFile(string fileFullPath)
        {
            if (File.Exists(fileFullPath))
            {
                AudioType type;
                string format = Path.GetExtension(fileFullPath).ToLower();
                switch (format)
                {
                    case ".wav":
                        type = AudioType.WAV;
                        break;
                    case ".mp3":
                        type = AudioType.MPEG;
                        break;
                    case ".ogg":
                        type = AudioType.OGGVORBIS;
                        break;
                    default:
                        type = AudioType.UNKNOWN;
                        break;
                }
                if (type == AudioType.UNKNOWN)
                {
                    BasePlugin.Log($"Not suitable audio format ({format}), Only support .wav/.mp3/.ogg. Path: {fileFullPath}", 2);
                    return null;
                }
                string path = fileFullPath;
                if (path.Substring(0, 8) != "file:///")
                {
                    path = "file:///" + fileFullPath;
                }
                UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(path, type);
                request.SendWebRequest();
                while (!request.isDone) { }
                AudioClip clip = DownloadHandlerAudioClip.GetContent(request);
                clip.name = Path.GetFileNameWithoutExtension(fileFullPath);
                return clip;
            }
            BasePlugin.Log($"File not found: {fileFullPath}", 2);
            return null;
        }

        public static SoundObject SoundObjectFromAudioClip(AudioClip audioClip, Color color, string subtitle = "Nothing", SoundType soundType = SoundType.Voice, float subtitleLength = -1f)
        {
            if (audioClip == null)
            {
                BasePlugin.Log("AudioClip can't be null!!!", 2);
                return null;
            }
            SoundObject soundObject = ScriptableObject.CreateInstance<SoundObject>();
            soundObject.soundClip = audioClip;
            soundObject.name = audioClip.name;
            soundObject.soundKey = subtitle;
            if (subtitle == "Nothing" || subtitleLength == 0f)
            {
                soundObject.subtitle = false;
            }
            else
            {
                soundObject.color = color;
                if (subtitleLength == -1f)
                {
                    soundObject.subDuration = audioClip.length;
                }
                else
                {
                    soundObject.subDuration = subtitleLength;
                }
            }
            soundObject.soundType = soundType;
            return soundObject;
        }
    }
}
