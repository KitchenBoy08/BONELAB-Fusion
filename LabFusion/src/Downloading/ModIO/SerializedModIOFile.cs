﻿using LabFusion.Data;
using LabFusion.Network;

namespace LabFusion.Downloading.ModIO;

public class SerializedModIOFile : IFusionSerializable
{
    public static readonly SerializedModIOFile Default = new(null);

    public ModIOFile File { get; private set; }

    public bool HasFile { get; private set; }

    public void Serialize(FusionWriter writer)
    {
        writer.Write(File.ModId);
        writer.Write(HasFile);
    }

    public void Deserialize(FusionReader reader)
    {
        File = new ModIOFile(
            reader.ReadInt32(),
            null
        );

        HasFile = reader.ReadBoolean();
    }

    public SerializedModIOFile() { }

    public SerializedModIOFile(ModIOFile? file)
    {
        HasFile = file.HasValue;

        if (HasFile)
        {
            this.File = file.Value;
        }
    }
}