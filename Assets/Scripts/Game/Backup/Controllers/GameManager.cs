using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Controllers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager GM;
        public static UIManager UI;
        public static PlayerManager P;
        public static MetricsManager MT;
        public static ConfigManager CONFIG;
        public static PlayerData PlayerData;
        public static PlayerProgress PlayerProgress;
        public static PlayerCollection PlayerCollection;
        public static PlayerCharacter PlayerCharacter;

        public bool Testing;
        public Vector3[] BS_Positions;

        private void Awake()
        {
            if (GM == null)
            {
                GM = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            // Initialize controllers
            CONFIG = new ConfigManager();
            PlayerData = new PlayerData();
            PlayerProgress = new PlayerProgress();
            PlayerCollection = new PlayerCollection();
            PlayerCharacter = new PlayerCharacter();
            MT = new MetricsManager();

            Debug.Log("GameManager initialized.");

            // Additional initialization logic...
        }

        private void Start()
        {
            GraphicsSettings.useScriptableRenderPipelineBatching = true;
            Debug.Log("GameManager started.");

            // Set up the game environment
            if (Testing)
            {
                // Testing mode setup
                if (PlayerCollection.Deck == null)
                {
                    PlayerCollection.AddUnitsAndCharactersDefault();
                }
            }

            // Initialize other systems
            InitializeBaseStations();

            // Additional start logic...
        }

        private void InitializeBaseStations()
        {
            Debug.Log("Initializing base stations...");
            // Base stations initialization logic...
        }

        // Additional methods and logic...
    }
}
