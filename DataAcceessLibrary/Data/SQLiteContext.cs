using DataAcceessLibrary.Models;

using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using System.IO;

namespace DataAcceessLibrary.Data
{


    public static class SQLiteContext
    {
        #region Configuration and Properties

        private static string _dbpath { get; set; }

        public static async void UseSqlite(string databaseName = "sqlite_upp2.db")//1 table eller flera????
        {
            // initiering path var DB ligger:
            await ApplicationData.Current.LocalFolder.CreateFileAsync(databaseName, CreationCollisionOption.OpenIfExists);
            _dbpath = $"Filename={Path.Combine(ApplicationData.Current.LocalFolder.Path, databaseName)}";

            using (var db = new SqliteConnection(_dbpath))
            {
                db.Open();

                var query  = "CREATE TABLE IF NOT EXISTS Customers (Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, FirstName TEXT NOT NULL, LastName TEXT NOT NULL, Adress TEXT NOT NULL, City TEXT NOT NULL, PostCode INTEGER NOT NULL, Created DATETIME NOT NULL );";
                var query1 = "CREATE TABLE IF NOT EXISTS Orders   ( Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, CustomerId INTEGER  NOT NULL, ProductId INTEGER  NOT NULL, Quantity INTEGER NOT NULL, Description TEXT NOT NULL, Status TEXT  NOT NULL, Created DATETIME NOT NULL , FOREIGE KEY (CostumerId) REFERENCES Costumer(Id),  FOREIGE KEY (ProductId)  REFERENCES Product(Id));";
                var query2 = "CREATE TABLE IF NOT EXISTS Products ( Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, Name TEXT NOT NULL, Description TEXT NOT NULL, Price  DECIMAL NOT NULL, Status TEXT  NOT NULL);  ";
                var query3 = "CREATE TABLE IF NOT EXISTS Comments ( Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, OrderId INTEGER NOT NULL, Description TEXT  NOT NULL, Created DATETIME NOT NULL, FOREIGE KEY (OrderId) REFERENCES Order(Id));  ";
               
                var cmd  = new SqliteCommand(query, db);
                var cmd1 = new SqliteCommand(query1, db);
                var cmd2 = new SqliteCommand(query2, db);
                var cmd3 = new SqliteCommand(query3, db);
                await cmd.ExecuteNonQueryAsync();// förvänta ingen tillbacka

                db.Close();
            }
        }
        #endregion



        #region Create Methods //skapa 

        public static async Task<long> CreateCustomerAsync(Customer customer)//skapad kund
        {
            long id = 0;//startar Id

            using (var db = new SqliteConnection(_dbpath))
            {
                db.Open();

                var query = "INSERT INTO Customers VALUES(null," +           //Id = id;
                    "@FirstName," +                                          //FirstName = firstName;
                    "@LastName," +                                           //LastName = lastName;
                    "@Adress," +                                             //Adress = adress;
                    "@City," +                                               //City = city;
                    "@PostCode," +                                           //PostCode = postCode;
                    "@Created);";                                            //Created = created;
                var cmd = new SqliteCommand(query, db);  //new inctance

                cmd.Parameters.AddWithValue("@FirstName", customer.FirstName);//++
                cmd.Parameters.AddWithValue("@LastName", customer.LastName);
                cmd.Parameters.AddWithValue("@Adress", customer.Adress);
                cmd.Parameters.AddWithValue("@City", customer.City);
                cmd.Parameters.AddWithValue("@PostCode", customer.PostCode);
                cmd.Parameters.AddWithValue("@Created", DateTime.Now);
                await cmd.ExecuteNonQueryAsync();


                cmd.CommandText = "SELECT last_insert_rowid()"; // retunera last insertad ID -64 bit/practical-32 bit
                                                                //= SELECT Id FROM Customers WHERE LastName=@LastName
                id = (long)await cmd.ExecuteScalarAsync();


                db.Close();
            }

            return id;
        }

        public static async Task<long> CreateOrderAsync(Order order)//skapad order
        {
            long id = 0;

            using (var db = new SqliteConnection(_dbpath))
            {
                db.Open();

                var query = "INSERT INTO Orders VALUES(null," +        //null =autoincrement
                    "@CustomerId," +
                    "@ProductId," +
                    "@Quantity," +
                    "@Description," +
                    "@Status," +
                    "@Created);";
                var cmd = new SqliteCommand(query, db);

                cmd.Parameters.AddWithValue("@CustomerId", order.CustomerId);
                cmd.Parameters.AddWithValue("@ProductId", order.ProductId);
                cmd.Parameters.AddWithValue("@Quantity", order.Quantity);
                cmd.Parameters.AddWithValue("@Description", order.Description);
                cmd.Parameters.AddWithValue("@Status", "new");
                cmd.Parameters.AddWithValue("@Created", DateTime.Now);
                await cmd.ExecuteNonQueryAsync();

                cmd.CommandText = "SELECT last_insert_rowid()"; //retunera ID
                id = (long)await cmd.ExecuteScalarAsync();

                db.Close();
            }

            return id;
        }


