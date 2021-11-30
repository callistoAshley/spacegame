using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Reflection;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.IO;

namespace spacegame
{
    public class DebugConsole : MonoBehaviour
    {
        // the text in the ui and the command we've written
        private Text text;
        private string command = string.Empty;
        private string lastText; // the text in the console prior to the execution of the command

        // singleton instance
        private static DebugConsole instance;
        
        private void Awake()
        {
            text = GetComponentInChildren<Text>();
            instance = this;

            text.text = "[alison]$";
        }

        // dictionary of keycodes and delegates for navigating around the console 
        private Dictionary<KeyCode, Action> navigationKeybinds = new Dictionary<KeyCode, Action>
        {
            {KeyCode.Backspace, () => instance.Backspace()},
            {KeyCode.Return, () => instance.Enter() },
        };

        private bool WrappedKeyDown(KeyCode input, out KeyCode output)
        {
            output = input;
            return Input.GetKeyDown(input);
        }

        private void ClearCommand()
        {
            command = string.Empty;
            lastText = text.text;
        }

        private void Update()
        {
            // add keyboard input to command string
            switch (Input.inputString)
            {
                // don't do anything if it's a special character like carriage return
                case "\r":
                    break;
                default:
                    command += Input.inputString;
                    break;
            }
            text.text = $"{lastText}[alison]$ {command}";

            // OK so this is a bit dumb but if i just write a hundred ifs then i don't satiate my massive programmer ego
            // basically we have all of the funky navigation stuff like backspace and moving back and forward through the string
            // in a dictionary with KeyCode keys and Action values
            // THEN we wrap KeyDown and skadoobly woobly and that's how we do it i am awesome
            KeyCode output = KeyCode.None;
            if (navigationKeybinds.Keys.Any(k => WrappedKeyDown(k, out output)))
            {
                navigationKeybinds[output].Invoke();
            }
        }

        public void Out(string input)
        {
            text.text += $"\n{input}\n";
        }

        private void Backspace()
        {
            if (command.Length > 1)
                command = command.Remove(command.Length - 2);
        }

        // TODO: comment this
        private void Enter()
        {
            if (command != string.Empty)
            {
                string commandName = command.Contains(' ') ? command.Substring(0, command.IndexOf(' ')) : command;
                string[] args = command.Replace($"{commandName}", string.Empty).Split(' ');

                Commands.Call(commandName, args);
            }
            ClearCommand();
        }

        public static class Commands
        {
            public static Dictionary<string, Delegate> commands = new Dictionary<string, Delegate>();
            public delegate void Command(string[] args);

            public static void RegisterCommands()
            {
                foreach (MethodInfo m in typeof(Commands).GetMethods())
                {
                    // try to get the command attribute from the method
                    CommandAttribute command = (CommandAttribute)m.GetCustomAttribute(typeof(CommandAttribute));

                    // if it exists, add it to the commands dictionary
                    if (command != null)
                    {
                        commands.Add(command.name, m.CreateDelegate(typeof(Command)));
                        Debug.Log($"registered debug command: {command.name} ({m.Name})");
                    }
                }

                Debug.Log("finished registering debug commands");
            }

            public static void Call(string command, string[] args)
            {
                args = args.Skip(1).ToArray();

                foreach (string s in commands.Keys)
                {
                    if (s == command)
                    {
                        CommandAttribute c = (CommandAttribute)commands[s].GetMethodInfo().GetCustomAttribute(typeof(CommandAttribute));
                        // catch insufficient args
                        if (args.Length < c.minimumArgs)
                        {
                            instance.Out($"insufficient args for command \"{command}\" ({c.minimumArgs} expected, received {args.Length})");
                            return;
                        }

                        commands[command].DynamicInvoke(new object[] { args });
                        return;
                    }
                }
                instance.Out($"no debug command \"{command}\"");
            }

            [Command("help")]
            public static void Help(string[] args)
            {
                try
                {
                    instance.Out($"commands:\n{string.Join(",", commands.Keys)}");
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.ToString());
                    instance.Out(ex.Message);
                }
            }

            [Command("clear")]
            public static void Clear(string[] args)
            {
                try
                {
                    instance.text.text = string.Empty;
                }
                catch (Exception ex)
                {
                    Debug.Log($"exception: {ex}");
                    instance.Out(ex.Message);
                }
            }

            [Command("change_map", 1)]
            public static void ChangeMap(string[] args)
            {
                try
                {
                    MapManager.ChangeMap(args[0], args.Length > 1 ? int.Parse(args[1]) : 0);
                }
                catch (Exception ex)
                {
                    Debug.Log($"exception: {ex}");
                    instance.Out(ex.Message);
                }
            }

            [Command("maps")]
            public static void MapList(string[] args)
            {
                try
                {
                    string[] sceneNames = new string[SceneManager.sceneCount];

                    for (int i = 0; i < SceneManager.sceneCount; i++)
                        sceneNames[i] = SceneManager.GetSceneAt(i).name;

                    instance.Out(string.Join(",", sceneNames));
                }
                catch (Exception ex)
                {
                    Debug.Log($"exception: {ex}");
                    instance.Out(ex.Message);
                }
            }

            [Command("cam_size", 1)]
            public static void CameraSize(string[] args)
            {
                try
                {
                    MainCamera.instance.GetComponent<Camera>().orthographicSize = float.Parse(args[0]);
                }
                catch (Exception ex)
                {
                    Debug.Log($"exception: {ex}");
                    instance.Out(ex.Message);
                }
            }

            [Command("scripts")]
            public static void ScriptsList(string[] args)
            {
                try
                {
                    IEnumerable<string> files = Directory.EnumerateFiles(Application.streamingAssetsPath + "/alisonscript", "*.alisonscript", SearchOption.AllDirectories);
                    List<string> filesList = files.ToList();
                    filesList.ForEach((string file) => file = file.Remove(0, file.LastIndexOf("\\")));

                    instance.Out(string.Join(",", filesList));
                }
                catch (Exception ex)
                {
                    Debug.Log($"exception: {ex}");
                    instance.Out(ex.Message);
                }
            }
        }

        private class CommandAttribute : Attribute
        {
            public string name;
            public int minimumArgs;

            public CommandAttribute(string name) : this(name, 0) { }
            public CommandAttribute(string name, int minimumArgs)
            {
                this.name = name;
                this.minimumArgs = minimumArgs;
            }
        }
    }
}
