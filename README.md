# Vault

Portable password manager open source and completely offline forever.


## Single file executable

The main application is a single `.exe` file, it does not require installation, it creates only 2 files:
- A database file (`vault.db`), and saves all the passwords on this file.
- A `config.json` file that will contain all the application settings.

You can move the executable file and the database file into a pendrive and take all the passwords with you, the `config.json` file is not required but you can also copy that file to take keep the settings.


## Double password

The entire database file is encrypted with sqlcipher encryption technology, and every single record inside the database is also encrypted with Rijndael algorithm.


## Completely offline

*"The only secure computer is a computer disconnected from the internet and shutdown!"*  
This application will never use internet to send or receive data, everything is made completely offline.  
And you can move everything into an external pendrive and shutdown your pc.



<br><br>
### License
Copyright (C) 2020-2021 devpelux (Salvatore Peluso)  
Licensed under MIT license.  

[![mit](https://upload.wikimedia.org/wikipedia/commons/thumb/0/0c/MIT_logo.svg/64px-MIT_logo.svg.png)][license]



[license]: https://github.com/devpelux/vault/blob/main/LICENSE "Licensed under MIT license"