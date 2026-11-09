using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using OldHUDRewrite.Patches;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace OldHUDRewrite
{
    [BepInPlugin("ganaisthere.plus.oldhudrewrite", "Old HUD - Rewrited", "0.1.0.0")]
    public class BasePlugin : BaseUnityPlugin
    {
        public static BasePlugin Instance { get; internal set; }
        public static AssetManagerIThink assetMan = new AssetManagerIThink();
        public static bool localizationLoaded = false;

        //----------------------------------------

        public ConfigEntry<StaminometerStyle>
            ConfigStaminometerStyle;

        public ConfigEntry<bool>
            ConfigItemUsingAnimations,
            ConfigOldEventNotification,
            ConfigOldYTPsDisplay,
            ConfigNoHandOnlyReticle,
            ConfigNoItemSliderAnimation,
            ConfigFGPDNothingItemIcon,
            ConfigBiggerItemIcon,
            ConfigBiggerItemText,
            ConfigRemoveNotebookIcon,
            ConfigOldNotebooksText;

        internal void Awake()
        {
            ConfigItemUsingAnimations = Config.Bind
            (
                "General",
                "Item Using Animations",
                true,
                "If true, Item Using Animations."
            );
            ConfigStaminometerStyle = Config.Bind
            (
                "General",
                "StaminometerStyle",
                StaminometerStyle.PlusV01,
                @"The Staminometer Style in the HUD.
- Classic: The stamina did not have the ""STAMINOMETER"" texture and it was only a plain bar, and the stamina meter was located at the top middle;
- PlusV01: Same as Classic, but the stamina meter was located at the bottom left corner of the screen;
- PlusV02: The stamina was featured as a green or red bar;
- VanillaNoAnimation: Similar to Vanilla, but without additional transition animations;
- Vanilla: No change, just use vanilla game's texture and animations."
            );
            ConfigOldEventNotification = Config.Bind
            (
                "General",
                "Old Event Notification",
                true,
                "If true, Old Event Notification."
            );
            ConfigOldYTPsDisplay = Config.Bind
            (
                "General",
                "Old YTPs Display",
                true,
                "If true, Old YTPs Display."
            );
            ConfigNoHandOnlyReticle = Config.Bind
            (
                "General",
                "No Hand Only Reticle",
                true,
                "If true, Reticle will not show a hand when something is front you."
            );
            ConfigNoItemSliderAnimation = Config.Bind
            (
                "General",
                "No Item Slider Animation",
                true,
                "If true, Inventory's item slider animation will be disabled."
            );
            ConfigFGPDNothingItemIcon = Config.Bind
            (
                "General",
                "FGPD Nothing ItemIcon",
                true,
                "If true, Inventory's \'Nothing\' item icon will use \'Item\' sprite from FGPD."
            );
            ConfigBiggerItemIcon = Config.Bind
            (
                "General",
                "Bigger Item Icon",
                true,
                "If true, Inventory's item icon will use large icon."
            );
            ConfigBiggerItemText = Config.Bind
            (
                "General",
                "Bigger Item Text",
                true,
                "If true, Not diet bsoda, Will DIET BSODA."
            );
            ConfigRemoveNotebookIcon = Config.Bind
            (
                "General",
                "Remove Notebook Icon",
                true,
                "If true, Not (NotebookIcon) 0/0, Only 0/0."
            );
            ConfigOldNotebooksText = Config.Bind
            (
                "General",
                "Old Notebooks Text",
                true,
                "If true, 0/0 => 0/0 Notebooks."
            );

            Instance = this;
            new Harmony(Info.Metadata.GUID).PatchAll();
            LocalizationManagerPatches.prepareLocalizationPaths = new List<string>
            {
                Path.Combine(Other.GetModFolder(this), "Subtitles_English.json")
            };
            AddTexture2D("PlaceHolder.png", "Textures");
            AddAudioClip("EventRing.ogg", "SoundObjects", true, "Sfx_EventRing");
        }

        internal AudioClip AddAudioClip(string fileFullName, string chlidPath = null, bool btwSoundObject = true, string subtitle = "Nothing")
        {
            string fileFullPath;
            if (chlidPath == null || chlidPath == "")
            {
                fileFullPath = Path.Combine(Other.GetModFolder(this), fileFullName);
            }
            else
            {
                fileFullPath = Path.Combine(Other.GetModFolder(this), chlidPath, fileFullName);
            }
            if (File.Exists(fileFullPath))
            {
                AudioClip audioClip = Other.AudioClipFromFile(fileFullPath);
                assetMan.Add(Path.GetFileNameWithoutExtension(fileFullPath), audioClip);
                if (btwSoundObject)
                {
                    assetMan.Add(Path.GetFileNameWithoutExtension(fileFullPath), Other.SoundObjectFromAudioClip(audioClip, Color.white, subtitle));
                }
                return audioClip;
            }
            Log($"File not found: {fileFullPath}", 2);
            return null;
        }

        internal Texture2D AddTexture2D(string fileFullName, string chlidPath = null, bool btwSprite = true, float pix = 50f)
        {
            string fileFullPath;
            if (chlidPath == null || chlidPath == "")
            {
                fileFullPath = Path.Combine(Other.GetModFolder(this), fileFullName);
            }
            else
            {
                fileFullPath = Path.Combine(Other.GetModFolder(this), chlidPath, fileFullName);
            }
            if (File.Exists(fileFullPath))
            {
                Texture2D texture2D = Other.Texture2DFromFile(fileFullPath);
                assetMan.Add(Path.GetFileNameWithoutExtension(fileFullPath), texture2D);
                if (btwSprite)
                {
                    Sprite sprite = Other.SpriteFromTexture2D(texture2D, pix);
                    assetMan.Add(Path.GetFileNameWithoutExtension(fileFullPath), sprite);
                }
                return texture2D;
            }
            Log($"File not found: {fileFullPath}", 2);
            return null;
        }

        internal static void Log(object data, int level = 0)
        {
            if (level == 1)
            {
                Instance.Logger.LogWarning(data);
            }
            else if (level == 2)
            {
                Instance.Logger.LogError(data);
            }
            else
            {
                Instance.Logger.LogInfo(data);
            }
        }
    }

    public enum StaminometerStyle
    {
        Classic,
        PlusV01,
        PlusV02,
        VanillaNoAnimation,
        Vanilla
    }
}
