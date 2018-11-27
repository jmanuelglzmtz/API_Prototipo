# API_Prototipo

* Hay que crear una base de datos llamada TestDb, en el caso que no se cuente con un servidor de sql server hay que comentar la linea 32 y descomentar la linea 36 del archivo StartUp.cs 

* En el archivo StartUp.cs linea 32 se encuentra la siguiente linea:
        var connection = @"Server=localhost\SQLEXPRESS;Database=TestDb;Trusted_Connection=True;ConnectRetryCount=0";
  para conexiones locales de sql server express, se modifica cuando se requiera apuntar a una base de datos diferente
  
* Para creaer la tabla de usuarios se cuenta con el script users.sql ubicado en API_Prototipo\ScriptDB



