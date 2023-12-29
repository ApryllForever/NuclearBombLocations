using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using SpaceCore.Events;
using SpaceCore.Interface;
using SpaceShared;
using SpaceShared.APIs;
using StardewModdingAPI;
using StardewModdingAPI.Enums;
using StardewModdingAPI.Events;
using StardewValley;
using xTile;
using xTile.Dimensions;
using xTile.Layers;
using xTile.ObjectModel;
using xTile.Tiles;
using Object = StardewValley.Object;
using System.Reflection;
using System.Threading;
using StardewValley.Menus;
using StardewValley.Locations;
using System.Xml.Linq;
using System.Runtime.Intrinsics.X86;
using StardewValley.Buildings;
using StardewValley.GameData.Buildings;
using SpaceCore.UI;
using StardewValley.Tools;
using StardewValley.Events;
using System.Timers;

namespace NuclearBombLocations
{
    public partial class Mod : StardewModdingAPI.Mod
    {

        public static Mod instance;

        internal static IMonitor ModMonitor { get; set; }

        internal static IModHelper ModHelper { get; set; }

        private readonly string PublicAssetBasePath = "Mods/SlimeTent";

        private float submergeTimer;

        private Texture2D submarineSprites;

        // public static Random myRand;

        public override void Entry(IModHelper helper)
        {
            instance = this;

            //var sc = Helper.ModRegistry.GetApi<ISpaceCoreApi>("spacechase0.SpaceCore");

            Helper.Events.GameLoop.GameLaunched += OnGameLaunched;

            Helper.Events.Specialized.LoadStageChanged += OnLoadStageChanged;

            Helper.Events.Content.AssetRequested += OnAssetRequested;

            //Helper.Events.GameLoop.DayEnding += OnDayEnding;

            Helper.Events.GameLoop.UpdateTicked += OnUpdateTicked;

            Helper.Events.Player.Warped += OnWarped;

            SpaceEvents.BeforeWarp += BeforeWarped;



            var harmony = new Harmony(ModManifest.UniqueID);

            harmony.Patch(
             original: AccessTools.Method(typeof(StardewValley.Menus.CarpenterMenu), nameof(StardewValley.Menus.CarpenterMenu.returnToCarpentryMenuAfterSuccessfulBuild)),
             postfix: new HarmonyMethod(typeof(Mod), nameof(Mod.ReturnMenu_Prefix))
          );

            harmony.Patch(
           original: AccessTools.Method(typeof(StardewValley.NPC), nameof(StardewValley.NPC.isGaySpouse)),
           prefix: new HarmonyMethod(typeof(Mod), nameof(Mod.NPC__isGaySpouse__Postfix))
        );
            /*

            harmony.Patch(
               original: AccessTools.Method(typeof(Utility), nameof(Utility.pickPersonalFarmEvent)),
               prefix: new HarmonyMethod(typeof(ModEntry), nameof(ModEntry.Utility_pickPersonalFarmEvent_Prefix))
            );
            harmony.Patch(
               original: AccessTools.Method(typeof(QuestionEvent), nameof(QuestionEvent.setUp)),
               prefix: new HarmonyMethod(typeof(ModEntry), nameof(ModEntry.QuestionEvent_setUp_Prefix))
            );

            harmony.Patch(
               original: AccessTools.Method(typeof(BirthingEvent), nameof(BirthingEvent.setUp)),
               prefix: new HarmonyMethod(typeof(ModEntry), nameof(ModEntry.BirthingEvent_setUp_Prefix))
            );

            harmony.Patch(
               original: AccessTools.Method(typeof(BirthingEvent), nameof(BirthingEvent.tickUpdate)),
               prefix: new HarmonyMethod(typeof(ModEntry), nameof(ModEntry.BirthingEvent_tickUpdate_Prefix))
            );  */



            harmony.PatchAll();

            HarmonyPatch_UntimedSpecialOrders.ApplyPatch(harmony, helper, Monitor);

        }


        public static bool NPC__isGaySpouse__Postfix(StardewValley.NPC __instance, ref bool __result)
        {
            if (!__instance.Name.Equals("MermaidRangerMarisol"))
                return true;

            Farmer spouse = __instance.getSpouse();
            if (spouse.IsMale)
                __result = true;

            else if (spouse != null)
                __result = false;


            return false;

        }




