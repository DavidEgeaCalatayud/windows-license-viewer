using System;
using System.Drawing;
using System.Windows.Forms;

namespace visualizador_License_Viewer
{
    public class AboutForm : Form
    {
        public AboutForm()
        {
            this.Text = "Acerca de Windows";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ClientSize = new Size(460, 480);
            this.Font = new Font("Segoe UI", 10);

            // Logo
            PictureBox logo = new PictureBox();
            logo.Image = SystemIcons.Information.ToBitmap();
            logo.SizeMode = PictureBoxSizeMode.StretchImage;
            logo.SetBounds(20, 20, 80, 80);

            Label title = new Label();
            title.Text = "Windows 11";
            title.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            title.SetBounds(120, 30, 300, 40);

            Label subtitle = new Label();
            subtitle.Text = "Microsoft Windows\nVersión 25H2 (compilación de SO 26200.6899)";
            subtitle.AutoSize = true;
            subtitle.SetBounds(120, 70, 320, 50);

            TextBox description = new TextBox();
            description.Multiline = true;
            description.ReadOnly = true;
            description.BorderStyle = BorderStyle.None;
            description.BackColor = this.BackColor;
            description.SetBounds(20, 120, 420, 250);
            description.Text =
                "El sistema operativo Windows 11 Enterprise y su interfaz de usuario " +
                "están protegidos por las leyes de marca comercial y otros derechos de propiedad " +
                "intelectual actuales y pendientes en los Estados Unidos y otros países o regiones.\r\n\r\n" +
                "La licencia de este producto se concede de acuerdo con los " +
                "Términos de licencia del software de Microsoft a:\r\n" +
                "Usuario de Windows";

            Button btnOk = new Button();
            btnOk.Text = "Aceptar";
            btnOk.SetBounds(350, 400, 90, 30);
            btnOk.Click += (s, e) => this.Close();

            this.Controls.Add(logo);
            this.Controls.Add(title);
            this.Controls.Add(subtitle);
            this.Controls.Add(description);
            this.Controls.Add(btnOk);
        }
    }
}
