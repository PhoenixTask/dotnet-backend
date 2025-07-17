# PhoenixTask API

A simple project management system 

This is the backEnd API of PhoneixTask project which is divided into two parts.

See front repository [here](https://github.com/AMN2080/PhoenixTask)!

## Prerequisites

### Install .NET 9 SDK

#### 🐧 For Linux (Ubuntu/Debian)

# Add Microsoft package repository
```bash
wget https://packages.microsoft.com/config/ubuntu/$(lsb_release -rs)/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb
```
# Install .NET SDK
```bash
sudo apt-get update
sudo apt-get install -y dotnet-sdk-9.0
```
# Verify installation
```bash
dotnet --version
```

#### 🪟  For Windows (10/11)

1. **Download the installer**  
   [Download .NET 9 SDK for Windows](https://dotnet.microsoft.com/download/dotnet/9.0)

2. **Run the installer**  
   - Double-click the downloaded `.exe` file
   - Follow the installation prompts (use default settings)

3. **Verify installation**  
   Open PowerShell or Command Prompt and run:
   ```powershell
   dotnet --version
## Run Locally

Clone the project

```bash
  git clone https://github.com/ErfanMelon/PhoenixTaskApi.git
```

Go to the project directory

```bash
  cd PhoenixTaskApi
```

Build project

```bash
  dotnet build .\PhoenixTask.sln
```
Go To Web Folder
```bash
  cd .\src\Web.Api\
```

Start the server

```bash
  dotnet run
```
## Documentation

To view requests please goto swagger page 👇

[Documentation](http://localhost:5000/swagger/index.html)


## Support

For support, email ErfanMelon@outlook.com .

