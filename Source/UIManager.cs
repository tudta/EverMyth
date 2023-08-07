using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCSharp;
using WCSharp.Buffs;
using WCSharp.DateTime;
using WCSharp.Dummies;
using WCSharp.Effects;
using WCSharp.Events;
using WCSharp.Json;
using WCSharp.Knockbacks;
using WCSharp.Lightnings;
using WCSharp.Missiles;
using WCSharp.SaveLoad;
using WCSharp.Shared;
using WCSharp.Sync;
using static Constants;
using static Regions;
using static War3Api.Common;
using static War3Api.Blizzard;

namespace Source
{
    public static class UIManager
    {
        private static Dictionary<int, MainMenu> _mainMenus;
        private static Dictionary<int, SaveLoadMenu> _saveLoadMenus;

        private static Dictionary<int, Stack<framehandle>> _currentlyOpenMenus;


        public static void Init()
        {
            Console.WriteLine("Initializing UI Manager!");
            GenerateUIMenus();
            CloseAllMenusGlobal();
            CreateEscapeTrigger();
        }

        private static void GenerateUIMenus()
        {
            InitializeCurrentlyOpenMenus();
            InitializeMainMenus();
            InitializeSaveLoadMenus();
        }

        private static void InitializeCurrentlyOpenMenus()
        {
            _currentlyOpenMenus = new Dictionary<int, Stack<framehandle>>();
            int playerId = 0;
            for (int i = 0; i < PlayerManager.Players.Count; i++)
            {
                playerId = PlayerManager.Players.ElementAt(i).Key;
                _currentlyOpenMenus.Add(playerId, new Stack<framehandle>());
            }
        }

        private static void InitializeMainMenus()
        {
            Console.WriteLine("Initializing main menus!");
            _mainMenus = new Dictionary<int, MainMenu>();
            int playerId = 0;
            for (int i = 0; i < PlayerManager.Players.Count; i++)
            {
                playerId = PlayerManager.Players.ElementAt(i).Key;
                _mainMenus.Add(playerId, new MainMenu());
                _mainMenus[playerId].Init();
                _currentlyOpenMenus[playerId].Push(_mainMenus[playerId].BaseFrameHandle);
            }
        }

        private static void InitializeSaveLoadMenus()
        {
            _saveLoadMenus = new Dictionary<int, SaveLoadMenu>();
            int playerId = 0;
            for (int i = 0; i < PlayerManager.Players.Count; i++)
            {
                playerId = PlayerManager.Players.ElementAt(i).Key;
                _saveLoadMenus.Add(playerId, new SaveLoadMenu());
                _saveLoadMenus[playerId].Init();
                _currentlyOpenMenus[playerId].Push(_saveLoadMenus[playerId].BaseFrameHandle);
            }
        }

        public static void OpenMenu(int playerId, CustomUIMenuType menuType)
        {
            if (GetLocalPlayer() == PlayerManager.Players[playerId])
            {
                switch (menuType)
                {
                    case CustomUIMenuType.MainMenu:
                        if (!IsMenuOpen(_mainMenus[playerId].BaseFrameHandle))
                        {
                            _mainMenus[playerId].OpenMenu();
                            _currentlyOpenMenus[playerId].Push(_mainMenus[playerId].BaseFrameHandle);
                        }
                        break;
                    case CustomUIMenuType.SaveLoadMenu:
                        if (!IsMenuOpen(_saveLoadMenus[playerId].BaseFrameHandle))
                        {
                            _saveLoadMenus[playerId].OpenMenu();
                            _currentlyOpenMenus[playerId].Push(_saveLoadMenus[playerId].BaseFrameHandle);
                        }
                        break;
                    case CustomUIMenuType.StatsWindow:
                        break;
                    default:
                        break;
                }
            }
        }

        public static void CloseMenu(int playerId, framehandle menu)
        {
            Console.WriteLine("Closing menu!");
            if (GetLocalPlayer() == PlayerManager.Players[playerId])
            {
                if (_currentlyOpenMenus[playerId].Peek() == menu)
                {
                    BlzFrameSetVisible(_currentlyOpenMenus[playerId].Pop(), false);
                }
            }
        }

        public static void CloseCurrentMenu(int playerId)
        {
            if (GetLocalPlayer() == PlayerManager.Players[playerId])
            {
                CloseMenu(playerId, _currentlyOpenMenus[playerId].Peek());
            }
        }

        public static void CloseAllMenus(int playerId)
        {
            if (GetLocalPlayer() == PlayerManager.Players[playerId])
            {
                for (int i = 0; i < _currentlyOpenMenus[playerId].Count; i++)
                {
                    CloseCurrentMenu(playerId);
                }
            }
        }

        private static void CloseAllMenusGlobal()
        {
            Console.WriteLine("Closing all menus global!");
            // For each player, close all menus.
            int playerId = 0;
            for (int i = 0; i < PlayerManager.Players.Count; i++)
            {
                playerId = PlayerManager.Players.ElementAt(i).Key;
                for (int x = 0; x < _currentlyOpenMenus[playerId].Count; x++)
                {
                    CloseMenuGlobal(playerId);
                }
            }
        }

        private static void CloseMenuGlobal(int playerId)
        {
            BlzFrameSetVisible(_currentlyOpenMenus[playerId].Pop(), false);
        }

        private static bool IsMenuOpen(framehandle menu)
        {
            if (BlzFrameIsVisible(menu))
            {
                Console.WriteLine("Menu is open!");
                return true;
            }
            Console.WriteLine("Menu is closed!");
            return false;
        }

        public static void SetMenuText(int playerId, framehandle menu, string menuText)
        {
            Console.WriteLine("Setting menu text!");
            if (GetLocalPlayer() == PlayerManager.Players[playerId])
            {
                BlzFrameSetText(menu, menuText);
            }
        }

        private static void CreateEscapeTrigger()
        {
            trigger escTrigger = CreateTrigger();
            TriggerAddAction(escTrigger, EscapeButtonMenuFunction);
            int playerId = 0;
            for (int i = 0; i < PlayerManager.Players.Count; i++)
            {
                playerId = PlayerManager.Players.ElementAt(i).Key;
                BlzTriggerRegisterPlayerKeyEvent(escTrigger, PlayerManager.Players[playerId], OSKEY_ESCAPE, 0, false);
            }
        }

        private static void EscapeButtonMenuFunction()
        {
            Console.WriteLine("Escape button pressed!");
            int playerId = GetPlayerId(GetTriggerPlayer());
            // Check if main menu is open
            if (IsMenuOpen(_mainMenus[playerId].BaseFrameHandle))
            {
                // Close top menu only
                CloseCurrentMenu(playerId);
                return;
            }
            OpenMenu(playerId, CustomUIMenuType.MainMenu);
        }
    }
}