<!doctype html>

<html lang="en">
<head>
  <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <title>OrdersAPI — .NET 8 Web API (CQRS + Events)</title>
  <style>
    body{font-family:Inter,system-ui,-apple-system,Segoe UI,Roboto,'Helvetica Neue',Arial;line-height:1.6;color:#111;background:#f7f9fb;padding:32px}
    .container{max-width:960px;margin:0 auto;background:#fff;padding:28px;border-radius:12px;box-shadow:0 6px 18px rgba(20,30,40,0.06)}
    header{display:flex;align-items:center;gap:16px}
    h1{margin:0;font-size:1.6rem}
    .badges img{height:20px;margin-right:8px}
    nav{margin-top:12px}
    pre{background:#0b1220;color:#e6eef8;padding:14px;border-radius:8px;overflow:auto}
    code{font-family:ui-monospace,SFMono-Regular,Menlo,Monaco,Roboto Mono,Segoe UI Mono,monospace}
    dl{display:grid;grid-template-columns:140px 1fr;gap:8px 20px}
    section{margin-top:20px}
    footer{margin-top:32px;font-size:0.9rem;color:#555}
    .inline-code{background:#eef2f7;padding:2px 6px;border-radius:6px}
  </style>
</head>
<body>
  <div class="container">
    <header>
      <div>
        <h1>OrdersAPI — .NET 8 Web API (CQRS + Event-driven Read Model)</h1>
        <div class="badges">
          <img src="https://img.shields.io/badge/build-passing-brightgreen" alt="build"/>
          <img src="https://img.shields.io/badge/license-MIT-blue.svg" alt="license"/>
        </div>
      </div>
    </header>

```
<p>A small ASP.NET Core Web API demonstrating CQRS with an event-driven read model: commands mutate the write database, events project changes into a separate read database.</p>

<section>
  <h2>Quick links</h2>
  <dl>
    <dt>Status</dt>
    <dd>Prototype</dd>
    <dt>Stack</dt>
    <dd>.NET 8, ASP.NET Core, EF Core (SQLite), FluentValidation</dd>
  </dl>
</section>

<section>
  <h2>Features</h2>
  <ul>
    <li>Separate <strong>Write</strong> and <strong>Read</strong> DbContexts (SQLite files under <span class="inline-code">Data/DB</span>).</li>
    <li>Commands + CommandHandlers (write-side) and Queries + QueryHandlers (read-side).</li>
    <li>Lightweight in-process <span class="inline-code">IEventPublisher</span> and projection handlers to sync read model.</li>
    <li>FluentValidation for command validation.</li>
    <li>Swagger / OpenAPI for API discovery.</li>
  </ul>
</section>

<section>
  <h2>Getting started</h2>

  <h3>Prerequisites</h3>
  <ul>
    <li><a href="https://dotnet.microsoft.com/en-us/download/dotnet/8.0">.NET 8 SDK</a></li>
    <li>Visual Studio 2022/2023 or VS Code (optional)</li>
    <li><code>dotnet-ef</code> tool for migrations:</li>
  </ul>
  <pre><code>dotnet tool install --global dotnet-ef</code></pre>

  <h3>Clone</h3>
  <pre><code>git clone &lt;repo-url&gt;
```

cd \<repo-folder></code></pre>

```
  <h3>Restore &amp; build</h3>
  <pre><code>dotnet restore
```

dotnet build</code></pre>

```
  <h3>Migrations (EF Core)</h3>
  <p>Migrations are kept separately for <code>WriteDbContext</code> and <code>ReadDbContext</code> under <code>Migrations/Write</code> and <code>Migrations/Read</code>.</p>

  <p>Create a migration for the write db:</p>
  <pre><code>dotnet ef migrations add InitialWrite --context WriteDbContext --output-dir Migrations/Write
```

dotnet ef database update --context WriteDbContext</code></pre>

```
  <p>For the read db:</p>
  <pre><code>dotnet ef migrations add InitialRead --context ReadDbContext --output-dir Migrations/Read
```

dotnet ef database update --context ReadDbContext</code></pre>

```
  <p>If using Visual Studio's Package Manager Console, select the correct Default Project and run <code>Add-Migration</code> / <code>Update-Database</code> with the <code>-Context</code> parameter.</p>

  <h3>Run the API</h3>
  <pre><code>dotnet run --project src/OrdersApi/OrdersApi.csproj
```

# then open [http://localhost:\&lt;port\&gt;/swagger](http://localhost:&lt;port&gt;/swagger)</code></pre>

```
</section>

<section>
  <h2>Typical flow</h2>
  <ol>
    <li><code>POST /orders</code> — client sends <code>CreateOrderCommand</code> (validated via FluentValidation).</li>
    <li><code>CreateOrderCommandHandler</code> persists the <code>Order</code> to the <strong>write</strong> DB and publishes <code>OrderCreatedEvent</code> via <code>IEventPublisher</code>.</li>
    <li><code>OrderCreatedProjectionHandler</code> receives the event and updates the <strong>read</strong> DB (<code>ReadDbContext</code>).</li>
    <li><code>GET /orders</code> and <code>GET /orders/{id}</code> read from the read DB and return DTOs.</li>
  </ol>
</section>

<section>
  <h2>Example requests</h2>
  <p>Create an order:</p>
  <pre><code>curl -X POST "http://localhost:5000/orders" \
```

-H "Content-Type: application/json"&#x20;
-d '{ "customerName": "Alice", "items": \[{ "productId": 1, "quantity": 2 }], "total": 29.99 }'</code></pre>

```
  <p>Get list of orders:</p>
  <pre><code>curl http://localhost:5000/orders</code></pre>

  <p>Get order by id:</p>
  <pre><code>curl http://localhost:5000/orders/1</code></pre>
</section>

<section>
  <h2>Project layout</h2>
  <pre><code>src/
```

├─ Program.cs
├─ Features/
│  ├─ Commands/
│  └─ Queries/
├─ Projections/
├─ Services/Events/
├─ Data/
│  ├─ WriteDbContext.cs
│  └─ ReadDbContext.cs
├─ Dtos/
└─ Migrations/
├─ Read/
└─ Write/
Data/DB/  # sqlite files</code></pre> </section>

```
<section>
  <h2>Troubleshooting</h2>
  <h3>DI error: Unable to resolve service for type <code>IEventPublisher</code></h3>
  <p>Make sure the event publisher is registered in <code>Program.cs</code>:</p>
  <pre><code>// Example registration
```

builder.Services.AddSingleton\<IEventPublisher, InProcessEventPublisher>();</code></pre>

```
  <h3>Migrations target wrong DB/context</h3>
  <p>Always pass <code>--context ReadDbContext</code> or <code>--context WriteDbContext</code> when creating or applying migrations.</p>
</section>

<section>
  <h2>Development notes</h2>
  <ul>
    <li>Keep projection handlers idempotent — events may be published or handled more than once during development or retries.</li>
    <li>Decide whether <code>IEventPublisher</code> should be <code>Singleton</code>, <code>Scoped</code>, or <code>Transient</code> consistently with how handlers access DbContexts.</li>
    <li>Consider moving to durable event transport (e.g., RabbitMQ, Kafka) when scaling beyond in-process projections.</li>
  </ul>
</section>

<section>
  <h2>Contributing</h2>
  <ol>
    <li>Fork the repo</li>
    <li>Create a branch (<code>feature/my-feature</code>)</li>
    <li>Add tests and migrations if needed</li>
    <li>Open a PR describing your change</li>
  </ol>
</section>

<footer>
  <p>MIT — see <code>LICENSE</code> file.</p>
</footer>
```

  </div>
</body>
</html>
