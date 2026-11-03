<div align="center">

# In-the-Hero-s-Shadow

[![Stars](https://img.shields.io/github/stars/miras2022/In-the-Hero-s-Shadow?style=flat-square)](https://github.com/miras2022/In-the-Hero-s-Shadow/stargazers)
[![Forks](https://img.shields.io/github/forks/miras2022/In-the-Hero-s-Shadow?style=flat-square)](https://github.com/miras2022/In-the-Hero-s-Shadow/network)
[![Issues](https://img.shields.io/github/issues/miras2022/In-the-Hero-s-Shadow?style=flat-square)](https://github.com/miras2022/In-the-Hero-s-Shadow/issues)
[![Watchers](https://img.shields.io/github/watchers/miras2022/In-the-Hero-s-Shadow?style=flat-square)](https://github.com/miras2022/In-the-Hero-s-Shadow/watchers)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg?style=flat-square)](https://opensource.org/licenses/MIT)
[![Web Demo](https://img.shields.io/badge/Play-Live%20Demo-success?style=flat-square&logo=unity)](https://idosgames.com/app/HEIA6MZ1/)

![C#](https://img.shields.io/badge/-C%23-555?style=flat-square&logo=c%23)
![Unity](https://img.shields.io/badge/-Unity-222?style=flat-square&logo=unity)
![URP](https://img.shields.io/badge/-Universal%20Render%20Pipeline-blue?style=flat-square)
![Solana](https://img.shields.io/badge/-Solana%20Web3-purple?style=flat-square)
![WebGL](https://img.shields.io/badge/-WebGL-orange?style=flat-square)

[🐛 Report Bug](https://github.com/miras2022/In-the-Hero-s-Shadow/issues) · [✨ Request Feature](https://github.com/miras2022/In-the-Hero-s-Shadow/issues) · [🎮 Play Online](https://idosgames.com/app/HEIA6MZ1/)

</div>

---

## 📋 Table of Contents

- [📖 About the Project](#-about-the-project)
- [🎮 Playable Demo](#-playable-demo)
- [📸 Screenshots](#-screenshots)
- [✨ Core Features & Mechanics](#-core-features--mechanics)
- [🗺️ World & Scene Architecture](#️-world--scene-architecture)
- [⚙️ Technical Stack](#️-technical-stack)
- [🚀 Installation & Setup](#-installation--setup)
- [💻 Usage & Development](#-usage--development)
- [🗺️ Roadmap](#-roadmap)
- [🤝 Contributing](#-contributing)
- [❓ FAQ](#-faq)
- [📄 License](#-license)
- [👤 Contact & Portfolio](#-contact--portfolio)

---

## 📖 About the Project

**In-the-Hero-s-Shadow** is an immersive, multi-scene narrative and interactive strategy game developed in **Unity** using the **Universal Render Pipeline (URP)** and modern C#. It blends rich atmospheric exploration across distinct medieval/fantasy districts with engaging minigames, dynamic zone management, and decentralized Web3 integration (via the iDosGamesSDK and Solana blockchain).

Designed with modular architecture, robust state management, and optimized WebGL performance, the project demonstrates advanced game development patterns, UI/UX design, custom minigame logic, and crypto wallet connectivity.

---

## 🎮 Playable Demo

You can play the latest production build of **In-the-Hero-s-Shadow** directly in your browser via WebGL:

👉 **[Play Online on iDosGames](https://idosgames.com/app/HEIA6MZ1/)**

---

## 📸 Screenshots

> *Explore dynamic environments, interactive zones, and engaging puzzle minigames.*

![Screenshot Placeholder](./screenshots/screenshot.png)

---

## ✨ Core Features & Mechanics

- **Interactive Minigames:**
  - **Confession Minigame:** Narrative-driven choice and dialogue puzzle mechanics (`ConfessionMinigame.cs`).
  - **Sequence Memory Game:** Cognitive pattern recall challenges (`SequenceMemoryGame.cs`).
  - **Tic-Tac-Toe:** Strategic tactical grid minigame (`TicTacToeManager.cs`).
  - **Card Battles / Management:** Dynamic card UI and deck/hand management (`CardTMP.cs`, `CardButton.prefab`).
  - **Crow Catcher:** Action-oriented timing and spawning arcade mechanics (`CrowCatcher.cs`, `CrowSpawner.cs`).
- **Zone & District Management:** Modular zone progression (`Zone.cs`, `ZoneManager.cs`) controlling player movement, unlocks, and interactions.
- **Web3 & Crypto Wallet Integration:** Seamless connection with Solana blockchain and IGC crypto wallet modules (`iDosGamesSDK`).
- **Modern Unity Input System:** Responsive cross-platform controls (`Controls.cs`, `Controls.inputactions`).
- **Polished UI & Audio Feedback:** TextMeshPro integration, dynamic main menus (`MainMenuUI.cs`), and scene transitions (`SceneLoader.cs`).

---

## 🗺️ World & Scene Architecture

The game is structured across multiple detailed Unity scenes (`Assets/Scenes/`):

| Scene Name | Description |
| :--- | :--- |
| `MainMenu.unity` | Main entry point with settings, profile, and navigation UI. |
| `Game.unity` | Core gameplay loop and game manager state orchestration. |
| `Castle.unity` | Royal castle environment and high-tier narrative zones. |
| `ArtisanQuarter.unity` | Bustling craft district with unique merchant interactions. |
| `Church.unity` | Sanctuary environment housing confession and spiritual minigames. |
| `FarmScene.unity` | Agricultural zone and resource management area. |
| `Market.unity` | Trading hub for items, cards, and economy transactions. |
| `taverna.unity` | Social hub featuring minigames and quests. |
| `EncounterMinigame.unity` | Dedicated arena for tactical encounters and challenges. |
| `CatScene.unity` | Special narrative interlude scene. |
| `WinScene.unity` / `LoseScene.unity` | Outcome screens with score tabulation and progression restart. |

---

## ⚙️ Technical Stack

- **Engine:** Unity (2022 LTS / 2023+ compatible)
- **Programming Language:** C# (.NET Standard)
- **Graphics / Rendering:** Universal Render Pipeline (URP), ShaderLab / HLSL
- **Input Management:** Unity Input System package
- **UI Framework:** Unity UGUI & TextMeshPro
- **Web3 SDK:** Solana Unity SDK & iDosGamesSDK Modules (Crypto Wallet IGC)
- **Build Target:** WebGL & Desktop Standalone

---

## ⚙️ Prerequisites

To open, modify, or build the project from source, ensure you have:

- **Unity Hub** & **Unity Editor** (Recommended version: `2022.3 LTS` or compatible with URP 17.2).
- **Git** for cloning the repository.
- **Node.js** (v18+) if working with auxiliary build tools or WebGL hosting templates.

---

## 🚀 Installation & Setup

1. **Clone the Repository:**
   ```bash
   git clone https://github.com/miras2022/In-the-Hero-s-Shadow.git
   cd In-the-Hero-s-Shadow
   ```
2. **Open in Unity:**
   - Launch **Unity Hub**.
   - Click **Add project from disk** and select the cloned `In-the-Hero-s-Shadow` directory.
   - Open the project in Unity Editor (let Unity resolve packages automatically).
3. **Open Starting Scene:**
   - Navigate to `Assets/Scenes/MainMenu.unity` in the Unity Project window.
   - Press **Play** in the Unity Editor to test the game.

---

## 💻 Usage & Development

### Building for WebGL
1. Go to **File > Build Settings**.
2. Select **WebGL** as the target platform (Switch Platform if necessary).
3. Ensure all required scenes are added to the Scenes in Build list (starting with `MainMenu`).
4. Click **Build & Run** to generate the WebGL bundle.

### Script Architecture Highlights
- **`GameManager.cs` / `GameManagerTMP.cs`:** Centralized singleton managing game states, scoring, and scene flow.
- **`ZoneManager.cs`:** Handles modular spatial zones and interaction triggers.
- **`iDosGamesSDK`:** Encapsulates Web3 wallet authentication and token transaction hooks.

---

## 🗺️ Roadmap

- [x] Core scene architecture (Castle, Market, Church, Taverna)
- [x] Interactive minigames (Confession, Tic-Tac-Toe, Sequence Memory)
- [x] Solana Web3 SDK & Crypto Wallet integration
- [x] WebGL deployment and optimization (`idosgames.com`)
- [ ] Additional character classes and skill trees
- [ ] Expanded quest lines and dynamic world events
- [ ] Enhanced multiplayer / leaderboard features via Solana smart contracts

See the [open issues](https://github.com/miras2022/In-the-Hero-s-Shadow/issues) for proposed features and known issues.

---

## 🤝 Contributing

Contributions are what make the open-source and indie developer community amazing. Any contributions or suggestions you make are **greatly appreciated**.

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

---

## ❓ FAQ

**Q: Can I play the game without downloading Unity?**
A: Yes! You can play the fully playable WebGL version directly in your browser at [idosgames.com](https://idosgames.com/app/HEIA6MZ1/).

**Q: What technologies were used to build this game?**
A: It is built in Unity using C#, Universal Render Pipeline (URP), TextMeshPro, the new Unity Input System, and integrated with Solana / iDosGamesSDK for Web3 capabilities.

**Q: How can I report a bug or suggest a feature?**
A: Please open an issue on the [GitHub Issues](https://github.com/miras2022/In-the-Hero-s-Shadow/issues) page.

---

## 📄 License

Distributed under the MIT License. See `LICENSE` for more information.

---

## 👤 Contact & Portfolio

**miras2022**
- GitHub: [@miras2022](https://github.com/miras2022)
- Playable Game: [iDosGames App](https://idosgames.com/app/HEIA6MZ1/)
- Repository: [GitHub - In-the-Hero-s-Shadow](https://github.com/miras2022/In-the-Hero-s-Shadow)

---

<div align="center">

Made with ❤️ and Unity by miras2022

</div>
