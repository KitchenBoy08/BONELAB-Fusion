﻿using LabFusion.Network;
using LabFusion.Senders;

using MelonLoader;

using UnityEngine;

namespace LabFusion.Preferences.Client;

public struct ClientSettings
{
    // Selected network layer
    public static FusionPref<string> NetworkLayerTitle { get; internal set; }
    public static FusionPref<int> ProxyPort { get; internal set; }

    // Nametag settings
    public static FusionPref<bool> NametagsEnabled { get; internal set; }
    public static FusionPref<Color> NametagColor { get; internal set; }

    // Nickname settings
    public static FusionPref<string> Nickname { get; internal set; }
    public static FusionPref<NicknameVisibility> NicknameVisibility { get; internal set; }

    public static VoiceChatSettings VoiceChat { get; private set; }

    // Gamemode settings
    public static FusionPref<bool> GamemodeLateJoining { get; internal set; }

    public static DownloadingSettings Downloading { get; private set; }

    public static void OnInitialize(MelonPreferences_Category category)
    {
        // Client settings
        NetworkLayerTitle = new FusionPref<string>(category, "Network Layer Title", NetworkLayerDeterminer.GetDefaultLayer().Title, PrefUpdateMode.IGNORE);
        ProxyPort = new FusionPref<int>(category, "Proxy Port", 28340, PrefUpdateMode.IGNORE);

        // Nametag
        NametagsEnabled = new FusionPref<bool>(category, "Client Nametags Enabled", true, PrefUpdateMode.LOCAL_UPDATE);
        NametagColor = new FusionPref<Color>(category, "Nametag Color", Color.white, PrefUpdateMode.CLIENT_UPDATE);

        // Nickname
        Nickname = new FusionPref<string>(category, "Nickname", string.Empty, PrefUpdateMode.IGNORE);
        NicknameVisibility = new FusionPref<NicknameVisibility>(category, "Nickname Visibility", Senders.NicknameVisibility.SHOW_WITH_PREFIX, PrefUpdateMode.LOCAL_UPDATE);

        VoiceChat = new VoiceChatSettings();
        VoiceChat.CreatePrefs(category);

        // Gamemodes
        GamemodeLateJoining = new FusionPref<bool>(category, "Gamemode Late Joining", true, PrefUpdateMode.IGNORE);

        Downloading = new DownloadingSettings();
        Downloading.CreatePrefs(category);
    }
}
