using System.Collections.Generic;
using System.Linq;
using Coimbra;
using FishNet;
using FishNet.Object;
using SS3D.Core.Systems.Lobby.Messages;
using UnityEngine;

namespace SS3D.Core.Systems.Lobby.View
{
    /// <summary>
    /// Controls the player list in the lobby
    /// </summary>
    public sealed class PlayerUsernameListView : NetworkBehaviour
    {
        // The UI element this is linked to
        [SerializeField] private Transform _root;
        
        // Username list, local list that is "networked" by the SyncList on LobbyManager
        [SerializeField] private List<PlayerUsernameView> _playerUsernames;
        
        // The username panel prefab
        [SerializeField] private GameObject _uiPrefab;

        private void Start()
        {
            Setup();
            SubscribeToEvents();
        }

        private void Setup()
        {
            SyncLobbyPlayers();
        }

        // Generic method to agglomerate all event managing
        private void SubscribeToEvents()
        {
            InstanceFinder.ClientManager.RegisterBroadcast<UserJoinedLobbyMessage>(HandleUserJoinedLobby);
            InstanceFinder.ClientManager.RegisterBroadcast<UserLeftLobbyMessage>(HandleUserLeftLobby);
        }

        private void HandleUserLeftLobby(UserLeftLobbyMessage m)
        {
            string ckey = m.Ckey;

            RemoveUsernameUI(ckey);
        }

        private void HandleUserJoinedLobby(UserJoinedLobbyMessage m)
        {
            string ckey = m.Ckey;

            AddUsernameUI(ckey);
        }

        /// <summary>
        /// Makes sure the players are shown correct with a late join
        /// </summary>
        private void SyncLobbyPlayers()
        {
            LobbySystem lobby = GameSystems.LobbySystem;

            List<string> lobbyPlayers = lobby.CurrentLobbyPlayers() != null ? lobby.CurrentLobbyPlayers() : new List<string>();

            foreach (string lobbyPlayer in lobbyPlayers)
            {
                AddUsernameUI(lobbyPlayer);
            }
        }

        /// <summary>
        /// Adds the new Username to the player list
        /// </summary>
        /// <param name="sender">Required by the ServiceLocator, unused in this function</param>
        /// <param name="data">A PlayerJoinedLobby event, that simply carries the Username</param>
        private void AddUsernameUI(string ckey)
        {
            // if this Username already exists we return
            if (_playerUsernames.Exists((player) => ckey == player.Name))
            {
                return;
            }
            
            // adds the UI element and updates the text
            GameObject uiInstance = Instantiate(_uiPrefab, _root);

            PlayerUsernameView playerUsernameView = uiInstance.GetComponent<PlayerUsernameView>();
            playerUsernameView.UpdateNameText(ckey);
            _playerUsernames.Add(playerUsernameView);
        }
        
        /// <summary>
        /// Removes the player from the list based on the Username
        /// </summary>
        /// <param name="sender">Required by the ServiceLocator, unused in this function</param>
        /// <param name="data">A PlayerJoinedLobby event, that simply carries the Username</param>
        private void RemoveUsernameUI(string ckey)
        {
            PlayerUsernameView removedUsername = null;

            foreach (PlayerUsernameView playerUsernameUI in _playerUsernames.Where(playerUsernameUI => playerUsernameUI.Name.Equals(ckey)))
            {
                removedUsername = playerUsernameUI;
                playerUsernameUI.gameObject.Destroy();
            }

            _playerUsernames.Remove(removedUsername);
            removedUsername!.gameObject.Destroy();
        }
    }
}