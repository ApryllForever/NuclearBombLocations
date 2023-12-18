

using HarmonyLib;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using StardewValley.Enchantments;
using System;
using NuclearBombLocations;
using StardewModdingAPI;
using StardewValley.BellsAndWhistles;
using StardewValley.Events;
using StardewValley.Characters;

namespace NuclearBombLocations





{




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
            else if (explosion >= 0)
            {
                if (Game1.currentLocation is ClairabelleLagoon)
                {
                    __instance.health.Value = 999999999999;
                    Game1.player.Money -= 100;
                    Game1.addHUDMessage(new HUDMessage("Ranger Anabelle tickets you for illegal wood harvesting in the national park! You got what was coming to you!!!", 1));

                }
            }
        }
    }
    /*
    [HarmonyPatch(typeof(NPC), nameof(NPC.isGaySpouse))]
    public static class GayNPCPatch
    {
        public static void NPC__isGaySpouse__Prefix(
                  StardewValley.NPC __instance,
                  ref bool __result)
        {

            if (!__instance.Name.Equals("MermaidRangerMarisol"))
                return;

            if (__instance.Name.Equals("MermaidRangerMarisol") && Game1.player.isMale
                    )
            {
                __result = true;
            }
            else if
                (__instance.Name.Equals("MermaidRangerMarisol") && !Game1.player.isMale
                    )
            {
                __result = false;
            }
        }
    }   
    */

    
    [HarmonyPatch(typeof(QuestionEvent), nameof(QuestionEvent.setUp))]
    public static class QuestionEventCPatch
    {
        public static bool QuestionEvent_setUp_Prefix(int ___whichQuestion, ref bool __result)
        {
            if (___whichQuestion == 1)
            {
                if (Game1.player.spouse.Equals("MermaidRangerMarisol"))
                {
                    __result = true;
                    return false;
                }
                Response[] answers = new Response[]
                {
                    new Response("Yes", Game1.content.LoadString("Strings\\Events:HaveBabyAnswer_Yes")),
                    new Response("Not", Game1.content.LoadString("Strings\\Events:HaveBabyAnswer_No"))
                };

                if (!Game1.player.isMale)
                {
                    Game1.currentLocation.createQuestionDialogue(Game1.content.LoadString("Strings\\Events:HavePlayerBabyQuestion"), answers, new GameLocation.afterQuestionBehavior(ModEntry.answerPregnancyQuestion));
                }
                else
                {
                    Game1.currentLocation.createQuestionDialogue(Game1.content.LoadString("Strings\\Events:HavePlayerBabyQuestion_Adoption" ), answers, new GameLocation.afterQuestionBehavior(ModEntry.answerPregnancyQuestion));
                }
                Game1.messagePause = true;
                __result = false;
                return false;
            }
            return true;
        }

    }
    




    [HarmonyPatch(typeof(BirthingEvent), nameof(BirthingEvent.setUp))]
    public static class BirthingEventSetup
    {
        public static bool BirthingEvent_setUp_Prefix(ref bool ___isMale, ref string ___message, ref bool __result)
        {

            if (!Game1.player.spouse.Equals("MermaidRangerMarisol"))
                return false;

           
            Game1.player.CanMove = false;
            ___isMale = false;
            if (Game1.player.isMale)
            {
                ___message = Game1.content.LoadString("Strings\\Events:BirthMessage_Adoption", Lexicon.getGenderedChildTerm(___isMale));
            }
            else 
            {
                ___message = Game1.content.LoadString("Strings\\Events:BirthMessage_PlayerMother", Lexicon.getGenderedChildTerm(___isMale));
            }
           
            __result = false;
            return false;
        }
    }


    [HarmonyPatch(typeof(GameLocation), nameof(GameLocation.UpdateWhenCurrentLocation))]
    public static class Game1UpdateDungeonLocationsPatch
    {
        public static void Postfix(SlimeTent __instance, GameTime time)
        {
            //if (Game1.menuUp && !Game1.IsMultiplayer)
            //{
            //return;
            //}
            if (Game1.IsClient)
            {
                return;
            }
            //MermaidTrain mermaidTrain = new MermaidTrain();
            //mermaidTrain.Update(time);

            //SlimeTent.UpdateWhenCurrentLocation();

        }
    }

}