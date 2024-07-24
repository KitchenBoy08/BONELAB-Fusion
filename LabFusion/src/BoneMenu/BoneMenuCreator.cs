﻿using LabFusion.Preferences;
using LabFusion.Player;
using LabFusion.Network;

using UnityEngine;

using BoneLib.BoneMenu;

namespace LabFusion.BoneMenu;

public static partial class BoneMenuCreator
{
    public static void RemoveEmptyPage(Page parent, Page child, Element link)
    {
        if (child.Elements.Count <= 0)
        {
            parent.Remove(link);
        }
    }

    #region MENU CATEGORIES
    public static void CreateColorPreference(Page page, IFusionPref<Color> pref)
    {
        var currentColor = pref;
        var colorR = page.CreateFloat("Red", Color.red, 0.05f, currentColor.GetValue().r, 0f, 1f, (r) =>
        {
            r = Mathf.Round(r * 100f) / 100f;
            var color = currentColor.GetValue();
            color.r = r;
            currentColor.SetValue(color);
        });
        var colorG = page.CreateFloat("Green", Color.green, 0.05f, currentColor.GetValue().g, 0f, 1f, (g) =>
        {
            g = Mathf.Round(g * 100f) / 100f;
            var color = currentColor.GetValue();
            color.g = g;
            currentColor.SetValue(color);
        });
        var colorB = page.CreateFloat("Blue", Color.blue, 0.05f, currentColor.GetValue().b, 0f, 1f, (b) =>
        {
            b = Mathf.Round(b * 100f) / 100f;
            var color = currentColor.GetValue();
            color.b = b;
            currentColor.SetValue(color);
        });
        var colorPreview = page.CreateFunction("■■■■■■■■■■■", currentColor.GetValue(), null);

        currentColor.OnValueChanged += (color) =>
        {
            colorR.Value = color.r;
            colorG.Value = color.g;
            colorB.Value = color.b;
            colorPreview.ElementColor = color;
        };
    }

    public static void CreateBytePreference(Page page, string name, byte increment, byte minValue, byte maxValue, IFusionPref<byte> pref)
    {
        var element = page.CreateInt(name, Color.white, increment, pref.GetValue(), minValue, maxValue, (v) =>
        {
            pref.SetValue((byte)v);
        });

        pref.OnValueChanged += (v) =>
        {
            element.Value = v;
        };
    }

    public static void CreateFloatPreference(Page page, string name, float increment, float minValue, float maxValue, IFusionPref<float> pref)
    {
        var element = page.CreateFloat(name, Color.white, increment, pref.GetValue(), minValue, maxValue, (v) =>
        {
            pref.SetValue(v);
        });

        pref.OnValueChanged += (v) =>
        {
            element.Value = v;
        };
    }

    public static void CreateBoolPreference(Page page, string name, IFusionPref<bool> pref)
    {
        var element = page.CreateBool(name, Color.white, pref.GetValue(), (v) =>
        {
            pref.SetValue(v);
        });

        pref.OnValueChanged += (v) =>
        {
            element.Value = v;
        };
    }

    public static void CreateEnumPreference<TEnum>(Page page, string name, IFusionPref<TEnum> pref) where TEnum : Enum
    {
        var element = page.CreateEnum(name, Color.white, pref.GetValue(), (v) =>
        {
            pref.SetValue((TEnum)v);
        });

        pref.OnValueChanged += (v) =>
        {
            element.Value = v;
        };
    }

    public static void CreateStringPreference(Page page, string name, IFusionPref<string> pref, Action<string> onValueChanged = null, int maxLength = PlayerIdManager.MaxNameLength)
    {
        string currentValue = pref.GetValue();
        var element = page.CreateString(name, Color.white, currentValue, (v) =>
        {
            pref.SetValue(v);
        });
        
        pref.OnValueChanged += (v) =>
        {
            element.Value = v;

            onValueChanged?.Invoke(v);
        };
    }
    #endregion

    private static Page _mainPage = null;

    public static void OnPrepareMainPage()
    {
        _mainPage = Page.Root.CreatePage("Fusion", Color.white);
    }

    public static void OpenMainPage()
    {
        Menu.OpenPage(_mainPage);
    }

    private static int _lastIndex;

    public static void OnPopulateMainPage()
    {
        // Clear page
        _mainPage.RemoveAll();

        // Create category for changing network layer
        var networkLayerManager = _mainPage.CreatePage("Network Layer Manager", Color.yellow);
        var func = networkLayerManager.CreateFunction("Players need to be on the same layer!", Color.yellow, null);
        
        _lastIndex = NetworkLayer.SupportedLayers.IndexOf(NetworkLayerDeterminer.LoadedLayer);

        networkLayerManager.CreateFunction($"Active Layer: {NetworkLayerDeterminer.LoadedTitle}", Color.white, null);
        
        var targetPanel = networkLayerManager.CreatePage($"Target Layer: {FusionPreferences.ClientSettings.NetworkLayerTitle.GetValue()}", Color.white);
        targetPanel.CreateFunction("Cycle", Color.white, () =>
        {
            int count = NetworkLayer.SupportedLayers.Count;
            if (count <= 0)
                return;

            _lastIndex++;
            if (count <= _lastIndex)
                _lastIndex = 0;

            FusionPreferences.ClientSettings.NetworkLayerTitle.SetValue(NetworkLayer.SupportedLayers[_lastIndex].Title);
        });
        FusionPreferences.ClientSettings.NetworkLayerTitle.OnValueChanged += (v) =>
        {
            targetPanel.Name = $"Target Layer: {v}";
        };

        targetPanel.CreateFunction("SET NETWORK LAYER", Color.green, () => InternalLayerHelpers.UpdateLoadedLayer());

        // Setup bonemenu for the network layer
        InternalLayerHelpers.OnSetupBoneMenuLayer(_mainPage);
    }

    public static void CreateUniversalMenus(Page page)
    {
        CreateGamemodesMenu(page);
        CreateSettingsMenu(page);
        CreateNotificationsMenu(page);
        CreateBanListMenu(page);
        CreateDownloadingMenu(page);

#if DEBUG
        // Debug only (dev tools)
        CreateDebugMenu(page);
#endif
    }
}