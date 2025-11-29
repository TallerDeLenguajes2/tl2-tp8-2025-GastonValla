using System.Text.Json;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
namespace MiApi.Data
{
    /// <summary>
    /// Clase de acceso a datos
    /// </summary>
    public class ProductoRepository
    {
        private readonly string ConnectionString = "Data Source=./Db/Tienda.db";
        private static readonly object _lock = new();
        /// <summary>
        /// Constructor de clase
        /// </summary>
        public ProductoRepository(){}
        /// <summary>
        /// Obtencion de una lista a partir de la db
        /// </summary>

        private List<Producto> ReadData()
        {
                List<Producto> retorno = new List<Producto>();
                Producto nuevoProducto;

                using (SqliteConnection connection = new SqliteConnection(ConnectionString))
                {
                    connection.Open();
                    using (SqliteCommand selectCmd = new SqliteCommand("SELECT * FROM productos;", connection))
                    using (SqliteDataReader reader = selectCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if((reader["IdProducto"]!=DBNull.Value)&&(reader["Precio"]!=DBNull.Value))
                            {
                                nuevoProducto = new Producto(reader["IdProducto"] == DBNull.Value? 0 : Convert.ToInt32(reader["IdProducto"]), 
                                                            (string?)reader["Descripcion"], 
                                                            (double)reader["Precio"]);
                                retorno.Add(nuevoProducto);
                            }
                        }
                    }
                    connection.Close();
                }
                return retorno;
        }
        /// <summary>
        /// Metodo de obtencion de datos de una tabla
        /// </summary>
        /// <returns></returns>
        public List<Producto> GetAll()
        {
            lock (_lock)
            {
                return ReadData();
            }
        }
        /// <summary>
        /// metodo para aniadir objetos a la db
        /// </summary>
        /// <param name="nuevo"></param>
        public bool Nuevo(string? DescripcionProd, double PrecioProd)
        {
            int filasAfectadas;
            lock (_lock)
            {
                using (SqliteConnection connection = new SqliteConnection(ConnectionString))
                {
                    connection.Open();
                    string insertQuery = "INSERT INTO productos (IdProducto, Descripcion, Precio) VALUES (@id,@desc,@precio)";
                    using (SqliteCommand insertCmd = new SqliteCommand(insertQuery, connection))
                    {
                        insertCmd.Parameters.AddWithValue("@id", MaxId()+1);
                        insertCmd.Parameters.AddWithValue("@desc", DescripcionProd);
                        insertCmd.Parameters.AddWithValue("@precio", PrecioProd);
                        filasAfectadas = insertCmd.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            return filasAfectadas > 0;
        }
        public bool Modificar(int? idProd, Producto nuevo)
        {
            int check;
            lock (_lock)
            {
                using (SqliteConnection connection = new SqliteConnection(ConnectionString))
                {
                    connection.Open();
                    string updateQuery = "UPDATE productos SET IdProducto=@id, Descripcion=@desc, Precio=@precio WHERE IdProducto=@idProducto";
                    using (SqliteCommand updateCmd = new SqliteCommand(updateQuery, connection))
                    {
                        updateCmd.Parameters.AddWithValue("@id", nuevo.IdProducto);
                        updateCmd.Parameters.AddWithValue("@desc", nuevo.Descripcion);
                        updateCmd.Parameters.AddWithValue("@precio", nuevo.Precio);
                        if(idProd!=null){updateCmd.Parameters.AddWithValue("@idProducto", idProd);}
                        else{updateCmd.Parameters.AddWithValue("@idProducto", 0);}
                        check = updateCmd.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            return check > 0;
        }
        public Producto? Buscar(int? idProd)
        {
            Producto? retorno;
            lock (_lock)
            {
                using (SqliteConnection connection = new SqliteConnection(ConnectionString))
                {
                    connection.Open();
                    string selectQuery = "SELECT IdProducto, Descripcion, Precio FROM productos WHERE IdProducto=@idProducto";
                    using (SqliteCommand selectCmd = new SqliteCommand(selectQuery, connection))
                    {
                        if(idProd!=null){selectCmd.Parameters.AddWithValue("@idProducto", idProd);}
                        else{selectCmd.Parameters.AddWithValue("@idProducto", 0);}
                        using (SqliteDataReader reader = selectCmd.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                retorno = new Producto(reader["IdProducto"] == DBNull.Value? 0 : Convert.ToInt32(reader["IdProducto"]),
                                                    (string?)reader["Descripcion"],
                                                    (double)reader["Precio"]);
                            }
                            else{retorno = null;}
                        }
                    }
                    connection.Close();
                }
            }
            return retorno;
        }
        public bool Borrar(int idProd)
        {
            int check;
            lock (_lock)
            {
                using (SqliteConnection connection = new SqliteConnection(ConnectionString))
                {
                    connection.Open();
                    string deleteQuery = $"DELETE FROM productos WHERE IdProducto=@id";
                    using (SqliteCommand deleteCmd = new SqliteCommand(deleteQuery, connection))
                    {
                        deleteCmd.Parameters.AddWithValue("@id",idProd);
                        check = deleteCmd.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            return check > 0;
        }
        public int MaxId()
        {
            int retorno;
            lock (_lock)
            {
                using (SqliteConnection connection = new SqliteConnection(ConnectionString))
                {
                    connection.Open();
                    string selectQuery = "SELECT IdProducto FROM productos ORDER BY IdProducto DESC LIMIT 1";
                    using (SqliteCommand selectCmd = new SqliteCommand(selectQuery, connection))
                    {
                        using (SqliteDataReader reader = selectCmd.ExecuteReader())
                        {
                            if(reader.Read()&&reader["IdProducto"]!=DBNull.Value)
                            {
                                retorno = Convert.ToInt32(reader["IdProducto"]);
                            }
                            else{retorno = 1;}
                        }
                    }
                    connection.Close();
                }
            }
            return retorno;
        }
    }
}