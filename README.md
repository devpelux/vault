# Vault
Portable password manager open source and completely offline.

![home](https://raw.githubusercontent.com/devpelux/vault/1.0.0-pre.1/Assets/home.png)


# Single file executable
The main application is a single `.exe` file, it does not require installation, it creates only 2 files:
- A database file (`vault.db`), and saves all the passwords on this file.
- A `config.json` file that will contain all the application settings.

You can move the executable file and the database file into a pendrive and take all the passwords with you, the `config.json` file is not required but you can also copy that file to take keep the settings.


# Sqlcipher encrypted database
The database engine used is sqlcipher, an encrypted version of sqlite.


# Completely offline
*"The only secure computer is a computer disconnected from the internet and shutdown!"*  
This application will never use internet to send or receive data, everything is made completely offline.  
You can move everything into an external pendrive and shutdown your pc!


# License
Copyright (C) 2020-2022 devpelux (Salvatore Peluso)  
Licensed under MIT license.

[![mit](https://raw.githubusercontent.com/devpelux/vault/1.0.0-pre.1/Assets/Mit.png)](https://github.com/devpelux/vault/blob/1.0.0-pre.1/LICENSE)
