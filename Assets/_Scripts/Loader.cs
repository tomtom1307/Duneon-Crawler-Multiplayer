using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project
{
    public class Loader : MonoBehaviour
    {
        public enum Scene
        {
            MainMenu,
            TestWorld,
            LoadingScene,
            LobbyScene,
            CharacterSelectScene,
            ProceduralTestingScene
        }

        private static Scene targetScene;

        public static void Load(Scene targetScene)
        {
            Loader.targetScene = targetScene;

            SceneManager.LoadScene(Scene.LoadingScene.ToString());
        }

        public static void LoadNetwork(Scene targetScene)
        {
            NetworkManager.Singleton.SceneManager.LoadScene(targetScene.ToString(), LoadSceneMode.Single);
        }

        public static void LoaderCallBack()
        {
            SceneManager.LoadScene(targetScene.ToString());
        }

    }
}
