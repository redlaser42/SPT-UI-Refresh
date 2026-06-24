using EFT.UI;
using Fika.Core.UI.Custom;
using SPT.Reflection.Patching;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace UIRefresh.Patches
{
    internal class FIKA_OnlinePlayers_Patch : ModulePatch
    {

        protected override MethodBase GetTargetMethod()
        {
            return typeof(MenuScreen).GetMethod(nameof(MenuScreen.method_9));
        }

        [PatchPostfix]
        public static void Postfix(bool minimized)
        {
            Logger.LogError("Fired FIKA Postfix");

            GameObject PlayersOnlineWidget = GameObject.Find("Preloader UI/Preloader UI/BottomPanel/Content/TaskBar/Tabs/GroupPanel/");
                if (PlayersOnlineWidget != null)
                {
                    PlayersOnlineWidget.gameObject.transform.Find("Canvas").Find("Border").GetComponent<Image>().enabled = false;
                    PlayersOnlineWidget.gameObject.transform.Find("Canvas").Find("Border").Find("Frame").GetComponent<Image>().enabled = false;
                }
                else
                {
                    Logger.LogError("Players Online Widget could not be found.");
                }
                return;
        }
    }
}