        private static void ReturnMenu_Prefix()
        {
            // Not our place, we don't care
            if (Game1.player.currentLocation is not ClairabelleLagoon)
            {
                return;
            }
            else
            {

                LocationRequest locationRequest = Game1.getLocationRequest("Custom_ClairabelleLagoon");
                locationRequest.OnWarp += delegate
                {
                    Game1.displayHUD = true;
                    Game1.player.viewingLocation.Value = null;
                    Game1.viewportFreeze = false;
                    Game1.viewport.Location = new Location(320, 1536);
                    // __instance.freeze = true;                                   will need reflection, is private
                    Game1.displayFarmer = true;
                    AnabelleConstructionMessage();
                };
                Game1.warpFarmer(locationRequest, Game1.player.TilePoint.X, Game1.player.TilePoint.Y, Game1.player.FacingDirection);

            }

        }


        public static void AnabelleConstructionMessage()
        {
            CarpenterMenu menu = new("MermaidRangerAnabelle");

            menu.exitThisMenu();



            Game1.player.forceCanMove();


            string text = "Strings\\StringsFromCSFiles:Annabelle.ConstructionBegin";


            string text2 = "Your work will begin tomorrow!!";


            //string[] array = ArgUtility.SplitBySpace(Blueprint.DisplayName);
            string text3 = "Your work will begin tomorrow!!!";



            Game1.DrawDialogue(Game1.getCharacterFromName("MermaidRangerAnabelle"), text);
        }


        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            Mod.ModHelper = Helper;
            Mod.ModMonitor = Monitor;



            var sc = Helper.ModRegistry.GetApi<ISpaceCoreApi>("spacechase0.SpaceCore");

            sc.RegisterSerializerType(typeof(NuclearLocation));

            sc.RegisterSerializerType(typeof(ClairabelleLagoon));

            sc.RegisterSerializerType(typeof(MermaidDugoutHouse));

            sc.RegisterSerializerType(typeof(SlimeTent));

            sc.RegisterSerializerType(typeof(NuclearSubmarinePen));

            sc.RegisterSerializerType(typeof(MermaidNuclearSub));

            sc.RegisterSerializerType(typeof(AtarraMountainTop));

            sc.RegisterSerializerType(typeof(NuclearSunkenShip));

            //sc.RegisterCustomProperty(typeof(Building), "[XmlInclude(typeof(SlimeTent))]", typeof(NetRef<SlimeTent>), AccessTools.Method(typeof(SlimeTent), nameof(SlimeTent.get_SlimeTent)), AccessTools.Method(typeof(SlimeTent), nameof(SlimeTent.set_SlimeTent)));

            // var spacecore = this.Helper.ModRegistry.GetApi<ISpaceCoreApi>("spacechase0.SpaceCore");

            //spacecore.AddEventCommand("NuclearMermaidSubmarineDive", PatchHelper.RequireMethod<Mod>(nameof(Mod.EventCommand_NuclearMermaidSubmarineDive)));


            
            Event.RegisterCommand("NuclearMermaidSubmarineDive", delegate
            {
                MermaidNuclearSub mermaidNuclearSub = new MermaidNuclearSub();
                mermaidNuclearSub.answerDialogueAction("SubmergeQuestion_Yes", LegacyShimsEvil.EmptyArray<string>());


            }

                 


            //Event.RegisterCommand("NuclearMermaidSubmarineDiving", delegate
            //{

            //}


                );



        }



        private void OnLoadStageChanged(object sender, LoadStageChangedEventArgs e)
        {
            if (e.NewStage == LoadStage.CreatedInitialLocations || e.NewStage == LoadStage.SaveAddedLocations)
            {
                Game1.locations.Add(new ClairabelleLagoon(Helper.ModContent));
                Game1.locations.Add(new SlimeTent(Helper.ModContent));
                Game1.locations.Add(new MermaidDugoutHouse(Helper.ModContent));
                Game1.locations.Add(new NuclearSubmarinePen(Helper.ModContent));
                Game1.locations.Add(new MermaidNuclearSub(Helper.ModContent));
                Game1.locations.Add(new AtarraMountainTop(Helper.ModContent));
                Game1.locations.Add(new NuclearSunkenShip(Helper.ModContent));
            }

        }

