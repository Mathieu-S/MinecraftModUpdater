# Minecraft Mod Updater

Minecraft Mod Updater is a package manager for Minecraft's mods. It work on any platform supported by .NET5.
This this tool don't download or launch Minecraft.

You can run this tool with the **UI (WIP)** or the **CLI**. If you are not on Windows, the **CLI** is your only solution.

If you are familiar with dependencies files like `packages.json`, `requirement.txt`, `.csproj`, `composer.json`, `Dockerfile`, etc. MMU work with its own file : `modList.json`

## MMU Features
* Multi-platforms (Windows, Linux, Mac)
* A simple CLI
* An UI if you are allergic to CLI. (WIP)
* A lighter solution than the official Curse app.
* A dependency file to restore your mods quickly.
* A special file format for creating and sharing a modpack. (WIP)

## Install
First, you need to install [.NET5](https://dotnet.microsoft.com/download). After that, you can download a [release](https://github.com/Mathieu-S/MinecraftModUpdater/releases) of this tool and install with this command :

```bash
dotnet tool install --global --add-source ./ MinecraftModUpdater.CLI
```

*Note: The ./ is where you download the file. This solution is temporary, later you can download and install the tool automatically from Nuget.*

## Usage
You can use it with `mmu`. The tool has the same commands as npm for NodeJs..

```bash
mmu init                                # Initialise a new modList.json file.
mmu install (require mod-name or Id)    # Install dependencies or a mod
mmu update (optional mod-name or Id)    # Update dependencies or a mod
mmu remove (mod-name or Id)             # Remove a mod
```

## FAQ
```
Q: Why this tool exist ?
R: Basically, I spent a LOT of my time to update manualiy my mods and I don't like the official Curse app.
```

```
Q: I already have a mod preset/collection, can I manage them with MMU ?
R: Yes and no, it is not possible at the moment to manage mods that you have not installed with this tool. Sorry.
```

```
Q: Why not UI for Linux or Mac ?
R: Unfortunately .NET5 doesn't have UI support for Linux and Mac.
```

```
Q: Hey your tool broke something in my game !
R: I'm really sorry about that, this project is actually in WIP so it's possible the tool to have some issues. Please report them.
```

```
Q: OMG Why did you use .NET ?! You should use X !
R: Ok listen. First of all, it's a free tool. Secondly, I code with what I like. Thirdly, if you are not satisfied, go to develop an equivalent of this tool in X!
```
