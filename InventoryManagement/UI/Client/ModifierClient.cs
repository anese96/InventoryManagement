using InventoryManagement.Data.DTO;
using InventoryManagement.InterfacesServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventoryManagement.UI.Client
{
    public partial class ModifierClient : Form
    {
        private readonly FunctionUI _functionUI;
        private readonly IService<ClientDto> _service;
        private int _id;
        public ModifierClient(int id, FunctionUI functionUI, IService<ClientDto> service)
        {
            _id = id;   
            _functionUI = functionUI;
            _service = service;
            InitializeComponent();
        }
    }
}
