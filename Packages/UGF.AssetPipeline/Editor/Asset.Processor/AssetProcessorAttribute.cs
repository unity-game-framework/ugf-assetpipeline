using System;
using JetBrains.Annotations;

namespace UGF.AssetPipeline.Editor.Asset.Processor
{
    [BaseTypeRequired(typeof(AssetProcessor))]
    [AttributeUsage(AttributeTargets.Class)]
    public class AssetProcessorAttribute : Attribute
    {
        public string MenuName { get; }

        public AssetProcessorAttribute(string menuName)
        {
            MenuName = menuName;
        }
    }
}
