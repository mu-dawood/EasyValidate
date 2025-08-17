namespace EasyValidate.Abstractions;

/// <summary>
/// Represents the basic contract for validation result operations.
/// </summary>
/// <docs-display-name>Validation Result Interface</docs-display-name>
/// <docs-icon>ClipboardCheck</docs-icon>
/// <docs-description>Base interface for validation results providing essential validation status checking capabilities.</docs-description>
public interface IPropertyResult
{
    /// <summary>
    /// Gets the property name associated with this validation result.
    /// </summary>
    /// <value>
    /// The property name for which this validation result was generated. Returns an empty string if not applicable.
    /// </value>
    string PropertyName { get; }

    /// <summary>
    /// Determines whether this validation result contains any validation errors.
    /// </summary>
    /// <returns>
    /// <c>true</c> if there are validation errors; otherwise, <c>false</c>.
    /// </returns>
    /// <docs-member>HasErrors()</docs-member>
    /// <docs-type>Method</docs-type>
    /// <docs-return-type>bool</docs-return-type>
    /// <summary>
    /// Determines whether this property result contains any direct validation errors.
    /// </summary>
    /// <returns>
    /// <c>true</c> if there are direct validation errors for this property; otherwise, <c>false</c>.
    /// </returns>
    /// <docs-member>HasErrors()</docs-member>
    /// <docs-type>Method</docs-type>
    /// <docs-return-type>bool</docs-return-type>
    bool HasErrors();

    /// <summary>
    /// Determines whether this property result contains any validation errors in its nested results (child objects or collections).
    /// </summary>
    /// <returns>
    /// <c>true</c> if any nested validation results contain errors; otherwise, <c>false</c>.
    /// </returns>
    /// <example>
    /// <code>
    /// if (result.HasNestedErrors()) {
    ///     Console.WriteLine("Nested validation failed!");
    /// }
    /// </code>
    /// </example>
    /// <docs-member>HasNestedErrors()</docs-member>
    /// <docs-type>Method</docs-type>
    /// <docs-return-type>bool</docs-return-type>
    bool HasNestedErrors();

    /// <summary>
    /// Checks if the validation result is valid (i.e., contains no errors).
    /// </summary>
    /// <returns>
    /// <c>true</c> if there are no validation errors; otherwise, <c>false</c>.
    /// </returns>
    /// <example>
    /// <code>
    /// if (result.IsValid()) {
    ///     Console.WriteLine("Validation passed!");
    /// }
    /// </code>
    /// </example>
    /// <docs-member>IsValid()</docs-member>
    /// <docs-type>Method</docs-type>
    /// <docs-return-type>bool</docs-return-type>
    bool IsValid();

    /// <summary>
    /// Checks if there are validation errors for this property within a specific validation chain.
    /// </summary>
    /// <param name="chainName">The validation chain identifier to filter by.</param>
    /// <returns>
    /// <c>true</c> if this property has validation errors in the given chain; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="chainName"/> is null or empty.</exception>
    /// <example>
    /// <code>
    /// if (result.HasErrors("primary")) {
    ///     Console.WriteLine("Property validation failed in primary chain!");
    /// }
    /// </code>
    /// </example>
    /// <docs-member>HasErrors(string)</docs-member>
    /// <docs-type>Method</docs-type>
    /// <docs-return-type>bool</docs-return-type>
    bool HasErrors(string chainName);


    /// <summary>
    /// Checks if the validation result is valid (no errors) for this property within a specific validation chain.
    /// </summary>
    /// <param name="chainName">The validation chain identifier to filter by.</param>
    /// <returns>
    /// <c>true</c> if this property is valid (no errors) in the given chain; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="chainName"/> is null or empty.</exception>
    /// <example>
    /// <code>
    /// if (result.IsValid("secondary")) {
    ///     Console.WriteLine("Property is valid in secondary chain!");
    /// }
    /// </code>
    /// </example>
    /// <docs-member>IsValid(string)</docs-member>
    /// <docs-type>Method</docs-type>
    /// <docs-return-type>bool</docs-return-type>
    bool IsValid(string chainName);

    /// <summary>
    /// Gets the validation errors as a read-only list.
    /// </summary>
    /// <value>
    /// A read-only list containing all validation errors found during the validation process. Each error contains detailed information about the validation failure including property path, error message, chain information, and formatting details. If there are no errors, the list will be empty.
    /// </value>
    /// <example>
    /// <code>
    /// foreach (var error in result.Errors) {
    ///     Console.WriteLine($"Property: {string.Join(".", error.Path)}");
    ///     Console.WriteLine($"Error: {error.FormattedMessage}");
    ///     if (error.Chain != null) {
    ///         Console.WriteLine($"Chain: {error.Chain}");
    ///     }
    /// }
    /// </code>
    /// </example>
    /// <docs-member>Errors</docs-member>
    /// <docs-type>Property</docs-type>
    /// <docs-return-type>IReadOnlyList&lt;ValidationError&gt;</docs-return-type>
    /// <summary>
    /// Gets the chain results for this property as a read-only list.
    /// </summary>
    /// <value>
    /// A read-only list containing all chain results associated with this property. Each <see cref="IChainResult"/> represents a validation chain and its results.
    /// </value>
    IReadOnlyCollection<IChainResult> Results { get; }

