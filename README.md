<div align="center">

# 💎 Match-3 game

**A real-time competitive Match-3 game built with Unity and Photon Fusion.**

[![Unity](https://img.shields.io/badge/Unity-2022.3%2B-black?style=for-the-badge&logo=unity)](https://unity.com/)
[![Photon Fusion](https://img.shields.io/badge/Photon_Fusion-Shared_Mode-blue?style=for-the-badge)](https://doc.photonengine.com/fusion/current/getting-started/fusion-intro)
[![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)](#)
[![DOTween](https://img.shields.io/badge/DOTween-Pro-brightgreen?style=for-the-badge)](#)

<img width="692" height="388" alt="2026-06-01 17-28-15 (1)" src="https://github.com/user-attachments/assets/40dd08a1-5967-49a2-b669-899e52a6b719" />

</div>

<br/>

## 📖 About The Project

This game is a competitive take on the popular Match-3 genre, designed for both puzzle enthusiasts and fans of intense head-to-head matches.

You can choose the format that suits you best: launch the Solo Mode to play offline, peacefully collect crystals, trigger powerful bombs, and set personal records. Alternatively, challenge your friends in the Online Multiplayer Mode by connecting to a shared room! In multiplayer, every second counts: spot the best combinations faster than your opponent and score the maximum points before the match timer runs out.

## ✨ Key Features

* ⚔️ **Real-time Multiplayer:** Direct player connection via a custom room-based lobby.
* 🛡️ **Cheat Prevention (State Authority):** Scores, timers, and end-game logic are strictly controlled by the State Authority. Clients cannot manipulate opponent data.
* 🚀 **Offline Fallback:** The architecture gracefully falls back to local single-player mode if the network is disconnected, allowing for easy offline testing.
* 🎭 **Asynchronous Enemy Board:** Utilizes a "Dummy Board" pattern. The opponent's board runs zero gameplay logic and acts strictly as a lightweight listener for network state changes.
* 🧹 **Robust Lifecycle Management:** Prevents "zombie sessions" (lingering `NetworkRunner` instances) and handles sudden disconnects safely without throwing `NullReferenceExceptions`.

---

## 🛠 Under the Hood (Architecture)

The codebase is highly modular, utilizing strict separation of concerns to keep the project scalable:

* `BoardService` — The core facade. Manages grid initialization, move validation, and sends state updates to the network.
* `MatchMachine` & `GravitySystem` — Encapsulated mathematical systems for detecting complex matches (L-shapes, cross-shapes) and calculating cell fall trajectories.
* `NetworkTimerService` — Ensures exact time synchronization across the network. Automatically handles end-game logic and winner determination.
* `NetworkDisconnectObserver` — A monitoring system that detects unexpected opponent drops and awards a technical victory to the remaining player.

⏱️ Estimation & Time Tracking

To maintain a structured development process, feature estimations, bug fixes, and network synchronization tasks were planned and tracked externally. The total estimated time for this project is ~32 hours. 

📊 [**View Detailed Estimation Tracker on Google Sheets**](https://docs.google.com/spreadsheets/d/1wObyJkTYyLdL5nhzleQBVyJh4kq5mrFOmAjm4NwGSiw/edit?usp=sharing)


