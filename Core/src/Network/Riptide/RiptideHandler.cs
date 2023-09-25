using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using LabFusion.Core.src.Network.Riptide.Enums;
using LabFusion.Data;
using LabFusion.Representation;
using LabFusion.SDK.Achievements;
using LabFusion.SDK.Gamemodes;
using LabFusion.Senders;
using LabFusion.Utilities;
using Riptide;
using SLZ.Marrow.SceneStreaming;

namespace LabFusion.Network
{
    public static class RiptideHandler
    {
        public static MessageSendMode ConvertToSendMode (NetworkChannel channel)
        {
            MessageSendMode sendMode;
            switch (channel)
            {
                case NetworkChannel.Unreliable:
                default:
                    sendMode = MessageSendMode.Unreliable;
                    break;
                case NetworkChannel.VoiceChat:
                    sendMode = MessageSendMode.Unreliable;
                    break;
                case NetworkChannel.Reliable:
                    sendMode = MessageSendMode.Reliable;
                    break;
            }
            return sendMode;
        }

        public unsafe static byte[] FusionMessageToBytes (FusionMessage fusionMessage)
        {
            int length = fusionMessage.Length;
            byte* buffer = fusionMessage.Buffer;

            byte[] bytes = ConvertBytePointerToByteArray(buffer, length);

            return bytes;
        }

        public unsafe static byte[] ConvertBytePointerToByteArray(byte* bytePtr, int length)
        {
            // Create a new managed byte array
            byte[] byteArray = new byte[length];

            // Copy the data from the byte pointer to the byte array using Marshal.Copy
            Marshal.Copy((IntPtr)bytePtr, byteArray, 0, length);

            return byteArray;
        }

        public static Message PrepareMessage(FusionMessage fusionMessage, NetworkChannel channel, ushort messageChannel, ushort playerID = 0)
        {
            if (RiptideNetworkLayer.CurrentServerType.GetType() == ServerTypes.P2P)
            {
                var message = Message.Create(ConvertToSendMode(channel), 0); // Create the message

                message.Release(); // Make sure the message is empty before adding bytes

                message.AddBytes(FusionMessageToBytes(fusionMessage)); // Add bytes

                return message;
            } else
            {
                var message = Message.Create(ConvertToSendMode(channel), messageChannel); // Create the message

                message.Release(); // Make sure the message is empty before adding bytes

                message.AddBytes(FusionMessageToBytes(fusionMessage));

                if (playerID != 0)
                {
                    message.AddUShort(playerID);
                }

                return message;
            }
        }

        [MessageHandler(0)]
        public static void HandleSomeMessageFromServer(Message message)
        {
            unsafe
            {
                int messageLength = message.WrittenLength;

                byte[] buffer = message.GetBytes();
                fixed (byte* messageBuffer = buffer)
                {
                    FusionMessageHandler.ReadMessage(messageBuffer, messageLength);
                }
            }
        }

        [MessageHandler(0)]
        private static void HandleSomeMessageFromClient(ushort riptideID, Message message)
        {
            unsafe
            {
                int messageLength = message.WrittenLength;

                byte[] buffer = message.GetBytes();
                fixed (byte* messageBuffer = buffer)
                {
                    FusionMessageHandler.ReadMessage(messageBuffer, messageLength, true);
                } 
            }
        }

        [MessageHandler(1)]
        private static void HandleServerResponse(Message message)
        {
            if (message.GetInt() == (int)ServerTypes.DEDICATED)
            {
                RiptideNetworkLayer.CurrentServerType.SetType(ServerTypes.DEDICATED);

                if (RiptideNetworkLayer.currentclient.Id == 1)
                {
                    RiptideNetworkLayer.isHost = true;

                    PlayerIdManager.SetLongId(RiptideNetworkLayer.currentclient.Id);

                    FusionSceneManager.HookOnDelayedLevelLoad(() =>
                    {
                        PermissionList.SetPermission(PlayerIdManager.LocalLongId, PlayerIdManager.LocalUsername, PermissionLevel.DEFAULT);
                    });

                    // Mimicking the OnStartServer method in order to make it custom
                    // Create local id
                    var id = new PlayerId(PlayerIdManager.LocalLongId, 0, InternalServerHelpers.GetInitialMetadata(), InternalServerHelpers.GetInitialEquippedItems());
                    id.Insert();
                    PlayerIdManager.ApplyLocalId();

                    // Register module message handlers so they can send messages
                    var names = ModuleMessageHandler.GetExistingTypeNames();
                    ModuleMessageHandler.PopulateHandlerTable(names);

                    // Register gamemodes
                    var gamemodeNames = GamemodeRegistration.GetExistingTypeNames();
                    GamemodeRegistration.PopulateGamemodeTable(gamemodeNames);

                    // Update hooks
                    MultiplayerHooking.Internal_OnStartServer();

                    // Send a notification
                    FusionNotifier.Send(new FusionNotification()
                    {
                        title = "Connected to Server",
                        message = "Connected to Dedicated Server",
                        showTitleOnPopup = true,
                        isMenuItem = false,
                        isPopup = true,
                        type = NotificationType.SUCCESS,
                    });

                    // Unlock achievement
                    if (AchievementManager.TryGetAchievement<HeadOfHouse>(out var achievement))
                        achievement.IncrementTask();

                    // Reload the scene
                    SceneStreamer.Reload();
                }
                else
                {
                    RiptideNetworkLayer.isHost = false;

                    PlayerIdManager.SetLongId(RiptideNetworkLayer.currentclient.Id);

                    ConnectionSender.SendConnectionRequest();
                }
            }
            else
            {
                RiptideNetworkLayer.CurrentServerType.SetType(ServerTypes.P2P);

                RiptideNetworkLayer.isHost = false;

                PlayerIdManager.SetLongId(RiptideNetworkLayer.currentclient.Id);

                ConnectionSender.SendConnectionRequest();
            }
        }

        [MessageHandler(1)]
        private static void HandleClientRequest(ushort riptideID, Message message)
        {
            RiptideNetworkLayer.currentserver.TryGetClient(riptideID, out Connection client);

            if (message.GetString() == "RequestServerType")
            {
                Riptide.Message sent = Riptide.Message.Create(MessageSendMode.Unreliable, 1);
                sent.AddInt((int)ServerTypes.P2P);
                RiptideNetworkLayer.currentserver.Send(sent, client);
            }
        }

        private static byte[] AddPlayerIDToBytes(byte[] bytes, byte playerID)
        {
            List<byte> byteList = bytes.ToList();

            byteList.Add(playerID);

            return byteList.ToArray();
        }

        [MessageHandler(11)]
        public static void HandleHostRequest(Message message)
        {
            if (RiptideNetworkLayer.CurrentServerType.GetType() == ServerTypes.DEDICATED)
            {
                RiptideNetworkLayer.isHost = true;

                Message response = Message.Create(MessageSendMode.Reliable, 11);
                response.Release();

                response.AddBool(true);

                RiptideNetworkLayer.currentclient.Send(response);

                FusionNotifier.Send(new FusionNotification()
                {
                    title = "New Host",
                    showTitleOnPopup = false,
                    isMenuItem = false,
                    isPopup = true,
                    message = $"You are now handling all server messages!",
                    popupLength = 3f,
                });

            } else
            {
                RiptideNetworkLayer.isHost = false;

                Message response = Message.Create(MessageSendMode.Reliable, 11);
                response.Release();

                response.AddBool(false);

                RiptideNetworkLayer.currentclient.Send(response);
            }
        }
    }
}