        public static async Task<long> CreateProductAsync(Product product)//done
        {
            long id = 0;

            using (var db = new SqliteConnection(_dbpath))
            {
                db.Open();
                //Id = id;
                //Name = name;
                //Description = description;
                //Price = price;
                //Status = status;
                var query = "INSERT INTO Products VALUES(null," +
                    "@Name," +
                    "@Description," +
                    "@Price," +
                    "@Status);";
                var cmd = new SqliteCommand(query, db);

                cmd.Parameters.AddWithValue("@Name", product.Name);
                cmd.Parameters.AddWithValue("@Description", product.Description);
                cmd.Parameters.AddWithValue("@Price", product.Price);
                cmd.Parameters.AddWithValue("@Status", "new");

                await cmd.ExecuteNonQueryAsync();

                cmd.CommandText = "SELECT last_insert_rowid()";  //retunera Id
                id = (long)await cmd.ExecuteScalarAsync();

                db.Close();
            }

            return id;
        }

        public static async Task CreateCommentAsync(Comment comment) //! retunera ingen , 
        {

            using (var db = new SqliteConnection(_dbpath))
            {
                db.Open();

                var query = "INSERT INTO Comments VALUES(NULL, @OrderId , @Description , @Created );";
                var cmd = new SqliteCommand(query, db);

                cmd.Parameters.AddWithValue("@OrderId", comment.OrderId);
                cmd.Parameters.AddWithValue("@Description", comment.Description);
                cmd.Parameters.AddWithValue("@Created", DateTime.Now);

                await cmd.ExecuteNonQueryAsync();//bara köra async

                db.Close();
            }
        }

        #endregion



        #region Get Methods  
        //hämta info beroende vad jag vill göra : 

        public static async Task<IEnumerable<Customer>> GetCustomers()//drop down lista med alla kunden;
        {
            var customers = new List<Customer>();

            using (var db = new SqliteConnection(_dbpath)) //stoppa in värde
            {
                db.Open();

                var query = "SELECT * FROM Customers"; // hämta alla kunder // kan ID , Name
                var cmd = new SqliteCommand(query, db);

                var result = await cmd.ExecuteReaderAsync();//förvänta info tillbacka

                if (result.HasRows)
                {
                    while (result.Read())
                    {
                        customers.Add(new Customer(result.GetInt64(0),  //Id
                            result.GetString(1),                        //FirstName
                            result.GetString(2),                        //LastName
                            result.GetString(3),                        //Adress
                            result.GetString(4),                        //City
                            result.GetInt32(5),                         //PostCode
                            result.GetDateTime(6)));                    //Created
                    }
                }
                //Id = id;
                //FirstName = firstName;
                //LastName = lastName;
                //Adress = adress;
                //City = city;
                //PostCode = postCode;
                //Created = created;

                db.Close();
            }

            return customers;
        }
        public static async Task<Customer> GetCustomerById(long id)
        {
            var customer = new Customer();//skapa tomt object for att kan return

            using (var db = new SqliteConnection(_dbpath))
            {
                db.Open();

                var query = "SELECT * FROM Customers WHERE Id = @Id"; //select by Id
                var cmd = new SqliteCommand(query, db);

                cmd.Parameters.AddWithValue("@Id", id);
                var result = await cmd.ExecuteReaderAsync();

                if (result.HasRows)
                {
                    while (result.Read())
                    {
                        customer = new Customer(result.GetInt64(0),
                            result.GetString(1),
                            result.GetString(2),
                            result.GetString(3),
                            result.GetString(4),
                            result.GetInt32(5),
                            result.GetDateTime(6));
                    }
                }

                //Id = id;
                //FirstName = firstName;
                //LastName = lastName;
                //Adress = adress;
                //City = city;
                //PostCode = postCode;
                //Created = created;
                db.Close();
            }

            return customer;
        }
        public static async Task<IEnumerable<string>> GetCustomerLastNames()
        {
            var customerlastnames = new List<string>();

            using (var db = new SqliteConnection(_dbpath))
            {
                db.Open();

                var query = "SELECT LastName FROM Customers";
                var cmd = new SqliteCommand(query, db);

                var result = await cmd.ExecuteReaderAsync();

                if (result.HasRows)
                {
                    while (result.Read())
                    {
                        customerlastnames.Add(result.GetString(0));
                    }
                }

                db.Close();
            }

            return customerlastnames;
        }
        public static async Task<Customer> GetCustomerByName(string name)
        {
            var customer = new Customer();

            using (var db = new SqliteConnection(_dbpath))
            {
                db.Open();

                var query = "SELECT * FROM Customers WHERE Name = @Name";
                var cmd = new SqliteCommand(query, db);

                cmd.Parameters.AddWithValue("@Name", name);
                var result = await cmd.ExecuteReaderAsync();

                if (result.HasRows)
                {
                    while (result.Read())
                    {
                        customer = new Customer(result.GetInt64(0),
                            result.GetString(1),
                            result.GetString(2),
                            result.GetString(3),
                            result.GetString(4),
                            result.GetInt32(5),
                            result.GetDateTime(6));
                    }
                }

                db.Close();
            }

            return customer;
        }
        public static async Task<long> GetCustomerIdByLastName(string lastname)
        {
            long customerid = 0;

            using (var db = new SqliteConnection(_dbpath))
            {
                db.Open();

                var query = "SELECT Id FROM Customers WHERE LastName = @LastName";
                var cmd = new SqliteCommand(query, db);

                cmd.Parameters.AddWithValue("@LastName", lastname);
                customerid = (long)await cmd.ExecuteScalarAsync();

                db.Close();
            }

            return customerid;
        }







