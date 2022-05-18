﻿using FullControls.SystemComponents;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Vault.Core;
using Vault.Core.Database;
using Vault.Core.Database.Data;

namespace Vault
{
    /// <summary>
    /// Window for managing reports.
    /// </summary>
    public partial class ReportWindow : AvalonWindow
    {
        private Report? lastReport;

        /// <summary>
        /// Initializes a new <see cref="ReportWindow"/>.
        /// </summary>
        public ReportWindow()
        {
            InitializeComponent();
            lastReport = DB.Instance.Reports.GetAll().LastOrDefault();
        }

        /// <summary>
        /// Executed when the window is loaded.
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Reload();
        }

        /// <summary>
        /// Executed when the new report button is clicked.
        /// </summary>
        private void NewReport_Click(object sender, RoutedEventArgs e)
        {
            GenerateReport();
            Reload();
        }

        /// <summary>
        /// Executed when the weak passwords link is clicked.
        /// </summary>
        private void WeakPasswordsLink_MouseDown(object sender, MouseButtonEventArgs e)
        {
            new WeakPasswordsWindow().ShowDialog();
        }

        /// <summary>
        /// Reloads the window data.
        /// </summary>
        private void Reload()
        {
            if (lastReport != null)
            {
                ReportTotal.Text = lastReport.Total.ToString();
                ReportDuplicated.Text = lastReport.Duplicated.ToString();
                ReportWeak.Text = lastReport.Weak.ToString();
                ReportOld.Text = lastReport.Old.ToString();
                ReportViolated.Text = lastReport.Violated.ToString();

                DateTimeOffset time = DateTimeOffset.FromUnixTimeSeconds(lastReport.Timestamp);
                ReportDate.Text = Utility.FormatDate(time);

                double score = CalculateScore(lastReport);
                ReportScore.Text = score != -1 ? score.ToString() : "--";
            }
            else
            {
                ReportTotal.Text = "-";
                ReportDuplicated.Text = "-";
                ReportWeak.Text = "-";
                ReportOld.Text = "-";
                ReportViolated.Text = "-";
                ReportDate.Text = "-";
                ReportScore.Text = "--";
            }
        }

        /// <summary>
        /// Calculates a score from the specified report.
        /// </summary>
        private double CalculateScore(Report report)
        {
            int maxWarnings = Utility.Max(report.Duplicated, report.Weak, report.Old, report.Violated);
            return report.Total > 0 ? Math.Round(100 - (maxWarnings / (double)report.Total * 100)) : -1;
        }

        /// <summary>
        /// Generates a new report and saves it.
        /// </summary>
        private void GenerateReport()
        {
            long now = DateTimeOffset.Now.ToUnixTimeSeconds();

            int total = DB.Instance.Passwords.Count();

            int duplicated = DB.Instance.Passwords.DuplicatedCount();
            int weak = DB.Instance.Passwords.WeakCount();
            int old = DB.Instance.Passwords.OldCount(Utility.UnixDaySeconds);
            int violated = DB.Instance.Passwords.ViolatedCount();

            if (lastReport == null)
            {
                lastReport = new Report(total, duplicated, weak, old, violated, now);
                DB.Instance.Reports.Add(lastReport);
            }
            else
            {
                lastReport = new Report(lastReport.Id, total, duplicated, weak, old, violated, now);
                DB.Instance.Reports.Update(lastReport);
            }
        }
    }
}
