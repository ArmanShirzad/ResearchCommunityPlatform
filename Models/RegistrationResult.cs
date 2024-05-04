namespace ResearchCommunityPlatform.Models
{
    public class RegistrationResult
    {/// <summary>
     /// Indicates whether the registration was successful.
     /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// A list of errors that occurred during the registration process.
        /// </summary>
        public List<string> Errors { get; set; } = new List<string>();

        /// <summary>
        /// Add a simplified constructor to manage the state easily.
        /// </summary>
        public RegistrationResult(bool success = false)
        {
            Success = success;
        }
    }
}
