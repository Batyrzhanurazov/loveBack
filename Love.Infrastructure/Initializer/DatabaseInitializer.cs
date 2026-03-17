using System.Data;
using Dapper;

namespace Love.Infrastructure.Initializer;

public class DatabaseInitializer(IDbConnection connection)
{
    public async Task InitializeAsync()
    {
        var sql = """
                  CREATE TABLE IF NOT EXISTS users (
                      id       SERIAL PRIMARY KEY,
                      login    TEXT NOT NULL UNIQUE,
                      password TEXT NOT NULL,
                      role     TEXT NOT NULL
                  );

                  CREATE TABLE IF NOT EXISTS messages (
                      id      SERIAL PRIMARY KEY,
                      message TEXT NOT NULL,
                      is_used BOOLEAN NOT NULL DEFAULT FALSE
                  );
                  """;

        await connection.ExecuteAsync(sql);
    }
}