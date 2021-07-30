using FullControls.SystemComponents;
using System;
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
    public partial class CategoriesWindow : FullWindow, FullControls.Common.IDialog
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
            WindowParams wp = WindowsSettings.Instance.GetWindowParams(nameof(CategoriesWindow), WindowParams.INVALID);
            if (wp.Height != -1) Height = wp.Height;
            if (wp.Width != -1) Width = wp.Width;
            if (wp.Top != -1) Top = wp.Top;
            if (wp.Left != -1) Left = wp.Left;

            WindowState = WindowsSettings.Instance.GetWindowState(nameof(CategoriesWindow), WindowState.Normal);

            Reload();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                WindowsSettings.Instance.SetWindowParams(nameof(CategoriesWindow), new(Height, Width, Top, Left));
            }
            if (WindowState != WindowState.Minimized)
            {
                WindowsSettings.Instance.SetWindowState(nameof(CategoriesWindow), WindowState);
            }
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState != WindowState.Minimized)
            {
                WindowsSettings.Instance.SetWindowState(nameof(CategoriesWindow), WindowState);
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
