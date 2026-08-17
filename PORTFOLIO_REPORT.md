# Software Deployment and Evolution: Complete Portfolio Technical Report & Screenshot Analysis

## Executive Summary & Workflow Overview
This document provides a comprehensive, step-by-step technical explanation of the software deployment workflow for **SampleApp** and its WiX installer project **SampleAppSetup**. It covers the complete evolution of the software across four criteria tiers:
1. **Pass Task**: Initial setup of C# WinForms application, WiX v3 toolset integration, and basic MSI installation creation.
2. **Credit Task**: Conversion from a simple console executable to a styled WinForms graphical user interface (`MainForm.cs`), creation of desktop & start menu shortcuts, and installation directory verification.
3. **Distinction Task**: Integration of external NuGet dependencies (`Newtonsoft.Json` and `Serilog`), updating project references, modifying `MainForm.cs` to execute JSON serialization & structured logging, and bundling external DLLs (`Newtonsoft.Json.dll`, `Serilog.dll`, `Serilog.Sinks.File.dll`) into the WiX installer (`Product.wxs`).
4. **High Distinction Task**: CI/CD release workflow, GitHub Release packaging, SHA-256 integrity hashing, generation of Microsoft WinGet package manifests (`Swinburne.SampleApp`), and submission documentation for `microsoft/winget-pkgs`.

---

## 1. Pass Level: Initial Application Setup & Basic WiX Configuration

### Pass Image 1: Create a Sample Desktop Application
- **File**: `Pass Image 1 Create a Sample Desktop Application.png`
- **Technical Explanation**: Shows the initial solution structure in Visual Studio containing the basic `SampleApp` C# project. The solution file anchors the core application layout before introducing installer components.
- **Workflow & Rationale**: Establishing a clean solution baseline ensures that project paths and build output directories (`bin\Debug` and `bin\Release`) are consistently resolved by downstream tools.

### Pass Image 2: Set up WiX Toolset and Visual Studio
- **File**: `Pass Image 2 Set up WiX Toolset and Visual Studio.png`
- **Technical Explanation**: Displays the installation and environment configuration of the WiX Toolset v3.11 alongside Visual Studio project targets.
- **Workflow & Rationale**: The WiX Toolset compiles XML manifest definitions (`.wxs`) using `candle.exe` (compiler) and `light.exe` (linker) into standard Windows Installer (`.msi`) packages.

### Pass Image 3: Creation of Installation Executable Release
- **File**: `Pass Image 3 Creation of Installation Executable Release.png`
- **Technical Explanation**: Highlights the successful execution of MSBuild building the `Release` configuration for `SampleApp.exe` and initializing the `SampleAppSetup.wixproj` project.
- **Workflow & Rationale**: Compiling in `Release` mode optimizes the C# IL assembly by removing debug symbols and enabling code optimization before packaging.

### Pass Image 4: Setup SampleApp Installation Process
- **File**: `Pass Image 4 Setup SampleApp Installation Process.png`
- **Technical Explanation**: Demonstrates the execution of the generated `SampleAppSetup.msi` installer wizard launching on the host system.
- **Workflow & Rationale**: Confirms that the Windows Installer service (`msiexec.exe`) parses the MSI database tables created by WiX without throwing errors.

### Pass Image 5: Installation Verification of Test Application
- **File**: `Pass Image 5 Installation Verification of Test Applicaiton.png`
- **Technical Explanation**: Shows the file system view of `C:\Program Files (x86)\SampleApp` containing the installed `SampleApp.exe`.
- **Workflow & Rationale**: Validates that target directory properties (`ProgramFilesFolder` -> `INSTALLFOLDER`) correctly target machine-wide program locations.

---

## 2. Credit Level: WinForms Conversion, GUI Implementation & Shortcuts

### Credit Image 1: Edits to edit the SampleApp.csproj (PT1 & PT2)
- **Files**: 
  - `Credit Image 1 Edits to edit the SampleApp.csproj PT1.png`
  - `Credit Image 1 Edits to edit the SampleApp.csproj PT2.png`
