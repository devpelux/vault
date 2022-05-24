using FullControls.SystemComponents;
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
        /// Gets a value indicating the days after which the password is old.
        /// </summary>
        public const ulong OLD_PASSWORD_DAYS_AFTER = 90;

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
            //If there is an owner for this window, then center the window to the owner.
            if (Owner != null) WindowStartupLocation = WindowStartupLocation.CenterOwner;

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
            new WeakPasswordsWindow() { Owner = this }.ShowDialog();
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

                ReportDate.Text = Utility.FormatDate(DateTimeOffset.FromUnixTimeSeconds(lastReport.Timestamp));

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
            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            int total = DB.Instance.Passwords.Count();

            int duplicated = DB.Instance.Passwords.DuplicatedCount();
            int weak = DB.Instance.Passwords.WeakCount();
            int old = DB.Instance.Passwords.OldCount(Utility.UNIX_DAY_SECONDS * OLD_PASSWORD_DAYS_AFTER);
            int violated = DB.Instance.Passwords.ViolatedCount();

            if (lastReport == null)
            {
                lastReport = new Report(total, duplicated, weak, old, violated, timestamp);
                DB.Instance.Reports.Add(lastReport);
            }
            else
            {
                lastReport = new Report(lastReport.Id, total, duplicated, weak, old, violated, timestamp);
                DB.Instance.Reports.Update(lastReport);
            }
        }
    }
}