        public void OnAssetRequested(object sender, AssetRequestedEventArgs e)
        {
            /* {  Attempt at making building using tractor code. Failing.


                 e.Edit(editor =>
                 {
                     var data = editor.AsDictionary<string, BuildingData>().Data;

                     data["MermaidSlimeTent"] = new BuildingData
                     {
                         Name = "Slime Tent",
                         Description = "This expirimental Navy structure provides the perfect home for slimes on your farm!",
                         Texture = $"{this.PublicAssetBasePath}/SlimeTent",
                         BuildingType = typeof(SlimeHutch).FullName,
                         SortTileOffset = 1,

                         SourceRect = {
                         X = 0,
                         Y = 0,
                         Width = 48,
                         Height = 80
           },

                         Builder = "MermaidRangerAnabelle",
                         BuildCost = 29000,
                         BuildMaterials = new[]
                         {
                             new BuildingMaterial()
                             {
                                 ItemId =  "(O)335",
                                 Amount = 20,
                             },
                             new BuildingMaterial()
                              {
                                 ItemId = "(O)337",
                                 Amount = 1
                              },
                             new BuildingMaterial()
                             {
                                 ItemId = "(O)428",
                                 Amount = 10,
                             },
                         }.ToList(),
                         BuildDays = 1,

                         Size = new Point(3, 4),
                         HumanDoor = new Point(1, 2),
                         IndoorMap = "Custom_SlimeTentInside",
                         IndoorMapType = typeof(SlimeTent).FullName,


                         //CollisionMap = "XXXX\nXOOX"
                     };
                 });
             }
            */
            //if (e.NameWithoutLocale.IsEquivalentTo("Maps/Custom_SlimeTentInside"))
            // {



            //  e.Edit(asset =>
            // {
            //var editor = asset.AsMap();

            // Map sourceMap = this.Helper.ModContent.Load<Map>("SlimeTentInside.tmx");

            // sourceMap.

            //Sinz Idea
            //I would check out how Tractor Mod handles replacing the whole horse thing with Tractor thing, and that having a reference to the slimetent Building instance would give you the GameLocation instance that you can then mess with, though if you want to do netfield schenigans then you are in harmony patch land anyway
            //  the mapPath variable on GameLocation instance would be the Maps\\{ IndoorMap}
            // value which you can use to only modify Slime Tents but not Slime Hutches

            //editor.PatchMap(sourceMap, targetArea: new Microsoft.Xna.Framework.Rectangle(30, 10, 20, 20));
            //   });

            // }





            //if (e.NameWithoutLocale.IsEquivalentTo("Maps/Custom_SlimeTentInside"))
            // {
            //   this.Helper.ModContent.Load<Map>("Assets/Custom_SlimeTentInside.tmx");


            //}
            //Maps\Custom_SlimeTentInside


            // e.Edit(asset =>
            //{
            //var editor = asset.AsMap();

            // Map sourceMap = ModEntry.Helper.ModContent.Load<Map>("AtomicScienceSilo.tmx");
            //Map sourceMap2 = ModEntry.Helper.ModContent.Load<Map>("AtomicScienceSilo.tmx");
            //editor.PatchMap(sourceMap, targetArea: new Rectangle(30, 10, 20, 20));
            // });

        }


        private void OnUpdateTicked(object sender, EventArgs e)
        {

        }

        public static void EventCommand_NuclearMermaidSubmarineDive(Event instance, GameLocation location, GameTime time, string[] split)
        {


            instance.CurrentCommand++;

            ++instance.CurrentCommand;
        }

        public void BeforeWarped(object sender, EventArgsBeforeWarp e)
        {
            string Stop = "You may not go through this door. Marisol does not know if you can be trusted yet enough to enter here. Only those closest to her may go here.";

            if (e.WarpTargetLocation.Name.Equals("Custom_NuclearSubmarinePen") && !Game1.player.mailReceived.Contains("NuclearBombCP.Marisol10HeartInvite"))
            {
                Game1.drawObjectDialogue(Stop);
                e.Cancel = true;

            }


          
           
        }

        public void OnWarped(object sender, WarpedEventArgs e)
        {
            // if (e.OldLocation is MermaidDugoutHouse && e.NewLocation is NuclearSubmarinePen && !Game1.player.mailReceived.Contains("NuclearBombCP.Marisol10HeartInvite"))
            //  {

            //   return;




            // }

        }



    }
    }

