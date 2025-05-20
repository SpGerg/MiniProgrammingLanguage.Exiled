# MPL Exiled
A port of the Exiled library to the MPL programming language for easier writing of plugins.


```lua
function broadcast_on_verified(player)
    player.broadcast(3, "Welcome to the server!")
end

function initialize_on_enabled()
    on_verified.subscribe(broadcast_on_verified)

    log("Plugin was enabled!")
end

function deinitialize_on_disabled()
    script_plugin.on_enabled.unsubscribe(initialize_on_enabled)
    script_plugin.on_disabled.unsubscribe(deinitialize_on_disabled)

    on_verified.unsubscribe(broadcast_on_verified)

    log("Plugin was disabled!")
end

script_plugin.on_enabled.subscribe(initialize_on_enabled)
script_plugin.on_disabled.subscribe(deinitialize_on_disabled)
```

# Getting Started
## Installation
To get started, you need to download from releases and move files to the dependencies:
  - MiniProgrammingLanguage.Core
  - MiniProgrammingLanguage.ExiledKit
  - MiniProgrammingLanguage.SharpKit.
    
In the Plugins folder MiniProgrammingLanguage.Exiled.

## Commands

| Command               | Details                                                                                     |
|-----------------------|-------------------------------------------------------------------------------------------- |
| **`mpl run [name]`**  | Runs the specified file in the scripts folder.<br><br>If the name **ends with ".mpl"**, looks for the file.<br>Otherwise, looks for a folder with **'root.mpl'**.<br><br>Triggers `on_enabled` on first run.<br>Triggers `on_disabled` when re-running.<br><br>**Plugin var name:** `(filename)_plugin` |
| **`mpl execute`**     | Executes passed code.<br><br>**Plugin var name:** `ra_plugin`<br><br>Example:<br>`/execute for i in 10 log(i) end` |
| **`mpl stop [name]`** |   Stops the script with the given name. <br><br> **If the script was started via console, it will have the name 'ra'**. |

## Quick Guide
Here I will explain the basic things in the language and API that will help you quickly start developing.

### Syntax
The syntax of the language is quite similar to Lua, but with its own improvements in the form of typing, simplicity and a convenient internal API for creating ports for C# things.

```lua
variable = 0

print(variable) -- 'log' in Exiled

function sum(left, right)
    return left + right
end

print((string) sum(2, 2))
```
In this small example you can see the simple use of functions and variables.

---
Types in the language are implemented simply with the ability to hide data using the 'static' and 'readonly' keywords.
```lua
module 'weapon'

static type weapon
    static readonly id -- Type not required
    static ammo: round_number
    static readonly max_ammo: round_number
    static function shoot()
end

staic readonly instance = create weapon
instance.id = 0
instance.max_ammo = 20
instance.ammo = instance.max_ammo

implement function weapon.shoot()
    ...
end

-- Other file
import weapon -- Name can be other

print(instance.max_ammo) -- Correct
instance.max_ammo = 25 -- Error
```
With the 'readonly' keyword we don't allow scripts with another module to change any values, and with 'static' we allow them to use them (that is, we make them public).

### Exiled
To create a plugin, we don't need to import any scripts, we can just create a file and start right now.

To subscribe to any event, we need to find out its name on the wiki page, and also see what arguments are passed.

```lua
function broadcast_on_verified(player)
    player.broadcast(3, "Welcome to the server!")
end

function initialize_on_enabled()
    on_verified.subscribe(broadcast_on_verified)

    log("Plugin was enabled!")
end

script_plugin.on_enabled.subscribe(initialize_on_enabled)
```
If we don't specify any argument in the function, it will simply be ignored when called, so we can take only those arguments that we need.

---

A simple knowledge of variables, functions, events and a bit of basic math and logic will allow you to create simple plugins, but this does not mean that you cannot create complex ones in MPL.
The language has everything for this, and you can read about it on the MPL wiki page.
