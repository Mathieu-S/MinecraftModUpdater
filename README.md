﻿# Minecraft Mod Updater

Minecraft Mod Updater is a package manager for Minecraft's mods (Java version). It work on any platform supported by .NET5.
This this tool don't download or launch Minecraft.

You can run this tool with the **UI (WIP)** or the **CLI**. If you are not on Windows, the **CLI** is your only solution.

MMU works with the same type of dependency file as your favorite developer tools (if you are a developer). Like NodeJs with `packages.json` or Python with `requirement.txt`, MMU has the `modlist.json`. Every information about your mods is stored inside the `modlist.json`.

## MMU Features
* Multi-platforms (Windows, Linux, Mac)
* A simple CLI
* An UI if you are allergic to CLI. **(WIP)**
* A lighter solution than the official Curse app.
* A dependency file to restore your mods quickly.
* A special file format for creating and sharing a modpack. **(WIP)**

## Install
First, you have to install [.NET5](https://dotnet.microsoft.com/download). After that, you can download the tool and install it with this command :

```bash
dotnet tool install --global MinecraftModUpdater.CLI
```

## Usage
You can use it with `mmu` in a terminal. Keep in mind you must enter these commands in root of your minecraft game. The CLI has built-in documentation.

## FAQ

### Why this tool exist?
Basically, I spent a LOT of my time to update manually my mods and I don't like the official Curse app, it's heavy, and contains trackers. I don't like Electron apps either.

### Where are mods downloaded from?
They are directly downloaded from [Curse Forge](https://www.curseforge.com/minecraft/mc-mods).

### What do you mean by mod Id?
On the mod's web page, you will see an "About Project" and Project ID section. It's that number.

### I already have a mod preset/collection, can I manage them with MMU?
No, it is not possible at the moment to manage mods that you have not installed with this tool. Sorry.

### Why not UI for Linux or Mac?
Unfortunately .NET5 doesn't have UI support for Linux and Mac. If people really want an UI for Linux/Mac, I'll try to make it with GTK. BUT NOT ELECTRON!

### Hey your tool broke something in my game!
I'm really sorry about that, this project is actually in WIP so it's possible the tool to have some issues. Please report them.

### OMG Why did you use .NET ?! You should use X!
Ok listen. First of all, it's a free tool. Secondly, I code with what I like. Thirdly, if you are not satisfied, go to develop an equivalent of this tool in X!
