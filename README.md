# PersonalSite

PersonalSite is a small .NET static site generator for a personal website. Content is written as Markdown files with YAML Front Matter, rendered through Razor templates, and emitted as plain static files that can be deployed to GitHub Pages, FTP, object storage, or any static host.

The project is intentionally a static site compiler, not a CMS. The content, configuration, templates, and assets are the source. The `output/` folder is disposable generated output.

## Architecture

- `PersonalSite.Core`: shared models, routing, navigation, and service contracts.
- `PersonalSite.Content`: Markdown discovery, YAML Front Matter parsing, and Markdig HTML conversion.
- `PersonalSite.Rendering`: RazorLight-based rendering for console applications.
- `PersonalSite.Output`: output cleanup, HTML writing, and static asset copying.
- `PersonalSite.Generator`: CLI entry point and generation pipeline orchestration.

RazorLight is used because it supports Razor templates from a console app without MVC controllers or ASP.NET request handling. The generator sets `PreserveCompilationContext` and `CopyLocalLockFileAssemblies` so RazorLight can compile templates at runtime.

## Folder Structure

```text
src/                       .NET projects
content/                   Markdown pages with YAML Front Matter
config/site.json           Global site configuration
templates/                 Razor layouts, pages, and partials
assets/                    Static CSS and JavaScript copied to output
output/                    Generated static website, ignored by Git
docs/                      Project documentation
```

## Generate the Site

```powershell
dotnet build PersonalSite.sln -m:1
dotnet run --project src/PersonalSite.Generator
```

Or use:

```powershell
.\build.ps1
.\generate.ps1
```

The scripts set `DOTNET_CLI_HOME` to a repo-local ignored folder when the variable is not already set. This helps in restricted shells where the .NET CLI cannot write first-run files to the user profile.

Generated files are written to `output/`.

Directory values in `config/site.json` must be relative paths that resolve inside the repository root.

## Edit Content

Edit Markdown files in `content/`. Each page starts with YAML Front Matter:

```yaml
---
title: "About Me"
slug: "about"
description: "A short page about me"
order: 2
showInNavigation: true
layout: "Main"
template: "Page"
---
```

The Markdown body below the closing `---` is converted to HTML with Markdig.

## Add a Page

Create a new `.md` file under `content/`, add front matter, and choose a slug. A slug of `index` or an empty slug generates `/`. A slug of `writing` generates `/writing/` and writes `output/writing/index.html`.

Set `showInNavigation: true` to include the page in navigation. Navigation items are sorted by `order`, then title.

## Output

Every run cleans `output/`, renders all pages, and copies `assets/` into `output/assets/`. This keeps generated output deterministic for the same source files.

## GitHub Actions Later

A future workflow can install the .NET SDK, run `dotnet run --project src/PersonalSite.Generator`, and publish `output/` to GitHub Pages. No database or server runtime is needed for the deployed site.