- **Technical Explanation**: Shows modifying line 8 of `SampleApp.csproj` from `<OutputType>Exe</OutputType>` to `<OutputType>WinExe</OutputType>`. Part 2 demonstrates adding assembly references for `System.Drawing` and `System.Windows.Forms`, alongside the item inclusion for `<Compile Include="MainForm.cs" />`.
- **Workflow & Rationale**: 
  > *"The first thing I did was edit the SampleApp.csproj changing line 8 which generates the output installation file. I changed this value from `Exe` to `WinExe` because I plan on developing a Graphical User Interface (GUI) instead of using a Character User Interface (CUI). This required adding two references to the ItemGroup for drawing the application GUI (`System.Drawing` and `System.Windows.Forms`) and adding an inclusion for compiling the `MainForm.cs` file."*

### Credit Image 2: Creating the MainForm.cs (PT1 - PT4)
- **Files**: 
  - `Credit Image 2 Creating the MainForm.cs PT1.png`
  - `Credit Image 2 Creating the MainForm.cs PT2.png`
  - `Credit Image 2 Creating the MainForm.cs PT3.png`
  - `Credit Image 2 Creating the MainForm.cs PT4.png`
- **Technical Explanation**: Illustrates the programmatically generated WinForms UI layout in `MainForm.cs`. Includes instantiation of `lblHeader`, `pnlCard`, `lblStatus`, `btnAction`, and a console-styled log output `txtLog` using a modern dark slate (`#0F172A`) palette and emerald text (`#34D399`).
- **Workflow & Rationale**: Programmatic layout instantiation eliminates reliance on the Visual Studio designer file (`MainForm.Designer.cs`), making the user interface completely declarative and version-control friendly.

### Credit Image 3: Editing the Program.cs
- **File**: `Credit Image 3 Editing the Program.cs.png`
- **Technical Explanation**: Displays `Program.cs` updated with `[STAThread]` attribute, `Application.EnableVisualStyles()`, `Application.SetCompatibleTextRenderingDefault(false)`, and `Application.Run(new MainForm())`.
- **Workflow & Rationale**: The Single-Threaded Apartment (`[STAThread]`) attribute is strictly required for COM components and Windows Forms visual styles to operate correctly during message loop execution.

### Credit Image 4 & 5: Building, Installing & Program Verification
- **Files**: 
  - `Credit Image 4 Building and Installing the Program.png`
  - `Credit Image 5 Program Installation Verification.png`
- **Technical Explanation**: Captures MSBuild execution of the updated solution followed by quiet/unattended installation (`msiexec /i ... /qb`) and folder inspection.
- **Workflow & Rationale**: Verifies that the rebuilt installer deploys the new WinExe executable clean of build conflicts.

### Credit Image 6: Edits to Product.wxs (PT1 & PT2)
- **Files**: 
  - `Credit Image 6 Edits to Product.wxs PT1.png`
  - `Credit Image 6 Edits to Product.wxs PT2.png`
- **Technical Explanation**: Details the addition of `ApplicationProgramsFolder` and `DesktopFolder` directory references in `Product.wxs`, alongside `ApplicationShortcut` and `DesktopShortcutComponent` definitions with registry key paths (`HKCU\Software\Swinburne\SampleApp`).
- **Workflow & Rationale**: WiX requires a non-file `RegistryValue` keypath for user-profile shortcuts (`HKCU`) to satisfy Windows Installer rules regarding per-machine vs. per-user component state management.

### Credit Image 7 & 8: Launching & Testing Sample Application
- **Files**: 
  - `Credit Image 7 Launching Sample Application.png`
  - `Credit Image 8 Testing Sample Application.png`
- **Technical Explanation**: Shows launching `SampleApp.exe` from the installed shortcut and clicking **Run Deployment Task**, triggering status label color transitions and real-time log append events.
- **Workflow & Rationale**: Validates UI state management and user event handlers in a fully installed desktop environment.

---

## 3. Distinction Level: External DLL Dependencies & WiX Bundling

