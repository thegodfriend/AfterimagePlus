using Modding;
using Modding.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AfterimagePlus
{
    public class AfterimagePlus : Mod, IGlobalSettings<Settings>, IMenuMod
    {
        public static AfterimagePlus Instance;
        
        public Settings settings = new();

        internal ImagePool afterimagePool = new();

        private AfterimageGenerator playerAfterimage;

        public bool ToggleButtonInsideMenu => true;

        //public override List<ValueTuple<string, string>> GetPreloadNames()
        //{
        //    return new List<ValueTuple<string, string>>
        //    {
        //        new ValueTuple<string, string>("White_Palace_18", "White Palace Fly")
        //    };
        //}

        public AfterimagePlus() : base("Afterimage †") { }
        public override string GetVersion() => "0.0.2-0";

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            Log("Initializing");

            Instance = this;

            ModHooks.HeroUpdateHook += HeroUpdateHook;
            ModHooks.OnEnableEnemyHook += ModHooks_OnEnableEnemyHook;

            GameObject afterimageTemplate = new();
            afterimageTemplate.AddComponent<tk2dSprite>();
            afterimageTemplate.AddComponent<tk2dSpriteAnimator>();
            afterimageTemplate.name = "afterimageTemplate";
            UnityEngine.Object.DontDestroyOnLoad(afterimageTemplate);
            afterimageTemplate.SetActive(false);
            afterimagePool.SetTemplate(afterimageTemplate);

            Log("Initialized");
        }

        private bool ModHooks_OnEnableEnemyHook(GameObject enemy, bool isAlreadyDead)
        {
            if (settings.enemyAfterimageOn) enemy.GetOrAddComponent<AfterimageGenerator>().SetAfterimageColor(new Color(0.5f, 0.25f, 0.25f, 1));

            return isAlreadyDead;
        }

        private void HeroUpdateHook()
        {
            playerAfterimage = HeroController.instance.gameObject.GetOrAddComponent<AfterimageGenerator>();
            playerAfterimage.enabled = settings.knightAfterimageOn;
        }

        public void OnLoadGlobal(Settings _settings) => settings = _settings;
        public Settings OnSaveGlobal() => settings;

        public List<IMenuMod.MenuEntry> GetMenuData(IMenuMod.MenuEntry? menu)
        {
            List<IMenuMod.MenuEntry> menus = new();
            
            menus.Add(
                new()
                {
                    Name = "Knight Afterimage",
                    //Description = "Toggle for the Knight having an afterimage.",
                    Values = new string[]
                    {
                        Language.Language.Get("MOH_ON", "MainMenu"),
                        Language.Language.Get("MOH_OFF", "MainMenu"),
                    },
                    Saver = i =>
                    {
                        settings.knightAfterimageOn = i == 0;
                        if (playerAfterimage)
                            playerAfterimage.enabled = settings.knightAfterimageOn;
                        
                    },
                    Loader = () => settings.knightAfterimageOn ? 0 : 1
                }
            );
            menus.Add(
                new()
                {
                    Name = "Enemy Afterimages",
                    //Description = "Toggle for enemies having afterimages.",
                    Values = new string[]
                    {
                        Language.Language.Get("MOH_ON", "MainMenu"),
                        Language.Language.Get("MOH_OFF", "MainMenu"),
                    },
                    Saver = i =>
                    {
                        settings.enemyAfterimageOn = i == 0;
                        foreach(HealthManager _hm in Resources.FindObjectsOfTypeAll<HealthManager>())
                        {
                            _hm.gameObject.GetOrAddComponent<AfterimageGenerator>().enabled = settings.enemyAfterimageOn;
                        }
                    },
                    Loader = () => settings.enemyAfterimageOn ? 0 : 1
                }
            );
            return menus;

        }
    }
}