using System;
using System.Threading;


/*
// Phase 1

// Defines withdraw and deposit with no thread safety
class BankAccount
{
    public int Balance;

    public BankAccount(int initialBalance)
    {
        Balance = initialBalance;
    }

    public bool Withdraw(int amount)
    {
        if (Balance >= amount)
        {
            Balance -= amount;
            Console.WriteLine($"Withdrew ${amount}. New balance: ${Balance}");
            return true;
        }
        return false;
    }

    public void Deposit(int amount)
    {
        Balance += amount;
        Console.WriteLine($"Deposited ${amount}. New balance: ${Balance}");
    }
}

class Program
{
    // Directly calls withdraw and deposit, risking race conditions
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
    // Implements lock for thread safety
    private readonly object _lock = new object(); 

    public BankAccount(int initialBalance)
    {
        Balance = initialBalance;
    }

    public bool Withdraw(int amount)
    {
        // Ensures atomicity ensuring only one thread can access at a time
        lock (_lock)
        {
            if (Balance >= amount)
            {
                Balance -= amount;
                Console.WriteLine($"Withdrew ${amount}. New balance: ${Balance}");
                return true;
            }
            return false;
        }
    }

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

class BankAccount
{
    public int Balance;
    public readonly object LockObject = new object();

    public BankAccount(int initialBalance)
    {
        Balance = initialBalance;
    }

    public bool Withdraw(int amount)
    {
        lock (LockObject)
        {
            if (Balance >= amount)
            {
                Balance -= amount;
                Console.WriteLine($"Withdrew ${amount}. New balance: ${Balance}");
                return true;
            }
            return false;
        }
    }

    public void Deposit(int amount)
    {
        lock (LockObject)
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
        bool gotFirstLock = false, gotSecondLock = false;

        try
        {
            // Attempts to acquire lock but times out if deadlock occurs
            gotFirstLock = Monitor.TryEnter(from.LockObject, TimeSpan.FromMilliseconds(100));
            if (!gotFirstLock)
            {
                Console.WriteLine($"Deadlock detected: Could not lock first account for ${amount}.");
                return;
            }
            // Creates delay (increases deadlock risk)
            Thread.Sleep(100); 

            gotSecondLock = Monitor.TryEnter(to.LockObject, TimeSpan.FromMilliseconds(100));
            if (!gotSecondLock)
            {
                Console.WriteLine($"Deadlock detected: Could not lock second account for ${amount}.");
                return;
            }

            if (from.Withdraw(amount))
            {
                to.Deposit(amount);
            }
        }
        finally
        {
            if (gotFirstLock) Monitor.Exit(from.LockObject);
            if (gotSecondLock) Monitor.Exit(to.LockObject);
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


//Phase 4

class BankAccount
{
    public int Id;
    public int Balance;
    public readonly object LockObject = new object();

    public BankAccount(int id, int initialBalance)
    {
        Id = id;
        Balance = initialBalance;
    }

    public bool Withdraw(int amount)
    {
        lock (LockObject)
        {
            if (Balance >= amount)
            {
                Balance -= amount;
                return true;
            }
            return false;
        }
    }

    public void Deposit(int amount)
    {
        lock (LockObject)
        {
            Balance += amount;
        }
    }
}

class Program
{
    static void Transfer(BankAccount from, BankAccount to, int amount)
    {
        // Determines the lock order based on id
        object firstLock = from.Id < to.Id ? from.LockObject : to.LockObject;
        object secondLock = from.Id < to.Id ? to.LockObject : from.LockObject;

        lock (firstLock)
        {
            // Simulates delay to ensure deadlock resolution works
            Thread.Sleep(50);
            lock (secondLock)
            {
                if (from.Withdraw(amount))
                {
                    to.Deposit(amount);
                    Console.WriteLine($"Transferred ${amount} from {from.Id} to {to.Id}");
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
        Thread t3 = new Thread(() => Transfer(account1, account2, 100));
        Thread t4 = new Thread(() => Transfer(account2, account1, 30));

        t1.Start();
        t2.Start();
        t3.Start();
        t4.Start();

        t1.Join();
        t2.Join();
        t3.Join();
        t4.Join();

        Console.WriteLine($"Final Balance: Account1: ${account1.Balance}, Account2: ${account2.Balance}");
    }
}




