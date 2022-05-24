# Vault
Portable password manager open source and completely offline.

[![download](https://img.shields.io/github/v/release/devpelux/vault?label=DOWNLOAD&sort=semver&style=for-the-badge)](https://github.com/devpelux/vault/releases/latest)
[![download_latest](https://img.shields.io/github/v/release/devpelux/vault?include_prereleases&label=LATEST%20RELEASE&sort=semver&style=for-the-badge)](https://github.com/devpelux/vault/releases)

![home](https://raw.githubusercontent.com/devpelux/vault/1.0.0-pre.4/Assets/home.png)


# Single file executable
The main application is a single `.exe` file, it does not require installation, it creates these files:
- A database file for each user with all the user data.
- A `config.json` file that will contain all the application settings.

You can move the executable file and the database file wherever you want and take all the passwords and data with you, the `config.json` file is not required but you can also copy that file to keep the settings.


# Sqlcipher encrypted database
The database engine used is [sqlcipher](https://github.com/sqlcipher/sqlcipher), an encrypted version of sqlite.  


# Open source
The source code is entirely hosted here, feel free to contribute via pull requests and sending issues.
This means that there are no hidden things, you can view the entire code, and check it.


# Completely offline
*"The only secure computer is a computer disconnected from the internet and shutdown!"*  
This application will never use internet to send, receive, or store data, and everything is completely offline.  
All data are yours! Only you have the control!
This also means that if you lost your data no one will be able to retrieve them anymore...
Just backup everything into an external pendrive, this app can be loaded also from pendrives.


# License
Copyright (C) 2020-2022 devpelux (Salvatore Peluso)  
Licensed under MIT license.

[![mit](https://raw.githubusercontent.com/devpelux/vault/1.0.0-pre.4/Assets/Mit.png)](https://github.com/devpelux/vault/blob/1.0.0-pre.4/LICENSE)
