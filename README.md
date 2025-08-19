# 🔐 Personal Password Manager (Console App - v1)

This is a simple **personal password manager** built in **C# (.NET Console Application)**.
It allows you to securely generate and store strong passwords locally, so you don’t have to rely on third-party managers like Google or 1Password.

The project is designed for **personal use only** (not yet production-ready), and acts as a **first step** toward building a scalable password manager that could later grow into a desktop or web app.

---

## 🚀 Features Implemented So Far

1. **Generate Strong Passwords**

   * Uses C#’s built-in `RNGCryptoServiceProvider` (cryptographically secure random generator).
   * Supports uppercase, lowercase, numbers, and special characters.
   * Password length is customizable.

2. **Store Passwords Securely in Memory**

   * Stores passwords in a local in-memory dictionary (key = account name, value = password).
   * Later, this can be swapped out for a database or file storage.

3. **Retrieve Stored Passwords**

   * You can search for a password by its account name.
   * Example: retrieve the Gmail password you generated earlier.

4. **Console Menu System**

   * Simple text-based interface with options to:

     * Initialize a vault
     * Generate password
     * Save password under an account name
     * View saved passwords
     * Exit program

---

## 🏗️ Project Structure (so far)

```
PasswordManager/
│
├── Program.cs       # Entry point of the console app (menu, user interaction)
├── PasswordGenerator.cs # Handles password generation logic
|-- VaultFileEnvelope.cs # Envelope with KDF info and the generated blob
|-- VaultModels.cs # Model the data we store before encryption
├── VaultFile.cs  # Manages saving & retrieving passwords (currently in memory)
```

---

## 📖 How It Works (Step by Step)

### 1. Start the Program

Run the console app. You’ll see a menu like:

```
My Password Manager
1. Init Vault
2. Add item
3. List items
4. Get item
5. Generate Password
0. Exit
Choose:
```

---

### 2. Initialize a Vault

* First time you run the app, choose option **1**.
* You’ll be asked to create a master password.
* This will create a vault file at:

```
~/.mypwm.vault.json
```
(That’s inside your user profile directory — so on Windows it might be C:\Users\<YourName>\.mypwm.vault.json.)

💡 This vault is where all your encrypted passwords will live.

---

### 3. Save a new password entry

* Select option **2**.
* Enter the master password created earlier.
* Enter an account name (e.g., `Gmail`).
* Enter the URL pointing to where the account is.
* Enter the username of the password to be created.
* Choose (y) to auto-generate a strong password.
* Choose (N) to type in your own password.

---

### 4. See saved items in your vault (but not passwords)

* Select option **3**.
* The program lists all accounts stored in the vault (but not their stored passwords):

```
Master Password: ************
- Gmail (https://mail.google.com) as johndoe@gmail.com
- GitHub (https://github.com) as johndoe_dev 
```
* So you know what accounts you’ve saved.
  
---

### 5. Get username and password for a specific account

* Select option **4**.
* Example:

```
Title to fetch: Gmail
Master password: ********
Username: johndoe@gmail.com
Password: yB$!kzvN#91@Mm3h  (consider copying manually)
```

---

### 6. Generate random password
* Select option **5**.
* Example:
```
Length (default 20): 32
=> tY6!qw$2Ns7j@KpL%R3uX9Vz
```
---

### 7. Exiting the program
* Select option **0**.
* Quits the program.

## 🔮 Future Improvements

* 🚀 Next Steps:

* Clipboard support — instead of printing the password, copy it to clipboard temporarily.

* Auto-lock — after X minutes of inactivity, require master password again.

* Better encryption key derivation — currently probably using AES + password directly; Introduce PBKDF2/Argon2 to make brute force attacks much harder.

* Optional sync — maybe later, put the vault in Dropbox/OneDrive/GitHub repo to allow access on multiple devices.

---

✅ With this version, you can **generate, store, and retrieve passwords** safely from your own machine — no cloud, no third-party, just you.

---
