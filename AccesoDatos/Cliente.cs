using System;
using System.Data.SqlClient;
using System.Data;

namespace AccesoDatos
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public void NuevoCliente(Cliente cliente)
        {
            try
            {
                string query = "INSERT INTO Clientes" +
                    "(Nombre) " +
                    "VALUES" +
                    "(@Nombre)";

                using (SqlConnection con = new SqlConnection(query))
                {
                    SqlTransaction transaction = con.BeginTransaction();
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Transaction = transaction;

                        cmd.Parameters.AddWithValue("@Nombre", cliente.Nombre);

                        cmd.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void EliminarCliente(int id)
        {
            try
            {
                string query = "DELETE FROM Clientes where Id = @Id";

                using (SqlConnection con = new SqlConnection(query))
                {
                    SqlTransaction transaction = con.BeginTransaction();
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Transaction = transaction;

                        cmd.Parameters.AddWithValue("@Id", id);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void ActualizarCliente(Cliente cliente)
        {
            try
            {
                string query = "UPDATE Clientes SET Nombre = @Nombre";

                using (SqlConnection con = new SqlConnection(query))
                {
                    SqlTransaction transaction = con.BeginTransaction();
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Transaction = transaction;

                        cmd.Parameters.AddWithValue("@Nombre", cliente.Nombre);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
