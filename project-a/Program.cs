using System;
using System.Threading;


class BankAccount
{
    public int Id;
    public int Balance;
    public readonly object LockObject = new object(); // Lock for thread safety

    public BankAccount(int id, int initialBalance)
    {
        Id = id;
        Balance = initialBalance;
    }
}

class Program
{
    static void Transfer(BankAccount from, BankAccount to, int amount)
    {
        // Always lock accounts in ID order
        object firstLock = from.Id < to.Id ? from.LockObject : to.LockObject;
        object secondLock = from.Id < to.Id ? to.LockObject : from.LockObject;

        lock (firstLock)
        {
            Thread.Sleep(50); // Simulate delay
            lock (secondLock)
            {
                if (from.Balance >= amount)
                {
                    from.Balance -= amount;
                    to.Balance += amount;
                    Console.WriteLine($"Transferred ${amount} from Account {from.Id} to Account {to.Id}");
                }
            }
        }
    }

    static void Main()
    {
        var account1 = new BankAccount(1, 500);
        var account2 = new BankAccount(2, 300);

        Thread t1 = new Thread(() => Transfer(account1, account2, 50));
        Thread t2 = new Thread(() => Transfer(account2, account1, 30));

        t1.Start();
        t2.Start();

        t1.Join();
        t2.Join();

        Console.WriteLine($"Final Balance: Account1: ${account1.Balance}, Account2: ${account2.Balance}");
    }
}



/*

//Phase 1

class BankAccount
{
    public int Balance; // Shared resource (not thread-safe!)

    public BankAccount(int initialBalance)
    {
        Balance = initialBalance;
    }
}

class Program
{
    static void Transfer(BankAccount from, BankAccount to, int amount)
    {
        // No thread safety: multiple threads can modify Balance at the same time
        if (from.Balance >= amount)
        {
            from.Balance -= amount; // Deduct money from sender
            to.Balance += amount;   // Add money to receiver
            Console.WriteLine($"Transferred ${amount}");
        }
    }

    static void Main()
    {
        var account1 = new BankAccount(500);
        var account2 = new BankAccount(300);

        // Start two threads that modify shared resources at the same time
        Thread t1 = new Thread(() => Transfer(account1, account2, 50));
        Thread t2 = new Thread(() => Transfer(account2, account1, 30));

        t1.Start();
        t2.Start();

        t1.Join();
        t2.Join();

        Console.WriteLine($"Final Balance: Account1: ${account1.Balance}, Account2: ${account2.Balance}");
    }
}

*/

/*
//Phase 2

class BankAccount
{
    public int Balance;
    private readonly object _lock = new object(); // Mutex (lock) for thread safety

    public BankAccount(int initialBalance)
    {
        Balance = initialBalance;
    }

    // Withdraw money safely
    public bool Withdraw(int amount)
    {
        lock (_lock) // Prevents other threads from accessing this account
        {
            if (Balance >= amount)
            {
                Balance -= amount;
                Console.WriteLine($"Withdrew ${amount}. New balance: ${Balance}");
                return true;
            }
            Console.WriteLine($"Insufficient funds! Withdrawal of ${amount} failed.");
            return false;
        }
    }

    // Deposit money safely
    public void Deposit(int amount)
    {
        lock (_lock)
        {
            Balance += amount;
            Console.WriteLine($"Deposited ${amount}. New balance: ${Balance}");
        }
    }
}

class Program
{
    static void Transfer(BankAccount from, BankAccount to, int amount)
    {
        if (from.Withdraw(amount))
        {
            to.Deposit(amount);
        }
    }

    static void Main()
    {
        var account1 = new BankAccount(500);
        var account2 = new BankAccount(300);

        // Threads now operate safely without data corruption
        Thread t1 = new Thread(() => Transfer(account1, account2, 50));
        Thread t2 = new Thread(() => Transfer(account2, account1, 30));

        t1.Start();
        t2.Start();

        t1.Join();
        t2.Join();

        Console.WriteLine($"Final Balance: Account1: ${account1.Balance}, Account2: ${account2.Balance}");
    }
}

*/

/*
//Phase 3
using System;
using System.Threading;

class BankAccount
{
    public int Balance;
    public readonly object LockObject = new object(); // Unique lock for each account

    public BankAccount(int initialBalance)
    {
        Balance = initialBalance;
    }
}

class Program
{
    static void Transfer(BankAccount from, BankAccount to, int amount)
    {
        lock (from.LockObject) // First lock
        {
            Thread.Sleep(100); // Simulate processing delay

            lock (to.LockObject) // Second lock (this may cause deadlock)
            {
                if (from.Balance >= amount)
                {
                    from.Balance -= amount;
                    to.Balance += amount;
                    Console.WriteLine($"Transferred ${amount}");
                }
            }
        }
    }

    static void Main()
    {
        var account1 = new BankAccount(500);
        var account2 = new BankAccount(300);

        Thread t1 = new Thread(() => Transfer(account1, account2, 50));
        Thread t2 = new Thread(() => Transfer(account2, account1, 30));

        t1.Start();
        t2.Start();

        t1.Join();
        t2.Join();

        Console.WriteLine($"Final Balance: Account1: ${account1.Balance}, Account2: ${account2.Balance}");
    }
}


*/

/*
//Phase 4
using System;
using System.Threading;

class BankAccount
{
    public int Id;
    public int Balance;
    public readonly object LockObject = new object(); // Lock for thread safety

    public BankAccount(int id, int initialBalance)
    {
        Id = id;
        Balance = initialBalance;
    }
}

class Program
{
    static void Transfer(BankAccount from, BankAccount to, int amount)
    {
        // Always lock accounts in ID order
        object firstLock = from.Id < to.Id ? from.LockObject : to.LockObject;
        object secondLock = from.Id < to.Id ? to.LockObject : from.LockObject;

        lock (firstLock)
        {
            Thread.Sleep(50); // Simulate delay
            lock (secondLock)
            {
                if (from.Balance >= amount)
                {
                    from.Balance -= amount;
                    to.Balance += amount;
                    Console.WriteLine($"Transferred ${amount} from Account {from.Id} to Account {to.Id}");
                }
            }
        }
    }

    static void Main()
    {
        var account1 = new BankAccount(1, 500);
        var account2 = new BankAccount(2, 300);

        Thread t1 = new Thread(() => Transfer(account1, account2, 50));
        Thread t2 = new Thread(() => Transfer(account2, account1, 30));

        t1.Start();
        t2.Start();

        t1.Join();
        t2.Join();

        Console.WriteLine($"Final Balance: Account1: ${account1.Balance}, Account2: ${account2.Balance}");
    }
}

*/

