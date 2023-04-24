using System.Data;
using System.Data.SqlClient;

namespace AccesoDatos
{
    public class ProductoExistencia
    {
        public void ACtualizarExistencia(SqlConnection con, SqlTransaction transaction, VentaDetalle concepto)
        {
            string query = "Update Existencias " +
                                "set Existencias = Existencia-@Cantidad " +
                                "where ProductoId = @Producto";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.CommandType= CommandType.Text;
                cmd.Transaction = transaction;

                cmd.Parameters.AddWithValue("@ProductoId", concepto.ProductoId);
                cmd.Parameters.AddWithValue("@Cantidad", concepto.Cantidad);
                cmd.ExecuteNonQuery();

            }
        }

        public void AgregarExistenciaENCero(SqlConnection con, SqlTransaction transaction, int productoId)
        {
            string query = "Insert Into Existencias (Existencia, ProductoId) VALUES (0, @ProductoId)";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Transaction= transaction;

                cmd.Parameters.AddWithValue("@ProductoId", productoId);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
