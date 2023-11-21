/*
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using xTile.Dimensions;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using StardewValley.BellsAndWhistles;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using xTile;
using StardewValley.Characters;
using StardewValley.Network;
using StardewValley.Objects;
using System.Linq;
using xTile.Tiles;
using StardewValley.Locations;
using Object = StardewValley.Object;
using StardewValley.GameData;
using StardewValley.Menus;
using StardewValley.Extensions;

namespace NuclearBombLocations
{
    [XmlType("Mods_ApryllForever_NuclearBombLocations_MermaidDugout")]
    public class MermaidDugout : NuclearLocation
    {
		static IModHelper Helper;

		public static IMonitor Monitor;


        private readonly NetEvent1Field<int, NetInt> rumbleAndFadeEvent = new NetEvent1Field<int, NetInt>();


        internal static void Setup(IModHelper Helper)
		{
            MermaidDugout.Helper = Helper;
			//Helper.Events.GameLoop.DayStarted += OnDayStarted;
		}

		public MermaidDugout() { }

		

		public MermaidDugout(IModContentHelper content)
        : base(content, "MermaidDugout", "MermaidDugout")
        {


		}

		

		protected override void initNetFields()
        {
            base.initNetFields();
            base.NetFields.AddField(rumbleAndFadeEvent); //genSeed
		}
       
        //static string EnterDungeon = "Do you wish to enter this forboding gate?";
        protected override void resetLocalState()
        {
			//this.seasonOverride = "spring";
			base.resetLocalState();

			//suspensionBridges.Clear();
			//SuspensionBridge bridge = new SuspensionBridge(44, 135);
			//suspensionBridges.Add(bridge);
			





            addCritter(new Crow(6, 13));
            addCritter(new Crow(11, 7));

            addCritter(new CrabCritter(new Vector2(48f, 5f) * 64f));
            addCritter(new CrabCritter(new Vector2(47f, 6f) * 64f));
            addCritter(new CrabCritter(new Vector2(76f, 5f) * 64f));


            addCritter(new CrabCritter(new Vector2(33f, 4f) * 64f));



        }

		public override void TransferDataFromSavedLocation(GameLocation l)
		{
			base.TransferDataFromSavedLocation(l);
		}




        private void addCrittersStartingAtTile(Vector2 tile, List<Critter> crittersToAdd)
        {
            if (crittersToAdd == null)
            {
                return;
            }

            int num = 0;
            HashSet<Vector2> hashSet = new HashSet<Vector2>();
            while (crittersToAdd.Count > 0 && num < 20)
            {
                if (hashSet.Contains(tile))
                {
                    tile = Utility.getTranslatedVector2(tile, Game1.random.Next(4), 1f);
                }
                else
                {
                    if (CanItemBePlacedHere(tile))
                    {
                        Critter critter = crittersToAdd.Last();
                        critter.position = tile * 64f;
                        critter.startingPosition = tile * 64f;
                        critters.Add(critter);
                        crittersToAdd.RemoveAt(crittersToAdd.Count - 1);
                    }

                    tile = Utility.getTranslatedVector2(tile, Game1.random.Next(4), 1f);
                    hashSet.Add(tile);
                }

                num++;
            }
        }

        public override void DayUpdate(int dayOfMonth)
        {
            base.DayUpdate(dayOfMonth);
          
        }





        public override void updateEvenIfFarmerIsntHere(GameTime time, bool skipWasUpdatedFlush = false)
		{
			base.updateEvenIfFarmerIsntHere(time, skipWasUpdatedFlush);
			rumbleAndFadeEvent.Poll();
			if (!Game1.currentLocation.Equals(this))
			{
				
			}
		}

		private void rumbleAndFade(int milliseconds)
		{
			rumbleAndFadeEvent.Fire(milliseconds);
		}

		private void performRumbleAndFade(int milliseconds)
		{
			if (Game1.currentLocation == this)
			{
				Rumble.rumbleAndFade(1f, milliseconds);
			}
		}

	


		static string NuclearShopDialogue = "Hey they love! what may I do for you today?";

	

		public override bool performAction(string action, Farmer who, Location tileLocation)
		{
			TemporaryAnimatedSprite sprite = new TemporaryAnimatedSprite();
			GameLocation location = new GameLocation();
			GameTime time = new GameTime();
			

			if (action == "RS.RCannon")
			{
				//int idNum = Game1.random.Next();
				//GameLocation location = Game1.currentLocation;
				//List < TemporaryAnimatedSprite > cannonSprites = new List<TemporaryAnimatedSprite>();
				//location.explode = 9f;


				//if (hissTime == 0f)
				//{
					//location.netAudio.StartPlaying("fuse");
					//Game1.pauseThenDoFunction(2000, cannonFire);
				Game1.flashAlpha = 1f;
				Game1.playSound("explosion");
				Game1.player.jump();
				Game1.player.xVelocity = -16f;

				//}
				//Game1.pauseThenDoFunction(100, cannonExplode);
				//int radius = 3;
				//RestStopLocations.Mod mod = new RestStopLocations.Mod();
				//var Game1_multiplayer = mod.Helper.Reflection.GetField<Multiplayer>(typeof(Game1), "multiplayer").GetValue();
				//multiplayer = Game1_multiplayer;
				//List<TemporaryAnimatedSprite> sprites = new List<TemporaryAnimatedSprite>();
				//var fuckMe = Game1.player.position.X;
				//Vector2 currentTile = new Vector2(Math.Min(map.Layers[0].LayerWidth - 1, Math.Max(0f, tileLocation.X - (float)radius)), Math.Min(map.Layers[0].LayerHeight - 1, Math.Max(0f, tileLocation.Y - (float)radius)));
				//sprites.Add(new TemporaryAnimatedSprite(23, 9999f, 6, 1, new Vector2(currentTile.X * 64f, currentTile.Y * 64f), flicker: false, (Game1.random.NextDouble() < 0.5) ? true : false)
				//{
				//light = true,
				//lightRadius = radius,
				//lightcolor = Color.Black,
				//alphaFade = 0.03f - (float)radius * 0.003f,
				//Parent = this
				//});
				//Game1_multiplayer.broadcastSprites(location, sprites);


				

				//{
					//location.temporarySprites.Add(new TemporaryAnimatedSprite(25, new Vector2(fuckMe + 3f, 0f *64F), Color.White, 8, flipped: false, 100f, 0, 64, 1f, 128));
				//}


					//new TemporaryAnimatedSprite("LooseSprites\\Cursors", new Microsoft.Xna.Framework.Rectangle(598, 1279, 3, 4), 53f, 5, 9, (placementTile * 64f + new Vector2(3f, 0f) * 64f), flicker: true, flipped: false, 5f, 0f, Color.Yellow, 4f, 0f, 0f, 0f, local: true);

			};
			if (action == "RS.LCannon")
			{
				Game1.flashAlpha = 1f;
				Game1.playSound("explosion");
				Game1.player.jump();
				Game1.player.xVelocity = 16f;
			}
			
				return base.performAction(action, who, tileLocation);
        }



        
        public override bool SeedsIgnoreSeasonsHere()
        {
            return true;
        }

        public override bool CanPlantSeedsHere(string itemId, int tileX, int tileY, bool isGardenPot, out string deniedMessage)
        {
			deniedMessage = string.Empty;
            return true;
        }

        public override bool CanPlantTreesHere(string itemId, int tileX, int tileY, out string deniedMessage)
        {
			deniedMessage = string.Empty;
            return true;
        }


        public override void UpdateWhenCurrentLocation(GameTime time)
		{
			GameLocation location = new GameLocation();
			base.UpdateWhenCurrentLocation(time);
			

			}

	

		public override void draw(SpriteBatch b)
		{

;
			base.draw(b);


		
			
		}

        public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
        {
            switch (base.getTileIndexAt(tileLocation, "Buildings"))
            {
                
                case 958:
                case 1080:
                case 1081:
                    base.ShowMineCartMenu(this, new Point(tileLocation.X, tileLocation.Y));
                    return true;
            }
            return base.checkAction(tileLocation, viewport, who);
        }


		public override void checkForMusic(GameTime time)
		{
			if (base.IsOutdoors && Game1.isMusicContextActiveButNotPlaying() && !Game1.IsRainingHere(this) && !Game1.eventUp)
			{
				if (Game1.random.NextDouble() < 0.003 && Game1.timeOfDay < 2100)
				{
					localSound("seagulls");
				}
				else if (Game1.isDarkOut() && Game1.timeOfDay < 2500)
				{
					Game1.changeMusicTrack("spring_night_ambient", track_interruptable: true);
				}
			}

			base.checkForMusic(time);
		}
		public override void cleanupBeforePlayerExit()
		{
			base.cleanupBeforePlayerExit();
			
			
		}

	}
}

*/