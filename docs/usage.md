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

Keep Markdown bodies content-only. Do not put raw HTML in Markdown for page features such as social links; use structured Front Matter and let Razor templates render the HTML.

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

## Add Social Links

Pages can define social links in YAML Front Matter. The `Contact` template renders them after the Markdown body:

```yaml
template: "Contact"
social:
  - name: "Twitter"
    url: "https://twitter.com/NioZero"
    label: "@NioZero"
    icon: "bxl-twitter"
  - name: "GitHub"
    url: "https://github.com/NioZero"
    label: "NioZero"
    icon: "bxl-github"
```

Icons use Boxicons class names, such as `bxl-twitter`, `bxl-github`, or `bxl-youtube`. Presentation belongs in Razor templates and CSS, so the Markdown body stays simple prose.

Social link URLs must be absolute `http`, `https`, or `mailto` URLs. The `label` is what appears on the page.

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
