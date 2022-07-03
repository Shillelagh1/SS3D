using System.Collections.Generic;
using FishNet;
using FishNet.Connection;
using FishNet.Managing.Server;
using FishNet.Object;
using SS3D.Core.Systems.Chat.Messages;

namespace SS3D.Core.Systems.Chat
{
    /// <summary>
    /// Controls the chat system, processing messages that come from clients and sending to all clients.
    /// </summary>
    public class ChatSystem : NetworkBehaviour
    {
        // For cache reasons
        private ServerManager _serverManager;

        private Dictionary<ChatChannels, List<string>> _chatMessages;

        public override void OnStartServer()
        {
            base.OnStartServer();

            ServerAddEventListeners();
        }

        [Server]
        private void ServerAddEventListeners()
        {
            _serverManager = InstanceFinder.ServerManager;

            _serverManager.RegisterBroadcast<RequestSendChatMessage>(HandleRequestChatMessage);
        }

        private void HandleRequestChatMessage(NetworkConnection conn, RequestSendChatMessage chatMessage)
        {
            SendChatMessage(chatMessage);
        }

        [Server]
        private void SendChatMessage(RequestSendChatMessage chatMessage)
        {
            ChatMessage message = chatMessage.Message;

            // TODO: Any necessary message checks

            _serverManager.Broadcast(new SendChatMessage(message));
        }
    }
}
