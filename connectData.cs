using System;
using System.Data;
using System.Data.SqlClient;

namespace QuanLyNhanVien3
{
    class connectData
    {
        public SqlConnection conn;

        private string strCon = @"Data Source=DESKTOP-10V42VO\SQLEXPRESS;Initial Catalog=QuanLyNhanSu;Integrated Security=True;";

        // Mở kết nối
        public void connect()
        {
            try
            {
                if (conn == null)
                    conn = new SqlConnection(strCon);

                if (conn.State != ConnectionState.Open)
                    conn.Open();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi kết nối SQL: " + ex.Message);
            }
        }

        // Đóng kết nối
        public void disconnect()
        {
            try
            {
                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();
            }
            catch { }
        }

        // Thực thi câu lệnh SQL không trả về dữ liệu
        public bool exeSQL(string cmd)
        {
            try
            {
                if (conn == null || conn.State != ConnectionState.Open)
                    connect(); // đảm bảo kết nối mở

                using (SqlCommand sc = new SqlCommand(cmd, conn))
                {
                    sc.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception ex)
            {
                //Có thể log ex.Message nếu muốn debug
                return false;
            }
        }

        // Thực thi câu lệnh SQL trả về DataTable
        public DataTable getDataTable(string query)
        {
            try
            {
                if (conn == null || conn.State != ConnectionState.Open)
                    connect();

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
