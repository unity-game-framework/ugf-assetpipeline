# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.0](https://github.com/unity-game-framework/ugf-assetpipeline/releases/tag/1.0.0) - 2020-10-07  

### Release Notes

- [Milestone](https://github.com/unity-game-framework/ugf-assetpipeline/milestone/4?closed=1)  
    

### Added

- Add utility method as shortcut ProjectWindowCreateFile ([#21](https://github.com/unity-game-framework/ugf-assetpipeline/pull/21))  
    - Add `AssetPipelineEditorUtility.StartProjectWindowCreateTextFile` method as shortcut to create text file in project window.

### Changed

- Replace IAssetInfo interface by system object ([#20](https://github.com/unity-game-framework/ugf-assetpipeline/pull/20))  
    - Replace `IAssetInfo` by `System.Object`.
- Update package dependencies ([#18](https://github.com/unity-game-framework/ugf-assetpipeline/pull/18))  
    - Change project registry to target to `public` bintray registry.
    - Update `com.ugf.editortools` to `1.3.1`.
    - Remove `com.ugf.customsettings` package.
- Update project to Unity 2020.1 ([#14](https://github.com/unity-game-framework/ugf-assetpipeline/pull/14))  

### Removed

- Remove Asset.Processor feature ([#16](https://github.com/unity-game-framework/ugf-assetpipeline/pull/16))

## [0.3.0-preview](https://github.com/unity-game-framework/ugf-assetpipeline/releases/tag/0.3.0-preview) - 2020-01-11  

- [Commits](https://github.com/unity-game-framework/ugf-assetpipeline/compare/0.2.0-preview...0.3.0-preview)
- [Milestone](https://github.com/unity-game-framework/ugf-assetpipeline/milestone/3?closed=1)

### Changed
- Package dependencies:
  - `com.ugf.editortools`: from `0.3.1-preview` to `0.4.0-preview`.
  - `com.ugf.customsettings`: from `1.1.0` to `2.0.0`.

## [0.2.0-preview](https://github.com/unity-game-framework/ugf-assetpipeline/releases/tag/0.2.0-preview) - 2020-01-10  

- [Commits](https://github.com/unity-game-framework/ugf-assetpipeline/compare/0.1.0-preview...0.2.0-preview)
- [Milestone](https://github.com/unity-game-framework/ugf-assetpipeline/milestone/2?closed=1)

### Added
- `AssetInfoImporterEditor.InfoName` to get display name of the info object.

### Removed
- `AssetInfoTextImporter`: use `AssetInfoImporter` instead.

## [0.1.0-preview](https://github.com/unity-game-framework/ugf-assetpipeline/releases/tag/0.1.0-preview) - 2020-01-09  

- [Commits](https://github.com/unity-game-framework/ugf-assetpipeline/compare/d54ee29...0.1.0-preview)
- [Milestone](https://github.com/unity-game-framework/ugf-assetpipeline/milestone/1?closed=1)

### Added
- This is a initial release.


