using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace spacegame
{
    public class SaveLoadManager : MonoBehaviour
    {
        public static readonly string savesPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\My Games\\space!!!!\\saves\\";
        public static SaveLoadManager instance;

        // a stopwatch to keep track of how long it has been since the player has saved
        public Stopwatch stopwatch = new Stopwatch();
        public bool stopwatchStarted;

        private void Awake()
        {
            instance = this;
        }

        public void Init()
        {
            // create the saves directory if it doesn't already exist
            if (!Directory.Exists(savesPath))
                Directory.CreateDirectory(savesPath);
        }
        
        public void Save()
        {
            // delete the save if it already exists because the binary writer does funky stuff i think
            string path = savesPath + "save0.alison";
            if (File.Exists(path)) File.Delete(path);

            Logger.WriteLine("saving");
            using (SaveLoad.SaveWriter writer = new SaveLoad.SaveWriter(new FileStream(savesPath + "save0.alison", FileMode.OpenOrCreate)))
            {
                writer.Dump((Vector2)Player.instance.gameObject.transform.position, "player_position");
                writer.Dump(SceneManager.GetActiveScene().name, "map");
                writer.Dump(GameState.GetBooleansDictionary(), "gsb");
                writer.Dump(GameState.GetIntegersDictionary(), "gsi");
                for (int i = 0; i < Player.followers.Count; i++)
                    // when we read the save file we'll just get every object in the save file with the Follower type,
                    // so these don't really need to have a name that we'll have to remember
                    writer.Dump(Player.followers[i], $"foll{i}");
            }
            Logger.WriteLine("saved!");
            
            stopwatch.Reset();
            stopwatch.Start();
            stopwatchStarted = true;
        }

        public void Load()
        {
            Logger.WriteLine("loading save");
            using (SaveLoad.SaveReader reader = new SaveLoad.SaveReader(new FileStream(savesPath + "save0.alison", FileMode.Open)))
            {
                try
                {
                    // double check the header first
                    if (!reader.VerifyHeader(out byte[] header))
                        throw new Exception($"couldn't verify file header of save data (ascii of first 13 bytes was {Encoding.ASCII.GetString(header)})");
                    reader.GetVersion(false); // this is just to move the position forward
                    // then read the data into the dictionary
                    reader.ReadTheStuff();

                    // read everything from the save file
                    Vector2 playerPosition = (Vector2)reader.GetObject("player_position");
                    string sceneName = (string)reader.GetObject("map");
                    Dictionary<string, bool> gameStateBools = (Dictionary<string, bool>)reader.GetObject("gsb");
                    Dictionary<string, int> gameStateInts = (Dictionary<string, int>)reader.GetObject("gsi");
                    List<Follower> followers = reader.GetObjectsOfType<Follower>().ToList() ?? new List<Follower>();

                    MapManager.ChangeMap(sceneName, 
                        // only set the player position and followers after the map has been loaded
                        callback: x => 
                        { 
                            Player.instance.transform.position = playerPosition;
                            foreach (Follower f in followers)
                            {
                                GameObject follower = Instantiate(f.gameObject, Player.instance.transform.position, Quaternion.identity);
                                follower.name = follower.name.Replace("(Clone)", string.Empty); // adios
                                Player.followers.Add(follower.GetComponent<Follower>());
                            }
                        });
                    GameState.ResetBooleansDictionary(gameStateBools);
                    GameState.ResetIntegersDictionary(gameStateInts);
                }
                catch (Exception ex)
                {
                    Logger.WriteLine($"an error was encountered while loading the save:\n{ex}");
                    MapManager.ChangeMap("save_corrupt");
                    // ok so this is a mess of a oneliner
                    // basically we wait 0.1 seconds to ensure that at least a frame has passed,
                    // then wait until the player has pressed any key on their keyboard
                    StartCoroutine(HandyDandies.instance.DoAfterSeconds(0.1f, 
                        () => StartCoroutine(HandyDandies.instance.DoAfter(() => Input.anyKeyDown, () => MapManager.ChangeMap("title")))));
                }
            }
            Logger.WriteLine("save loaded!");

            stopwatch.Reset();
        }
    }
}