    /// <summary>
    /// Gets the nested validation results for child objects or collections as a read-only list.
    /// </summary>
    /// <value>
    /// A read-only list containing all nested validation results for child objects or collections. Each <see cref="IValidationResult"/> represents a nested validation context.
    /// </value>
    IReadOnlyCollection<IValidationResult> NestedResults { get; }

    /// <summary>
    /// Adds a nested validation result for a child object to this property result, prefixing all member names with the specified member name.
    /// </summary>
    /// <param name="other">The <see cref="IValidate"/> object to validate and add as a nested result.</param>
    /// <remarks>
    /// This method is useful for validating nested objects where validation errors from child objects need to be included in the parent property result with proper member path prefixes. All errors from <paramref name="other"/> will be added and prefixed appropriately.
    /// </remarks>
    /// <example>
    /// <code>
    /// propertyResult.AddNestedResult(childObject);
    /// // Errors will appear as "Parent.Child.PropertyName"
    /// </code>
    /// </example>
    /// <docs-member>AddNestedResult(IValidate)</docs-member>
    /// <docs-type>Method</docs-type>
    /// <docs-return-type>void</docs-return-type>
    void AddNestedResult<T>(T other) where T : IValidate;
    /// <summary>
    /// Asynchronously adds a nested validation result for a child object to this property result, prefixing all member names with the specified member name.
    /// </summary>
    /// <param name="other">The <see cref="IAsyncValidate"/> object to validate and add as a nested result.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <remarks>
    /// Use this method to validate nested objects asynchronously. Validation errors from the child object will be included in the parent property result, with member names properly prefixed. All errors from <paramref name="other"/> are added and prefixed accordingly.
    /// </remarks>
    /// <example>
    /// <code>
    /// await propertyResult.AddNestedResultAsync(childObject);
    /// // Errors will appear as "Parent.Child.PropertyName"
    /// </code>
    /// </example>
    /// <docs-member>AddNestedResultAsync(IAsyncValidate)</docs-member>
    /// <docs-type>Method</docs-type>
    /// <docs-return-type>Task</docs-return-type>
    Task AddNestedResultAsync<T>(T other) where T : IAsyncValidate;


    /// <summary>
    /// Adds nested validation results from a collection to this property result, prefixing all member names with the property name and item index.
    /// </summary>
    /// <typeparam name="T">The type of the items in the collection, must implement <see cref="IValidate"/>.</typeparam>
    /// <param name="collection">The <see cref="IEnumerable{T}"/> collection to validate and add as nested results.</param>
    /// <remarks>
    /// This method is useful for validating nested collections where validation errors from child objects need to be included in the parent property result with proper member path prefixes including array indices. All errors from <paramref name="collection"/> will be added and prefixed with the appropriate property name and item index.
    /// </remarks>
    /// <example>
    /// <code>
    /// propertyResult.AddNestedResult(user.Addresses);
    /// // Errors will appear as "Addresses.0.PropertyName", "Addresses.1.PropertyName", etc.
    /// </code>
    /// </example>
    void AddNestedResult<T>(IEnumerable<T> collection) where T : IValidate;

    /// <summary>
    /// Asynchronously adds nested validation results from a collection to this property result, prefixing all member names with the property name and item index.
    /// </summary>
    /// <typeparam name="T">The type of the items in the collection, must implement <see cref="IAsyncValidate"/>.</typeparam>
    /// <param name="collection">The <see cref="IEnumerable{T}"/> collection to validate and add as nested results.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <remarks>
    /// This method is useful for validating nested collections asynchronously. Validation errors from child objects will be included in the parent property result with proper member path prefixes including array indices. All errors from <paramref name="collection"/> will be added and prefixed with the appropriate property name and item index.
    /// </remarks>
    /// <example>
    /// <code>
    /// await propertyResult.AddNestedResultAsync(user.Addresses);
    /// // Errors will appear as "Addresses.0.PropertyName", "Addresses.1.PropertyName", etc.
    /// </code>
    /// </example>
    Task AddNestedResultAsync<T>(IEnumerable<T> collection) where T : IAsyncValidate;

    /// <summary>
    /// Adds a chain result to this property result, allowing aggregation of validation results from multiple chains.
    /// </summary>
    /// <param name="result">The <see cref="IChainResult"/> to add to this property result.</param>
    /// <remarks>
    /// This method is useful for scenarios where a property may be validated by multiple chains (e.g., different validation strategies or rulesets), and their results need to be aggregated under the same property.
    /// </remarks>
    /// <example>
    /// <code>
    /// propertyResult.AddChainResult(chainResult);
    /// </code>
    /// </example>
    void AddChainResult(IChainResult result);

    /// <summary>
    /// Gets the chain result for the specified validation chain name, if it exists.
    /// </summary>
    /// <param name="chainName">The name of the validation chain to retrieve.</param>
    /// <returns>
    /// The <see cref="IChainResult"/> for the specified chain name, or <c>null</c> if not found.
    /// </returns>
    /// <example>
    /// <code>
    /// var chainResult = propertyResult.Chain("primary");
    /// if (chainResult != null &amp;&amp; chainResult.HasErrors()) {
    ///     Console.WriteLine("Primary chain validation failed!");
    /// }
    /// </code>
    /// </example>
    IChainResult? Chain(string chainName);

}
