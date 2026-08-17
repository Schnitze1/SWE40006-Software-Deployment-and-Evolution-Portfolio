using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;
using Serilog;

namespace SampleApp
{
    public class DeploymentPayload
    {
        public string AppName { get; set; }
        public string Version { get; set; }
        public DateTime Timestamp { get; set; }
        public string ExecutedBy { get; set; }
    }

    public class MainForm : Form
    {
        private Label lblHeader;
        private Label lblStatus;
        private Button btnAction;
        private TextBox txtLog;
        private Panel pnlCard;

        public MainForm()
        {
            InitializeComponent();
            ConfigureSerilog();
        }

        private void ConfigureSerilog()
        {
            string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app.log");
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(logPath)
                .CreateLogger();

            Log.Information("Serilog logger initialized successfully.");
        }

        private void InitializeComponent()
        {
            this.lblHeader = new Label();
            this.lblStatus = new Label();
            this.btnAction = new Button();
            this.txtLog = new TextBox();
            this.pnlCard = new Panel();

            this.SuspendLayout();

            // Form settings
            this.Text = "Sample Application - Deployment Suite (Distinction Edition)";
            this.Size = new Size(520, 440);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(245, 247, 250);

            // Header
            this.lblHeader.Text = "Sample Desktop Application";
            this.lblHeader.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            this.lblHeader.ForeColor = Color.FromArgb(30, 41, 59);
            this.lblHeader.Location = new Point(20, 20);
            this.lblHeader.AutoSize = true;

            // Card Panel
            this.pnlCard.Location = new Point(20, 60);
            this.pnlCard.Size = new Size(460, 80);
            this.pnlCard.BackColor = Color.White;
            this.pnlCard.BorderStyle = BorderStyle.FixedSingle;

            // Status Label inside Card
            this.lblStatus.Text = "Status: Ready (Newtonsoft.Json & Serilog Loaded)";
            this.lblStatus.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            this.lblStatus.ForeColor = Color.FromArgb(71, 85, 105);
            this.lblStatus.Location = new Point(15, 25);
            this.lblStatus.AutoSize = true;
            this.pnlCard.Controls.Add(this.lblStatus);

            // Action Button
            this.btnAction.Text = "Run Deployment Task";
            this.btnAction.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            this.btnAction.Size = new Size(200, 36);
            this.btnAction.Location = new Point(20, 155);
            this.btnAction.BackColor = Color.FromArgb(37, 99, 235);
            this.btnAction.ForeColor = Color.White;
            this.btnAction.FlatStyle = FlatStyle.Flat;
            this.btnAction.FlatAppearance.BorderSize = 0;
            this.btnAction.Click += new EventHandler(this.BtnAction_Click);

            // Log Textbox
            this.txtLog.Location = new Point(20, 205);
            this.txtLog.Size = new Size(460, 180);
            this.txtLog.Multiline = true;
            this.txtLog.ScrollBars = ScrollBars.Vertical;
            this.txtLog.ReadOnly = true;
            this.txtLog.Font = new Font("Consolas", 9.0f);
            this.txtLog.BackColor = Color.FromArgb(15, 23, 42);
            this.txtLog.ForeColor = Color.FromArgb(52, 211, 153);
            this.txtLog.Text = "[" + DateTime.Now.ToString("HH:mm:ss") + "] Application & Dependencies Initialized." + Environment.NewLine;

            // Add controls to Form
            this.Controls.Add(this.lblHeader);
            this.Controls.Add(this.pnlCard);
            this.Controls.Add(this.btnAction);
            this.Controls.Add(this.txtLog);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        public string StatusText
        {
            get => lblStatus.Text;
            set => lblStatus.Text = value;
        }

        public string LogText
        {
            get => txtLog.Text;
            set => txtLog.Text = value;
        }

        public void AppendLog(string message)
        {
            txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
            Log.Information(message);
        }

        private void BtnAction_Click(object sender, EventArgs e)
        {
            StatusText = "Status: Task Executed (JSON & Serilog Active)";
            lblStatus.ForeColor = Color.FromArgb(22, 163, 74);

            var payload = new DeploymentPayload
            {
                AppName = "SampleApp",
                Version = "1.0.0.0",
                Timestamp = DateTime.Now,
                ExecutedBy = Environment.UserName
            };

            // Demonstrate Newtonsoft.Json external dependency usage
            string jsonOutput = JsonConvert.SerializeObject(payload, Formatting.Indented);
            
            AppendLog("Executed deployment task successfully.");
            AppendLog($"Serialized JSON Payload:\n{jsonOutput}");
        }
    }
}
