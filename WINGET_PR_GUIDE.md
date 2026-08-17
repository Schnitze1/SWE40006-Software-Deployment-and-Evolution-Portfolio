# WinGet Package Submission Guide & PR Checklist

## Package Details
- **Package Identifier**: `Swinburne.SampleApp`
- **Package Version**: `1.0.0.0`
- **Installer Type**: `msi`
- **Architecture**: `x86`
- **Installer URL**: [https://github.com/Schnitze1/SWE40006-Software-Deployment-and-Evolution-Portfolio/releases/download/v1.0.0/SampleAppSetup.msi](https://github.com/Schnitze1/SWE40006-Software-Deployment-and-Evolution-Portfolio/releases/download/v1.0.0/SampleAppSetup.msi)
- **Installer SHA-256**: `6B34D440F7B410DE334EA16294251BF322E1FB57520526BE0B5CB141B3D81D22`
- **Product Code**: `{D5AE62C5-C0FA-4593-A96C-949A576C757A}`

---

## Step 9 Execution Checklist for `microsoft/winget-pkgs` PR

### Option A: Via GitHub Web Interface (Simplest)
1. Navigate to the official repository: [microsoft/winget-pkgs](https://github.com/microsoft/winget-pkgs).
2. Click **Fork** (top right) to create a copy under your account (`Schnitze1/winget-pkgs`).
3. Inside your fork, create a new branch named `add-swinburne-sampleapp`.
4. Upload/Create the 3 manifest files at the exact relative path:
   `manifests/s/Swinburne/SampleApp/1.0.0.0/`
   - `Swinburne.SampleApp.yaml`
   - `Swinburne.SampleApp.installer.yaml`
   - `Swinburne.SampleApp.locale.en-US.yaml`
5. Open a **Pull Request** targeting `microsoft/winget-pkgs:master`.
6. PR Title: `New package: Swinburne.SampleApp version 1.0.0.0`

### Option B: Via Git CLI
```bash
# 1. Clone your fork
git clone https://github.com/Schnitze1/winget-pkgs.git
cd winget-pkgs

# 2. Create topic branch
git checkout -b add-swinburne-sampleapp

# 3. Copy manifests into directory hierarchy
mkdir -p manifests/s/Swinburne/SampleApp/1.0.0.0/
cp "d:/Repos/SWE40006-Software Deployment and Evolution/manifests/s/Swinburne/SampleApp/1.0.0.0/*" manifests/s/Swinburne/SampleApp/1.0.0.0/

# 4. Commit and push
git add manifests/s/Swinburne/SampleApp/1.0.0.0/
git commit -m "New package: Swinburne.SampleApp version 1.0.0.0"
git push origin add-swinburne-sampleapp

# 5. Open PR link displayed in terminal output
```

---

## Automated Validation Pipeline
Once the PR is opened, Microsoft's automated GitHub Action workflows (`Azure Pipelines / WinGet Automation`) will automatically:
1. Validate YAML syntax and version scheme matching `1.6.0`.
2. Download `SampleAppSetup.msi` from GitHub Releases.
3. Re-verify the SHA-256 hash against `6B34D440F7B410DE334EA16294251BF322E1FB57520526BE0B5CB141B3D81D22`.
4. Perform silent installation tests in a Windows Sandbox environment.
