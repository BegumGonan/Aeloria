🎮 Aeloria – Unity 2D Farming & Crafting Prototype

A solo-developed Unity 2D game prototype focused on building a complete gameplay loop including farming, crafting, inventory systems, time progression, and world interaction mechanics.

This project was developed entirely by me to explore modular game architecture and core gameplay systems commonly found in farming/survival RPG games.

🧠 Overview

Aeloria is a systems-driven 2D farming prototype inspired by games like Stardew Valley.

The player manages resources, grows crops, gathers materials, crafts potions, and progresses through a day-based cycle.

The main focus of this project is not content, but gameplay systems design and implementation.

🎮 Core Features


🌱 Farming System

Tile-based farming (plow, plant, water, harvest)

Crop growth system with multiple stages

Growth progression tied to in-game days

Watering required for crop progression

🧪 Crafting System (Cauldron)

Ingredient-based potion brewing

Time-based crafting system (in-game hours)

Brewing animation using tile changes

Finished potion pickup system

🎒 Inventory System

Dual inventory structure (Backpack + Toolbar)

Stackable items system

Drag & drop UI interaction

Split stacks (single / full item dragging)

Drop items into world physics

🌳 Resource Gathering

Tree chopping system with health & stump states

Wood drop system with physics-based scattering

Tree regrowth after multiple in-game days

🕒 Time & Day Cycle

Accelerated in-game time system

Day progression with sleep mechanic

End-of-day world reset:

Crop growth update

Soil drying

Spawner refresh

Tree regrowth checks

⚡ Energy System

Action-based stamina system

Visual energy progression UI

Energy consumption per interaction

Potion-based energy recovery

🏠 World Interaction

Indoor / outdoor transition system

Camera boundary switching per environment

Fade in / fade out scene transitions

🎮 Controls

Movement

WASD → Move player

Interaction

Left Click → Interact / Harvest / Use tools

Right Click → Use potion / special interaction

Inventory

I → Open/Close inventory

Mouse Drag → Move items

Right Mouse Drag → Move single item

Mouse Wheel → Change toolbar slot

Drag outside inventory → Drop item into world

🧾 Gameplay Notes

Crops must be watered daily to continue growing

Energy system limits player actions and encourages resource management

Sleeping advances the day and resets world state

Toolbar system determines active tools for interactions

🛠 Built With

Unity 2D

Unity Input System

Tilemap System

TextMeshPro

Universal Render Pipeline (URP)

💡 Project Goals

This project was created to practice and demonstrate:

Modular Unity architecture design

Gameplay system implementation (not just mechanics)

Inventory/UI system programming

Tile-based world interaction systems

Time-based simulation logic

Game loop design (day → action → sleep → progression)

👤 Development

Developer: Solo Project

Role: Gameplay Programmer / Systems Designer

Focus: Gameplay systems architecture & Unity programming

📌 Summary

Aeloria is a gameplay systems prototype focused on building the foundation of a farming RPG.
The emphasis is on systems design, interaction logic, and modular Unity architecture rather than content volume.
