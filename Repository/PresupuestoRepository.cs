using System.Text.Json;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
namespace MiApi.Data
{
    /// <summary>
    /// Clase de acceso a datos
    /// </summary>
    public class PresupuestoRepository
    {
        private readonly string ConnectionString = "Data Source=./Db/Tienda.db";
        private static readonly object _lock = new();
        /// <summary>
        /// Constructor de clase
        /// </summary>
        public PresupuestoRepository(){}
        /// <summary>
        /// Obtencion de una lista a partir de la db
        /// </summary>

        private List<Presupuesto> ReadData()
        {
                List<Presupuesto> retorno = new List<Presupuesto>();
                Presupuesto nuevoPresupuesto;

                using (SqliteConnection connection = new SqliteConnection(ConnectionString))
                {
                    connection.Open();
                    using (SqliteCommand selectCmd = new SqliteCommand("SELECT * FROM presupuestos;", connection))
                    using (SqliteDataReader reader = selectCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if((reader["IdPresupuesto"]!=DBNull.Value)&&(reader["FechaCreacion"]!=DBNull.Value))
                            {
                                nuevoPresupuesto = new Presupuesto(reader["IdPresupuesto"] == DBNull.Value? 0 : Convert.ToInt32(reader["IdPresupuesto"]), 
                                                            (string?)reader["NombreDestinatario"], 
                                                            (string?)reader["FechaCreacion"]);
                                retorno.Add(nuevoPresupuesto);
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
        public List<Presupuesto> GetAll()
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
        public bool Nuevo(string? NombrePres)
        {
            int filasAfectadas;
            lock (_lock)
            {
                using (SqliteConnection connection = new SqliteConnection(ConnectionString))
                {
                    connection.Open();
                    string insertQuery = "INSERT INTO presupuestos (IdPresupuesto, NombreDestinatario, FechaCreacion) VALUES (@id,@nombre,@fecha)";
                    using (SqliteCommand insertCmd = new SqliteCommand(insertQuery, connection))
                    {
                        insertCmd.Parameters.AddWithValue("@id", MaxId()+1);
                        insertCmd.Parameters.AddWithValue("@nombre", NombrePres);
                        insertCmd.Parameters.AddWithValue("@fecha", DateTime.Now.ToString("yyyy-MM-dd"));
                        filasAfectadas = insertCmd.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            return filasAfectadas > 0;
        }

        /// <summary>
        /// metodo para aniadir productos a un presupuesto
        /// </summary>
        /// <param name="nuevo"></param>
        public bool AgregarProducto(int? IdPresupuesto, int? IdProducto, int? Cantidad)
        {
            int filasAfectadas;
            lock (_lock)
            {
                using (SqliteConnection connection = new SqliteConnection(ConnectionString))
                {
                    connection.Open();
                    string insertQuery = "INSERT INTO presupuestoDetalle (IdPresupuesto, IdProducto, Cantidad) VALUES (@id,@idProd,@cant)";
                    using (SqliteCommand insertCmd = new SqliteCommand(insertQuery, connection))
                    {
                        insertCmd.Parameters.AddWithValue("@id", IdPresupuesto);
                        insertCmd.Parameters.AddWithValue("@idProd", IdProducto);
                        insertCmd.Parameters.AddWithValue("@cant", Cantidad);
                        filasAfectadas = insertCmd.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            return filasAfectadas > 0;
        }

        public bool Modificar(int? idPres, Presupuesto nuevo)
        {
            int check;
            lock (_lock)
            {
                using (SqliteConnection connection = new SqliteConnection(ConnectionString))
                {
                    connection.Open();
                    string updateQuery = "UPDATE presupuestos SET IdPresupuesto=@id, NombreDestinatario=@nombre, FechaCreacion=@fecha WHERE IdPresupuesto=@idPresupuesto";
                    using (SqliteCommand updateCmd = new SqliteCommand(updateQuery, connection))
                    {
                        updateCmd.Parameters.AddWithValue("@id", nuevo.IdPresupuesto);
                        updateCmd.Parameters.AddWithValue("@nombre", nuevo.NombreDestinatario);
                        updateCmd.Parameters.AddWithValue("@fecha", nuevo.FechaCreacion);
                        if(idPres!=null){updateCmd.Parameters.AddWithValue("@idPresupuesto", idPres);}
                        else{updateCmd.Parameters.AddWithValue("@idPresupuesto", 0);}
                        check = updateCmd.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            return check > 0;
        }
        public Presupuesto? Buscar(int? idPres)
        {
            Presupuesto? retorno = null;
            lock (_lock)
            {
                using (SqliteConnection connection = new SqliteConnection(ConnectionString))
                {
                    connection.Open();
                    string selectQuery = 
                    "SELECT IdPresupuesto, NombreDestinatario, FechaCreacion, pd.IdProducto, p.Descripcion, p.Precio, pd.Cantidad FROM presupuestos LEFT JOIN PresupuestoDetalle pd USING (IdPresupuesto) LEFT JOIN Productos p USING (IdProducto) WHERE IdPresupuesto=@idPresupuesto";
                    bool bandera = false;
                    using (SqliteCommand selectCmd = new SqliteCommand(selectQuery, connection))
                    {
                        if(idPres!=null){selectCmd.Parameters.AddWithValue("@idPresupuesto", idPres);}
                        else{selectCmd.Parameters.AddWithValue("@idPresupuesto", 0);}
                        using (SqliteDataReader reader = selectCmd.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                if(bandera == false){
                                retorno = new Presupuesto(reader["IdPresupuesto"] == DBNull.Value? 0 : Convert.ToInt32(reader["IdPresupuesto"]),
                                                    (string?)reader["NombreDestinatario"],
                                                    (string?)reader["FechaCreacion"]);
                                bandera = true;
                                }
                                if(bandera==true)
                                {
                                    if(reader["Descripcion"]!=DBNull.Value)
                                    {
                                    retorno.Detalle.Add(new PresupuestoDetalle(new Producto(reader["IdProducto"] == DBNull.Value? 0 : Convert.ToInt32(reader["IdPresupuesto"]),
                                                                            (string?)reader["Descripcion"], 
                                                                            (double)reader["Precio"]),
                                                                            reader["Cantidad"]== DBNull.Value? 0 : Convert.ToInt32(reader["Cantidad"])));
                                    }
                                }
                            }
                        }
                    }
                    connection.Close();
                }
            }
            return retorno;
        }
        public bool Borrar(int? idPres)
        {
            int check;
            lock (_lock)
            {
                using (SqliteConnection connection = new SqliteConnection(ConnectionString))
                {
                    connection.Open();
                    string deleteQuery = $"DELETE FROM presupuestos WHERE IdPresupuesto=@id";
                    using (SqliteCommand deleteCmd = new SqliteCommand(deleteQuery, connection))
                    {
                        if(idPres!=null){deleteCmd.Parameters.AddWithValue("@id",idPres);}
                        else{deleteCmd.Parameters.AddWithValue("@id",0);}
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
                    string selectQuery = "SELECT IdPresupuesto FROM presupuestos ORDER BY IdPresupuesto DESC LIMIT 1";
                    using (SqliteCommand selectCmd = new SqliteCommand(selectQuery, connection))
                    {
                        using (SqliteDataReader reader = selectCmd.ExecuteReader())
                        {
                            if(reader.Read()&&reader["IdPresupuesto"]!=DBNull.Value)
                            {
                                retorno = Convert.ToInt32(reader["IdPresupuesto"]);
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