### Distinction Image 1: Creating packages.config file
- **File**: `Distinction Image 1 Creating packages.config file .png`
- **Technical Explanation**: Displays `packages.config` defining external NuGet dependencies:
  - `Newtonsoft.Json` (v13.0.3)
  - `Serilog` (v2.12.0)
  - `Serilog.Sinks.File` (v5.0.0)
- **Workflow & Rationale**: Explicitly declaring dependencies in `packages.config` enables automated dependency restoration across build environments.

### Distinction Image 2: Editing SampleApp.csproj to reference DLLs (PT1 & PT2)
- **Files**: 
  - `Distinction Image 2 Editing SampleApp.csproj to reference DLLs PT1.png`
  - `Distinction Image 2 Editing SampleApp.csproj to reference DLLs PT2.png`
- **Technical Explanation**: Shows `<Reference Include="...">` elements added to `SampleApp.csproj` pointing to `<HintPath>..\packages\...` for `Newtonsoft.Json.dll`, `Serilog.dll`, and `Serilog.Sinks.File.dll`.
- **Workflow & Rationale**: MSBuild uses these hint paths to locate and copy external assemblies into the output `bin\Release\` folder during compilation.

### Distinction Image 3 & 4: Installing & Verifying NuGet Packages
- **Files**: 
  - `Distinction Image 3 Installing NuGet Packages.png`
  - `Distinction Image 4 Verification of NuGet Package Installation.png`
- **Technical Explanation**: Screenshots of command line `nuget.exe restore` restoring package binaries into `..\packages\`.
- **Workflow & Rationale**: Ensures assembly files exist on disk before compilation.

### Distinction Image 5: Update MainForm.cs to utilize external DLLs (PT1 - PT3)
- **Files**: 
  - `Distinction Image 5 Update MainForm.cs to utilize the external DLLs PT1.png`
  - `Distinction Image 5 Update MainForm.cs to utilize the external DLLs PT2.png`
  - `Distinction Image 5 Update MainForm.cs to utilize the external DLLs PT3.png`
- **Technical Explanation**: Code snippets showing:
  - Import statements `using Newtonsoft.Json;` and `using Serilog;`.
  - Serilog logger configuration initializing file logging (`app.log`).
  - Creation of `DeploymentPayload` object and serialization via `JsonConvert.SerializeObject(payload, Formatting.Indented)`.
- **Workflow & Rationale**: Demonstrates practical integration of third-party JSON parsing and structured file logging within the WinForms event loop.

### Distinction Image 6: Update Product.wxs in SampleAppSetup to include external DLLs
- **File**: `Distinction Image 6 Update Product.wxs in SampleAppSetup to include the external dependency DLLs.png`
- **Technical Explanation**: Shows the critical WiX update in `Product.wxs` adding component references for the three DLLs:
  ```xml
  <Component Id="NewtonsoftJsonDLL" Guid="{A1B2C3D4-E5F6-4701-8901-234567890ABC}">
    <File Id="NewtonsoftJsonDLLFile" Source="$(var.SampleApp.TargetDir)Newtonsoft.Json.dll" KeyPath="yes" />
  </Component>
  <Component Id="SerilogDLL" Guid="{B2C3D4E5-F6A7-4802-9012-345678901BCD}">
    <File Id="SerilogDLLFile" Source="$(var.SampleApp.TargetDir)Serilog.dll" KeyPath="yes" />
  </Component>
  <Component Id="SerilogSinksFileDLL" Guid="{C3D4E5F6-A7B8-4903-0123-456789012CDE}">
    <File Id="SerilogSinksFileDLLFile" Source="$(var.SampleApp.TargetDir)Serilog.Sinks.File.dll" KeyPath="yes" />
  </Component>
  ```
- **Workflow & Rationale**: Without including external DLLs as explicit WiX components, installing the app on a machine without those DLLs in the Global Assembly Cache (GAC) causes immediate runtime crash (`System.IO.FileNotFoundException`).

---

## 4. High Distinction Level: Git Versioning, GitHub Release & WinGet Manifest Submission

### High Distinction Image 1 - 4: Git Staging, Commit, Push & GitHub Verification
- **Files**: 
  - `High Distinction Image 1 Stage Project Source Files for Git.png`
  - `High Distinction Image 2 Create Git Commit.png`
  - `High Distinction Image 3 Push changes to remote GitHub repository.png`
  - `High Distinction Image 4 Verify Git Push to Github .png`
- **Technical Explanation**: Terminal execution logs showing `git add`, `git commit -m "feat: Add Newtonsoft.Json and Serilog..."`, and `git push origin main`, followed by a GitHub repository browser verification showing commit hash `0cb38ec`.
- **Workflow & Rationale**: Enforces strict version control and remote synchronization prior to release tagging.

### High Distinction Image 5 & 6: Git Tagging & SHA-256 Checksum Calculation
- **Files**: 
  - `High Distinction Image 5 Create and Push Git Tag v1.0.0.png`
  - `High Distinction Image 6  Obtain SHA-256 Hash of SampleAppSetup.msi.png`
- **Technical Explanation**: Terminal output executing `git tag -a v1.0.0` and PowerShell `Get-FileHash -Algorithm SHA256` producing hash: `6B34D440F7B410DE334EA16294251BF322E1FB57520526BE0B5CB141B3D81D22`.
- **Workflow & Rationale**: SHA-256 cryptographically guarantees package integrity for global package managers like WinGet, detecting tamper or corruption during transmission.

### High Distinction Image 7: Publish Executable to New Release
- **File**: `High Distinction Image 7 Publish executable to New Release.png`
- **Technical Explanation**: Visual verification of GitHub Release `v1.0.0` hosting `SampleAppSetup.msi` as an attached binary release asset.
- **Workflow & Rationale**: Provides an immutable, publicly accessible HTTPS download URL (`https://github.com/.../releases/download/v1.0.0/SampleAppSetup.msi`) required by WinGet installer manifests.

