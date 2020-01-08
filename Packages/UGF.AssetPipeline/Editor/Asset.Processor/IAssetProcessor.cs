namespace UGF.AssetPipeline.Editor.Asset.Processor
{
    public interface IAssetProcessor
    {
        void OnImport(string path);
        void OnDelete(string path);
        void OnMoved(string pathFrom, string pathTo);
    }
}
