# 🛠️ Make Your Life Easier — Windows Utility Toolkit

**Make Your Life Easier** is a modern WPF app for Windows, built to automate installations, clean up the system, store credentials offline, and provide quick access to external tools — all through a smooth, responsive UI.

---

## ⚙️ Core Features

- 🧹 **System Cleanup**: Temp, Prefetch, Windows.old, Recycle Bin.
- 📦 **App Installer**: Uses `winget`, Dropbox & Google Drive.
- 🔐 **Credential Vault**: Offline manager with blur/hide support.
- 🌐 **Crack/Game Links**: Quick launcher for trusted external sites.
- 🪟 **Custom UI**: Animated message boxes, topmost alerts, draggable windows.

---

## 📂 Main Modules

| Module         | Role                                      |
|----------------|-------------------------------------------|
| `MainWindow`   | Navigation hub                            |
| `ClearTemp`    | Deep system cleaner                       |
| `InstallApps`  | Smart app downloader via winget           |
| `public_install` | ZIP-based installers w/ automation     |
| `private_password` | Secure credential viewer             |
| `CrackSite`    | External download launcher                |
| `CustomMessageBox` | Enhanced UI alerts                    |
| `info`         | App info + GitHub redirect                |

---

## 💻 Tech Stack

- **C#**, **.NET (WPF)**
- Async/await, Dispatcher, `System.IO`, `Process`, `HttpClient`
- ZIP handling, Powershell integration, registry tweaks

---

## 📃 License

🔗 [View EULA](https://thomasthanos.github.io/Make_your_life_easier/Licence/EULA.html)  
© 2025 *Make Your Life Easier*. All rights reserved.

## 🧪 Preview

![Preview](https://github.com/user-attachments/assets/ad5381f9-188e-470e-8a14-df47c07bbf0f)

