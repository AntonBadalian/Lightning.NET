using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using LightningDB;

namespace SecondProcess;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            Expression.Lambda(Expression.Constant(42)).Compile();
            Console.WriteLine("JIT detected: dynamic compilation works");
        }
        catch (Exception)
        {
            Console.WriteLine("Native AOT detected: dynamic compilation not supported");
        }

        // Use a temp folder
        var tempPath = Path.Combine(Path.GetTempPath(), "lmdb_test");
        Directory.CreateDirectory(tempPath);

        using var env = new LightningEnvironment(tempPath);
        env.Open(EnvironmentOpenFlags.None);

        // Start a write transaction to insert test data
        using (var tx = env.BeginTransaction())
        {
            using var db = tx.OpenDatabase();
            tx.Put(db, Encoding.UTF8.GetBytes("hello"), Encoding.UTF8.GetBytes("world"));
            tx.Commit();
        }

        // Read it back
        using (var tx = env.BeginTransaction(TransactionBeginFlags.ReadOnly))
        {
            using var db = tx.OpenDatabase();
            var result = tx.Get(db, Encoding.UTF8.GetBytes("hello"));
            var value = result.value.CopyToNewArray();
            Console.WriteLine(Encoding.UTF8.GetString(value)); // prints "world"
            tx.Commit();
        }
    }
}