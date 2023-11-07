

using HarmonyLib;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using StardewValley.Enchantments;
using System;
using NuclearBombLocations;

namespace NuclearBombLocations
{
    [HarmonyPatch(typeof(Game1), nameof(Game1.getLocationFromNameInLocationsList))]
    internal static class Game1FetchDungeonInstancePatch
    {
      //  public static bool Prefix(string name, bool isStructure, ref GameLocation __result)
        //{
            //if (name.StartsWith(HellDungeon.BaseLocationName))
           // {
               // __result = HellDungeon.GetLevelInstance(name);
              //  return false;
           // }
            //if (name.StartsWith(BluebellaDungeon.BaseLocationName))
           // {
            //    __result = BluebellaDungeon.GetLevelInstance(name);
            //   return false;
            //}
           // if (name.StartsWith(SapphireVolcano.BaseLocationName))
           // {
            //    __result = SapphireVolcano.GetLevelInstance(name);
            //    return false;
           // }
          //  return true;

       // }
    }
}





[HarmonyPatch(typeof(Tree), nameof(Tree.performToolAction))]
    public static class TreeToolActionPatch
{
    public static void Prefix(Tree __instance, Tool t, int explosion)
    {
          
        if (t is Axe)
        {
           if (Game1.currentLocation is ClairabelleLagoon)
            {
                __instance.health.Value = 999999999999;
                Game1.player.Money -= 100;
                Game1.addHUDMessage(new HUDMessage("Ranger Anabelle tickets you for illegal wood harvesting in the national park! You got what was coming to you!!!", 1));
            }
        }
        else if ( explosion >= 0)
        {
            __instance.health.Value = 999999999999;
            Game1.player.Money -= 100;
            Game1.addHUDMessage(new HUDMessage("Ranger Anabelle tickets you for illegal wood harvesting in the national park! You got what was coming to you!!!", 1));

        }
    }
}

/*
[HarmonyPatch(typeof(Tree), nameof(Tree.performToolAction))]
public static class SevereTreeToolActionPatch
{
    public static void Prefix(Tree __instance, Tool t, int explosion)
    {

        if (t is Axe)
        {
            if (Game1.currentLocation is EmeraldForestShrine)
            {
                __instance.health.Value = 999999999999;
                Game1.player.Money -= 1000000000;
                Game1.player.health = 0;
                Game1.addHUDMessage(new HUDMessage("The fairies slaughter you for trying to cut wood in the Sacred Forest!!!", 1));
            }
        }
    }
}*/



