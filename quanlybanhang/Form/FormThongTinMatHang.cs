﻿using quanlybanhang.Model;
using quanlybanhang.Reponsitory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace quanlybanhang
{
    public partial class FormThongTinMatHang : Form
    {
        TextBox[] tbArr;
        IMatHangRepo repo = new MatHangRepo();
        List<MatHang> list = new List<MatHang>();
        public FormThongTinMatHang()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            { // nhập dữ liệu từ datagridview vào text tương ứng
                txbID.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                txbName.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                txbGiaBan.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                txbGiaNhap.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                txtQuantity.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
            }
            catch (Exception)
            {

            }
        }

        private void FormThongTinMatHang_Load(object sender, EventArgs e)
        {
            tbArr = new TextBox[] { txbName, txbGiaBan, txbGiaNhap, txtQuantity };
            list = repo.getAll(); // lấy danh sách mặt hàng
            // gán dữ liệu vào datagridview
            dataGridView1.Columns["Id"].DataPropertyName = "Id";
            dataGridView1.Columns["Code"].DataPropertyName = "Code";
            dataGridView1.Columns["Name"].DataPropertyName = "Name";
            dataGridView1.Columns["SalePrice"].DataPropertyName = "SalePrice";
            dataGridView1.Columns["PurchasePrice"].DataPropertyName = "PurchasePrice";
            dataGridView1.Columns["quantity"].DataPropertyName = "Quantity";
            dataGridView1.DataSource = list;
        }

        private void btnInput_Click(object sender, EventArgs e)
        {
            foreach (TextBox tb in tbArr) // mở khóa toàn bộ text để nhập dữ liệu
            {
                tb.Enabled = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            foreach (TextBox tb in tbArr) // reset toàn bộ text và đưa về trạng thái không cho nhập
            {
                tb.ResetText();
                tb.Enabled = false;
            }
            FormThongTinMatHang_Load(sender, e);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool valid = true; // kiểm tra điều kiện để lưu
            foreach (TextBox tb in tbArr)
            {
                if (tb.Text == null || tb.Text.Equals("")) 
                {
                    MessageBox.Show("Vui lòng nhập đủ thông tin");
                    tb.Focus();
                    valid = false; // điều kiện sai khi chưa nhập đủ thông tin
                    break;
                }
            }
            if (valid) // cho phép nhập khi đủ điều kiện
            {
                int MaxId = repo.getMaxId() + 1; // lấy id của sản phẩm cuối cùng 
                string code = "MH" + MaxId.ToString(); // tạo mã mặt hàng
                MatHang check = repo.getByCode(code); // kiểm tra xem mặt hàng có tồn tại với mã đã tạo không
                if (check != null)
                {
                    MessageBox.Show("Mặt hàng đã tồn tại");
                }
                else
                {
                    MatHang mh = new MatHang(code, txbName.Text, Int64.Parse(txbGiaBan.Text), Int64.Parse(txbGiaNhap.Text), Int32.Parse(txtQuantity.Text));
                    repo.create(mh);
                    btnCancel_Click(sender, e);
                    MessageBox.Show("Lưu thành công");
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            bool valid = true; //điều kiện để sửa
            foreach (TextBox tb in tbArr)
            {
                if (tb.Text == null || tb.Text.Equals(""))
                {
                    MessageBox.Show("Vui lòng nhập đủ thông tin");
                    tb.Focus();
                    valid = false; //chưa nhập đủ thông tin thì điều kiện = flase
                    break;
                }
            }
            if (valid) // cho phép sửa khi đã nhập đủ thông tin
            {
                MatHang check = repo.getByCode(txbID.Text);
                if (check == null)
                {
                    MessageBox.Show("Mặt hàng không tồn tại");
                }
                else
                {
                    MatHang mh = new MatHang(check.Id, txbID.Text, txbName.Text, Int64.Parse(txbGiaBan.Text), Int64.Parse(txbGiaNhap.Text), Int32.Parse(txtQuantity.Text));
                    repo.update(mh);
                    btnCancel_Click(sender, e);
                    MessageBox.Show("Sửa thành công");
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            MatHang check = repo.getByCode(txbID.Text); // kiểm tra mặt hàng có tồn tại hay không
            if (check == null)
            {
                MessageBox.Show("Mặt hàng không tồn tại");
            }
            else
            {
                repo.delete(check.Id);
                FormThongTinMatHang_Load(sender, e);
                btnCancel_Click(sender, e);
                MessageBox.Show("Xóa thành công");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
