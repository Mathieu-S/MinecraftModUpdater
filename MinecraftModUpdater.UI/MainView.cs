using System;
using System.Windows.Forms;
using MinecraftModUpdater.Core.Services;

namespace MinecraftModUpdater.UI
{
    public partial class MainView : Form
    {
        private readonly ModService _modService;
        
        public MainView()
        {
            InitializeComponent();
            _modService = new ModService(@"E:\Bureau\Sandbox");
        }

        private async void refreshButton_Click(object sender, EventArgs e)
        {
            refreshButton.Enabled = false;
            searchModNameTextBox.Enabled = false;
            curseModListBox.Enabled = false;
            
            await _modService.RefreshModListAsync();
            curseModListBox.DataSource = _modService.Mods;
            
            refreshButton.Enabled = true;
            searchModNameTextBox.Enabled = true;
            curseModListBox.ClearSelected();
            curseModListBox.Enabled = true;
        }

        private void searchModNameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(searchModNameTextBox.Text))
            {
                curseModListBox.DataSource = _modService.Mods;
            }
            else
            {
                curseModListBox.DataSource = _modService.SearchByName(searchModNameTextBox.Text);
            }
        }

        private void selectedCurseMod(object sender, EventArgs e)
        {
            if (curseModListBox.SelectedIndex < 0)
            {
                installModButton.Enabled = false;
            }
            else
            {
                installModButton.Enabled = true;
            }
        }
    }
}