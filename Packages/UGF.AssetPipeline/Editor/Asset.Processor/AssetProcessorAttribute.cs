using System;
using JetBrains.Annotations;

namespace UGF.AssetPipeline.Editor.Asset.Processor
{
    [BaseTypeRequired(typeof(AssetProcessor))]
    [AttributeUsage(AttributeTargets.Class)]
    public class AssetProcessorAttribute : Attribute
    {
    }
}
