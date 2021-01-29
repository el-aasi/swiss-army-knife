namespace SCM.SwissArmyKnife.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Bunch of extensions for Tasks to make LINQ-based syntax a bit prettier.
    /// E.g instead of (await foo()).Select(DoStuff)
    /// you can write
    /// await foo().Select(DoStuff)
    /// </summary>
    public static class FluentTaskExtensions
    {
        /// <summary>
        /// If you have an async function you can call await MyMethodAsync().Select(/*...*/)
        /// instead of (await MyMethodAsync()).Select(/*...*/)
        /// </summary>
        public static async Task<TOut> Select<TIn, TOut>(this Task<TIn> task, Func<TIn, TOut> selector) =>
            selector(await task.ConfigureAwait(false));

        /// <summary>
        /// If you have an async function you can call await MyMethodAsync().First()
        /// instead of (await MyMethodAsync()).First()
        /// </summary>
        public static async Task<TOut> First<TOut>(this Task<IEnumerable<TOut>> task) =>
            (await task.ConfigureAwait(false)).First();

        /// <summary>
        /// If you have an async function you can call await MyMethodAsync().SelectMany(/*...*/)
        /// instead of (await MyMethodAsync()).SelectMany(/*...*/)
        /// </summary>
        public static async Task<TOut> SelectMany<TIn, TOut>(this Task<TIn> task, Func<TIn, Task<TOut>> selector) =>
            await selector(await task.ConfigureAwait(false)).ConfigureAwait(false);

        /// <summary>
        /// If you have an async function you can call await MyMethodAsync().SelectMany(/*...*/)
        /// instead of (await MyMethodAsync()).SelectMany(/*...*/)
        /// </summary>
        public static async Task<TOut> SelectMany<TOut>(this Task task, Func<Task<TOut>> selector) => await selector().ConfigureAwait(false);

        /// <summary>
        /// If you have an async function you can call await MyMethodAsync().ToList()
        /// instead of (await MyMethodAsync()).ToList()
        /// </summary>
        public static async Task<List<TOut>> ToList<TOut>(this Task<IEnumerable<TOut>> task) =>
            (await task.ConfigureAwait(false)).ToList();
    }
}
