using Hasbro.TheGameOfLife.Car;
using Hasbro.TheGameOfLife.Shared;
using Hasbro.TheGameOfLife.TrafficJam;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Hasbro.TheGameOfLife.Editor
{
    public partial class GameDesignEditorWindow : OdinMenuEditorWindow
    {
        [Inject] private AppConfig AppConfig { get; set; }
        [Inject] private GeneralConfig GeneralConfig { get; set; }
        [Inject] private TrafficJamConfig TrafficJamConfig { get; set; }
        [Inject] private CarConfig CarConfig { get; set; }
        [Inject] private CarTargetFollowerConfig CarTargetFollowerConfig { get; set; }

        private readonly Color selectionColor = new Color(1f, 0.239f, 0.407f);

        private const string GameDesign ="Game Design";

        [MenuItem(ProjectPath.MenuItem + "/" + GameDesign)]
        private static void OpenMenu()
        {
            GetWindow<GameDesignEditorWindow>(GameDesign).Show();
        }

        #region Menu Tree
        protected override OdinMenuTree BuildMenuTree()
        {
            OdinMenuTree tree = new OdinMenuTree()
            {
                Config = new OdinMenuTreeDrawingConfig
                {
                    DefaultMenuStyle =
                    {
                        SelectedColorDarkSkin = selectionColor,
                        SelectedColorLightSkin = selectionColor
                    }
                }
            };

            DrawTree(tree);
            return tree;
        }

        private void DrawTree(OdinMenuTree tree)
        {
            this.InjectServices();

            // Game Values 
            tree.Add($"App Config", AppConfig, EditorIcons.GridBlocks);
            tree.Add($"General", GeneralConfig, EditorIcons.SettingsCog);

            // Traffic Jam
            string TrafficJam = "Traffic Jam";
            tree.Add(TrafficJam, TrafficJamConfig, EditorIcons.Car);
            tree.Add($"{TrafficJam}/Car Config", CarConfig, EditorIcons.SingleUser);
            tree.Add($"{TrafficJam}/Target Follower Config", CarTargetFollowerConfig, EditorIcons.ImageCollection);
            tree.Add($"{TrafficJam}/Computer", null, EditorIcons.PacmanGhost);
            
            // Sort
            tree.EnumerateTree().SortMenuItemsByName();
        }
        #endregion
    }
}
