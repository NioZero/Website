# Architecture

PersonalSite is structured as a static site compiler. It reads source files, builds an in-memory model of the site, renders final HTML, and writes disposable static output.

## Content Layer

`PersonalSite.Content` discovers `.md` files under the configured content directory. Each file must begin with YAML Front Matter. The parser reads metadata into `PageMetadata` and keeps the Markdown body separate. Markdig converts the Markdown body into HTML.

## Configuration Layer

Global site settings live in `config/site.json` and are loaded with `Microsoft.Extensions.Configuration`. The configuration controls the site name, author, language, base URL, and the relative directories used by the generator.

Configured directories are resolved relative to the repository root and rejected if they are absolute paths or escape the repository. This keeps output cleanup and asset copying scoped to the project.

## Rendering Layer

`PersonalSite.Rendering` uses RazorLight to render Razor templates from the `templates/` folder in a console app. Page templates can use layouts and shared partials such as navigation and footer.

This project uses Razor instead of T4 because Razor is a familiar HTML templating language, supports layouts and partials naturally, and does not require design-time code generation.

Template and layout names from front matter are restricted to simple file names. The renderer caches the RazorLight engine for the template directory so repeated page rendering does not rebuild the engine every time.

## Generation Layer

`PersonalSite.Generator` orchestrates the pipeline:

1. Clean the output directory.
2. Discover Markdown files.
3. Parse metadata and Markdown.
4. Convert Markdown to HTML.
5. Build URLs, output paths, and navigation.
6. Render pages with Razor.
7. Write HTML and copy assets.

Dependency injection is used to keep these boundaries clear without making the first version heavier than necessary.

## Output Layer

`PersonalSite.Output` writes generated HTML files and copies static assets. The `output/` directory is ignored by Git because it can always be regenerated from source content, config, templates, and assets.

## No Database

The content files are the source of truth. A database would add runtime state, backups, migrations, and hosting requirements that do not fit a personal static website generator.

## Markdown and Front Matter

Markdown keeps content easy to edit in any text editor. YAML Front Matter provides structured metadata for titles, descriptions, slugs, ordering, navigation, layouts, and templates.

## Static Site Compiler

The generator performs all work before deployment. The generated website is plain HTML, CSS, JavaScript, and copied assets. Static hosting does not need ASP.NET, MVC controllers, a database, or server-side rendering.
