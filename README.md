# Reqnroll Automation Framework

A **_browser-based test automation_** solution utilizing **_Reqnroll_** (**_BDD_**) and **_Selenium WebDriver_**, driven by the **_MSTest_** framework and structured around the **_Page Object Model_** (**_POM_**) architectural pattern.

## Features

- **_BDD-driven testing_** powered by **_Reqnroll_** and **_Gherkin syntax_** for clear **_human-readable_** scenario specification.
- **_Cross-browser testing_** supporting **_Chrome_** and **_Edge_** in both **_headed_** and **_headless_** modes.
- **_Parallel execution_** capability using custom **_MSTest_** `.runsettings` configurations for accelerated pipeline feedback.
- **_CI/CD integrated_** via **_GitHub Actions_** with automated artifact uploading, rerun handling, and custom NuGet authentication.

## Architecture Overview

<pre>
<b>ReqnrollAutomation</b>
├── <img src=".github/assets/icons/folder-github.svg" width="16"/> <b>.github</b>
│   └── <img src=".github/assets/icons/folder-gh-workflows.svg" width="16"/> <b>workflows</b>
│       ├── <img src=".github/assets/icons/yaml.svg" width="16"/> reqnroll-automation-core.yml
│       └── <img src=".github/assets/icons/yaml.svg" width="16"/> reqnroll-automation.yml
├── <img src=".github/assets/icons/folder-src.svg" width="16"/> <b>ReqnrollAutomation</b>
│   ├── <img src=".github/assets/icons/folder-config.svg" width="16"/> <b>Config</b>
│   │   ├── <img src=".github/assets/icons/json.svg" width="16"/> config.json
│   │   ├── <img src=".github/assets/icons/csharp.svg" width="16"/> ConfigAdapter.cs
│   │   └── <img src=".github/assets/icons/csharp.svg" width="16"/> ConfigManager.cs
│   ├── <img src=".github/assets/icons/folder-tools.svg" width="16"/> <b>Drivers</b>
│   │   └── <img src=".github/assets/icons/csharp.svg" width="16"/> DriverFactory.cs
│   ├── <img src=".github/assets/icons/folder-test.svg" width="16"/> <b>Features</b>
│   │   ├── <img src=".github/assets/icons/folder-project.svg" width="16"/> CarfaxCanadaWebsite
│   │   │   ├── <img src=".github/assets/icons/cucumber.svg" width="16"/> AccessibilityVerification.feature
│   │   │   ├── <img src=".github/assets/icons/cucumber.svg" width="16"/> FooterComponentVerification.feature
│   │   │   └── ...
│   │   └─ <img src=".github/assets/icons/folder-project.svg" width="16"/> SwagLabs
│   │       ├── <img src=".github/assets/icons/cucumber.svg" width="16"/> UserAuthenticationVerification.feature
│   │       └── ...
│   ├── <img src=".github/assets/icons/folder-helper.svg" width="16"/> <b>Helpers</b>
│   │   └── <img src=".github/assets/icons/csharp.svg" width="16"/> CredentialManager.cs
│   ├── <img src=".github/assets/icons/folder-hook.svg" width="16"/> <b>Hooks</b>
│   │   └── <img src=".github/assets/icons/csharp.svg" width="16"/> TestHooks.cs
│   ├── <img src=".github/assets/icons/folder-class.svg" width="16"/> <b>Models</b>
│   │   └── <img src=".github/assets/icons/csharp.svg" width="16"/> SwagLabsCredentials.cs
│   ├── <img src=".github/assets/icons/folder-views.svg" width="16"/> <b>Pages</b>
│   │   ├── <img src=".github/assets/icons/folder-project.svg" width="16"/> CarfaxCanadaWebsite
│   │   │   ├── <img src=".github/assets/icons/csharp.svg" width="16"/> HomePage.cs
│   │   │   └── ...
│   │   ├── <img src=".github/assets/icons/folder-project.svg" width="16"/> SwagLabs
│   │   │   ├── <img src=".github/assets/icons/csharp.svg" width="16"/> InventoryPage.cs
│   │   │   ├── <img src=".github/assets/icons/csharp.svg" width="16"/> LoginPage.cs
│   │   │   └── ...
│   │   └── <img src=".github/assets/icons/csharp.svg" width="16"/> BasePage.cs
│   ├── <img src=".github/assets/icons/folder-test.svg" width="16"/> <b>StepDefinitions</b>
│   │   ├── <img src=".github/assets/icons/folder-project.svg" width="16"/> CarfaxCanadaWebsite
│   │   │   ├── <img src=".github/assets/icons/csharp.svg" width="16"/> CarfaxCanadaBaseStepDefinitions.cs
│   │   │   ├── <img src=".github/assets/icons/csharp.svg" width="16"/> AccessibilityStepDefinitions.cs
│   │   │   ├── <img src=".github/assets/icons/csharp.svg" width="16"/> FooterComponentStepDefinitions.cs
│   │   │   └── ...
│   │   ├── <img src=".github/assets/icons/folder-project.svg" width="16"/> SwagLabs
│   │   │   ├── <img src=".github/assets/icons/csharp.svg" width="16"/> SwagLabsBaseStepDefinitions.cs
│   │   │   ├── <img src=".github/assets/icons/csharp.svg" width="16"/> UserAuthenticationStepDefinitions.cs
│   │   │   └── ...
│   │   └── <img src=".github/assets/icons/csharp.svg" width="16"/> BaseStepDefinitions.cs
│   ├── <img src=".github/assets/icons/csharp.svg" width="16"/> GlobalUsings.cs
│   ├── <img src=".github/assets/icons/json.svg" width="16"/> credentials.json
│   ├── <img src=".github/assets/icons/json.svg" width="16"/> reqnroll.json
│   └── <img src=".github/assets/icons/visualstudio.svg" width="16"/> ReqnrollAutomation.csproj
├── <img src=".github/assets/icons/folder-src.svg" width="16"/> <b>ReqnrollAutomation.Core</b>
│   ├── <img src=".github/assets/icons/folder-config.svg" width="16"/> <b>Config</b>
│   │   ├── <img src=".github/assets/icons/json.svg" width="16"/> config.json
│   │   ├── <img src=".github/assets/icons/csharp.svg" width="16"/> ConfigProvider.cs
│   │   └── <img src=".github/assets/icons/csharp.svg" width="16"/> IConfigAdapter.cs
│   ├── <img src=".github/assets/icons/folder-plugin.svg" width="16"/> <b>Extensions</b>
│   │   ├── <img src=".github/assets/icons/csharp.svg" width="16"/> ReqnrollContextExtensions.cs
│   │   ├── <img src=".github/assets/icons/csharp.svg" width="16"/> WebDriverExtensions.cs
│   │   └── <img src=".github/assets/icons/csharp.svg" width="16"/> WebElementExtensions.cs
│   └── <img src=".github/assets/icons/folder-helper.svg" width="16"/> <b>Helpers</b>
│       ├── <img src=".github/assets/icons/csharp.svg" width="16"/> ExtentReportPatcher.cs
│       ├── <img src=".github/assets/icons/csharp.svg" width="16"/> LogMessageFormatter.cs
│       ├── <img src=".github/assets/icons/csharp.svg" width="16"/> PathHelper.cs
│       └── <img src=".github/assets/icons/csharp.svg" width="16"/> ReportManager.cs
├── <img src=".github/assets/icons/folder-coverage.svg" width="16"/> <b>TestResults</b>
│   ├── <img src=".github/assets/icons/folder-project.svg" width="16"/> CarfaxCanadaWebsite
│   │   ├── <img src=".github/assets/icons/folder-update.svg" width="16"/> 2026-08-01_135952
│   │   │   ├── <img src=".github/assets/icons/folder-docs.svg" width="16"/> Reports
│   │   │   │   └── <img src=".github/assets/icons/html.svg" width="16"/> ExtentReport.html
│   │   │   └── <img src=".github/assets/icons/folder-images.svg" width="16"/> Screenshots
│   │   │       ├── <img src=".github/assets/icons/image.svg" width="16"/> PASSED_Accessibility_toggle_enables_theme_contrast_140001_8.png
│   │   │       └── ...
│   │   └── ...
│   └── <img src=".github/assets/icons/folder-project.svg" width="16"/> SwagLabs
│       └── ...
├── <img src=".github/assets/icons/nuget.svg" width="16"/> nuget.config
├── <img src=".github/assets/icons/settings.svg" width="16"/> parallel.runsettings
├── <img src=".github/assets/icons/settings.svg" width="16"/> sequential.runsettings
└── <img src=".github/assets/icons/visualstudio.svg" width="16"/> ReqnrollAutomation.slnx
</pre>

## Prerequisites

1.  **_.NET 10 SDK_** installed.
2.  **_Supported Web Browsers_** installed:
    - **_Google Chrome_**
    - **_Microsoft Edge_**
3.  **_Visual Studio 2026_** set up (with **_Reqnroll for Visual Studio 2022 and 2026_** extension configured).
