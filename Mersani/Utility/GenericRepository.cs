//using Dapper;
//using Mersani.Utility.Exceptions;
//using Microsoft.Extensions.Configuration;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Mersani.Utility
//{
//    public abstract class GenericRepository<T> : IGenericRepo<T> where T : class
//    {
//        protected readonly IConfiguration configuration;
//        protected readonly Database database;
//        public GenericRepository(IConfiguration _configuration, Database _database)
//        {
//            configuration = _configuration;
//            database = _database;
//        }

//        public bool Add(T entity, string query)
//        {
//            throw new NotImplementedException();
//        }

//        public bool Delete(int id, string query)
//        {
//            throw new NotImplementedException();
//        }

//        public T Get(int id, string query)
//        {
//            throw new NotImplementedException();
//        }

//        public List<T> GetAll(string query)
//        {
//            var result = new List<T>();
//            try
//            {
//                var conn = database.GetConnection();
//                if (conn.State == ConnectionState.Closed)
//                {
//                    conn.Open();
//                }
//                if (conn.State == ConnectionState.Open)
//                {
//                    //var query = "SELECT * FROM GAS_CURRENCY";
//                    result = SqlMapper.Query<T>(conn, query).ToList();
//                }
//            }
//            catch (Exception)
//            {
//                throw new NotFoundException($"No Row Selected!");
//            }
//            return result;
//        }

//        public bool Update(T entity, string query)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
