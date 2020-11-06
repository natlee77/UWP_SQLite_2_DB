  
  CREATE  TABLE IF NOT EXISTS Customers (
    Id        INTEGER    NOT NULL   PRIMARY KEY AUTOINCREMENT,  --pk
    FirstName TEXT   NOT NULL,
    LastName  TEXT   NOT NULL,
    Adress    TEXT   NOT NULL,
    City      TEXT   NOT NULL,
    PostCode  INTEGER    NOT NULL,
    Created   DATETIME   NOT NULL
    );

  CREATE TABLE IF NOT EXISTS Orders (
    Id         INTEGER  NOT NULL   PRIMARY KEY  AUTOINCREMENT,          --PK
    CustomerId INTEGER  NOT NULL,                                       --FK    
    ProductId  INTEGER  NOT NULL,                                       --FK
    Created    DATETIME NOT NULL,
    Quantity   INTEGER  NOT NULL,
    Description TEXT    NOT NULL,   
    Status      TEXT    NOT NULL,       
    

    FOREIGN KEY (CostumerId) REFERENCES   Costumers(Id), 
    FOREIGN KEY (ProductId)  REFERENCES   Products(Id), 
    );

  CREATE TABLE IF NOT EXISTS Products (
    Id          INTEGER   NOT NULL   PRIMARY KEY AUTOINCREMENT,         --PK
    Name        TEXT      NOT NULL,
    Description TEXT      NOT NULL,
    Price       money     NOT NULL,
    Status      TEXT      NOT NULL, 
   );
  
   
   CREATE TABLE IF NOT EXISTS Comments (
    Id         INTEGER    NOT NULL   PRIMARY KEY AUTOINCREMENT,         --PK
    OrderId    INTEGER    NOT NULL,                                     --FK
    Description TEXT      NOT NULL,
    Created    DATETIME   NOT NULL,

    FOREIGN KEY (OrderId)  REFERENCES   Orders(Id), 
   );
    

  

    
   
  