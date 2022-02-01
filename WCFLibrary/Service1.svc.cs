using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

namespace WCFLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        static readonly string cs = @"server=localhost;userid=root;password=drogba11;database=booksdb";
        readonly MySqlConnection connection1 = new MySqlConnection(cs);

        public ServiceData ConnectionOpen()
        {
            ServiceData data = new ServiceData();
            try
            {
                connection1.Open();
                data.Result = true;
                return data;
            }
            catch (MySqlException ex)
            {

                data.Result = false;
                data.ErrorMessage = "Cannot establish connection with database";
                data.ErrorDetails = ex.ToString();
                try
                {
                    throw new FaultException<ServiceData>(data, data.ErrorMessage);
                }
                catch
                {
                    return data;
                }
            }
            catch (Exception ex)
            {
                data.Result = false;
                data.ErrorMessage = "Unknown error";
                data.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceData>(data, ex.ToString());
            }
        }

        public void DeleteBook(int bookId)
        {
            
            MySqlCommand cmd = new MySqlCommand("DeleteBook", connection1);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@param_Id", MySqlDbType.Int32).Value = bookId;
            try
            {
                connection1.Open();
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException)
            {
                try
                {
                    throw new FaultException<NotFoundFault>(new NotFoundFault("No such book"));

                }
                catch
                {
                    return;
                }
            }
            finally
            {
                connection1.Close();
            }
            
        }

        public Book GetBook(string bookName)
        {
            Book book = new Book();
            
            MySqlCommand cmd = new MySqlCommand("GetBook", connection1);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@param_name", MySqlDbType.VarChar).Value = bookName;
            try
            {
                connection1.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    book.Book_id = Convert.ToInt32(reader["Book_id"]);
                    book.Name = reader["Name"].ToString();
                    book.Price = Convert.ToInt32(reader["Price"]);
                }
            }
            catch (MySqlException ex)
            {
                try
                {
                    throw new FaultException<NotFoundFault>(new NotFoundFault(ex.Message));

                }
                catch
                {
                    return book;
                }
            }
            finally
            {
                connection1.Close();
            }
            
            return book;
        }
        private List<Book> books = new List<Book>();
        public List<Book> ListBooks()
        {
            lock (books)
            {
                List<Book> allBooks = new List<Book>();

                string cs = @"server=localhost;userid=root;password=drogba11;database=booksdb";
                using (MySqlConnection connection = new MySqlConnection(cs))
                {
                    string query = "select * from books;";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.CommandType = CommandType.Text;
                    try
                    {
                        connection.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        {
                            while (reader.Read())
                            {
                                Book book = new Book();
                                book.Book_id = Convert.ToInt32(reader["Book_id"]);
                                book.Name = reader["Name"].ToString();
                                book.Price = Convert.ToInt32(reader["Price"]);
                                allBooks.Add(book);
                                books.Add(book);
                            }
                        }
                    }
                    catch(MySqlException ex)
                    {
                        throw new FaultException<NotFoundFault>(new NotFoundFault(ex.Message));
                    }
                    finally
                    {
                        connection.Close();
                    }
                    return allBooks;
                }

            }
        }

        public void RentBook(User user, Book book)
        {
            string cs = @"server=localhost;userid=root;password=drogba11;database=booksdb";
            using (MySqlConnection connection = new MySqlConnection(cs))
            {
                MySqlCommand cmd = new MySqlCommand("RentBook", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                MySqlParameter UserIdParameter = new MySqlParameter
                {
                    ParameterName = "@param_userId",
                    Value = user.User_id
                };
                cmd.Parameters.Add(UserIdParameter);
                MySqlParameter BookIdParameter = new MySqlParameter
                {
                    ParameterName = "@param_bookId",
                    Value = book.Book_id
                };
                cmd.Parameters.Add(BookIdParameter);
                try
                {
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
                catch(MySqlException ex)
                {
                    try
                    {
                       throw new FaultException<NotFoundFault>(new NotFoundFault(ex.Message));

                    }
                    catch
                    {
                        return;
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void SaveBook(Book book)
        {
            string cs = @"server=localhost;userid=root;password=drogba11;database=booksdb";
            using (MySqlConnection connection = new MySqlConnection(cs))
            {
                MySqlCommand cmd = new MySqlCommand("SaveBook", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                MySqlParameter idParameter = new MySqlParameter
                {
                    ParameterName = "@param_Id",
                    Value = book.Book_id
                };
                cmd.Parameters.Add(idParameter);
                MySqlParameter nameParameter = new MySqlParameter
                {
                    ParameterName = "@param_name",
                    Value = book.Name
                };
                cmd.Parameters.Add(nameParameter);
                MySqlParameter priceParameter = new MySqlParameter
                {
                    ParameterName = "@param_price",
                    Value = book.Price
                };
                cmd.Parameters.Add(priceParameter);
                try
                {
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    try
                    {
                        throw new FaultException<DuplicatePrimaryKey>(new DuplicatePrimaryKey(ex.Message));

                    }
                    catch
                    {
                        return;
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void SaveUser(User user)
        {
            string cs = @"server=localhost;userid=root;password=drogba11;database=booksdb";
            using (MySqlConnection connection = new MySqlConnection(cs))
            {
                MySqlCommand cmd = new MySqlCommand("SaveUser", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                MySqlParameter nameParameter = new MySqlParameter
                {
                    ParameterName = "@param_name",
                    Value = user.Name
                };
                cmd.Parameters.Add(nameParameter);
                MySqlParameter phoneParameter = new MySqlParameter
                {
                    ParameterName = "@param_phone",
                    Value = user.PhoneNumber
                };
                cmd.Parameters.Add(phoneParameter);
                MySqlParameter passwordParameter = new MySqlParameter
                {
                    ParameterName = "@param_password",
                    Value = user.Password
                };
                cmd.Parameters.Add(passwordParameter);
                try
                {
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
                catch(MySqlException)
                {
                    try
                    {
                        throw new FaultException<DuplicatePrimaryKey>(new DuplicatePrimaryKey("This user is already in the database"));

                    }
                    catch
                    {
                        return;
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
