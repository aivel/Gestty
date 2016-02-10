using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GoogleApi;

namespace DesktopTestApp
{
    public partial class Form1 : Form
    {
        private GoogleApi.GoogleAuth _auth;
        private const string ClientId = "720428859311-vfc0maqhcrfq19htgi6sm9m5a79qch6q.apps.googleusercontent.com";
        private const string ClientSecret = "seElXR24k7laFWsPlWU6LqVD";
        //
        private const string SuccessCodePrefix = "Success code=";
        private const string FailCodePrefix = "Denied error=";

        public Form1()
        {
            InitializeComponent();

            _auth = new GoogleAuth(ClientId, ClientSecret, scopes: Scopes.Make(new []{Scopes.Calendar}));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var s = _auth.BuildRequestUrl();

            var flag = false;

            wb.DocumentTitleChanged += (o, args) =>
            {
                Text = wb.DocumentTitle;

                if (wb.DocumentTitle.StartsWith(SuccessCodePrefix))
                {
                    var successCode = wb.DocumentTitle.Substring(SuccessCodePrefix.Length);

                    var token = _auth.RequestToken(successCode);

                    if (flag) return;

                    MessageBox.Show(token, wb.DocumentTitle);
                    flag = true;
                }
            };

            wb.Navigate(s);
        }
    }
}
