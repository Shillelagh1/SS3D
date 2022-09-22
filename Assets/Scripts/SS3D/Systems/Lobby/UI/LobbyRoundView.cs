using FishNet;
using FishNet.Object;
using SS3D.Core;
using SS3D.Core.Attributes;
using SS3D.Systems.Rounds.Messages;
using UnityEngine;
using UnityEngine.UI;

namespace SS3D.Systems.Lobby.UI
{
    public class LobbyRoundView : SpessBehaviour
    {
        [SerializeField][NotNull] private Button _embarkButton;

        private void Start()
        {
            AddEventListeners();
        }

        private void AddEventListeners()
        {
            _embarkButton.onClick.AddListener(HandleEmbarkButtonPressed);
        }

        private void HandleEmbarkButtonPressed()
        {
            RequestStartRound();
        }

        private void RequestStartRound()
        {
            RequestStartRoundMessage requestStartRoundMessage = new();
            InstanceFinder.ClientManager.Broadcast(requestStartRoundMessage);
        }
    }
}