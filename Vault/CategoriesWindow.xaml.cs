using FullControls;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using Vault.Core;
using Vault.CustomControls;

namespace Vault
{
    /// <summary>
    /// Finestra per la gestione delle categorie.
    /// </summary>
    public partial class CategoriesWindow : EWindow, IDialog
    {
        private List<Category> categories;

        public const int WindowID = 1;

        public const string OK = "CategoriesWindow.OK";


        public CategoriesWindow()
        {
            InitializeComponent();
        }

        public object GetResult() => OK;

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Session.Instance.CategoriesWindowsData != null)
            {
                Height = Session.Instance.CategoriesWindowsData.Height;
                Width = Session.Instance.CategoriesWindowsData.Width;
                Top = Session.Instance.CategoriesWindowsData.Top;
                Left = Session.Instance.CategoriesWindowsData.Left;
            }
            Reload();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (Session.Instance.CategoriesWindowsData != null)
            {
                Session.Instance.CategoriesWindowsData = Session.Instance.CategoriesWindowsData with
                {
                    Height = Height,
                    Width = Width,
                    Top = Top,
                    Left = Left,
                };
            }
            else
            {
                Session.Instance.CategoriesWindowsData = new WindowsData(WindowsDatas.NewID, Session.Instance.UserID, WindowID, Height, Width, Top, Left);
            }
        }

        private void Reload()
        {
            categories = VaultDB.Instance.Categories.GetRecords(Session.Instance.UserID);
            categories.Add(new Category(Categories.NewID, Session.Instance.UserID, "", true));
            CategoryList.ItemsSource = categories;
        }

        private void CategoryPreview_EditModeChanged(object sender, EditModeChangedEventArgs e)
        {
            if (!e.EditMode)
            {
                CategoryPreview categoryPreview = (CategoryPreview)sender;
                Category targetCategory = categories.Find(e => e.ID == categoryPreview.ID);
                if (categoryPreview.Label != "")
                {
                    if (targetCategory.ID == Categories.NewID)
                    {
                        VaultDB.Instance.Categories.AddRecord(targetCategory with { Label = categoryPreview.Label });
                        Reload();
                    }
                    else
                    {
                        VaultDB.Instance.Categories.UpdateRecord(targetCategory with { Label = categoryPreview.Label });
                    }
                }
                else
                {
                    if (targetCategory.ID != Categories.NewID)
                    {
                        if (categories.Count > 2)
                        {
                            if (!VaultDB.Instance.Categories.RemoveRecord(targetCategory.ID))
                            {
                                _ = new DialogWindow(new MessageWindow($"Eliminare prima tutto ciò che fa parte del gruppo {targetCategory.Label}!",
                                    "Errore", MessageBoxImage.Error)).Show();
                            }
                        }
                        else
                        {
                            _ = new DialogWindow(new MessageWindow($"Deve esistere almeno un gruppo!",
                                    "Errore", MessageBoxImage.Error)).Show();
                        }
                        Reload();
                    }
                    else categoryPreview.EditMode = true;
                }
            }
        }
    }
}
