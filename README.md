# BONELAB TideFusion
## Networking
This mod utilizes both Steam and Peer 2 Peer networking and is available on both Quest and PCVR platforms.

## Modules
Fusion supports a system called "Modules". This allows other code mods to addon and sync their own events in Fusion.
Fusion also has an SDK for integrating features into Marrow™ SDK items, maps, and more.

## Marrow SDK Integration
NOTICE:
When using the integration, if you have the extended sdk installed, delete the "MarrowSDK" script that comes with the extended sdk.
It can be found in "Scripts/SLZ.Marrow.SDK/SLZ/Marrow/MarrowSDK".
The reason for this is that this is already included in the real sdk, and Fusion uses some values from it, causing it to get confused.

You can download the integration unity package from the Releases tab of this repository.
Alternatively, you can download the files raw at this link:
https://github.com/Lakatrazz/BONELAB-Fusion/tree/main/Core/marrow-integration

## Module Example
The module example can be found here:
https://github.com/Lakatrazz/Fusion-Module-Example

## Credits
- Lakatrazz
- Trev
- EverythingOnArm

## Licensing
- The source code of [Facepunch.Steamworks](https://github.com/Facepunch/Facepunch.Steamworks) is included under the MIT License. The full license can be found [here](https://github.com/Facepunch/Facepunch.Steamworks/blob/master/LICENSE).
- The source code of [LiteNetLib](https://github.com/RevenantX/LiteNetLib) is included under the MIT License. The full license can be found [here](https://github.com/RevenantX/LiteNetLib/blob/master/LICENSE.txt).
- The source code of [RiptideNetworking](https://github.com/RiptideNetworking/Riptide) is included under the MIT License. The full license can be found [here](https://github.com/RiptideNetworking/Riptide/blob/main/LICENSE.md)

## Licensing
- The source code of [Facepunch.Steamworks](https://github.com/Facepunch/Facepunch.Steamworks) is included under the MIT License. The full license can be found [here](https://github.com/Facepunch/Facepunch.Steamworks/blob/master/LICENSE).
- The source code of [LiteNetLib](https://github.com/RevenantX/LiteNetLib) is included under the MIT License. The full license can be found [here](https://github.com/RevenantX/LiteNetLib/blob/master/LICENSE.txt).

## Setting up the Source Code
1. Clone the git repo into a folder
2. Setup a "managed" folder in the "Core" folder.
3. Drag the dlls from Melonloader/Managed into the managed folder.
4. Drag MelonLoader.dll and 0Harmony.dll into the managed folder.
5. Reference RiptideNetworking manually.
6. You're done!

## Disclaimer
#### THIS PROJECT IS NOT AFFILIATED WITH SLZ, FUSION, OR ANY OTHER MULTIPLAYER MOD. DO NOT GO INTO THE FUSION DISCORD EXPECTING SUPPORT FOR ISSUES IN THE MOD.
