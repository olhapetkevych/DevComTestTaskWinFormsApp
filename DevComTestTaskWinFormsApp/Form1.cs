using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DevComTestTaskWinFormsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private readonly SqlConnection con = new SqlConnection("Data Source=DESKTOP-2JIJ3JV;Initial Catalog=devcomtaskdb_1;Integrated Security=True");
        private int userId;
        private int orderId;

        private void Form1_Load(object sender, EventArgs e)
        {
            usersTableAdapter.Fill(devcomtaskdb_1DataSet.Users);
            ordersTableAdapter.Fill(devcomtaskdb_1DataSet.Orders);
        }

        private void MyDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs anError)
        {
            MessageBox.Show("Wrong data! Please input the right data!");
        }

        private void SaveUsersButton_Click(object sender, EventArgs e)
        {
            try
            {
                usersTableAdapter.Update(devcomtaskdb_1DataSet.Users);
                MessageBox.Show("Changes in table Users are saved succesfully");
            }
            catch (Exception)
            {
                MessageBox.Show("Cannot save wrong data! Changes in table Users are not saved!\n" +
                    "Please input the right data and try to save changes again!");
            }

        }

        private void DeleteUserButton_Click(object sender, EventArgs e)
        {
            if (userId == 0)
            {
                MessageBox.Show("Please select row to delete!");
                return;
            }
            try
            {
                var cmd = new SqlCommand("DELETE FROM Users WHERE UserID = @id", con);
                con.Open();
                cmd.Parameters.AddWithValue("@id", userId);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("User deleted successfully!");
                usersTableAdapter.Fill(devcomtaskdb_1DataSet.Users);
                userId = 0;
            }
            catch (SqlException)
            {
                MessageBox.Show("Cannot delete selected user! This user has related orders!");
            }
        }


        private void UsersDataGridView_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            userId = Convert.ToInt32(usersDataGridView.Rows[e.RowIndex].Cells[0].Value.ToString());
        }

        private void InsertOrderButton_Click(object sender, EventArgs e)
        {
            int orderUserID;
            try
            {
                orderUserID = Convert.ToInt32(userIDTextBox.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("Wrong user ID!");
                return;
            }
            double orderCost;
            try
            {
                orderCost = Convert.ToDouble(orderCostTextBox.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("Wrong order cost!");
                return;
            }
            try
            {
                con.Open();
                var myCmd = new SqlCommand("CheckOrder", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                myCmd.Parameters.AddWithValue("@userID", orderUserID);
                myCmd.Parameters.AddWithValue("@orderDate", orderDateTimePicker.Value);
                var returnParameter = myCmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;
                myCmd.ExecuteNonQuery();
                int result = Convert.ToInt32(returnParameter.Value);
                if (result != 0)
                {
                    MessageBox.Show("Only one order may be created for user per day!");
                    con.Close();
                    return;
                }                
                var cmd = new SqlCommand("INSERT INTO Orders (UserID, OrderDate, OrderCost, ItemsDescription, ShippingAddress) VALUES (@userID, @orderDate, @orderCost, @itemsDescription, @shippingAddress)", con);
                cmd.Parameters.AddWithValue("@userID", orderUserID);
                cmd.Parameters.AddWithValue("@orderDate", orderDateTimePicker.Value);
                cmd.Parameters.AddWithValue("@orderCost", orderCost);
                cmd.Parameters.AddWithValue("@itemsDescription", itemsDescriptionTextBox.Text);
                cmd.Parameters.AddWithValue("@shippingAddress", shippingAddressTextBox.Text);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Order created successfully");
                UpdateOrdersInformation();
            }
            catch (Exception)
            {
                MessageBox.Show("Cannot save wrong data! Changes in table Orders are not saved!\n" +
                    "Please input the right data and try to save changes again!");
            }       
            
        }

        private void UpdateOrderButton_Click(object sender, EventArgs e)
        {
            if (orderId == 0)
            {
                MessageBox.Show("Please select row to update!");
                return;
            }
            int orderUserID;
            try
            {
                orderUserID = Convert.ToInt32(userIDTextBox.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("Wrong user ID!");
                return;
            }
            double orderCost;
            try
            {
                orderCost = Convert.ToDouble(orderCostTextBox.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("Wrong order cost!");
                return;
            }
            try
            {
                con.Open();
                var cmd = new SqlCommand("UPDATE Orders SET UserID = @userID, OrderDate = @orderDate, OrderCost = @orderCost, ItemsDescription = @itemsDescription, ShippingAddress = @shippingAddress WHERE OrderID = @orderID", con);
                cmd.Parameters.AddWithValue("@userID", orderUserID);
                cmd.Parameters.AddWithValue("@orderDate", orderDateTimePicker.Value);
                cmd.Parameters.AddWithValue("@orderCost", orderCost);
                cmd.Parameters.AddWithValue("@itemsDescription", itemsDescriptionTextBox.Text);
                cmd.Parameters.AddWithValue("@shippingAddress", shippingAddressTextBox.Text);
                cmd.Parameters.AddWithValue("orderID", orderId);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Order updated successfully");
                orderId = 0;
                UpdateOrdersInformation();
            }
            catch (Exception)
            {
                MessageBox.Show("Cannot save wrong data! Changes in table Orders are not saved!\n" +
                    "Please input the right data and try to save changes again!");
            }
        }

        private void UpdateOrdersInformation()
        {
            ordersTableAdapter.Fill(devcomtaskdb_1DataSet.Orders);
            userIDTextBox.Text = orderCostTextBox.Text = itemsDescriptionTextBox.Text = shippingAddressTextBox.Text = "";
            orderDateTimePicker.Value = DateTime.Now;
        }

        private void OrdersDataGridView_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            orderId = Convert.ToInt32(ordersDataGridView.Rows[e.RowIndex].Cells[0].Value.ToString());
            userIDTextBox.Text = ordersDataGridView.Rows[e.RowIndex].Cells[1].Value.ToString();
            orderDateTimePicker.Value = (DateTime)ordersDataGridView.Rows[e.RowIndex].Cells[2].Value;
            orderCostTextBox.Text = ordersDataGridView.Rows[e.RowIndex].Cells[3].Value.ToString();
            itemsDescriptionTextBox.Text = ordersDataGridView.Rows[e.RowIndex].Cells[4].Value.ToString();
            shippingAddressTextBox.Text = ordersDataGridView.Rows[e.RowIndex].Cells[5].Value.ToString();
        }
    }
}
