namespace Zadanie_rekrutacyjne.Models
{
    /// <summary>
    /// Class made for controllers to remember if the first loading of data already happened.
    /// </summary>
    public class WasLoadedModel
    {
        /// <summary>
        /// Boolean value containing information about first load of data.
        /// </summary>
        public bool WasLoaded { get; set; } = false;
    }
}
