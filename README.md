# Trackify
A lightweight Windows desktop overlay that shows what music is currently playing, including song info and genre

## Goal

Track what music is playing on the system in real time and display it in a minimal overlay UI with:

* Song title
* Artist name
* Genre detection
* Media controls (play, pause, skip)

---

## Download

The latest Windows version can be downloaded from the **Releases** page.

1. Download the latest `Trackify-v1.0.0-win-x64.zip`
2. Extract the ZIP
3. Run `Trackify.exe`

**Note:** You may have to download .Net sdk
**Note 2:** To see the popup you must start playing music from either spotify or youtube music

## Setup

* Built using **WPF (.NET)**
* Created a blank window and confirmed runtime setup
* Developed in **VS Code**
<img width="1354" height="1042" alt="Screenshot 2026-06-14 163214" src="https://github.com/user-attachments/assets/d0b46c7a-6e57-4f48-9dc1-698077accbdc" />


## UI

* Dark overlay-style interface
* Transparent, rounded popup design
* Displays:

  * Song title
  * Artist
  * Genre tag
* Initial testing used placeholder data before live integration
<img width="1430" height="697" alt="Screenshot 2026-06-14 164627" src="https://github.com/user-attachments/assets/88771d3c-f144-45c2-b80d-e52630c21bfb" />


## Song Detection

* Uses **Windows Global Media Session API**
* Detects playback from apps like:

  * Spotify
  * YouTube
  * System media players
* Automatically updates when the song changes
<img width="1735" height="514" alt="Screenshot 2026-06-14 173149" src="https://github.com/user-attachments/assets/1b182631-ebca-4720-9c27-f4d598d9aed6" />

## Genre System

* Integrated **Last.fm API**
* Fetches genre based on artist name
* Falls back to `Unknown` if no data is found
<img width="1267" height="611" alt="Screenshot 2026-06-14 175556" src="https://github.com/user-attachments/assets/d88d4a09-7572-4a87-8d1e-a993ee1e1fae" />

## Overlay System

* Popup-style floating window (FluentFlyout inspired)
* Semi-transparent design
* Smooth fade-in and fade-out animations
* Auto-hides after a short duration
<img width="639" height="267" alt="Screenshot 2026-06-14 204333" src="https://github.com/user-attachments/assets/24e706ea-3df7-4b31-b6be-c3967fff1443" />

## Media Controls

* Added playback controls:

  * Play / Pause
  * Next track
  * Previous track
* Styled modern buttons inside overlay UI

## System Features

* System tray integration
* Custom tray icon `icon.ico`
* Runs in background after launch
* Clean exit behavior (hidden window instead of closing)
* Shows Album Cover

<img width="865" height="1009" alt="Screenshot 2026-06-14 214644" src="https://github.com/user-attachments/assets/4984a823-0048-4a54-acb6-491ba31e0fb2" />

## Current State

### Working MVP

* Live song detection
* Artist + title display
* Genre lookup working
* Auto-updates on song change
* Overlay popup UI
* Media control buttons
* System tray support
* Styled Fluent UI

<img width="515" height="210" alt="image" src="https://github.com/user-attachments/assets/fb7d7ad8-2d83-4d22-b5a8-f13f429d58ab" />


Run the app:

```
dotnet run
```
