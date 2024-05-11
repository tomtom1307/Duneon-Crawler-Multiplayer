using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Project
{
    public class TestingLobbyUI : MonoBehaviour
    {
        [SerializeField] private Button CreateGameButton;
        [SerializeField] private Button JoinGameButton;

        private void Awake()
        {
            CreateGameButton.onClick.AddListener(() =>
            {
                DuneonCrawlerMultiplayer.instance.StartHost();
                Loader.LoadNetwork(Loader.Scene.CharacterSelectScene);
            });
            JoinGameButton.onClick.AddListener(() =>
            {
                DuneonCrawlerMultiplayer.instance.StartClient();
            });
        }
    }
}
