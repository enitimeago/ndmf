﻿# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [unreleased]

### Fixed

### Changed

### Removed

### Security

### Deprecated

## [1.3.1-rc.0] - [2024-01-31]

### Fixed
- Outfit sub-animators can cause transforms to move when using GestureManager (#147)

### Changed
- Run all preprocess hooks in Apply On Play processing, to better align with VRCFury handling. (#145)

## [1.3.0] - [2024-01-29]

### Added
- New localization framework
- New error reporting framework
- API to record when one object is replaced by another
- Added a non-component-based check for double execution of hooks (#142)
- Exposed the qualified name variant of `Sequence.BeforePass` (#109)

### Changed
- Adjusted hook processing order to improve compatibility with VRCFury (#122)
- Worked around a hack in VRCFury that broke optimization plugins (#126)

### Fixed
- Play mode processing fails when installed via UPM (#89)

## [1.2.5] - [2023-11-15]

### Fixed
- GUID collisions with packages derived from the VRChat template package (#84)

## [1.2.4] - [2023-11-12]

### Fixed
- Duplicate references error when Lyuma's Av3emulator is installed due to Unity bug (#80)

## [1.2.3] - [2023-11-11]

### Added

### Fixed
- Fixed an issue where apply on play might not work when multiple scenes are open (#61)
- Fixed an issue where Apply on Play would not work properly when Lyuma's Av3Emulator had preprocess hooks disabled
  (bdunderscore/modular-avatar#516) (#64)

### Changed

- Make Apply on Play non-persistent, as users seem to frequently have issues with it left turned off.

### Removed
- Removed a vestigial "Avatar Toolkit -> Apply on Play" menu item, which didn't do anything when selected. (#70)

### Security

### Deprecated
- Deprecated APIs for finding avatar roots defined outside RuntimeUtil (#73)

## [1.2.2] - [2023-10-09]

### Fixed
- Fixed an issue where apply on play might not work when multiple scenes are open (#61)

## [1.2.1] - [2023-10-08]

### Fixed
- Removed .git folder from built packages

## [1.2.0] - [2023-10-07]

### Fixed
- Re-released 1.1.1 as 1.2.0 as it contained new APIs (#34). Please use 1.2.0 for version declarations if these new APIs
  are needed.

## [1.1.1] - [2023-10-07]

### Added
- Exposed APIs for finding avatar roots in RuntimeUtil (#34)
- Support Unity projects without VRCSDK (#50)
- Add warning when an outdated version of Modular Avatar is installed (#54)

## [1.1.0] - [2023-10-05]

### Added
- Added toplevel menu for manual bake avatar, even when MA is also installed (#35)
- Added support for multiple ExportsPlugin declarations (#40)
- Added additional convenience overloads for BeforePlugin and AfterPlugin

### Fixed
- Create ApplyOnPlayGlobalActivator correctly when creating and opening scenes (#31)
- Time resolution on the plugin sequence display is milliseconds, not 0.01 ms (#43)
- BeforePlugin/AfterPlugin declarations don't work (#44)

### Changed
- Position of the Plugin sequence display window is now preserved after restarting the Unity Editor (#42)

### Removed

### Security

## [1.0.2] - [2023-09-26]

### Added
* Added missing license declaration

### Fixed
- Generated assets are not visible on the Project window (#26)
- Apply on play is not disabled if av3emulator is active on other scene
- Avatar names with non-path-safe characters break builds (again) (#18)

### Changed

### Removed

### Security

## [1.0.1] - [2023-09-24]

### Fixed

- Move optimization passes to -1025 to ensure it runs before VRChat destroys IEditorOnly components.

## [1.0.0] - [2023-09-24]

### Added

- Initial public release

### Fixed

- Remove missing script components from the avatar at the start of the build.
- Fixed build failures when avatar names contained non-path-safe characters (#18)
- Suppress apply-on-play when Lyuma's Av3Emulator is active (improves compatibility) (#16)

### Changed

### Removed

- Samples have been removed from the build, as they are not part of the stable NDMF API surface area. They'll probably
  be moved to a separate repository at some point.

## [0.4.0] - [2023-09-20]

0.4.0 and prior releases did not Keep A Changelog.
