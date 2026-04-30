# Usage

## Run Locally

Build the solution:

```powershell
dotnet build PersonalSite.sln -m:1
```

Generate the site:

```powershell
dotnet run --project src/PersonalSite.Generator
```

The generated website is written to `output/`.

In restricted shells, use `.\build.ps1` and `.\generate.ps1`; they keep .NET CLI first-run files inside the repo-local ignored `.dotnet-cli/` folder.

## Edit Content

Open a Markdown file in `content/`, edit the YAML Front Matter or Markdown body, then run the generator again.

## Add Pages

Create a new `.md` file in `content/`:

```markdown
---
title: "Writing"
slug: "writing"
description: "Notes and articles"
order: 5
showInNavigation: true
layout: "Main"
template: "Page"
---

# Writing

Add page content here.
```

The slug controls routing. `writing` becomes `/writing/` and writes `output/writing/index.html`.

## Modify Layout

Edit `templates/Layouts/Main.cshtml` for the global HTML shell. Edit `templates/Pages/Page.cshtml` for the default page body. Shared navigation and footer live under `templates/Partials/`.

## Modify CSS

Edit `assets/css/site.css`. The entire `assets/` folder is copied to `output/assets/` on each generation.

## Clean and Regenerate

The generator cleans `output/` automatically before every run:

```powershell
dotnet run --project src/PersonalSite.Generator
```

You can also remove `output/` manually if needed; it contains no source files.
