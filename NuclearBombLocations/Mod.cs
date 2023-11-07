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

namespace NuclearBombLocations
{
    public class Mod : StardewModdingAPI.Mod
    {

        public static Mod instance;

        internal static IMonitor ModMonitor { get; set; }

        internal static IModHelper ModHelper { get; set; }





        public override void Entry(IModHelper helper)
        {
            instance = this;


            Helper.Events.GameLoop.GameLaunched += OnGameLaunched;

            Helper.Events.Specialized.LoadStageChanged += OnLoadStageChanged;

           // Helper.Events.GameLoop.UpdateTicked += OnUpdateTicked;


            var harmony = new Harmony(ModManifest.UniqueID);

            harmony.PatchAll();

        }


        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            Mod.ModHelper = Helper;
            Mod.ModMonitor = Monitor;



            var sc = Helper.ModRegistry.GetApi<ISpaceCoreApi>("spacechase0.SpaceCore");

            sc.RegisterSerializerType(typeof(NuclearLocation));

            sc.RegisterSerializerType(typeof(ClairabelleLagoon));


        }



        private void OnLoadStageChanged(object sender, LoadStageChangedEventArgs e)
        {
            if (e.NewStage == LoadStage.CreatedInitialLocations || e.NewStage == LoadStage.SaveAddedLocations)
            {
                Game1.locations.Add(new ClairabelleLagoon(Helper.ModContent));

            }

        }


        /*
        private void OnUpdateTicked(object sender, EventArgs e)
            {


                if (Game1.gameMode == 3 && Game1.player.currentLocation is ClairabelleLagoon)
                {
                   
                    Vector2 playerTile;
                    playerTile = Game1.player.Tile;
                    if (Game1.player.currentLocation.lastTouchActionLocation.Equals(Vector2.Zero))
                    {
                        string touchActionProperty;
                        touchActionProperty = Game1.player.currentLocation.doesTileHaveProperty((int)playerTile.X, (int)playerTile.Y, "TouchAction", "Back");
                        Game1.player.currentLocation.lastTouchActionLocation = playerTile;
                        if (touchActionProperty != null)
                        {
                            Game1.player.currentLocation.performTouchAction(touchActionProperty, playerTile);
                        }
                    }
                    else if (!Game1.player.currentLocation.lastTouchActionLocation.Equals(playerTile))
                    {
                        Game1.player.currentLocation.lastTouchActionLocation = Vector2.Zero;
                    }
                    foreach (Farmer farmer in Game1.player.currentLocation.farmers)
                    {
                        Vector2 playerPos;
                        playerPos = farmer.Tile;
                        Vector2[] adjacentTilesOffsets;
                        adjacentTilesOffsets = Character.AdjacentTilesOffsets;
                        foreach (Vector2 offset in adjacentTilesOffsets)
                        {
                            Vector2 v;
                            v = playerPos + offset;
                        //if (Game1.player.currentLocation.objects.TryGetValue(v, out var obj))
                        //{
                        // obj.farmerAdjacentAction();

                        foreach (SerializableDictionary<Vector2, Object> foobar in Game1.player.currentLocation.objects)
                        {
                            farmerAdjacentAction(Game1.player.currentLocation);
                       }


                            ;

                           // }
                        }
                    }
                }

            }*/






        }
    }

