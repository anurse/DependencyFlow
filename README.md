# Dependency Flow Widgets

Widgets to display .NET Core dependency flow data.

## Configuration

The following configuration options are needed and should be provided using `dotnet user-secrets set...`:

* `Maestro:Token` - a token for accessing Maestro++ APIs
* `GitHub:Token` - a personal access token for GitHub, only requires access to the repos you want a widget for.