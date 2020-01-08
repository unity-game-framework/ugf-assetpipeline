using UnityEngine;

namespace UGF.AssetPipeline.Editor.Asset.Processor
{
    public abstract class AssetProcessor : ScriptableObject
    {
        public virtual void OnImport(string path)
        {
        }

        public virtual void OnDelete(string path)
        {
        }

        public virtual void OnMoved(string pathFrom, string pathTo)
        {
        }
    }
}