### High Distinction Image 8 & 9: WinGet Package Manifest Creation & PR Documentation
- **Files**: 
  - `High Distinction Image 8 Commit and Push WinGet Package Manifest Files.png`
  - `High Distinction Image 9 Commit and Push PR Submission Documentation (WINGET_PR_GUIDE.md).png`
- **Technical Explanation**: Shows terminal output committing the generated WinGet YAML manifests under `manifests/s/Swinburne/SampleApp/1.0.0.0/` (`Swinburne.SampleApp.yaml`, `installer.yaml`, `locale.en-US.yaml`) and the PR submission guide (`WINGET_PR_GUIDE.md`).
- **Workflow & Rationale**: Completes the full end-to-end software deployment cycle by preparing valid, schema-compliant manifest files for submission to Microsoft's official `microsoft/winget-pkgs` repository.

---

## 5. Self-Troubleshooting, Analysis & Discussion

During implementation, several technical challenges were encountered and resolved:

1. **Assembly Resolution Failure during MSI Deployment**:
   - *Problem*: Initial build of `SampleAppSetup.msi` packaged only `SampleApp.exe`. When launched on a test environment, `SampleApp.exe` crashed instantly.
   - *Analysis*: WinForms applications referencing NuGet libraries copy DLLs to the local `bin\Release` output. WiX does not automatically discover referenced assemblies unless explicitly declared.
   - *Resolution*: Updated `Product.wxs` with explicit `Component` and `File` elements for `Newtonsoft.Json.dll`, `Serilog.dll`, and `Serilog.Sinks.File.dll`, referencing `$(var.SampleApp.TargetDir)`.

2. **Per-Machine vs Per-User Registry KeyPath Conflict**:
   - *Problem*: ICE57 warning during WiX linking for desktop shortcuts.
   - *Analysis*: Shortcuts installed to `DesktopFolder` or `ProgramMenuFolder` under per-machine installation scope (`InstallScope="perMachine"`) require an explicit registry keypath under `HKCU`.
   - *Resolution*: Defined `RegistryValue Root="HKCU" Key="Software\Swinburne\SampleApp"` as `KeyPath="yes"` for all shortcut components in `Product.wxs`.
