﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BoneLib;

using LabFusion.Data;

using UnityEngine;

namespace LabFusion.Utilities {
    internal static class FusionContentLoader {
        public static AssetBundle ContentBundle { get; private set; }

        public static GameObject PointShopPrefab { get; private set; }

        public static Texture2D SabrelakeLogo { get; private set; }
        public static Texture2D LavaGangLogo { get; private set; }

        public static AudioClip SyntheticCavernsRemix { get; private set; }
        public static AudioClip WWWWonderLan { get; private set; }
        public static AudioClip SicklyBugInitiative { get; private set; }

        public static AudioClip LavaGangVictory { get; private set; }
        public static AudioClip SabrelakeVictory { get; private set; }

        public static AudioClip LavaGangFailure { get; private set; }
        public static AudioClip SabrelakeFailure { get; private set; }

        public static AudioClip UISelect { get; private set; }
        public static AudioClip UIDeny { get; private set; }
        public static AudioClip UIConfirm { get; private set; }

        public static AudioClip PurchaseFailure { get; private set; }
        public static AudioClip PurchaseSuccess { get; private set; }

        public static AudioClip EquipItem { get; private set; }

        private static readonly string[] _combatSongNames = new string[4] {
            "music_FreqCreepInModulationBuggyPhysics",
            "music_SicklyBugInitiative",
            "music_SyntheticCavernsRemix",
            "music_WWWonderlan",
        };

        private static readonly List<AudioClip> _combatPlaylist = new List<AudioClip>();
        public static AudioClip[] CombatPlaylist => _combatPlaylist.ToArray();

        public static void OnBundleLoad() {
            ContentBundle = FusionBundleLoader.LoadAssetBundle(ResourcePaths.ContentBundle);

            if (ContentBundle != null) {
                PointShopPrefab = ContentBundle.LoadPersistentAsset<GameObject>(ResourcePaths.PointShopPrefab);

                SabrelakeLogo = ContentBundle.LoadPersistentAsset<Texture2D>(ResourcePaths.SabrelakeLogo); 
                LavaGangLogo = ContentBundle.LoadPersistentAsset<Texture2D>(ResourcePaths.LavaGangLogo);

                foreach (var song in _combatSongNames) {
                    _combatPlaylist.Add(ContentBundle.LoadPersistentAsset<AudioClip>(song));
                }

                LavaGangVictory = ContentBundle.LoadPersistentAsset<AudioClip>(ResourcePaths.LavaGangVictory);
                SabrelakeVictory = ContentBundle.LoadPersistentAsset<AudioClip>(ResourcePaths.SabrelakeVictory);

                LavaGangFailure = ContentBundle.LoadPersistentAsset<AudioClip>(ResourcePaths.LavaGangFailure);
                SabrelakeFailure = ContentBundle.LoadPersistentAsset<AudioClip>(ResourcePaths.SabrelakeFailure);

                UISelect = ContentBundle.LoadPersistentAsset<AudioClip>(ResourcePaths.UISelect);
                UIDeny = ContentBundle.LoadPersistentAsset<AudioClip>(ResourcePaths.UIDeny);
                UIConfirm = ContentBundle.LoadPersistentAsset<AudioClip>(ResourcePaths.UIConfirm);

                PurchaseFailure = ContentBundle.LoadPersistentAsset<AudioClip>(ResourcePaths.PurchaseFailure);
                PurchaseSuccess = ContentBundle.LoadPersistentAsset<AudioClip>(ResourcePaths.PurchaseSuccess);

                EquipItem = ContentBundle.LoadPersistentAsset<AudioClip>(ResourcePaths.EquipItem);
            }
            else
                FusionLogger.Error("Content Bundle failed to load!");
        }

        public static void OnBundleUnloaded() {
            // Unload content bundle
            if (ContentBundle != null)
                ContentBundle.Unload(true);
        }
    }
}
