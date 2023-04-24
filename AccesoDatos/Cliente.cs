﻿using System;
using System.Data.SqlClient;
using System.Data;

namespace AccesoDatos
{
    public class Clientes
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public void NuevoCliente(Clientes cliente)
        {
            try
            {
                // Query para insertar un cliente nuevo //
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
                // Query para Borrar un cliente //
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
        public void ActualizarCliente(Clientes cliente)
        {
            try
            {
                // Query para Actualizar un cliente //
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
