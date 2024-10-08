﻿using HarmonyLib;

using LabFusion.Network;

using UnityEngine;

using Il2CppSLZ.Marrow;

namespace LabFusion.Patching
{
    [HarmonyPatch(typeof(ImpactSFX))]
    public static class ImpactSFXPatches
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(ImpactSFX.ImpactSound))]
        public static bool ImpactSound(ImpactSFX __instance, Collision c)
        {
            if (NetworkInfo.HasServer && __instance._host != null)
            {
                var properties = ImpactProperties.Cache.Get(c.gameObject);

                if (properties)
                {
                    return ImpactAttackValidator.ValidateAttack(__instance.gameObject, __instance._host, properties);
                }
            }

            return true;
        }
    }
}
