using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Language.Lua;
using UnityEngine;
using UnityEngine.Bindings;
using UnityEngine.UI;

namespace SmugglersPack.Patches
{
    public class ItemUIPatches
    {
        public static string OverlayBackSafeSteal = "OverlayBackSafeSteal";

        public static Sprite? GreenStolenIcon;
        public static void Load()
        {
            GreenStolenIcon = Lavender.RuntimeImporter.ImageLoader.LoadSprite(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "thief_mask.png"));
        }

        private static Transform? FindChildRecursive(Transform parent, string name)
        {
            var child = parent.Find(name);
            if (child != null)
            {
                return child;
            }

            for (var i = 0; i < parent.childCount; i++)
            {
                var ret = FindChildRecursive(parent.GetChild(i), name);
                if (ret != null)
                {
                    return ret;
                }
            }

            return null;
        }

        private static bool LoggedBackgroundCreateFailed = false;

        [HarmonyPatch(typeof(Slot), nameof(Slot.Start))]
        [HarmonyPostfix]
        public static void Slot_Start_Postfix(Slot __instance)
        {
            if (FindChildRecursive(__instance.gameObject.transform, OverlayBackSafeSteal) != null)
            {
                // If the overlay already exists, then early out
                return;
            }

            var StealSafeOverlay = new GameObject(OverlayBackSafeSteal);
            RectTransform recttransform = StealSafeOverlay.AddComponent<RectTransform>();
            CanvasRenderer canvasrenderer = StealSafeOverlay.AddComponent<CanvasRenderer>();
            UnityEngine.UI.Image img = StealSafeOverlay.AddComponent<UnityEngine.UI.Image>();

            Transform? PaddingTransform = FindChildRecursive(__instance.gameObject.transform, "Padding");

            if (PaddingTransform != null)
            {
                RectTransform backgroundrect = (RectTransform)PaddingTransform.GetChild(0);

                recttransform.SetParent(PaddingTransform, false);
                recttransform.SetSiblingIndex(1); // Push as far down the z-order as we can get while still being in front of the background itself
                recttransform.anchoredPosition = backgroundrect.anchoredPosition;
                recttransform.anchorMax = backgroundrect.anchorMax;
                recttransform.anchorMin = backgroundrect.anchorMin;
                recttransform.offsetMax = backgroundrect.offsetMax;
                recttransform.offsetMin = backgroundrect.offsetMin;
                recttransform.ForceUpdateRectTransforms();


                img.sprite = GreenStolenIcon;
                img.color = Plugin.Settings.HiddenSlotColor.Value;

                StealSafeOverlay.SetActive(false); // overlay is disabled by default
            }
            else
            {
                GameObject.Destroy(StealSafeOverlay);
                if (!LoggedBackgroundCreateFailed)
                {
                    Plugin.Log.LogWarning($"Failed to insert protected overlay into Slot {__instance.name}");
                    LoggedBackgroundCreateFailed = true;
                }
            }
        }

        [HarmonyPatch(typeof(Slot), nameof(Slot.UpdateSlot))]
        [HarmonyPostfix]
        public static void Slot_UpdateSlot_Postfix(Slot __instance)
        {
            if (__instance.ItemInSlot != null)
            {
                ItemData? itemdata = __instance.GetItemItemData();
                if (itemdata != null)
                {
                    Image img = itemdata.stolenImage.GetComponent<Image>();
                    if (img != null)
                    {
                        img.color = SmugglerPackController.IsProtectedSlot(__instance.slotcontrol) ? Plugin.Settings.ProtectedItemStolenColor.Value : new Color(1, 1, 1, 1);
                    }
                }
            }

            if (__instance.slotcontrol != null)
            {
                if (SmugglerPackController.IsProtectedSlot(__instance.slotcontrol))
                {
                    Transform? overlay = FindChildRecursive(__instance.transform, OverlayBackSafeSteal);
                    if (overlay != null)
                    {
                        overlay.gameObject.SetActive(true);
                    }
                }
                else
                {
                    Transform? overlay = FindChildRecursive(__instance.transform, OverlayBackSafeSteal);
                    if (overlay != null)
                    {
                        overlay.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
