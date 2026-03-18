# C# Code Quality Standards

> **Audience:** All developers — human and AI agentic — contributing to this organization's codebase.
> **Scope:** C# language conventions.
> **Version:** 1.0 | Last Updated: March 2026

---

## How to Read This Document

Rules are written as clear **DO** / **DON'T** directives. Where the C# community is legitimately divided, we note the debate and make a recommendation — but ultimately defer to developer discretion and team agreement at the project level. Every rule includes a code example.

---

## 1. Naming

We follow the [Microsoft C# naming conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/identifier-names) as our baseline.

### 1.1 General Casing Rules

| Identifier | Convention | Example |
|---|---|---|
| Classes, structs, records | PascalCase | `OrderProcessor` |
| Interfaces | PascalCase with `I` prefix | `IOrderRepository` |
| Methods | PascalCase | `CalculateTotal()` |
| Properties | PascalCase | `FirstName` |
| Public fields | PascalCase | `MaxRetryCount` |
| Private / internal fields | `_camelCase` | `_orderRepository` |
| Local variables | camelCase | `orderTotal` |
| Parameters | camelCase | `customerId` |
| Constants | PascalCase | `MaxPageSize` |
| Enums | PascalCase (type and values) | `OrderStatus.Pending` |
| Type parameters | PascalCase with `T` prefix | `TEntity` |

---

### 1.2 Classes and Types

**DO** name classes with nouns or noun phrases that clearly describe their responsibility.

```csharp
// ✅ DO
public class InvoiceGenerator { }
public class CustomerRepository { }

// ❌ DON'T
public class DoStuff { }
public class Manager { }  // too vague — manager of what?
```

**DON'T** include the type name redundantly in the identifier.

```csharp
// ✅ DO
public class Order { }
public List<Order> Orders { get; set; }

// ❌ DON'T
public class OrderClass { }
public List<Order> OrderList { get; set; }
```

**DON'T** use generalizing structural adjectives such as `Abstract`, `Base`, `Prototype`, `Helper`, or `Utility` in class names. The name should describe what the class *is* or *does*, not its position in a hierarchy.

```csharp
// ✅ DO
public abstract class Animal { }
public class SqlOrderRepository : OrderRepository { }

// ❌ DON'T
public abstract class AbstractAnimal { }
public class BaseOrderRepository { }
public class OrderHelper { }
```

---

### 1.3 Interfaces

**DO** prefix interfaces with `I`.

```csharp
// ✅ DO
public interface IPaymentProcessor { }

// ❌ DON'T
public interface PaymentProcessor { }
public interface PaymentProcessorInterface { }
```

**DON'T** name an interface simply after what it is (e.g., a direct mirror of the implementing class name). Interface names should describe a *capability* or *contract*, not just wrap a class name.

```csharp
// ✅ DO — describes a capability or role
public interface IOrderRepository { }
public interface INotifiable { }

// ❌ DON'T — interface name is just the class name with an I prefix and no distinct meaning
public interface IEmailService { }  // when EmailService is the only conceivable implementation
```

**Exception:** Abstract interfaces and DI contracts — where the interface exists specifically to uniquely identify a class for the DI container — may mirror the implementing type name.

```csharp
// ✅ ACCEPTABLE — sole purpose is DI registration identity
public interface IEmailNotificationService { }
public class EmailNotificationService : IEmailNotificationService { }
```

**DON'T** use marker interfaces (interfaces with no members) to tag or categorize types. Prefer attributes or explicit properties instead.

```csharp
// ❌ DON'T — marker interface used as a type tag
public interface IAuditable { }
public class Order : IAuditable { }

// ✅ DO — use an attribute or property to express the same intent
[Auditable]
public class Order { }
```

**Exception:** Marker interfaces are acceptable when used specifically to support dependency injection registration or resolution.

```csharp
// ✅ ACCEPTABLE — marker used for DI scanning/registration
public interface IDomainEvent { }
services.AddAllImplementationsOf<IDomainEvent>();
```

---

### 1.4 Methods

**DO** name methods with verb or verb-phrase names that describe the action performed.

```csharp
// ✅ DO
public void SendNotification(Notification notification) { }
public Order GetOrderById(int orderId) { }

// ❌ DON'T
public void Notification(Notification notification) { }
public Order Order(int id) { }
```

---

### 1.5 Private Fields

**DO** prefix private instance fields with an underscore and use camelCase.

```csharp
// ✅ DO
private readonly IOrderRepository _orderRepository;
private int _retryCount;

// ❌ DON'T
private readonly IOrderRepository orderRepository;
private readonly IOrderRepository OrderRepository;
```

---

### 1.6 Boolean Identifiers

**DO** prefix boolean properties, fields, and variables with `is`, `has`, `can`, or `should` where it improves clarity.

```csharp
// ✅ DO
public bool IsActive { get; set; }
public bool HasPendingOrders { get; set; }
bool canRetry = retryCount < MaxRetries;

// ❌ DON'T
public bool Active { get; set; }
public bool PendingOrders { get; set; }
```

**DO** name booleans in the affirmative (positive state). A `true` value should represent the presence or enablement of something.

```csharp
// ✅ DO
public bool IsEnabled { get; set; }
public bool IsActive { get; set; }
public bool HasContent { get; set; }
public bool IsVisible { get; set; }
```

**DON'T** name booleans in the negative. Double-negatives in conditional logic are difficult to read and error-prone.

```csharp
// ❌ DON'T
public bool IsDisabled { get; set; }   // use IsEnabled
public bool IsInactive { get; set; }   // use IsActive
public bool IsEmpty { get; set; }      // use HasContent
public bool IsHidden { get; set; }     // use IsVisible

// ❌ DON'T — double negative in practice
if (!user.IsDisabled) { }   // reads as "if not not-enabled"

// ✅ DO
if (user.IsEnabled) { }
```

---

### 1.7 Abbreviations and Acronyms

**DON'T** use abbreviations unless they are universally understood (e.g., `Id`, `Url`, `Http`).

```csharp
// ✅ DO
public int CustomerId { get; set; }
public string HttpMethod { get; set; }

// ❌ DON'T
public int CustId { get; set; }
public string HM { get; set; }
```

**DO** treat two-letter acronyms as uppercase, and acronyms of three or more letters as PascalCase.

```csharp
// ✅ DO
public string Id { get; set; }
public string IOStream { get; set; }   // two-letter: all caps
public string HttpRequest { get; set; } // three+: PascalCase

// ❌ DON'T
public string ID { get; set; }
public string HttpRQST { get; set; }
```

---

### 1.8 Solution and Project Namespaces

**DON'T** repeat the solution name in the namespace of child projects when the solution is a container grouping multiple independent helper projects. Each project should stand on its own as a first-class namespace under the company or domain root.

> **Rationale:** Repeating the solution name adds noise and can obscure genuinely distinct libraries. A consumer shouldn't have to know that `MyCompany.EmailFormatter` lives inside a solution called `Common` — and namespaces like `MyCompany.Common.Foo` vs `MyCompany.Core.Foo` become hard to distinguish at a glance. Flat, purposeful namespaces communicate intent clearly.

```
// ✅ DO — each project is a first-class namespace
Solution: MyCompany.Common (container only)
  ├── MyCompany.EmailFormatter
  ├── MyCompany.TokenReplacer
  └── MyCompany.DateTimeExtensions

// ❌ DON'T — solution name repeated in every child namespace
Solution: MyCompany.Common
  ├── MyCompany.Common.EmailFormatter
  ├── MyCompany.Common.TokenReplacer
  └── MyCompany.Common.DateTimeExtensions
```

**DO** name project namespaces after what they *do* or what they *are*, rooted at the company or domain level.

```csharp
// ✅ DO
namespace MyCompany.EmailFormatter;
namespace MyCompany.TokenReplacer;

// ❌ DON'T
namespace MyCompany.Common.EmailFormatter;
namespace MyCompany.Common.TokenReplacer;
```

---

## 2. Nullability & Null Safety

Nullable reference types (NRTs) are a **per-project decision**, with `enable` as the recommended default. Each project must explicitly declare its nullability stance in the `.csproj` file or at the file level, and document any deviation from the default in the project README.

### 2.1 Project-Level Nullability Declaration

**DO** explicitly declare nullability context — do not leave it implicit. **Recommend `enable`** unless there is a documented reason to disable it (e.g., a large legacy codebase mid-migration).

```xml
<!-- ✅ RECOMMENDED default -->
<Nullable>enable</Nullable>

<!-- ✅ ACCEPTABLE — but must be documented in the project README -->
<Nullable>disable</Nullable>
```

```csharp
// ✅ DO: file-level override when incrementally enabling NRTs in a transitioning codebase
#nullable enable

// ❌ DON'T: leave nullability context unspecified and undocumented
```

---

### 2.2 Null Checks

**DO** use null-conditional (`?.`) and null-coalescing (`??`) operators to reduce verbosity.

```csharp
// ✅ DO
string name = customer?.Name ?? "Unknown";
customer?.Address?.PostalCode;

// ❌ DON'T
string name = customer != null ? customer.Name != null ? customer.Name : "Unknown" : "Unknown";
```

**DON'T** use `!` (null-forgiving operator) to silence warnings without a documented reason.

```csharp
// ✅ DO — use only when you have verified non-null and add a comment
string name = customer!.Name; // Customer is guaranteed non-null by the factory that calls this method

// ❌ DON'T — silencing the compiler without justification
string name = customer!.Name;
```

---

### 2.3 Null Guards on Public API Boundaries

**DO** validate parameters for null at public method boundaries and throw `ArgumentNullException`.

```csharp
// ✅ DO (.NET 6+)
public void ProcessOrder(Order order)
{
    ArgumentNullException.ThrowIfNull(order);
    // ...
}

// ✅ DO (pre-.NET 6)
public void ProcessOrder(Order order)
{
    if (order is null) throw new ArgumentNullException(nameof(order));
    // ...
}

// ❌ DON'T — silently proceed with a null argument
public void ProcessOrder(Order order)
{
    var total = order.Total; // NullReferenceException at runtime
}
```

---

### 2.4 Returning Null vs. Empty

**DO** return empty collections instead of null from methods that return collections.

```csharp
// ✅ DO
public IEnumerable<Order> GetOrdersByCustomer(int customerId)
{
    return _orders.Where(o => o.CustomerId == customerId) ?? Enumerable.Empty<Order>();
}

// ❌ DON'T
public IEnumerable<Order> GetOrdersByCustomer(int customerId)
{
    return null; // forces every caller to null-check
}
```

**RECOMMEND** using the Null Object pattern or `Optional<T>` semantics over returning null from single-value methods when absence is a valid, expected state.

```csharp
// ✅ RECOMMENDED — signals "not found" without null
public Order? FindOrderById(int orderId)
{
    return _orders.FirstOrDefault(o => o.Id == orderId);
}

// caller handles absence explicitly
var order = FindOrderById(id);
if (order is null) { /* handle not found */ }
```

---

## 3. Async/Await Patterns

### 3.1 Method Naming

**DO** suffix all async methods with `Async`.

```csharp
// ✅ DO
public async Task<Order> GetOrderByIdAsync(int orderId, CancellationToken cancellationToken) { }

// ❌ DON'T
public async Task<Order> GetOrderById(int orderId) { }
```

**Exception:** Interface implementations and overrides of framework-defined methods (e.g., event handlers, `IHostedService.StartAsync`) do not need the suffix added beyond what the framework defines.

---

### 3.2 CancellationToken

**DO** accept a `CancellationToken` as the last parameter on every async public method.

```csharp
// ✅ DO
public async Task<IEnumerable<Product>> GetProductsAsync(
    ProductFilter filter,
    CancellationToken cancellationToken)
{
    return await _repository.GetAsync(filter, cancellationToken);
}

// ❌ DON'T
public async Task<IEnumerable<Product>> GetProductsAsync(ProductFilter filter)
{
    return await _repository.GetAsync(filter); // cannot be cancelled
}
```

**DO** propagate the `CancellationToken` to all downstream async calls — do not swallow it.

```csharp
// ✅ DO
await _httpClient.GetAsync(url, cancellationToken);
await _dbContext.SaveChangesAsync(cancellationToken);

// ❌ DON'T
await _httpClient.GetAsync(url);             // token lost
await _dbContext.SaveChangesAsync();          // token lost
```

---

### 3.3 Avoid async void

**DON'T** use `async void` except for event handlers.

```csharp
// ✅ DO
public async Task ProcessAsync(CancellationToken cancellationToken) { }

// ✅ ACCEPTABLE — event handler only
private async void Button_Click(object sender, EventArgs e)
{
    await ProcessAsync(CancellationToken.None);
}

// ❌ DON'T
public async void ProcessAsync() { } // exceptions are unobservable and can crash the process
```

---

### 3.4 ConfigureAwait

**Default stance: ASP.NET Core applications do not need `ConfigureAwait(false)`** — there is no synchronization context to deadlock against.

**DO** use `ConfigureAwait(false)` in library code or any code that may run under a legacy synchronization context (e.g., WinForms, WPF, legacy ASP.NET).

```csharp
// ✅ DO — library or legacy context code
public async Task<string> FetchDataAsync(CancellationToken cancellationToken)
{
    var response = await _httpClient.GetAsync("/data", cancellationToken).ConfigureAwait(false);
    return await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
}

// ✅ ACCEPTABLE — ASP.NET Core application code (no synchronization context)
public async Task<string> FetchDataAsync(CancellationToken cancellationToken)
{
    var response = await _httpClient.GetAsync("/data", cancellationToken);
    return await response.Content.ReadAsStringAsync(cancellationToken);
}

// ❌ DON'T — omit ConfigureAwait(false) in shared library code without understanding the risk
```

**If a project mixes ASP.NET Core and legacy sync-context code, `ConfigureAwait(false)` must be used consistently throughout that project.**

---

### 3.5 Avoid .Result and .Wait()

**DON'T** block on async code using `.Result`, `.Wait()`, or `.GetAwaiter().GetResult()` in application code.

```csharp
// ✅ DO
var order = await GetOrderByIdAsync(id, cancellationToken);

// ❌ DON'T
var order = GetOrderByIdAsync(id, cancellationToken).Result;   // potential deadlock
var order = GetOrderByIdAsync(id, cancellationToken).GetAwaiter().GetResult(); // same risk
```

**Exception:** Entry points that cannot be async (e.g., `Main` in certain frameworks, static constructors) — document the reason when blocking is unavoidable.

---

## 4. Exception Handling

Exception handling is context-dependent. The rules below establish guardrails while preserving flexibility.

### 4.1 Exceptions Are for Exceptional Circumstances

**DO** throw exceptions when a failure represents a genuinely exceptional condition — something that shouldn't happen in normal program flow and that the caller cannot reasonably anticipate without an exception signal.

```csharp
// ✅ DO — requesting a non-existent entity is exceptional; the caller expects it to exist
public async Task<Order> GetOrderByIdAsync(int orderId, CancellationToken cancellationToken)
{
    var order = await _repository.FindAsync(orderId, cancellationToken);
    if (order is null)
        throw new OrderNotFoundException(orderId);
    return order;
}
```

**DON'T** use exceptions as flow control for expected, non-exceptional outcomes. Use null returns, `bool` try-pattern methods, or null-coalescing instead.

```csharp
// ❌ DON'T — counting results is not an exceptional scenario; absence of matches is expected
public async Task<int> CountMatchingOrdersAsync(OrderFilter filter, CancellationToken cancellationToken)
{
    try
    {
        return await _repository.CountAsync(filter, cancellationToken);
    }
    catch (NotFoundException)
    {
        return 0; // absence of results is not an exception
    }
}

// ✅ DO — use null-coalescing or the try-pattern for expected absence
public async Task<int> CountMatchingOrdersAsync(OrderFilter filter, CancellationToken cancellationToken)
{
    return await _repository.CountAsync(filter, cancellationToken) ?? 0;
}
```

---

### 4.2 Library Exception Boundaries

Libraries that are consumed as dependencies by other libraries or projects **must not leak native .NET exceptions or third-party library exceptions** across their public API boundary. Callers should only need to handle exceptions defined by your library.

**DO** wrap all unhandled exceptions at the top-level public boundary of a library with a custom exception type.

```csharp
// ✅ DO — library wraps all unexpected exceptions at its boundary
public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request, CancellationToken cancellationToken)
{
    try
    {
        return await _gateway.SubmitAsync(request, cancellationToken);
    }
    catch (HttpRequestException ex)
    {
        throw new PaymentLibraryException("A network error occurred communicating with the payment gateway.", ex);
    }
    catch (Exception ex)
    {
        throw new PaymentLibraryException("An unexpected error occurred in the payment library.", ex);
    }
}

// ❌ DON'T — leaking HttpRequestException or SqlException forces callers to take a dependency on your internals
public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request, CancellationToken cancellationToken)
{
    return await _gateway.SubmitAsync(request, cancellationToken); // HttpRequestException escapes
}
```

**DO** define a root exception type for your library that all library-specific exceptions inherit from, so callers can catch broadly or narrowly as needed.

```csharp
// ✅ DO
public class PaymentLibraryException : Exception
{
    public PaymentLibraryException(string message) : base(message) { }
    public PaymentLibraryException(string message, Exception innerException) : base(message, innerException) { }
}

public class PaymentNotFoundException : PaymentLibraryException
{
    public PaymentNotFoundException(string paymentId)
        : base($"Payment '{paymentId}' was not found.") { }
}
```

---

### 4.3 Catch What You Can Handle

**DO** catch the most specific exception type applicable to the failure you are handling.

```csharp
// ✅ DO
try
{
    await _repository.SaveAsync(order, cancellationToken);
}
catch (DbUpdateConcurrencyException ex)
{
    _logger.LogWarning(ex, "Concurrency conflict saving order {OrderId}", order.Id);
    throw new OrderConflictException("Order was modified by another process.", ex);
}

// ❌ DON'T — catching broadly when you only handle one specific case
try
{
    await _repository.SaveAsync(order, cancellationToken);
}
catch (Exception ex)
{
    _logger.LogWarning(ex, "Concurrency conflict");
    throw new OrderConflictException("Order was modified.", ex);
}
```

---

### 4.4 Catching Exception Broadly

Catching `Exception` is acceptable when the intent is to log-and-rethrow at a boundary (e.g., a top-level handler, middleware, or background service).

**DO** always rethrow or wrap when catching `Exception` broadly — never swallow.

```csharp
// ✅ DO — boundary-level catch with logging and rethrow
try
{
    await ProcessJobAsync(cancellationToken);
}
catch (Exception ex)
{
    _logger.LogError(ex, "Unhandled error in background job {JobName}", jobName);
    throw; // preserve stack trace
}

// ❌ DON'T — swallowing silently
try
{
    await ProcessJobAsync(cancellationToken);
}
catch (Exception)
{
    // nothing — failure is invisible
}
```

---

### 4.5 Never Catch and Ignore

**DON'T** catch an exception and take no action.

```csharp
// ❌ DON'T
try
{
    SendNotification(notification);
}
catch (Exception) { }  // failure is invisible to the system and to operators
```

---

### 4.6 Exception Wrapping

**DO** wrap lower-level exceptions in domain-meaningful exceptions at layer boundaries, and always include the original exception as `innerException`.

```csharp
// ✅ DO
try
{
    await _httpClient.PostAsync(endpoint, content, cancellationToken);
}
catch (HttpRequestException ex)
{
    throw new PaymentGatewayException("Failed to reach payment gateway.", ex);
}

// ❌ DON'T — losing the original context
catch (HttpRequestException)
{
    throw new PaymentGatewayException("Failed to reach payment gateway."); // inner exception lost
}
```

---

### 4.7 Use throw, Not throw ex

**DO** use `throw;` to rethrow — never `throw ex;` which resets the stack trace.

```csharp
// ✅ DO
catch (Exception ex)
{
    _logger.LogError(ex, "Unexpected error");
    throw;
}

// ❌ DON'T
catch (Exception ex)
{
    _logger.LogError(ex, "Unexpected error");
    throw ex; // stack trace is reset — origin of the error is lost
}
```

---

### 4.8 Custom Exception Types

**DO** create custom exception types for domain-specific failure scenarios that callers may need to distinguish.

```csharp
// ✅ DO
public class OrderNotFoundException : Exception
{
    public int OrderId { get; }

    public OrderNotFoundException(int orderId)
        : base($"Order {orderId} was not found.")
    {
        OrderId = orderId;
    }

    public OrderNotFoundException(int orderId, Exception innerException)
        : base($"Order {orderId} was not found.", innerException)
    {
        OrderId = orderId;
    }
}

// ❌ DON'T — using generic exceptions for domain errors callers need to handle
throw new Exception("Order not found");
throw new InvalidOperationException("Order 42 was not found");
```

---

## Appendix A: Community-Debated Conventions

The following areas have no mandated rule. The recommendation is noted — apply it consistently within a project.

| Topic | Recommendation | Rationale |
|---|---|---|
| `var` vs explicit types | **Recommend `var`** when the type is obvious from the right-hand side | Reduces noise; explicit types still preferred when type is non-obvious |
| File-scoped namespaces | **Recommend file-scoped** (`namespace Foo.Bar;`) | Reduces indentation in modern C# projects |
| Primary constructors | **Recommend** in simple cases | Clean for dependency injection; avoid when logic is needed |
| Expression-bodied members | **Recommend** for single-expression members | Concise; avoid for complex logic |

---

## Appendix B: AI Agent Developer Notes

This document applies equally to AI agents generating, reviewing, or modifying C# code. Agents must:

- Treat every **DO** as a strict rule unless context makes compliance impossible.
- Treat every **DON'T** as a hard constraint — flag violations rather than silently introduce them.
- For **RECOMMEND** rules, apply the recommendation by default and note when deviating.
- When project-level decisions are referenced (e.g., nullability, ConfigureAwait stance), read the project README or `.csproj` before generating code.
- When in doubt, produce compliant code and surface the uncertainty in a comment or PR note.

---

*This is a living document. Raise proposed changes via pull request with rationale.*