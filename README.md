# 🎨 Enum Library

![Unity](https://img.shields.io/badge/Unity-2022.3+-black.svg?style=for-the-badge&logo=unity)
![Contributions welcome](https://img.shields.io/badge/Contributions-Welcome-brightgreen.svg?style=for-the-badge)
[![Odin Inspector](https://img.shields.io/badge/Odin_Inspector-Required-blue?style=for-the-badge)](https://odininspector.com/)

***
![Banner Image](https://via.placeholder.com/1000x300.png?text=assets+Enum+Library+Editor+for+Unity)
***

![GitHub Forks](https://img.shields.io/github/forks/Ddemon26/Enum-Library)
![GitHub Contributors](https://img.shields.io/github/contributors/Ddemon26/Enum-Library)

![GitHub Stars](https://img.shields.io/github/stars/Ddemon26/Enum-Library)
![GitHub Repo Size](https://img.shields.io/github/repo-size/Ddemon26/Enum-Library)

[![Join our Discord](https://img.shields.io/badge/Discord-Join%20Us-7289DA?logo=discord&logoColor=white)](https://discord.gg/knwtcq3N2a)
![Discord](https://img.shields.io/discord/1047781241010794506)


![GitHub Issues](https://img.shields.io/github/issues/Ddemon26/Enum-Library)
![GitHub Pull Requests](https://img.shields.io/github/issues-pr/Ddemon26/Enum-Library)
![GitHub Last Commit](https://img.shields.io/github/last-commit/Ddemon26/Enum-Library)

![GitHub License](https://img.shields.io/github/license/Ddemon26/Enum-Library)
![Static Badge](https://img.shields.io/badge/Noobs-0-blue)

✨ **Enum Library Editor** is a Unity tool designed to simplify and automate the generation of enums and ScriptableObjects from assets. It streamlines the process of referencing assets in your Unity projects, improving both development efficiency and project organization.

![Demo GIF](https://media.giphy.com/media/l4Ep6KDbnTvdhGMP6/giphy.gif)

## 📜 Table of Contents
- [Features](#features)
- [Getting Started](#getting-started)
- [Installation](#installation)
- [Usage](#usage)
- [Customization](#customization)
- [Contributing](#contributing)
- [License](#license)

## ✨ Features

- **Enum Generation**: Automatically creates C# enums based on selected assets, filtering out invalid or duplicate entries.
- **ScriptableObject Generation**: Generates ScriptableObjects tied to enums, allowing you to easily associate data (e.g., `int`, `float`, `Sprite`, `AudioClip`, etc.) with enum values.
- **Static Class Generation**: Generates static classes to allow easy referencing of assets in your code.
- **Customization Options**: Supports customizable enum names, namespaces, and value types for your ScriptableObjects.
- **Asset Validation**: Built-in validation ensures that only valid assets are used for enum generation, reducing errors.
- **Folder Structure Management**: Automatically organizes generated files into structured folder hierarchies for improved project management.

## 🚀 Getting Started

Follow these steps to start using the **Enum Library Editor**:

1. **Install Dependencies**: Ensure that [Odin Inspector](https://odininspector.com/) is installed, as it is required for custom editor features.

2. **Open the Enum Generator**: In Unity, navigate to `Tools > Enum Generator` to open the tool's editor window.

3. **Select Assets**: Drag and drop assets into the editor to automatically generate enums based on them.

4. **Generate**: Click `Generate Enum` to create your enum and related files, or generate a ScriptableObject with associated values for the selected enum.

## 🔧 Installation

1. Clone or download this repository.
2. Add the folder to the `Assets` directory in your Unity project.
3. Install [Odin Inspector](https://odininspector.com/).
4. Open the Unity Editor and access the Enum Library through the `Tools` menu.

## 🛠️ Usage

1. **Enum Creation**: After opening the tool, input a list of names for your enums, or drag and drop assets. The tool automatically handles name validation, filtering, and generation of the C# enum.
2. **ScriptableObject Creation**: Use the editor to associate your enum values with different asset types. Choose a type (`int`, `float`, `Sprite`, etc.) and the tool will generate a ScriptableObject with a dictionary that maps enum values to asset data.
3. **Customization**: Toggle options for customizing enum names, class names, and folder paths to suit your project needs.

## ⚙️ Customization

- **Custom Names**: You can rename enums and namespaces to fit your project conventions.
- **Value Type Selection**: Choose from various types (`int`, `float`, `Sprite`, etc.) when generating ScriptableObjects.
- **Folder Structure**: Define the output path for generated enums and ScriptableObjects, allowing better organization of your assets.

## 🤝 Contributing

Contributions are welcome! To contribute:

1. Fork the repository.
2. Create a new feature branch (`git checkout -b feature/NewFeature`).
3. Make your changes and commit (`git commit -m 'Add new feature'`).
4. Push to the branch (`git push origin feature/NewFeature`).
5. Open a pull request.

## 📄 License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.