        public static async Task<IEnumerable<Order>> GetOrders()
        {
            var orders = new List<Order>();

            using (var db = new SqliteConnection(_dbpath))
            {
                db.Open();

                var query = "SELECT * FROM Orders";
                var cmd = new SqliteCommand(query, db);

                var result = await cmd.ExecuteReaderAsync();

                if (result.HasRows)
                {
                    while (result.Read())
                    {//Id = id;
                     //CustomerId = customerId;
                     //ProductId = productId;
                     //Quantity = quantity;
                     //Description = description;
                     //Status = status;
                     //Created = created;
                        var order = new Order(result.GetInt64(0),
                            result.GetInt64(1),
                            result.GetInt64(2),
                            result.GetInt16(3),
                            result.GetString(4),
                            result.GetString(5),
                            result.GetDateTime(6));

                        order.Customer = await GetCustomerById(result.GetInt64(1));
                        order.Comments = await GetCommentsByOrderId(result.GetInt64(0));
                        //order.Status = await GetOrderByStatus(result.GetString(5));

                        orders.Add(order);
                    }
                }
                
                db.Close();
            }

            return orders;
        }

        public static async Task<ICollection<Comment>> GetCommentsByOrderId(long orderid)
        {
            var comments = new List<Comment>();

            using (var db = new SqliteConnection(_dbpath))
            {
                db.Open();

                var query = "SELECT * FROM Comments WHERE OrderId = @OrderId";
                var cmd = new SqliteCommand(query, db);

                cmd.Parameters.AddWithValue("@OrderId", orderid);
                var result = await cmd.ExecuteReaderAsync();

                if (result.HasRows)
                {
                    while (result.Read())
                    {
                        comments.Add(new Comment(result.GetInt64(0), 
                            result.GetInt64(1),
                            result.GetString(2),
                            result.GetDateTime(3)));

                        //Id = id;
                        //OrderId = orderId;
                        //Description = description;
                        //Created = created;
                    }
                }

                db.Close();
            }

            return comments;
        }
        public static async Task<ICollection<Order>> GetOrderByStatus(string status)
        {
            var orders = new List<Order>();

            using (var db = new SqliteConnection(_dbpath))
            {
                db.Open();

                var query = "SELECT * FROM Orders WHERE Status = @Status";
                var cmd = new SqliteCommand(query, db);

                cmd.Parameters.AddWithValue("@Status", status);
                var result = await cmd.ExecuteReaderAsync();

                if (result.HasRows)
                {
                    while (result.Read())
                    {
                        orders.Add(new Order(result.GetInt64(0),
                            result.GetInt64(1), 
                            result.GetInt64(2),
                            result.GetInt16(3),
                            result.GetString(4), 
                            result.GetString(5),
                            result.GetDateTime(6)));
                    }
                }

                db.Close();
            }

            return orders;
        }

        #endregion

        #region Urdate Metods





        public static async Task UpdateOrderByStatus()
        {
             

            using (var db = new SqliteConnection(_dbpath))
            {
                db.Open();

                var query = "UPDATE Orders SET  Status = aktive, Status = completed WHERE  Id = @Id"; //UPDATE table1 SET a = 6 WHERE d = 18 AND c = ‘drdhк’;
                                                                                                   // UPDATE table SET column_1 = new_value_1,  column_2 = new_value_2  WHERE    search_condition ORDER column_or_expression LIMIT row_count OFFSET offset;
                                                                                                   //UPDATE table1 SET d = 55 WHERE a = 1;//INSERT INTO table1  VALUES (5, ‘Сейтаридис’, ‘Грек’, 18);


                var cmd = new SqliteCommand(query, db);

               
                var result = await cmd.ExecuteNonQueryAsync();


                

                //  UPDATE table_name
                //SET column1 = value1, column2 = value2...., columnN = valueN
                //WHERE[condition];

                db.Close();
            }
           
        }                 

        
        #endregion

    }
}
