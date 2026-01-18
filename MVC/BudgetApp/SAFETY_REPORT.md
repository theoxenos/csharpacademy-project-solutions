# Safety Analysis Report - BudgetApp

This report documents potential safety and security problems identified in the BudgetApp project.

## Summary of Findings

| ID | Issue | Severity | Status |
|---|---|---|---|
| SEC-01 | Missing Anti-Forgery Protection (CSRF) | Critical | **Fixed** |
| SEC-02 | Mass Assignment / Overposting | Critical | **Fixed** |
| SEC-03 | XSS in CSS Style Attributes | High | **Fixed** |
| SEC-04 | Data Exposure in Detail Endpoints | Medium | Logged |
| SEC-05 | Lack of Rate Limiting | Low | Logged |

---

## Detailed Findings

### SEC-01: Missing Anti-Forgery Protection (CSRF)
- **Severity:** Critical
- **Description:** The application performs state-changing operations (Create, Update, Delete) via AJAX without validating Anti-Forgery Tokens. An attacker could trick a logged-in user into performing unwanted actions.
- **Fix:** Added `[ValidateAntiForgeryToken]` to all POST, PUT, and DELETE actions and updated JavaScript (`site.js`) to include the token in request headers for all AJAX calls.

### SEC-02: Mass Assignment / Overposting
- **Severity:** Critical
- **Description:** Controllers accept domain models (`Transaction`, `Category`) directly in action parameters. An attacker could potentially modify properties that should not be user-changeable by including extra fields in the request body.
- **Fix:** Added `[Bind]` attributes to `Create` and `Update` actions to whitelist allowed properties.

### SEC-03: XSS in CSS Style Attributes
- **Severity:** High
- **Description:** The `Category.Color` property is rendered directly into `style="background-color: @category.Color"`. If a user can set a malicious value (e.g., `red; xss:expression(alert(1))`), they might execute arbitrary JavaScript or break the layout.
- **Fix:** Added regex validation to the `Color` property in `Category` model to ensure it only contains valid hex color codes (e.g., `#FFFFFF` or `#FFF`).

### SEC-04: Data Exposure in Detail Endpoints
- **Severity:** Medium
- **Description:** The `Detail` actions in `TransactionsController` and `CategoriesController` return the entire entity object. While currently safe, this can lead to accidental exposure of sensitive internal fields if the models are expanded in the future.
- **Recommendation:** Use specific ViewModels/DTOs for detail responses to control exactly what data is returned.

### SEC-05: Lack of Rate Limiting
- **Severity:** Low
- **Description:** There is no rate limiting on the API endpoints, making the application susceptible to brute-force or denial-of-service attacks.
- **Recommendation:** Implement ASP.NET Core Rate Limiting middleware.
