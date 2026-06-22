using InventoryManagement.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventoryManagement.UI.Login
{
    public partial class Login : Form
    {
        private readonly Repositorys.UserRepository _userRepository;
        private readonly AppDbContext _appDbContext;

        private TextBox txtUser, txtPassword;
        public Login(Repositorys.UserRepository userRepository, AppDbContext appDbContext)
        {
           
            InitializeComponent();
            _userRepository = userRepository;
            _appDbContext = appDbContext;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var user = _userRepository.Login(
                txtUser.Text.Trim(),
                txtPassword.Text);

            if (user == null)
            {
                MessageBox.Show(
                    "Nom d'utilisateur ou mot de passe incorrect.",
                    "Connexion",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtPassword.Clear();
                txtPassword.Focus();
                return;
            }

            CurrentUser.Login(user);

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
