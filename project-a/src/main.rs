use std::sync::{Arc, Mutex};
use std::thread;

struct BankAccount {
    balance: i32,
}

fn transfer (from: Arc<Mutex<BankAccount>>, to: Arc<Mutex<BankAccount>>, amount: i32) {
    
}

fn main() {
    let account = Arc::new(Mutex::new(BankAccount { balance: 0 }));

    let cAccount = Arc::clone(&account);
    //let handle ::thread::spawn();

    let account = account.lock().unwrap();
    println!("balance: {}", account.balance);
}