using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;


namespace AccesoDatos
{
    public class Venta
    {
        public int Id { get; set; }
        public int Folio { get; set; }
        public DateTime Fecha { get; set; }
        public int ClienteId { get; set; }
        public decimal Total { get; set; }
        public List<VentaDetalle> Conceptos { get; set; } = new List<VentaDetalle>();


        public SqlDataAdapter ObtenerVenta()
        {
            try
            {
                string query = "SELECT * FROM Ventas";
                SqlDataAdapter clientes = new SqlDataAdapter(query, Conexion.ConnectionString);

                return clientes;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void crearVenta(Venta venta)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Conexion.ConnectionString))
                {
                    SqlTransaction transaction;
                    con.Open();
                    transaction = con.BeginTransaction();

                    try
                    {
                        string query = "select top(1) Folio from Folios";
                        int folioActual = 0;
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Transaction = transaction;

                            if (!int.TryParse(cmd.ExecuteScalar().ToString(), out folioActual))
                            {
                                throw new Exception("Ocurrio un error al obtener el folio");
                            }
                        }



                        query = "INSERT INTO Ventas " +
                            "(Folio,Fecha,ClienteId,Total) " +
                            "VALUES " +
                            "(@Folio,@Fecha,@ClienteId,@Total);select scope_identity()";

                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Transaction = transaction;
                            cmd.Parameters.AddWithValue("@Folio", venta.Folio);
                            cmd.Parameters.AddWithValue("@Fecha", venta.Fecha);
                            cmd.Parameters.AddWithValue("@ClienteId", venta.ClienteId);
                            cmd.Parameters.AddWithValue("@Total", venta.Total);


                            if (!int.TryParse(cmd.ExecuteScalar().ToString(), out int idVenta))
                            {
                                throw new Exception("Ocurrio un error al obtener el id de la venta");
                            }
                            venta.Id = idVenta;
                        }

                        foreach (VentaDetalle concepto in venta.Conceptos)
                        {

                            query = "INSERT INTO VentasDetalle" +
                                    "(VentaId,ProductoId,Cantidad,Descripcion,PrecioUnitario,Importe) " +
                                    "VALUES" +
                                    "(@VentaId,@ProductoId,@Cantidad,@Descripcion,@PrecioUnitario,@Importe)";

                            using (SqlCommand cmd = new SqlCommand(query, con))
                            {
                                cmd.CommandType = CommandType.Text;
                                cmd.Transaction = transaction;

                                cmd.Parameters.AddWithValue("@VentaId", venta.Id);
                                cmd.Parameters.AddWithValue("@ProductoId", concepto.ProductoId);
                                cmd.Parameters.AddWithValue("@Cantidad", concepto.Cantidad);
                                cmd.Parameters.AddWithValue("@Descripcion", concepto.Descripcion);
                                cmd.Parameters.AddWithValue("@PrecioUnitario", concepto.PrecioUnitario);
                                cmd.Parameters.AddWithValue("@Importe", concepto.Importe);
                                cmd.ExecuteNonQuery();

                            }


                            query = "UPDATE Existencias " +
                                    "SET Existencia -= @Cantidad " +
                                    "WHERE ProductoId = @ProductoId";

                            using (SqlCommand cmd = new SqlCommand(query, con))
                            {
                                cmd.CommandType = CommandType.Text;
                                cmd.Transaction = transaction;

                                cmd.Parameters.AddWithValue("@ProductoId", concepto.ProductoId);
                                cmd.Parameters.AddWithValue("@Cantidad", concepto.Cantidad);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        query = "Update Folios set Folio = Folio + 1 ";

                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Transaction = transaction;

                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }

    }
}
