﻿using LabFusion.Utilities;

using UnityEngine;

namespace LabFusion.SDK.Achievements
{
    public class Rampage : Achievement
    {
        public override string Title => "Rampage";

        public override string Description => "Win a Deathmatch match without dying a single time.";

        public override int BitReward => 1500;

        public override Texture2D PreviewImage => FusionAchievementLoader.GetPair(nameof(Rampage)).Preview;
    }
}
