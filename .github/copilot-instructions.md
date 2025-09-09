# PrintToolAI - .NET Console Application
PrintToolAI is a simple .NET 8.0 console application written in C# that prints command-line arguments in a formatted string. The repository includes unit tests using MSTest framework and is configured for both Windows and Linux builds.

Always reference these instructions first and fallback to search or bash commands only when you encounter unexpected information that does not match the info here.

## Working Effectively

### Prerequisites and SDK Installation
- **REQUIRED**: .NET SDK 8.0.401 as specified in global.json
- Check installed SDKs: `dotnet --list-sdks`
- If the required SDK version is not available:
  - **CRITICAL**: The build system requires exactly version 8.0.401
  - You may need to modify global.json to use an available compatible version (8.0.x)
  - Test builds will fail with SDK version mismatches

### Build Process (NEVER CANCEL - Set timeouts appropriately)
1. **Restore packages**: `dotnet restore --locked-mode`
   - Takes ~15 seconds. NEVER CANCEL. Set timeout to 60+ seconds.
   - Uses locked-mode for reproducible builds
   
2. **Build main project**: `dotnet build PrintTool/PrintTool.csproj --no-restore -c Release -a x64`
   - Takes ~10 seconds. NEVER CANCEL. Set timeout to 60+ seconds.
   
3. **Build test project**: `dotnet build PrintTool.UnitTests/PrintTool.UnitTests.csproj --no-restore -c Release -a x64`
   - Takes ~2 seconds. NEVER CANCEL. Set timeout to 60+ seconds.

### Testing (NEVER CANCEL)
- **Run tests**: `dotnet test --no-build --configuration Release --logger:"console" --arch x64`
  - Takes ~3 seconds. NEVER CANCEL. Set timeout to 120+ seconds.
  - All tests must pass before proceeding
  - Uses MSTest framework with 3 unit tests for the Splitter class

### Publishing and Distribution
- **Publish for Linux**: `dotnet publish PrintTool/PrintTool.csproj -a x64 --output artifacts/PrintToolLinux --no-restore -c Release`
  - Takes ~2 seconds. NEVER CANCEL. Set timeout to 60+ seconds.
  - Creates self-contained executable in artifacts/PrintToolLinux/
  
- **Publish for Windows**: `dotnet publish PrintTool/PrintTool.csproj -a x64 --output artifacts/PrintToolWindows --no-restore -c Release`
  - Takes ~2 seconds. NEVER CANCEL. Set timeout to 60+ seconds.

### Running the Application
- **Linux**: `./artifacts/PrintToolLinux/PrintTool "arg1" "arg2" "arg3"`
- **Direct from build**: `dotnet run --project PrintTool/PrintTool.csproj -- "arg1" "arg2" "arg3"`
- **Expected output format**: `Print: arg1, arg2, arg3`

## Validation Scenarios
**ALWAYS manually validate changes by running these scenarios:**

1. **Basic functionality test**: 
   ```bash
   ./artifacts/PrintToolLinux/PrintTool "Hello" "world" "from" "PrintTool"
   # Expected output: Print: Hello, world, from, PrintTool
   ```

2. **Empty arguments test**:
   ```bash
   ./artifacts/PrintToolLinux/PrintTool
   # Expected output: Print: 
   ```

3. **Single argument test**:
   ```bash
   ./artifacts/PrintToolLinux/PrintTool "single"
   # Expected output: Print: single
   ```

4. **Unit test validation**:
   ```bash
   dotnet test --no-build --configuration Release --logger:"console" --arch x64
   # All 3 tests must pass: Splitter_NoArgs_EmptyString, Splitter_OneArg_OneArg, Splitter_TwoArgs_TwoArgsCombinedWithComma
   ```

## CI/CD Integration
- **GitHub Actions**: `.github/workflows/main.yml` builds for both Windows and Linux
- **Build steps mirror local development**: restore → build → test → publish
- **Deploy steps**: test the published executable with sample arguments
- **Always run the full CI pipeline commands locally** before pushing changes

## Key Project Structure
```
PrintToolAI/
├── PrintTool/                          # Main console application
│   ├── PrintTool.csproj                # .NET 8.0 console app project
│   ├── Program.cs                      # Entry point with Splitter class
│   └── packages.lock.json              # Locked package versions
├── PrintTool.UnitTests/                # Unit test project  
│   ├── PrintTool.UnitTests.csproj      # MSTest project
│   ├── ProgramTests.cs                 # Tests for Splitter class
│   ├── Usings.cs                       # Global usings
│   └── packages.lock.json              # Locked test package versions
├── PrintTool.sln                       # Solution file
├── global.json                         # SDK version requirement (8.0.401)
├── .github/workflows/main.yml           # CI/CD pipeline
└── .vscode/                            # VS Code configuration
    ├── launch.json                     # Debug configuration
    └── tasks.json                      # Build tasks
```

## Common Issues and Solutions
- **SDK version mismatch**: Modify global.json to use available SDK version if 8.0.401 is not available
- **Test failures**: The Splitter class has internal visibility - ensure tests reference the correct assembly
- **Build artifacts**: Artifacts folder is ignored by git (.gitignore) - this is correct behavior
- **Package restore**: Always use `--locked-mode` to ensure reproducible builds

## Development Workflow
1. **Always start with**: `dotnet restore --locked-mode` 
2. **Build incrementally**: Main project first, then tests
3. **Test before publishing**: Ensure all unit tests pass
4. **Validate functionality**: Run the executable with test arguments
5. **Never skip validation**: Test both success and edge cases

## Time Expectations Summary
- Package restore: ~15 seconds
- Build main project: ~10 seconds  
- Build tests: ~2 seconds
- Run tests: ~3 seconds
- Publish: ~2 seconds
- **Total development cycle**: ~32 seconds
- **CRITICAL**: Always set timeouts to 2-3x expected time to prevent premature cancellation